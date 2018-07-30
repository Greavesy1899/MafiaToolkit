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
            MaterialData.Load();
            Application.Run(new FrameResourceTool());
        }
    }
}
