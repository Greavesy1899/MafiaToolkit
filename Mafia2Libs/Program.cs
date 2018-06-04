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
            //using (BinaryReader reader = new BinaryReader(File.Open("FrameNameTable_0.bin", FileMode.Open)))
            //{
            //    FrameNameTable actor = new FrameNameTable();
            //    actor.ReadFromFile(reader);
            //}
            //using (BinaryReader reader = new BinaryReader(File.Open("Actors_0.bin", FileMode.Open)))
            //{
            //    ActorParse actor = new ActorParse(reader);
            //}
            //XMLTest xml = new XMLTest();
            //ItemDescParse itemDescs = new ItemDescParse();
            Application.Run(new FrameResourceTool());
        }
    }
}
