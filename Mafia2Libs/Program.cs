using System.Windows.Forms;
using System.IO;
using Mafia2;
using System;

namespace Mafia2Tool {
    class Program {
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new MaterialTool());

            Application.Run(new FrameResourceTool());
        }
    }
}
