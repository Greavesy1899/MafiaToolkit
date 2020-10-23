using System;

namespace Utils.Discord
{
    internal class DiscordController
    {
        public DiscordRPC.RichPresence presence;
        DiscordRPC.EventHandlers handlers;
        public string applicationId = "477892129235009537";
        public string optionalSteamId = string.Empty;

        /// <summary>
        ///     Initializes Discord RPC
        /// </summary>
        public void Initialize()
        {
            handlers = new DiscordRPC.EventHandlers();
            handlers.readyCallback = ReadyCallback;
            handlers.disconnectedCallback += DisconnectedCallback;
            handlers.errorCallback += ErrorCallback;
            DiscordRPC.Initialize(applicationId, ref handlers, true, optionalSteamId);
        }

        public void UpdatePresence()
        {
            DiscordRPC.UpdatePresence(ref presence);
        }

        public void Shutdown()
        {
            DiscordRPC.Shutdown();
        }

        public void ReadyCallback()
        {
            Console.WriteLine("Discord RPC is ready!");
        }

        public void DisconnectedCallback(int errorCode, string message)
        {
            Console.WriteLine($"Error: {errorCode} - {message}");
        }

        public void ErrorCallback(int errorCode, string message)
        {
            Console.WriteLine($"Error: {errorCode} - {message}");
        }
    }
}