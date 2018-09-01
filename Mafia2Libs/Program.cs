using System;
using System.IO;
using System.Windows.Forms;
using Mafia2;

namespace Mafia2Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (BinaryReader reader = new BinaryReader(File.Open("D:/Users/Connor/Desktop/SteamLibrary/steamapps/common/Mafia II/pc/sds/city/extracted/midtown.sds/08mid_neon02e.ifl", FileMode.Open)))
            {
                AnimatedTexture texture = new AnimatedTexture(reader);
            }

                //setup logger.
                Log.DeleteOldLog();
            Log.WriteLine("Toolkit has launched.");

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
