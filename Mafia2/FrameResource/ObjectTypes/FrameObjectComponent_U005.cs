using System.IO;

namespace Mafia2
{
    public class FrameObjectComponent_U005 : FrameObjectBase
    {
        int unk01;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public FrameObjectComponent_U005() : base()
        {

        }

        public FrameObjectComponent_U005(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk01 = reader.ReadInt32();
        }

    }

}
