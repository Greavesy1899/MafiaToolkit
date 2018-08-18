using ModelViewer.Programming.SystemClasses;
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
            //Animation2 animation = new Animation2();
            //using (BinaryReader reader = new BinaryReader(File.Open("D:/Users/Connor/Desktop/SteamLibrary/steamapps/common/Mafia II/pc/sds/basic_anim/extracted/basic_anim2.sds/07-IDLE_DIFF_6-P.an2", FileMode.Open)))
            //{
            //    animation.ReadFromFile(reader);
            //}

            //Cutscene cutscene = new Cutscene();
            //using (BinaryReader reader = new BinaryReader(File.Open("D:/Users/Connor/Desktop/SteamLibrary/steamapps/common/Mafia II/pc/dlcs/cnt_joes_adventures/sds/fmv/extracted/dlc_ja_pp_test.sds/Cutscene_0.cut", FileMode.Open)))
            //{
            //    cutscene.ReadFromFile(reader);
            //}

            //do vital inits;
            CheckINIExists();
            ToolkitSettings.ReadINI();

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
