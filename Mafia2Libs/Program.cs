using System;
using System.IO;
using System.Windows.Forms;
using Utils.Settings;
using Utils.Lang;

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
            ToolkitSettings.ReadINI();
            Language.ReadLanguageXML();
            Application.Run(new GameExplorer());
        }

        /// <summary>
        /// Check if ini exists.
        /// </summary>
        private static void CheckINIExists()
        {
            //only here because the exe name was changed, and lots of people had different .ini names.
            if (File.Exists("Mafia2Toolkit.ini") && !File.Exists("MafiaToolkit.ini"))
            {
                File.Move("Mafia2Toolkit.ini", "MafiaToolkit.ini");
                File.Delete("Mafia2Toolkit.ini");
            }
            if (File.Exists(Path.Combine(Application.ExecutablePath, "MafiaToolkit.ini")))
                return;
            else
                new IniFile();
        }
    }
}
