using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mafia2Tool.MCP.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.AspNetCore;

namespace Mafia2Tool.MCP;

/// <summary>
/// Hosts the MCP server for SDS file browsing via SSE transport
/// </summary>
public static class McpServerHost
{
    public const int DefaultPort = 5123;
    public const string DefaultHost = "localhost";

    private static WebApplication? _app;
    private static CancellationTokenSource? _cts;
    private static Task? _serverTask;
    private static readonly string LogFile = Path.Combine(Path.GetTempPath(), "MafiaToolkit_MCP.log");

    /// <summary>
    /// Gets whether the MCP server is currently running
    /// </summary>
    public static bool IsRunning => _app != null && _serverTask != null && !_serverTask.IsCompleted;

    /// <summary>
    /// Gets the URL the MCP server is listening on
    /// </summary>
    public static string? ServerUrl => IsRunning ? $"http://{DefaultHost}:{DefaultPort}/sse" : null;

    private static void Log(string message)
    {
        var line = $"[{DateTime.Now:HH:mm:ss}] {message}";
        try
        {
            File.AppendAllText(LogFile, line + Environment.NewLine);
        }
        catch { }
        System.Diagnostics.Debug.WriteLine($"[MCP] {message}");
    }

    /// <summary>
    /// Starts the MCP server in the background
    /// </summary>
    public static void Start(int port = DefaultPort)
    {
        if (IsRunning)
        {
            Log("Server is already running");
            return;
        }

        _cts = new CancellationTokenSource();

        // Start in a new thread to avoid blocking
        _serverTask = Task.Factory.StartNew(() =>
        {
            try
            {
                Log($"Creating WebApplication builder...");

                var builder = WebApplication.CreateSlimBuilder();

                // Suppress logging
                builder.Logging.ClearProviders();

                // Configure Kestrel to only bind to localhost
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenLocalhost(port);
                });

                Log($"Registering services...");

                // Add CORS for local connections
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .WithExposedHeaders("Mcp-Session-Id");
                    });
                });

                // Register MCP services
                builder.Services.AddSingleton<SdsService>();

                // Configure MCP server
                builder.Services
                    .AddMcpServer()
                    .WithHttpTransport()
                    .WithToolsFromAssembly(typeof(McpServerHost).Assembly);

                Log($"Building application...");

                _app = builder.Build();

                // Enable CORS
                _app.UseCors();

                // Add mock OAuth endpoints to satisfy Claude Code's OAuth discovery
                _app.MapGet("/.well-known/oauth-authorization-server", () => Results.Json(new
                {
                    issuer = $"http://{DefaultHost}:{port}",
                    authorization_endpoint = $"http://{DefaultHost}:{port}/authorize",
                    token_endpoint = $"http://{DefaultHost}:{port}/token",
                    registration_endpoint = $"http://{DefaultHost}:{port}/register",
                    response_types_supported = new[] { "code" },
                    grant_types_supported = new[] { "authorization_code" },
                    code_challenge_methods_supported = new[] { "S256" }
                }));

                _app.MapPost("/register", async (HttpContext ctx) =>
                {
                    // Read the request body to get redirect_uris
                    var body = await System.Text.Json.JsonSerializer.DeserializeAsync<System.Text.Json.JsonElement>(ctx.Request.Body);
                    var redirectUris = new[] { "http://localhost" };
                    if (body.TryGetProperty("redirect_uris", out var uris) && uris.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        redirectUris = uris.EnumerateArray().Select(u => u.GetString() ?? "").ToArray();
                    }

                    return Results.Json(new
                    {
                        client_id = "mafia-toolkit-local",
                        client_secret = "not-needed",
                        client_id_issued_at = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        client_secret_expires_at = 0,
                        redirect_uris = redirectUris
                    });
                });

                _app.MapGet("/authorize", (HttpContext ctx) =>
                {
                    var redirectUri = ctx.Request.Query["redirect_uri"].ToString();
                    var state = ctx.Request.Query["state"].ToString();
                    var code = "local-auth-code";
                    return Results.Redirect($"{redirectUri}?code={code}&state={state}");
                });

                _app.MapPost("/token", () => Results.Json(new
                {
                    access_token = "local-access-token",
                    token_type = "Bearer",
                    expires_in = 3600
                }));

                // Map MCP SSE endpoint
                _app.MapMcp();

                Log($"Starting server on http://{DefaultHost}:{port}/sse");

                // Run synchronously in this thread
                _app.Run($"http://{DefaultHost}:{port}");
            }
            catch (Exception ex)
            {
                Log($"Server error: {ex}");
            }
        }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        // Give it a moment to start
        Thread.Sleep(500);
        Log($"Server task started, IsRunning={IsRunning}");
    }

    /// <summary>
    /// Stops the MCP server
    /// </summary>
    public static async Task StopAsync()
    {
        if (_app == null)
        {
            return;
        }

        try
        {
            Log("Stopping server...");
            _cts?.Cancel();
            using var stopCts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            await _app.StopAsync(stopCts.Token);
        }
        catch (Exception ex)
        {
            Log($"Error stopping server: {ex.Message}");
        }
        finally
        {
            _app?.DisposeAsync().AsTask().Wait(1000);
            _app = null;
            _cts?.Dispose();
            _cts = null;
            _serverTask = null;
            Log("Server stopped");
        }
    }

    /// <summary>
    /// Stops the MCP server synchronously
    /// </summary>
    public static void Stop()
    {
        StopAsync().GetAwaiter().GetResult();
    }
}
