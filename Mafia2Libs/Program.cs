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

            using (BinaryReader reader = new BinaryReader(File.Open("proxy_11_Italy.m2t", FileMode.Open)))
            {
                Model model = new Model();
                model.ReadFromM2T(reader);
            }

            try
            {
                IniFile ini = new IniFile();
                string path = ini.Read("MaterialPath", "Directories");
                MaterialData.Default = MaterialsParse.ReadMatFile(path + "/default.mtl");
                MaterialData.Default50 = MaterialsParse.ReadMatFile(path + "/default50.mtl");
                MaterialData.Default60 = MaterialsParse.ReadMatFile(path + "/default60.mtl");
                MaterialData.HasLoaded = true;
                MaterialsParse.SetMaterials(MaterialData.Default);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load materials. Error occured: \n\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            Application.Run(new FrameResourceTool());
        }
    }
}
