using System;
using System.IO;

namespace Mafia2
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

        public FrameObjectTarget() : base()
        {
        }

        public FrameObjectTarget(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk01 = reader.ReadInt32();
            unk02 = reader.ReadInt32();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);
            writer.Write(unk02);
        }
    }
}
