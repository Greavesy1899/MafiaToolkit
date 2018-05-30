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
            //using (BinaryReader reader = new BinaryReader(File.Open("SoundSectors_L_Motor_Show.bin", FileMode.Open)))
            //{
            //    SoundSector soundSector = new SoundSector(reader);
            //}
            Application.Run(new FrameResourceTool());
        }
    }
}
