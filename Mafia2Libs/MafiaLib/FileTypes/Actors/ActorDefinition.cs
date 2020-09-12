using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Actors
{
    public class ActorDefinition
    {
        ulong frameNameHash; //hash, this is the same as in the frame.
        ushort unk01; //always zero
        ushort namePos; //starting position for the name.
        uint frameIndex; //links to FrameResource
        string name;

        public ulong FrameNameHash {
            get { return frameNameHash; }
            set { frameNameHash = value; }
        }
        public uint FrameIndex {
            get { return frameIndex; }
            set { frameIndex = value; }
        }
        [Browsable(false)]
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
            frameNameHash = 0;
            unk01 = 0;
            namePos = 0;
            frameIndex = 0;
            name = "NewDefintion";
        }

        public ActorDefinition(BinaryReader reader)
        {
            frameNameHash = 0;
            unk01 = 0;
            namePos = 0;
            frameIndex = 0;
            name = "";
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            frameNameHash = reader.ReadUInt64();
            unk01 = reader.ReadUInt16();
            namePos = reader.ReadUInt16();
            frameIndex = reader.ReadUInt32();
            name = "";
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(frameNameHash);
            writer.Write(unk01);
            writer.Write(namePos);
            writer.Write(frameIndex);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", frameNameHash, name);
        }
    }
}
