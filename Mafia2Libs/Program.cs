using System;
using System.IO;
using System.Windows.Forms;
using Mafia2;
using ApexSDK;
using System.Threading;
//using ModelViewer.Programming.SystemClasses;

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

            Log.DeleteOldLog();
            Log.CreateFile();

            //do vital inits;
            CheckINIExists();
            ToolkitSettings.ReadINI();

            Language.ReadLanguageXML();

            //SystemClass.StartRenderForm("Model Viewer", 1024, 720, true, "Model", false, 0);
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
