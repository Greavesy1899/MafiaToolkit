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

            //XMLTest test = new XMLTest();
            //Prefab prefab = new Prefab("E:/Games/Steam/steamapps/common/Mafia II/pc/sds_en/shops/extracted/maria_agnelo/PREFAB_0.bin");
            //using (BinaryReader reader = new BinaryReader(File.Open(Properties.Settings.Default.SDSPath2 + "/Collisions_0.bin", FileMode.Open)))
            //{
                //Collision areas = new Collision(Properties.Settings.Default.SDSPath2 + "/Collisions_0.bin");
            //}
            MaterialData.Default = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default.mtl"); MaterialsParse.WriteMatFile("default.mtl");
            MaterialData.Default50 = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default50.mtl");
            MaterialData.Default60 = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default60.mtl");
            MaterialsParse.SetMaterials(MaterialData.Default);


            Application.Run(new FrameResourceTool());
        }
    }
}
