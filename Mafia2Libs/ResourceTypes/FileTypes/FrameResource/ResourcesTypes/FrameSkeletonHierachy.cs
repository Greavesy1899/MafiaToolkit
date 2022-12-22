using System;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.FrameResource
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

        public FrameSkeletonHierachy(FrameResource OwningResource) : base(OwningResource)
        {
            parentIndices = new byte[0];
            lastChildIndices = new byte[0];
            unkData = new byte[0];
        }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            int count = reader.ReadInt32(isBigEndian); //usually matches amount of nodes
            parentIndices = reader.ReadBytes(count); //[m_numRefs]; 0xFF == root;
            unkNum = reader.ReadByte8(); //always 0?
            lastChildIndices = reader.ReadBytes(count); //[m_numRefs]; last child index. If respective item has no child, the previouse value is copied [n-1], so the array is sorted in increase;
            unkData = reader.ReadBytes(count + 1); //[m_numRefs+1]; always [num+1, 1,2,3,...,num,0]
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(parentIndices.Length);
            writer.Write(parentIndices);
            writer.Write(unkNum);
            writer.Write(lastChildIndices);
            writer.Write(unkData);
        }

        public override string ToString()
        {
            return $"Skeleton Hierarchy Block";
        }
    }
}
