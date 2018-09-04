using System;
using System.IO;

namespace Mafia2
{
    public class TAPIndices
    {
        private TAP0Chunk tapChunk;
        private UAP0Chunk uapChunk;
        private VAP0Chunk vapChunk;

        public TAPIndices(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            tapChunk = new TAP0Chunk(reader);

            if (!tapChunk.Valid)
                return;

            uapChunk = new UAP0Chunk(reader);

            if (!uapChunk.Valid)
                return;

            vapChunk = new VAP0Chunk(reader);

            if (!vapChunk.Valid)
                return;
        }

        public class TAP0Chunk
        {
            private int signature = 0x30504154;
            private int chunkSize;
            private bool valid;

            public int ChunkSize {
                get { return chunkSize; }
                set { chunkSize = value; }
            }
            public bool Valid {
                get { return valid; }
            }

            public TAP0Chunk(BinaryReader reader)
            {
                if (reader.ReadInt32() == signature)
                {
                    chunkSize = reader.ReadInt32();
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
        }

        public class UAP0Chunk
        {
            private int signature = 0x30504155;
            private int chunkSize;
            private int unk0;
            private bool valid;

            public int ChunkSize {
                get { return chunkSize; }
                set { chunkSize = value; }
            }
            public int Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public bool Valid {
                get { return valid; }
            }

            public UAP0Chunk(BinaryReader reader)
            {
                if (reader.ReadInt32() == signature)
                {
                    chunkSize = reader.ReadInt32();
                    unk0 = reader.ReadInt32();
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
        }

        public class VAP0Chunk
        {
            private int signature = 0x30504156;
            private int chunkSize;
            private int unk0size;
            private int[] unk0;
            private bool valid;

            public int ChunkSize {
                get { return chunkSize; }
                set { chunkSize = value; }
            }
            public int Unk0Size {
                get { return unk0size; }
                set { unk0size = value; }
            }
            public int[] Unk0 {
                get { return unk0; }
                set { unk0 = value; }
            }
            public bool Valid {
                get { return valid; }
            }

            public VAP0Chunk(BinaryReader reader)
            {
                if (reader.ReadInt32() == signature)
                {
                    chunkSize = reader.ReadInt32();
                    unk0size = reader.ReadInt32();
                    unk0 = new int[unk0size * 3];

                    for (int i = 0; i != unk0.Length; i++)
                    {
                        unk0[i] = reader.ReadInt32();
                    }
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
        }
    }
}
