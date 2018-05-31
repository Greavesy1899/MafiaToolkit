using System;
using System.IO;

namespace Mafia2
{
    public class FrameSkeletonHierachy
    {
        byte[] parentIndices;
        byte[] lastChildIndices;

        public byte[] ParentIndices {
            get { return parentIndices; }
            set { parentIndices = value; }
        }
        public byte[] LastChildIndices {
            get { return lastChildIndices; }
            set { lastChildIndices = value; }
        }

        public FrameSkeletonHierachy(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            parentIndices = reader.ReadBytes(count);
            int num = reader.ReadByte();
            lastChildIndices = reader.ReadBytes(count);
            reader.ReadBytes(count + 1);
        }
    }
}
