using System.IO;

namespace Mafia2
{
    public class FrameObjectArea : FrameObjectJoint
    {
        int unk01;
        int unk02;
        Float4[] unkFloats;
        Bounds unkBounds;

        //-1 means invert the float, eg: 25.459 would be -25.459
        //data[0] = top face			 1
        //data[1] = right face		 1
        //data[2] = front face		-1
        //data[3] = back face			 1
        //data[4] = bottom face		-1
        //data[5] = left face			-1

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public int Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }
        public Float4[] UnkFloats {
            get { return unkFloats; }
            set { unkFloats = value; }
        }
        public Bounds Bounds {
            get { return unkBounds; }
            set { unkBounds = value; }
        }
        public FrameObjectArea(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk01 = reader.ReadInt32();
            unk02 = reader.ReadInt32();
            unkFloats = new Float4[unk02];

            for (int i = 0; i != unkFloats.Length; i++)
                unkFloats[i] = new Float4(reader);

            unkBounds = new Bounds(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);
            writer.Write(unk02);

            for (int i = 0; i != unkFloats.Length; i++)
                unkFloats[i].WriteToFile(writer);

            unkBounds.WriteToFile(writer);
        }

        public override string ToString()
        {
            return Name.String;
        }
    }
}
