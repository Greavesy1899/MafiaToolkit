using System;
using System.IO;

namespace Mafia2
{
    public class FrameSkeletonHierachy : FrameEntry
    {
        byte[] parentIndices;
        byte[] lastChildIndices;
        byte unkNum;
        byte[] unkData;

        public byte[] ParentIndices {
            get { return parentIndices; }
            set { parentIndices = value; }
        }
        public byte[] LastChildIndices {
            get { return lastChildIndices; }
            set { lastChildIndices = value; }
        }
        public byte Unk01 {
            get { return unkNum; }
            set { unkNum = value; }
        }
        public byte[] UnkData {
            get { return unkData; }
            set { unkData = value; }
        }

        public FrameSkeletonHierachy(BinaryReader reader) : base()
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            parentIndices = reader.ReadBytes(count);
            unkNum = reader.ReadByte();
            lastChildIndices = reader.ReadBytes(count);
            unkData = reader.ReadBytes(count + 1);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(parentIndices.Length);
            writer.Write(parentIndices);
            writer.Write(unkNum);
            writer.Write(lastChildIndices);
            writer.Write(unkData);
        }
    }
}
