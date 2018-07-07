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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //using (BinaryReader reader = new BinaryReader(File.Open(Properties.Settings.Default.SDSPath2 + "/NAV_AIWORLD_DATA_0.bin", FileMode.Open)))
            //{
            //    NAVData navData = new NAVData(reader);
            //}

            MaterialData.Default = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default.mtl");
            MaterialData.Default50 = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default50.mtl");
            MaterialData.Default60 = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default60.mtl");
            MaterialsParse.SetMaterials(MaterialData.Default);


            Application.Run(new FrameResourceTool());
        }
    }
}
