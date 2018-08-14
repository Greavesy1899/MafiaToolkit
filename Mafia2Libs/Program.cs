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
            //do vital inits;
            CheckINIExists();
            ToolkitSettings.ReadINI();

            //begin form inits;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
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
            else
                new IniFile();
        }
    }
}
