using System;
using System.IO;
using System.Windows.Forms;

namespace Mafia2Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CheckINIExists();
            DiscordPrefs.InitRPC();
            MaterialData.Load();
            Application.Run(new GameExplorer());
        }

        /// <summary>
        /// Check if ini exists.
        /// </summary>
        private static void CheckINIExists()
        {
            if (File.Exists("Mafia2Tool.ini"))
                return;
            
            IniFile file = new IniFile();
            file.Write("MafiaII", "", "Directories");
        }
    }
}
