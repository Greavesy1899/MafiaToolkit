using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mafia2Tool.MCP;

/// <summary>
/// Builds a structured error payload that MCP tools can include in their JSON responses.
/// Surfaces the full exception chain (type + message + first stack frames) so consumers don't
/// see a bare "InvalidOperationException: Operation is not valid due to the current state of the
/// object." with no clue where it came from.
/// </summary>
public static class McpError
{
    private const int MaxDepth = 6;
    private const int MaxFrames = 10;

    public static object Build(Exception ex, string? fallbackMessage = null)
    {
        if (ex == null)
        {
            return new { message = fallbackMessage ?? "Unknown error" };
        }

        // Reflection invoke wraps the real exception in TargetInvocationException; unwrap to surface it.
        while (ex is TargetInvocationException tie && tie.InnerException != null)
        {
            ex = tie.InnerException;
        }

        var chain = new List<object>();
        for (Exception? e = ex; e != null && chain.Count < MaxDepth; e = e.InnerException)
        {
            chain.Add(new
            {
                type = e.GetType().FullName,
                message = e.Message,
                stack = SummarizeStack(e.StackTrace),
            });
        }

        return new
        {
            type = ex.GetType().FullName,
            message = ex.Message,
            chain,
        };
    }

    private static string[] SummarizeStack(string? stack)
    {
        if (string.IsNullOrEmpty(stack)) return Array.Empty<string>();
        var lines = stack.Split('\n');
        var trimmed = new List<string>(Math.Min(lines.Length, MaxFrames));
        for (int i = 0; i < lines.Length && trimmed.Count < MaxFrames; i++)
        {
            var line = lines[i].Trim();
            if (line.Length == 0) continue;
            trimmed.Add(line);
        }
        return trimmed.ToArray();
    }
}
