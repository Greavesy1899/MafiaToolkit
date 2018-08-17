using System.ComponentModel;
using System.IO;
using System.Text;

namespace Mafia2
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Hash
    {
        ulong hash;
        string _string;
        short size;

        [ReadOnly(true)]
        public ulong uHash {
            get { return hash; }
            set { hash = value; }
        }
        [ReadOnly(true)]
        public string String {
            get { return _string; }
            set { _string = value; }
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
            _string = Encoding.ASCII.GetString(reader.ReadBytes(size));
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(size);
            writer.Write(_string.ToCharArray());
        }

        public override string ToString()
        {
            return _string;
        }
    }
}
