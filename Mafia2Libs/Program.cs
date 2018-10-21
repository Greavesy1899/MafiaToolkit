using System;
using System.IO;
using System.Windows.Forms;
using Mafia2;
using ApexSDK;
//using ModelViewer.Programming.SystemClasses;

namespace Mafia2Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            FrameProps props = new FrameProps(new BinaryReader(File.Open("C://Program Files (x86)//Steam//steamapps//common//Mafia II//edit//tables//FrameProps.bin", FileMode.Open)));
            //Prefab prefab = new Prefab("D://Users//Connor//Desktop//SteamLibrary//steamapps//common//Mafia II//pc//sds//cars//extracted//cars_universal.sds//PREFAB_55.prf");
            //setup logger.
            Log.DeleteOldLog();

            //do vital inits;
            CheckINIExists();
            ToolkitSettings.ReadINI();
            Language.ReadLanguageXML();

            //begin form inits;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
