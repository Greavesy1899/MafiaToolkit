using System.IO;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectComponent_U005 : FrameObjectBase
    {
        int unk01;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public FrameObjectComponent_U005(FrameResource OwningResource) : base(OwningResource)
        {
            unk01 = 0;
        }

        public FrameObjectComponent_U005(FrameObjectComponent_U005 other) : base(other)
        {
            unk01 = other.unk01;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            unk01 = reader.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);
        }

    }

}
