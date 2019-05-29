using System.IO;

namespace ResourceTypes.ItemDesc
{
    public class CollisionSphere
    {
        float radius;

        public CollisionSphere(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            radius = reader.ReadSingle();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(radius);
        }
    }
}
