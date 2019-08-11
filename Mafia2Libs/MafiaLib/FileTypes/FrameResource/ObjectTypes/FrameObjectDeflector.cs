using System.IO;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectDeflector : FrameObjectJoint
    {
        public FrameObjectDeflector() : base()
        {

        }

        public FrameObjectDeflector(FrameObjectDeflector other) : base(other)
        {

        }

        public FrameObjectDeflector(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
        }
    }

}
