using System.IO;

namespace Mafia2
{
    public class FrameHeaderScene : FrameEntry
    {
        Hash name;

        public Hash Name {
            get { return name; }
            set { name = value; }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            name = new Hash(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            name.WriteToFile(writer);
        }

        public override string ToString()
        {
            return Name.String;
        }
    }
}
