using System.IO;

namespace Mafia2 {
    public class FrameObjectFrame : FrameObjectJoint {
        Hash unk_19_hash;

        public FrameObjectFrame(BinaryReader reader) : base() {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader) {
            base.ReadFromFile(reader);
            unk_19_hash = new Hash(reader);
        }
    }
}
