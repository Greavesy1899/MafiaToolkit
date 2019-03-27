using System;
using Utils.Logging;
using Utils.Discord;
using System.Windows.Forms;

namespace Utils.Settings
{
    public class ToolkitSettings
    {
        private static IniFile ini;

        //Directories keys;
        public static string M2Directory;

        //Discord keys;
        public static bool DiscordEnabled;
        public static bool DiscordElapsedTimeEnabled;
        public static bool DiscordStateEnabled;
        public static bool DiscordDetailsEnabled;

        //ModelViewer keys;
        public static bool VSync;
        public static float ScreenDepth;
        public static float ScreenNear;
        public static string ShaderPath;
        public static string DataPath;
        public const int Width = 1920;
        public const int Height = 1080;

        //Model Exporting keys;
        public static int Format;
        public static string ExportPath;

        //Material Library Keys:
        public static string MaterialLibs;

        //Misc vars;
        private static long ElapsedTime;
        private static DiscordController controller;
        public const string DiscordLibLocation = "libs/discord-rpc";
        public static bool LoggingEnabled;
        public static int Language;

        /// <summary>
        /// Read and store settings.
        /// </summary>
        public static void ReadINI()
        {
            ini = new IniFile();
            ElapsedTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            M2Directory = ReadKey("MafiaII", "Directories");
            bool.TryParse(ReadKey("Enabled", "Discord", "True"), out DiscordEnabled);
            bool.TryParse(ReadKey("ElapsedTimeEnabled", "Discord", "True"), out DiscordElapsedTimeEnabled);
            bool.TryParse(ReadKey("StateEmabled", "Discord", "True"), out DiscordStateEnabled);
            bool.TryParse(ReadKey("DetailsEnabled", "Discord", "True"), out DiscordDetailsEnabled);
            bool.TryParse(ReadKey("VSync", "ModelViewer", "True"), out VSync);
            float.TryParse(ReadKey("ScreenDepth", "ModelViewer", "10000"), out ScreenDepth);
            float.TryParse(ReadKey("ScreenNear", "ModelViewer", "1"), out ScreenNear);
            bool.TryParse(ReadKey("Logging", "Misc", "True"), out LoggingEnabled);
            int.TryParse(ReadKey("Language", "Misc", "0"), out Language);
            int.TryParse(ReadKey("Format", "Exporting", "0"), out Format);
            ExportPath = ReadKey("ModelExportPath", "Directories", Application.StartupPath);
            MaterialLibs = ReadKey("MaterialLibs", "Materials", "");


            ShaderPath = @"Shaders\";
            DataPath = @"Data\";
            Log.LoggingEnabled = LoggingEnabled;

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