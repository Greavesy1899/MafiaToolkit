using System.IO;

namespace Mafia2
{
    public class FrameObjectJoint : FrameObjectBase
    {

        byte dataSize;
        NodeStruct[] nodeData;

        public byte DataSize {
            get { return dataSize; }
            set { dataSize = value; }
        }
        public NodeStruct[] Data {
            get { return nodeData; }
            set { nodeData = value; }
        }

        public FrameObjectJoint() : base()
        {

        }

        public override void CreateBasic()
        {
            base.CreateBasic();
            dataSize = 0;
            nodeData = new NodeStruct[dataSize];

            for (int i = 0; i != dataSize; i++)
                nodeData[i] = new NodeStruct();
        }

        public FrameObjectJoint(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            base.ReadFromFile(reader);
            dataSize = reader.ReadByte();
            nodeData = new NodeStruct[dataSize];

            for (int i = 0; i != dataSize; i++)
                nodeData[i] = new NodeStruct(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            base.WriteToFile(writer);
            writer.Write(dataSize);

            for (int i = 0; i != dataSize; i++)
                nodeData[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(name.String) ? $"Joint" : name.String;
        }

        public struct NodeStruct
        {

            int unk1;
            Hash unk2;
            Hash unk3;

            public int Unk_01 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public Hash Unk_02_Hash {
                get { return unk2; }
                set { unk2 = value; }
            }
            public Hash Unk_03_Hash {
                get { return unk3; }
                set { unk3 = value; }
            }

            public NodeStruct(BinaryReader reader)
            {
                unk1 = reader.ReadInt32();
                unk2 = new Hash(reader);
                unk3 = new Hash(reader);
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
