using System.Collections.Generic;
using System.IO;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameHeaderScene : FrameEntry
    {
        HashName name;

        List<FrameObjectBase> children = new List<FrameObjectBase>();

        public List<FrameObjectBase> Children {
            get { return children; }
            set { children = value; }
        }

        public HashName Name {
            get { return name; }
            set { name = value; }
        }

        public FrameHeaderScene(FrameResource OwningResource) : base(OwningResource) { }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            name = new HashName(reader, isBigEndian);
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
