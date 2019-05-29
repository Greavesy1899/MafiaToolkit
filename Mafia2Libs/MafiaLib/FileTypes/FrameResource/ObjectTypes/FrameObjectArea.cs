using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectArea : FrameObjectJoint
    {
        int unk01;
        int unk02;
        Vector4[] unkFloats;
        BoundingBox unkBounds;

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
        public Vector4[] UnkFloats {
            get { return unkFloats; }
            set { unkFloats = value; }
        }
        public BoundingBox Bounds {
            get { return unkBounds; }
            set { unkBounds = value; }
        }

        public float BoundsMinimumX {
            get { return unkBounds.Minimum.X; }
            set { unkBounds.Minimum.X = value; }
        }
        public float BoundsMinimumY {
            get { return unkBounds.Minimum.Y; }
            set { unkBounds.Minimum.Y = value; }
        }
        public float BoundsMinimumZ {
            get { return unkBounds.Minimum.Z; }
            set { unkBounds.Minimum.Z = value; }
        }
        public float BoundsMaximumX {
            get { return unkBounds.Maximum.X; }
            set { unkBounds.Maximum.X = value; }
        }
        public float BoundsMaximumY {
            get { return unkBounds.Maximum.Y; }
            set { unkBounds.Maximum.Y = value; }
        }
        public float BoundsMaximumZ {
            get { return unkBounds.Maximum.Z; }
            set { unkBounds.Maximum.Z = value; }
        }

        public FrameObjectArea(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public FrameObjectArea() : base()
        {
            unk01 = 0;
            unk02 = 0;
            unkFloats = new Vector4[unk02];
            unkBounds = new BoundingBox();
        }

        public FrameObjectArea(FrameObjectArea other) : base(other)
        {
            unk01 = other.unk01;
            unk02 = other.unk02;
            unkFloats = other.unkFloats;
            unkBounds = other.unkBounds;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk01 = reader.ReadInt32();
            unk02 = reader.ReadInt32();
            unkFloats = new Vector4[unk02];

            for (int i = 0; i != unkFloats.Length; i++)
                unkFloats[i] = Vector4Extenders.ReadFromFile(reader);

            unkBounds = BoundingBoxExtenders.ReadFromFile(reader);
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
