using System;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectTarget : FrameObjectJoint
    {
        private int unk01;
        private int unk02;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public int Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }

        public FrameObjectTarget(FrameResource OwningResource) : base(OwningResource) { }

        public FrameObjectTarget(FrameObjectTarget other) : base(other)
        {
            unk01 = other.unk01;
            unk02 = other.unk02;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            unk01 = reader.ReadInt32(isBigEndian);
            unk02 = reader.ReadInt32(isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);
            writer.Write(unk02);
        }
    }
}
