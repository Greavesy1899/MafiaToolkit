using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Extensions;

namespace ResourceTypes.BufferPools
{
    public class VertexBufferManager
    {
        List<VertexBufferPool> bufferPools;
        List<FileInfo> loadedPoolNames;

        public List<VertexBufferPool> BufferPools {
            get { return bufferPools; }
            set { bufferPools = value; }
        }
        public List<FileInfo> LoadedPoolNames {
            get { return loadedPoolNames; }
            set { loadedPoolNames = value; }
        }

        /// <summary>
        /// Construct manager with passed files.
        /// </summary>
        /// <param name="files"></param>
        public VertexBufferManager(List<FileInfo> files, bool isBigEndian = false)
        {
            loadedPoolNames = files;
            ReadFiles(isBigEndian);
        }

        /// <summary>
        /// Add new buffer to first non-full pool.
        /// </summary>
        /// <param name="buffer"></param>
        public void AddBuffer(VertexBuffer buffer)
        {
            int poolToInput = -1;

            for (int i = 0; i != bufferPools.Count; i++)
            {
                if (bufferPools[i].Buffers.Count != 128)
                {
                    poolToInput = i;
                }
            }

            if (poolToInput == -1)
            {
                bufferPools.Add(new VertexBufferPool());
                bufferPools[bufferPools.Count - 1].Buffers.Add(buffer.Hash, buffer);
            }
            else
            {
                bufferPools[poolToInput].Buffers.Add(buffer.Hash, buffer);
            }
        }

        public bool HasBuffer(VertexBuffer buffer)
        {
            for (int i = 0; i < bufferPools.Count; i++)
            {
                if (bufferPools[i].Buffers.ContainsValue(buffer))
                {
                    return true;
                }
            }

            return false;
        }

        public bool RemoveBuffer(VertexBuffer buffer)
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
            bufferPools = new List<VertexBufferPool>();
            for (int i = 0; i != loadedPoolNames.Count; i++)
            {
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(loadedPoolNames[i].FullName), false))
                {
                    bufferPools.Add(new VertexBufferPool(stream, isBigEndian));
                }
            }
        }

        public VertexBuffer GetBuffer(ulong vertexRef)
        {
            for (int i = 0; i != bufferPools.Count; i++)
            {
                VertexBuffer buff;
                if (bufferPools[i].Buffers.TryGetValue(vertexRef, out buff))
                    return buff;
            }
            return null;
        }

        public void WriteToFile(bool isBigEndian = false)
        {
            for (int i = 0; i != bufferPools.Count; i++)
            {
                if (loadedPoolNames.Count > i)
                {
                    using (MemoryStream stream = new MemoryStream())
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
                        File.WriteAllBytes(loadedPoolNames[0].DirectoryName + "/VertexBufferPool_" + offset + ".vbp", stream.ToArray());
                    }
                }
            }
        }
    }

    public class VertexBufferPool
    {
        //MAX BUFFER SIZE IS 128
        BufferType version;
        int numBuffers;
        int size;
        Dictionary<ulong, VertexBuffer> buffers = new Dictionary<ulong, VertexBuffer>();

        public Dictionary<ulong, VertexBuffer> Buffers {
            get { return buffers; }
            set { buffers = value; }
        }

        /// <summary>
        /// Construct pool and read buffers.
        /// </summary>
        /// <param name="reader"></param>
        public VertexBufferPool(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public VertexBufferPool()
        {
            version = BufferType.Vertex;
            numBuffers = 0;
            size = 0;
            buffers = new Dictionary<ulong, VertexBuffer>();
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            int expectedSize = 0;

            version = (BufferType)stream.ReadByte();
            numBuffers = stream.ReadInt32(isBigEndian);
            size = stream.ReadInt32(isBigEndian);

            for (int i = 0; i != numBuffers; i++)
            {
                VertexBuffer buffer = new VertexBuffer(stream, isBigEndian);
                expectedSize += buffer.Data.Length;
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
                size += (buffers.ElementAt(i).Value.Data.Length);
            }

            stream.Write(size, isBigEndian);

            for (int i = 0; i != buffers.Count; i++)
            {
                buffers.ElementAt(i).Value.WriteToFile(stream, isBigEndian);
            }
        }
    }

    public class VertexBuffer
    {
        ulong hash;
        uint length;
        byte[] data;

        public ulong Hash {
            get { return hash; }
            set { hash = value; }
        }
        public byte[] Data {
            get { return data; }
            set {
                data = value;
                length = (uint)data.Length;
            }
        }

        public VertexBuffer(ulong hash)
        {
            this.hash = hash;
        }

        public VertexBuffer(MemoryStream stream, bool isBigEndian)
        {
            ReadFromFile(stream, isBigEndian);
        }

        public void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            hash = stream.ReadUInt64(isBigEndian);
            length = stream.ReadUInt32(isBigEndian);
            data = stream.ReadBytes((int)length);
        }

        public void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            stream.Write(hash, isBigEndian);
            stream.Write(length, isBigEndian);
            stream.Write(data);
        }
    }
}
