using System.ComponentModel;
using System.IO;

namespace Mafia2
{
    public class FrameObjectDummy : FrameObjectJoint
    {
        Bounds unk_19_bounds;

        public Bounds Unk19 {
            get { return unk_19_bounds; }
            set { unk_19_bounds = value; }
        }

        public FrameObjectDummy(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            unk_19_bounds = new Bounds(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            unk_19_bounds.WriteToFile(writer);
        }
    }
}
