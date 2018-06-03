using System.IO;

namespace Mafia2
{
    public class FrameObjectArea : FrameObjectJoint
    {
        int unk01;
        int unk02;
        float[] unkFloats;
        Bounds unkBounds;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public int Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }
        public float[] UnkFloats {
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
            unkFloats = new float[unk02 * 4];

            for(int i = 0; i != unkFloats.Length; i++)
                unkFloats[i] = reader.ReadSingle();

            unkBounds = new Bounds(reader);
        }

        public override string ToString()
        {
            return "Area Block";
        }
    }
}
