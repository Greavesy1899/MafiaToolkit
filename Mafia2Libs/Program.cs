using System;
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

            MaterialData.Default = MaterialsParse.ReadMatFile("default.mtl");
            MaterialData.Default50 = MaterialsParse.ReadMatFile("default50.mtl");
            MaterialData.Default60 = MaterialsParse.ReadMatFile("default60.mtl");

            Application.Run(new FrameResourceTool());
        }
    }
}
