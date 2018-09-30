using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    public class FrameObjectDummy : FrameObjectJoint
    {
        BoundingBox unk19;

        public BoundingBox Unk19 {
            get { return unk19; }
            set { unk19 = value; }
        }

        public FrameObjectDummy(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk19 = new BoundingBox(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            unk19.WriteToFile(writer);
        }

        public override string ToString()
        {
            return name.String;
        }

    }
}
