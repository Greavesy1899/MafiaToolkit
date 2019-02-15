using System.IO;

namespace ResourceTypes.FrameResource
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
            unk01 = 0;
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

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);
        }

    }

}
