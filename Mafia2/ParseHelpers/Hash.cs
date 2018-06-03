using System.ComponentModel;
using System.IO;
using System.Text;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Hash
    {
        ulong hash;
        string name;
        short size;

        [ReadOnly(true)]
        public ulong uHash {
            get { return hash; }
            set { hash = value; }
        }
        [ReadOnly(true)]
        public string Name {
            get { return name; }
            set { name = value; }
        }
        public Hash() { }
        public Hash(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            size = reader.ReadInt16();
            name = Encoding.ASCII.GetString(reader.ReadBytes(size));
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", name, hash);
        }
    }
}
