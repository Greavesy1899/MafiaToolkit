using System.Collections.Generic;
using System.IO;
namespace Mafia2
{
    public class VertexBufferManager
    {
        VertexBufferPool[] bufferPools;

        public VertexBufferPool[] BufferPools {
            get { return bufferPools; }
            set { bufferPools = value; }
        }

        public VertexBufferManager(List<FileInfo> files)
        {
            bufferPools = new VertexBufferPool[files.Count];
            int i = 0;
            foreach (FileInfo file in files)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
                    bufferPools[i] = new VertexBufferPool(reader);

                i++;
            }
        }

        public BufferLocationStruct SearchBuffer(ulong indexRef)
        {
            for (int i = 0; i != bufferPools.Length; i++)
            {
                for (int c = 0; c != bufferPools[i].Buffers.Length; c++)
                {
                    if (indexRef == bufferPools[i].Buffers[c].Hash)
                        return new BufferLocationStruct(i, c);
                }
            }
            return null;
        }

        /// <summary>
        /// Get buffer from manager. use VertexBufferRef from FrameGeometry.
        /// </summary>
        /// <param name="vertexRef">vertexBufferRef</param>
        /// <returns></returns>
        public VertexBuffer GetBuffer(ulong vertexRef)
        {
            for (int i = 0; i != bufferPools.Length; i++)
            {
                for (int c = 0; c != bufferPools[i].Buffers.Length; c++)
                {
                    if (vertexRef == bufferPools[i].Buffers[c].Hash)
                        return bufferPools[i].Buffers[c];
                }
            }
            return null;
        }

        public void WriteToFile()
        {
            for (int i = 0; i != bufferPools.Length; i++)
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open("VertexBufferPool_" + i + ".bin", FileMode.Create)))
                    bufferPools[i].WriteToFile(writer);
            }
        }
    }

    public class VertexBufferPool
    {
        //MAX BUFFER SIZE IS 128
        BufferType version;
        int numBuffers;
        int size;
        VertexBuffer[] buffers;

        public VertexBuffer[] Buffers {
            get { return buffers; }
            set { buffers = value; }
        }

        public VertexBufferPool(BinaryReader reader)
        {
            ReadFromFile(reader);
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

        public int SearchBuffer(ulong vertexRef)
        {
            for(int i = 0; i != Buffers.Length; i++)
            {
                if (vertexRef == Buffers[i].Hash)
                    return i;
            }

            return -1;
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
            set {
                data = value;
                len = data.Length;
            }
        }

        public VertexBuffer(ulong hash)
        {
            this.hash = hash;
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
