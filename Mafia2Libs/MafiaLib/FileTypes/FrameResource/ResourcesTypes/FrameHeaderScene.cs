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

        public FrameHeaderScene() : base() { }
        public FrameHeaderScene(Hash name) : base()
        {
            this.name = name;

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
