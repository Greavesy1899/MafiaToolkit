using System.Windows.Forms;
using Mafia2;
using System.IO;

namespace Mafia2Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //ItemDescParse itemDescs = new ItemDescParse();
            Application.Run(new FrameResourceTool());
        }
    }
}
