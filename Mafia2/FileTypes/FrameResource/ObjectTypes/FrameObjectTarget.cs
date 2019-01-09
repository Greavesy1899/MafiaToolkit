using System;
using System.IO;

namespace Mafia2
{
    public class FrameObjectTarget : FrameObjectJoint
    {

        public FrameObjectTarget(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
