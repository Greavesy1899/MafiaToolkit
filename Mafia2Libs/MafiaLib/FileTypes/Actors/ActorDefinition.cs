using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTypes.Actors
{
    public class ActorDefinition
    {
        ulong hash; //hash, this is the same as in the frame.
        ushort unk01; //always zero
        ushort namePos; //starting position for the name.
        uint frameIndex; //links to FrameResource
        string name;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public uint FrameIndex {
            get { return frameIndex; }
            set { frameIndex = value; }
        }
        public ushort NamePos {
            get { return namePos; }
            set { namePos = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }

        public ActorDefinition()
        {
            hash = 0;
            unk01 = 0;
            namePos = 0;
            frameIndex = 0;
            name = "NewDefintion";
        }

        public ActorDefinition(BinaryReader reader)
        {
            hash = 0;
            unk01 = 0;
            namePos = 0;
            frameIndex = 0;
            name = "";
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            unk01 = reader.ReadUInt16();
            namePos = reader.ReadUInt16();
            frameIndex = reader.ReadUInt32();
            name = "";
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(unk01);
            writer.Write(namePos);
            writer.Write(frameIndex);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", hash, name);
        }
    }
}
