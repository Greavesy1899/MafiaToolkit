using System.IO;
using Utils.Types;

namespace ResourceTypes.ItemDesc
{
    public class CollisionBase
    {
        ulong hash;
        short colMaterial;
        Utils.Types.TransformMatrix matrix;
        byte unk_byte;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public TransformMatrix Matrix {
            get { return matrix; }
            set { matrix = value; }
        }

        public CollisionBase(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            colMaterial = reader.ReadInt16();
            matrix = new TransformMatrix(reader);
            unk_byte = reader.ReadByte();
        }
    }
}
