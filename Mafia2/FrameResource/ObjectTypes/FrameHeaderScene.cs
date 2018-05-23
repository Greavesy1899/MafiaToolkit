using System.IO;

namespace Mafia2
{
    public class FrameHeaderScene
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

        public override string ToString()
        {
            return string.Format("Header Block");
        }
    }
}
