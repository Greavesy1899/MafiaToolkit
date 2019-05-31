using System.ComponentModel;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectArea : FrameObjectJoint
    {
        int unk01;
        int planesSize;
        Vector4[] planes;
        BoundingBox bounds;

        public int Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }

        [Browsable(false)]
        public int PlaneSize {
            get { return planesSize; }
            set { planesSize = value; }
        }

        public Vector4[] Planes {
            get { return planes; }
            set { planes = value; }
        }
        public BoundingBox Bounds {
            get { return bounds; }
            set { bounds = value; }
        }

        public float BoundsMinimumX {
            get { return bounds.Minimum.X; }
            set { bounds.Minimum.X = value; }
        }
        public float BoundsMinimumY {
            get { return bounds.Minimum.Y; }
            set { bounds.Minimum.Y = value; }
        }
        public float BoundsMinimumZ {
            get { return bounds.Minimum.Z; }
            set { bounds.Minimum.Z = value; }
        }
        public float BoundsMaximumX {
            get { return bounds.Maximum.X; }
            set { bounds.Maximum.X = value; }
        }
        public float BoundsMaximumY {
            get { return bounds.Maximum.Y; }
            set { bounds.Maximum.Y = value; }
        }
        public float BoundsMaximumZ {
            get { return bounds.Maximum.Z; }
            set { bounds.Maximum.Z = value; }
        }

        public FrameObjectArea(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public FrameObjectArea() : base()
        {
            unk01 = 0;
            planesSize = 0;
            planes = new Vector4[planesSize];
            bounds = new BoundingBox();
        }

        public FrameObjectArea(FrameObjectArea other) : base(other)
        {
            unk01 = other.unk01;
            planesSize = other.planesSize;
            planes = other.planes;
            bounds = other.bounds;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk01 = reader.ReadInt32();
            planesSize = reader.ReadInt32();
            planes = new Vector4[planesSize];

            for (int i = 0; i != planes.Length; i++)
                planes[i] = Vector4Extenders.ReadFromFile(reader);

            bounds = BoundingBoxExtenders.ReadFromFile(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(unk01);
            writer.Write(planes.Length);

            for (int i = 0; i != planes.Length; i++)
                planes[i].WriteToFile(writer);

            bounds.WriteToFile(writer);
        }

        public void FillPlanesArray()
        {
            planes = new Vector4[6];
            planes[0] = new Vector4(-1, 0, 0, bounds.Maximum.X);
            planes[1] = new Vector4(1, 0, 0, bounds.Maximum.X);
            planes[2] = new Vector4(0, -1, 0, bounds.Maximum.Y);
            planes[3] = new Vector4(0, 1, 0, bounds.Maximum.Y);
            planes[4] = new Vector4(0, 0, -1, bounds.Maximum.Z);
            planes[5] = new Vector4(0, 0, 1, bounds.Maximum.Z);
        }

        public override string ToString()
        {
            return Name.String;
        }
    }
}
