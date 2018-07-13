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

            try
            {
                MaterialData.Default = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default.mtl");
                MaterialData.Default50 = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default50.mtl");
                MaterialData.Default60 = MaterialsParse.ReadMatFile(Properties.Settings.Default.MaterialPath + "/default60.mtl");
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
