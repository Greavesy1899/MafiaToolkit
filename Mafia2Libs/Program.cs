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
            using (BinaryReader reader = new BinaryReader(File.Open("Actors_0.bin", FileMode.Open)))
            {
                ActorParse actor = new ActorParse(reader);
            }
            //ItemDescParse itemDescs = new ItemDescParse();
            Application.Run(new FrameResourceTool());
        }
    }
}
