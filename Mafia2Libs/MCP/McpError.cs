using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using Mafia2Tool.MCP.Services;

namespace Mafia2Tool.MCP;

/// <summary>
/// Helpers for surfacing exception detail in MCP tool responses.
///
/// The MCP C# SDK normally hides exception detail behind a generic "An error occurred invoking
/// 'X'." message to avoid leaking internal implementation. This server is bound to localhost
/// (see <see cref="McpServerHost"/>) and used as a developer debugging tool, so we deliberately
/// emit the full type + inner-exception chain + first stack frames into the tool's JSON
/// response. Without this, repros like "encrypted-SDS fails with InvalidOperationException deep
/// inside BlockReaderStream" are effectively un-diagnosable from the client.
/// </summary>
public static class McpError
{
    private const int MaxChainDepth = 6;
    private const int MaxStackFrames = 10;

    /// <summary>
    /// Builds the structured detail object embedded as <c>errorDetail</c> in tool responses.
    /// Unwraps <see cref="TargetInvocationException"/> so reflection-invoked failures point at
    /// the real source.
    /// </summary>
    public static object Build(Exception ex)
    {
        while (ex is TargetInvocationException tie && tie.InnerException != null)
        {
            ex = tie.InnerException;
        }

        var chain = new List<object>(MaxChainDepth);
        for (Exception? e = ex; e != null && chain.Count < MaxChainDepth; e = e.InnerException)
        {
            chain.Add(new
            {
                type = e.GetType().FullName,
                message = e.Message,
                stack = SummarizeStack(e.StackTrace),
            });
        }
        return new { chain };
    }

    /// <summary>
    /// Serializes the standard <c>{ success = false, error, errorDetail }</c> failure payload
    /// returned from a catch block.
    /// </summary>
    public static string FailJson(Exception ex) =>
        JsonSerializer.Serialize(new { success = false, error = ex.Message, errorDetail = Build(ex) });

    /// <summary>
    /// Serializes the failure payload for an <see cref="SdsService.OpenFile"/> miss, forwarding
    /// the service's structured <see cref="SdsService.LastErrorDetail"/> so the caller sees the
    /// real cause instead of just "Failed to open SDS file".
    /// </summary>
    public static string OpenFailureJson(SdsService service, string fallback = "Failed to open SDS file") =>
        JsonSerializer.Serialize(new
        {
            success = false,
            error = service.LastError ?? fallback,
            errorDetail = service.LastErrorDetail,
        });

    private static string[] SummarizeStack(string? stack)
    {
        if (string.IsNullOrEmpty(stack)) return Array.Empty<string>();
        var lines = stack.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var take = Math.Min(lines.Length, MaxStackFrames);
        var trimmed = new string[take];
        for (int i = 0; i < take; i++) trimmed[i] = lines[i].Trim();
        return trimmed;
    }
}
