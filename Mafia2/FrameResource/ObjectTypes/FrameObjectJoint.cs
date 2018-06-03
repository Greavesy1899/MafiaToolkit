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
            {
                nodeData[i] = new NodeStruct(reader);
            }
        }

        public override string ToString()
        {
            return string.Format("Joint Block");
        }

        public struct NodeStruct
        {

            int unknown_01;
            Hash unknown_02_hash;
            Hash unknown_03_hash;

            public int Unk_01 {
                get { return unknown_01; }
                set { unknown_01 = value; }
            }
            public Hash Unk_02_Hash {
                get { return unknown_02_hash; }
                set { unknown_02_hash = value; }
            }
            public Hash Unk_03_Hash {
                get { return unknown_03_hash; }
                set { unknown_03_hash = value; }
            }

            public NodeStruct(BinaryReader reader)
            {
                unknown_01 = reader.ReadInt32();
                unknown_02_hash = new Hash(reader);
                unknown_03_hash = new Hash(reader);
            }
        }

    }

}
