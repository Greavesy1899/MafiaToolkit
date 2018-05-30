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
            short num = reader.ReadInt16();
            name = Encoding.ASCII.GetString(reader.ReadBytes(num));
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", name, hash);
        }
    }
}
