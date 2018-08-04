using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class IndexBufferManager
    {
        IndexBufferPool[] bufferPools;

        public IndexBufferPool[] BufferPools {
            get { return bufferPools; }
            set { bufferPools = value; }
        }

        public IndexBufferManager(List<FileInfo> files)
        {
            bufferPools = new IndexBufferPool[files.Count];
            int i = 0;
            foreach (FileInfo file in files)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
                    bufferPools[i] = new IndexBufferPool(reader);

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
        /// Get buffer from manager. use IndexBufferRef from FrameGeometry.
        /// </summary>
        /// <param name="indexRef">indexBufferRef</param>
        /// <returns></returns>
        public IndexBuffer GetBuffer(ulong indexRef)
        {
            for (int i = 0; i != bufferPools.Length; i++)
            {
                for (int c = 0; c != bufferPools[i].Buffers.Length; c++)
                {
                    if (indexRef == bufferPools[i].Buffers[c].Hash)
                        return bufferPools[i].Buffers[c];
                }
            }
            return null;
        }

        public void WriteToFile()
        {
            for(int i = 0; i != bufferPools.Length; i++)
            {
                using(BinaryWriter writer = new BinaryWriter(File.Open("IndexBufferPool_"+i+".bin", FileMode.Create)))
                    bufferPools[i].WriteToFile(writer);
            }
        }
    }

    public class IndexBufferPool
    {
        //MAX BUFFER SIZE IS 128
        private BufferType version;
        private int numBuffers;
        private int size;
        private IndexBuffer[] buffers;

        public IndexBuffer[] Buffers {
            get { return buffers; }
            set { buffers = value; }
        }

        public IndexBufferPool(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            version = (BufferType)reader.ReadByte();
            numBuffers = reader.ReadInt32();
            size = reader.ReadInt32();

            buffers = new IndexBuffer[numBuffers];

            for (int i = 0; i != numBuffers; i++)
            {
                buffers[i] = new IndexBuffer(reader);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((byte)version);
            writer.Write(numBuffers);
            writer.Write(size);

            for (int i = 0; i != buffers.Length; i++)
            {
                buffers[i].WriteToFile(writer);
            }
        }
    }

    public class IndexBuffer
    {
        private ulong hash;
        private int u;
        private int len;
        private ushort[] data;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public ushort[] Data {
            get { return data; }
            set {
                data = value;
                len = (data.Length * 2);
            }
        }

        public IndexBuffer(ulong hash)
        {
            this.hash = hash;
            u = 1;
        }
        public IndexBuffer(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            u = reader.ReadInt32();
            len = reader.ReadInt32();
            data = new ushort[len / 2];

            int num = 0;
            int index = 0;

            while (num < len)
            {
                data[index] = reader.ReadUInt16();
                num += 2;
                ++index;
            }
        }
        
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(u);
            writer.Write(len);

            for(int i = 0; i != data.Length; i++)
            {
                writer.Write(data[i]);
            }
        }
    }
}
