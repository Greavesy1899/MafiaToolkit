using System.ComponentModel;
using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectDummy : FrameObjectJoint
    {
        BoundingBox bounds;

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

        public FrameObjectDummy(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public FrameObjectDummy(FrameObjectDummy other) : base(other)
        {
            bounds = other.bounds;
        }

        public FrameObjectDummy() : base()
        {
            bounds = new BoundingBox();
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            bounds = BoundingBoxExtenders.ReadFromFile(reader, isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            bounds.WriteToFile(writer);
        }

        public override string ToString()
        {
            return name.ToString();
        }

    }
}
