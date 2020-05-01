using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Extensions;

namespace ResourceTypes.BufferPools
{
    public class IndexBufferManager
    {
        List<IndexBufferPool> bufferPools;
        List<FileInfo> loadedPoolNames;

        public List<IndexBufferPool> BufferPools {
            get { return bufferPools; }
            set { bufferPools = value; }
        }
        public List<FileInfo> LoadedPoolNames {
            get { return loadedPoolNames; }
            set { loadedPoolNames = value; }
        }

        public IndexBufferManager(List<FileInfo> files, bool isBigEndian = false)
        {
            loadedPoolNames = files;
            ReadFiles(isBigEndian);
        }

        public void AddBuffer(IndexBuffer buffer)
        {
            int poolToInput = -1;

            for (int i = 0; i != bufferPools.Count; i++)
            {
                if (bufferPools[i].Buffers.Count != 128)
                {
                    poolToInput = i;
                }
            }

            if(poolToInput == -1)
            {
                bufferPools.Add(new IndexBufferPool());
                bufferPools[bufferPools.Count-1].Buffers.Add(buffer.Hash, buffer);
            }
            else
            {
                bufferPools[poolToInput].Buffers.Add(buffer.Hash, buffer);
            }
        }

        public bool HasBuffer(IndexBuffer buffer)
        {
            for (int i = 0; i < bufferPools.Count; i++)
            {
                if (bufferPools[i].Buffers.ContainsKey(buffer.Hash))
                {
                    return true;
                }
            }

            return false;
        }

        public bool RemoveBuffer(IndexBuffer buffer)
        {
            for (int i = 0; i < bufferPools.Count; i++)
            {
                if (bufferPools[i].Buffers.Remove(buffer.Hash))
                {
                    return true;
                }
            }
            return false;
        }

        public void ReadFiles(bool isBigEndian)
        {
            bufferPools = new List<IndexBufferPool>();
            for (int i = 0; i != loadedPoolNames.Count; i++)
            {
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(loadedPoolNames[i].FullName), false))
                {
                    bufferPools.Add(new IndexBufferPool(stream, isBigEndian));
                }
            }
        }

        public IndexBuffer GetBuffer(ulong indexRef)
        {
            IndexBuffer buff = null;
            for (int i = 0; i != bufferPools.Count; i++)
            {            
                if (bufferPools[i].Buffers.TryGetValue(indexRef, out buff))
                {
                    return buff;
                }
            }
            return buff;
        }

        public void WriteToFile(bool isBigEndian = false)
        {
            for(int i = 0; i != bufferPools.Count; i++)
            {
                if (loadedPoolNames.Count > i)
                {
                    using(MemoryStream stream = new MemoryStream())
                    {
                        bufferPools[i].WriteToFile(stream, isBigEndian);
                        File.WriteAllBytes(loadedPoolNames[i].FullName, stream.ToArray());
                    }
                }
                else
                {
                    int offset = 20 + i;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bufferPools[i].WriteToFile(stream, isBigEndian);
                        File.WriteAllBytes(loadedPoolNames[0].DirectoryName + "/IndexBufferPool_" + offset + ".ibp", stream.ToArray());
                    }
                }
            }
        }
    }

    public class IndexBufferPool
    {
        //MAX BUFFER SIZE IS 128
        private BufferType version;
        private int numBuffers;
        private uint size;
        private Dictionary<ulong, IndexBuffer> buffers = new Dictionary<ulong, IndexBuffer>();

        public Dictionary<ulong, IndexBuffer> Buffers {
            get { return buffers; }
            set { buffers = value; }
        }

        public IndexBufferPool(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public IndexBufferPool()
        {
            version = BufferType.Index;
            numBuffers = 0;
            size = 0;
            buffers = new Dictionary<ulong, IndexBuffer>();
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            version = (BufferType)stream.ReadByte8();
            numBuffers = stream.ReadInt32(isBigEndian);
            size = stream.ReadUInt32(isBigEndian);

            for (int i = 0; i != numBuffers; i++)
            {
                IndexBuffer buffer = new IndexBuffer(stream, isBigEndian);
                buffers.Add(buffer.Hash, buffer);
            }
        }

        public void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            size = 0;

            stream.WriteByte((byte)version);
            stream.Write(buffers.Count, isBigEndian);

            //need to make sure we update total size of buffers.
            for (int i = 0; i != buffers.Count; i++)
            {
                uint usage = (uint)buffers.ElementAt(i).Value.Data.Length*2;
                size += usage;
            }
            size += 128;
            stream.Write(size, isBigEndian);

            for (int i = 0; i != buffers.Count; i++)
            {
                buffers.ElementAt(i).Value.WriteToFile(stream, isBigEndian);
            }
        }
    }

    public class IndexBuffer
    {
        private ulong hash;
        private int unk0;
        private uint length;
        private ushort[] data;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public ushort[] Data {
            get { return data; }
            set {
                data = value;
                length = (uint)(data.Length * 2);
            }
        }

        public IndexBuffer(ulong hash)
        {
            this.hash = hash;
        }

        public IndexBuffer(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            hash = stream.ReadUInt64(isBigEndian);
            unk0 = stream.ReadInt32(isBigEndian);
            length = stream.ReadUInt32(isBigEndian);
            data = new ushort[length / 2];

            int num = 0;
            int index = 0;

            while (num < length)
            {
                data[index] = stream.ReadUInt16(isBigEndian);
                num += 2;
                ++index;
            }
        }
        
        public void WriteToFile(MemoryStream stream, bool isBigEndian = false)
        {
            stream.Write(hash, isBigEndian);
            stream.Write(unk0, isBigEndian);
            stream.Write(length, isBigEndian);

            for (int i = 0; i != data.Length; i++)
            {
                stream.Write(data[i], isBigEndian);
            }
        }
    }
}
