using System.IO;
using Mafia2;

namespace Mafia2 {
    public class FrameObjectCollision : FrameObjectBase {

        ulong hash;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public FrameObjectCollision(BinaryReader reader) : base() {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader) {
            base.ReadFromFile(reader);
            hash = reader.ReadUInt64();
        }

        public override string ToString()
        {
            return string.Format("Collision Block");
        }
    }
}