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
            using (BinaryReader reader = new BinaryReader(File.Open("cityareas.bin", FileMode.Open)))
            {
                CityAreas areas = new CityAreas(reader);
            }
            //XMLTest xml = new XMLTest();
            //Application.Run(new MaterialTool());
            //ItemDescParse itemDesc = new ItemDescParse();
            Application.Run(new FrameResourceTool());
        }
    }
}
