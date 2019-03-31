using System;
using System.IO;
using System.Windows.Forms;
using SharpDX;
using System.ComponentModel;
using Utils.Settings;
using Utils.Lang;

namespace Mafia2Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //begin form inits;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Log.DeleteOldLog();
            //Log.CreateFile();

            ////do vital inits;
            CheckINIExists();
            ToolkitSettings.ReadINI();
            Language.ReadLanguageXML();
            MaterialData.Load();
            Application.Run(new GameExplorer());
        }

        /// <summary>
        /// Check if ini exists.
        /// </summary>
        private static void CheckINIExists()
        {
            if (File.Exists(Path.Combine(Application.ExecutablePath, "Mafia2Tool.ini")))
                return;
            else
                new IniFile();
        }
    }
}
