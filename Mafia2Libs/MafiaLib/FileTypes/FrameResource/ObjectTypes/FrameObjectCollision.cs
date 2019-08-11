using System.IO;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectCollision : FrameObjectBase
    {

        ulong hash;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }

        public FrameObjectCollision(MemoryStream reader, bool isBigEndian) : base()
        {
            ReadFromFile(reader, isBigEndian);
        }

        public FrameObjectCollision() : base()
        {
            hash = 0;
        }

        public FrameObjectCollision(FrameObjectCollision other) : base(other)
        {
            hash = other.hash;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            hash = reader.ReadUInt64(isBigEndian);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(hash);
        }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}