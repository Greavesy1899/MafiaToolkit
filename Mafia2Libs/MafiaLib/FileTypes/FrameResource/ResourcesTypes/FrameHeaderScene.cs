using System.Collections.Generic;
using System.IO;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameHeaderScene : FrameEntry
    {
        Hash name;

        List<FrameObjectBase> children = new List<FrameObjectBase>();

        public List<FrameObjectBase> Children {
            get { return children; }
            set { children = value; }
        }

        public Hash Name {
            get { return name; }
            set { name = value; }
        }

        public FrameHeaderScene() : base() { }
        public FrameHeaderScene(Hash name) : base()
        {
            this.name = name;

        }
        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            name = new Hash(reader, isBigEndian);
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
