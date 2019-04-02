using System.ComponentModel;
using System.IO;
using Mafia2;
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

        public FrameObjectDummy(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public FrameObjectDummy(FrameObjectDummy other) : base(other)
        {
            bounds = other.bounds;
        }

        public FrameObjectDummy() : base()
        {
            bounds = new BoundingBox();
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            bounds = BoundingBoxExtenders.ReadFromFile(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            bounds.WriteToFile(writer);
        }

        public override string ToString()
        {
            return name.String;
        }

    }
}
