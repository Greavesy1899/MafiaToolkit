using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.Types;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectJoint : FrameObjectBase
    {

        byte dataSize;
        NodeStruct[] nodeData;

        [Browsable(false)]
        public byte DataSize {
            get { return dataSize; }
            set { dataSize = value; }
        }
        [Browsable(false)]
        public NodeStruct[] Data {
            get { return nodeData; }
            set { nodeData = value; }
        }

        public FrameObjectJoint(FrameResource OwningResource) : base(OwningResource)
        {
            dataSize = 0;
            nodeData = new NodeStruct[dataSize];

            for (int i = 0; i != dataSize; i++)
            {
                nodeData[i] = new NodeStruct();
            }
        }

        public FrameObjectJoint(FrameObjectJoint other) : base(other)
        {
            dataSize = other.dataSize;
            nodeData = other.nodeData;
        }

        public override void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            base.ReadFromFile(reader, isBigEndian);
            dataSize = reader.ReadByte8();
            nodeData = new NodeStruct[dataSize];

            for (int i = 0; i != dataSize; i++)
            {
                nodeData[i] = new NodeStruct(reader, isBigEndian);
            }
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(dataSize);

            for (int i = 0; i != dataSize; i++)
            {
                nodeData[i].WriteToFile(writer);
            }
        }

        public override string ToString()
        {
            return name.ToString();
        }

        public struct NodeStruct
        {

            int unk1;
            HashName unk2;
            HashName unk3;

            public int Unk_01 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public HashName Unk_02_Hash {
                get { return unk2; }
                set { unk2 = value; }
            }
            public HashName Unk_03_Hash {
                get { return unk3; }
                set { unk3 = value; }
            }

            public NodeStruct(MemoryStream reader, bool isBigEndian)
            {
                unk1 = reader.ReadInt32(isBigEndian);
                unk2 = new HashName(reader, isBigEndian);
                unk3 = new HashName(reader, isBigEndian);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(unk1);
                unk2.WriteToFile(writer);
                unk3.WriteToFile(writer);
            }
        }

    }

}
