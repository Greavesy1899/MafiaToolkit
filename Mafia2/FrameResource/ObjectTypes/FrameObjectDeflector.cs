using System.IO;

namespace Mafia2
{
    public class FrameObjectDeflector : FrameObjectJoint
    {
        public FrameObjectDeflector() : base()
        {

        }

        public FrameObjectDeflector(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
        }
    }

}
