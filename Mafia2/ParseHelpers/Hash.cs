using System.IO;
using System.Text;

namespace Mafia2 {
    public struct Hash {
        ulong hash;
        string name;


        public ulong uHash {
            get { return hash; }
            set { hash = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }

        public Hash(BinaryReader reader) {
            hash = 0;
            name = "";

            ReadFromFile(reader);
        }
        public void ReadFromFile(BinaryReader reader) {
            hash = reader.ReadUInt64();
            short num = reader.ReadInt16();
            name = Encoding.ASCII.GetString(reader.ReadBytes(num));
        }

        public override string ToString() {
            return string.Format("{0}", name);
        }
    }
}
