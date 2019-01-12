using System.ComponentModel;
using System.IO;

namespace Mafia2
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

        public FrameObjectDummy() : base()
        {
            bounds = new BoundingBox();
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            bounds = new BoundingBox(reader);
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
