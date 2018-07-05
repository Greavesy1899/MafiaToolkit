using System.Collections.Generic;
using System.IO;
namespace Mafia2
{
    public class VertexBufferPool
    {
        //MAX BUFFER SIZE IS 128
        BufferType version;
        int numBuffers;
        int size;
        VertexBuffer[] buffers;

        private List<VertexBuffer[]> prebuffers = new List<VertexBuffer[]>();

        public VertexBuffer[] Buffers {
            get { return buffers; }
            set { buffers = value; }
        }

        public VertexBufferPool(List<FileInfo> files)
        {
            foreach (FileInfo file in files)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
                    ReadFromFile(reader);

                using (BinaryWriter writer = new BinaryWriter(File.Open(file.Name, FileMode.Create)))
                    WriteToFile(writer);
            }
            BuildBuffer();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            version = (BufferType)reader.ReadByte();
            numBuffers = reader.ReadInt32();
            size = reader.ReadInt32();

            buffers = new VertexBuffer[numBuffers];

            for (int i = 0; i != numBuffers; i++)
            {
                buffers[i] = new VertexBuffer(reader);
            }
            prebuffers.Add(buffers);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((byte)version);
            writer.Write(numBuffers);
            writer.Write(size);

            for (int i = 0; i != numBuffers; i++)
            {
                buffers[i].WriteToFile(writer);
            }
        }

        public void BuildBuffer()
        {
            int totalsize = 0;

            for (int i = 0; i != prebuffers.Count; i++)
                totalsize += prebuffers[i].Length;

            List<VertexBuffer> listBuffer = new List<VertexBuffer>();

            for (int i = 0; i != prebuffers.Count; i++)
            {
                for (int x = 0; x != prebuffers[i].Length; x++)
                {
                    listBuffer.Add(prebuffers[i][x]);
                }
            }

            Buffers = listBuffer.ToArray();
        }
    }

    public class VertexBuffer
    {
        ulong hash;
        int len;
        byte[] data;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public byte[] Data {
            get { return data; }
            set { data = value; }
        }

        public VertexBuffer(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            len = reader.ReadInt32();
            data = reader.ReadBytes(len);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(len);
            writer.Write(data);
        }
    }
}
