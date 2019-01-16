using System.IO;

namespace Mafia2
{
    public class CollisionBox
    {
        Vector3 vector;

        public CollisionBox(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            vector = new Vector3(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }
    }
}
