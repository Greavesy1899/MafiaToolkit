using MafiaToolkit.McpServer.Services;
using MafiaToolkit.McpServer.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace MafiaToolkit.McpServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        // Create the MCP server builder
        var builder = Host.CreateApplicationBuilder(args);

        // Disable console logging to avoid corrupting MCP stdio communication
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.None);

        // Register services
        builder.Services.AddSingleton<SdsService>();

        // Configure MCP server with stdio transport
        builder.Services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly();

        // Build and run
        var host = builder.Build();
        await host.RunAsync();
    }
}
