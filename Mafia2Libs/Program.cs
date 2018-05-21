using System.Windows.Forms;
using System.IO;
using Mafia2;

namespace Mafia2Tool {
    class Program {
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //using (BinaryReader reader = new BinaryReader(File.Open("FrameNameTable_0.bin", FileMode.Open)))
            //{
            //    FrameNameTable nameTable = new FrameNameTable();
            //    nameTable.ReadFromFile(reader);
            //}
            //Application.Run(new MaterialTool());
            Application.Run(new FrameResourceTool());
        }
    }
}
