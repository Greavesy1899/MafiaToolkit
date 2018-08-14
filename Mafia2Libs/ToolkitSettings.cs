using System;

namespace Mafia2Tool
{
    public class ToolkitSettings
    {
        private static IniFile ini;

        //Directories keys;
        public static string M2Directory;
        public static string MaterialDirectory;

        //Discord keys;
        public static bool DiscordEnabled;
        public static bool DiscordElapsedTimeEnabled;
        public static bool DiscordStateEnabled;
        public static bool DiscordDetailsEnabled;

        //Misc vars;
        private static long ElapsedTime;
        private static DiscordController controller;
        public const string DiscordLibLocation = "libs/discord-rpc";

        /// <summary>
        /// Read and store settings.
        /// </summary>
        public static void ReadINI()
        {
            ini = new IniFile();
            ElapsedTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            M2Directory = ReadKey("MafiaII", "Directories");
            MaterialDirectory = ReadKey("MaterialPath", "Directories");
            bool.TryParse(ReadKey("Enabled", "Discord", "True"), out DiscordEnabled);
            bool.TryParse(ReadKey("ElapsedTimeEnabled", "Discord", "True"), out DiscordElapsedTimeEnabled);
            bool.TryParse(ReadKey("StateEmabled", "Discord", "True"), out DiscordStateEnabled);
            bool.TryParse(ReadKey("DetailsEnabled", "Discord", "True"), out DiscordDetailsEnabled);

            if (DiscordEnabled)
                InitRichPresence();
        }

        /// <summary>
        /// Read Key from the ini file and do some checks.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="section"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static string ReadKey(string key, string section, string defaultValue = null)
        {
            if (!ini.KeyExists(key, section))
                ini.Write(key, defaultValue, section);
            else
                return ini.Read(key, section);

            return defaultValue;
        }

        /// <summary>
        /// Write Key to the ini file and do some checks.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="section"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static void WriteKey(string key, string section, string defaultValue)
        {
            ini.Write(key, defaultValue, section);
        }

        /// <summary>
        /// Initialise Discord Rich Presence.
        /// </summary>
        private static void InitRichPresence()
        {
            controller = new DiscordController();
            controller.Initialize();
            controller.presence = new DiscordRPC.RichPresence()
            {
                smallImageKey = "",
                smallImageText = "",
                largeImageKey = "main_art",
                largeImageText = "",
                startTimestamp = ElapsedTime
            };
            UpdateRichPresence("Using the Game Explorer");
        }

        /// <summary>
        /// Update Discord Rich Presence.
        /// </summary>
        /// <param name="details"></param>
        public static void UpdateRichPresence(string details = null)
        {
            if (!DiscordEnabled)
            {
                DiscordRPC.Shutdown();
                controller = null;
            }
            else
            {
                if (controller == null)
                    InitRichPresence();

                controller.presence.state = DiscordStateEnabled ? "Making mods for Mafia II." : null;
                controller.presence.details = DiscordDetailsEnabled ? "" : null;
                controller.presence.startTimestamp = DiscordElapsedTimeEnabled ? ElapsedTime : 0;

                DiscordRPC.UpdatePresence(ref controller.presence);
            }
        }
    }
}