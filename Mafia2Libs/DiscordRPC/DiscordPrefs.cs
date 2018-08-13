using System;

namespace Mafia2Tool
{
    class DiscordPrefs
    {
        public static DiscordController controller;
        public const string LibLocation = "libs/discord-rpc";

        public static void InitRPC()
        {
            controller = new DiscordController();
            controller.Initialize();
            controller.presence = new DiscordRPC.RichPresence()
            {
                smallImageKey = "",
                smallImageText = "",
                largeImageKey = "main_art",
                largeImageText = "",
                startTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds
        };
            controller.presence.state = "Making mods for Mafia II.";
            Update("Using the Game Explorer");
        }

        public static void Update(string details)
        {
            controller.presence.details = details;
            DiscordRPC.UpdatePresence(ref controller.presence);
        }
    }
}
