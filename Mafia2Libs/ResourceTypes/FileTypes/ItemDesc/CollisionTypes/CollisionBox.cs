using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.ItemDesc
{
    public class CollisionBox
    {
        private Vector3 vector;

        public CollisionBox(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            vector = Vector3Utils.ReadFromFile(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }
    }
}
