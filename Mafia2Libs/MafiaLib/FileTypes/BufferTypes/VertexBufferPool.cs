using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public VertexBufferManager(List<FileInfo> files)
        {
            loadedPoolNames = files;
            ReadFiles();
        }

        /// <summary>
        /// Get buffer from manager. use IndexBufferRef from FrameGeometry.
        /// </summary>
        /// <param name="indexRef">indexBufferRef</param>
        /// <returns></returns>
        public BufferLocationStruct SearchBuffer(ulong indexRef)
        {
            for (int i = 0; i != bufferPools.Count; i++)
            {
                int c = 0;
                foreach (KeyValuePair<ulong, VertexBuffer> entry in bufferPools[i].Buffers)
                {
                    if (entry.Key == indexRef)
                        return new BufferLocationStruct(i, c);
                    c++;
                }
            }
            return null;
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

        /// <summary>
        /// Remove buffer if found.
        /// </summary>
        /// <param name="buffer"></param>
        public void RemoveBuffer(VertexBuffer buffer)
        {
            for (int i = 0; i != bufferPools.Count; i++)
            {
                if (bufferPools[i].Buffers.Remove(buffer.Hash))
                    return;
            }
        }

        /// <summary>
        /// Read files which are passed through constructor.
        /// </summary>
        public void ReadFiles()
        {
            bufferPools = new List<VertexBufferPool>();
            for (int i = 0; i != loadedPoolNames.Count; i++)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(loadedPoolNames[i].FullName, FileMode.Open)))
                    bufferPools.Add(new VertexBufferPool(reader));
            }
        }

        /// <summary>
        /// Get buffer from manager. use VertexBufferRef from FrameGeometry.
        /// </summary>
        /// <param name="vertexRef">vertexBufferRef</param>
        /// <returns></returns>
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

        /// <summary>
        /// writer pools to their files.
        /// </summary>
        public void WriteToFile()
        {
            for (int i = 0; i != bufferPools.Count; i++)
            {
                if (loadedPoolNames.Count > i)
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(loadedPoolNames[i].FullName, FileMode.Create)))
                        bufferPools[i].WriteToFile(writer);
                }
                else
                {
                    int offset = 20 + i;
                    using (BinaryWriter writer = new BinaryWriter(File.Open(loadedPoolNames[0].DirectoryName + "/VertexBufferPool_" + offset + ".vbp", FileMode.Create)))
                        bufferPools[i].WriteToFile(writer);
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
        public VertexBufferPool(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public VertexBufferPool()
        {
            version = BufferType.Vertex;
            numBuffers = 0;
            size = 0;
            buffers = new Dictionary<ulong, VertexBuffer>();
        }

        /// <summary>
        /// read all buffers from the file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            int expectedSize = 0;

            version = (BufferType)reader.ReadByte();
            numBuffers = reader.ReadInt32();
            size = reader.ReadInt32();

            for (int i = 0; i != numBuffers; i++)
            {
                VertexBuffer buffer = new VertexBuffer(reader);
                expectedSize += buffer.Data.Length;
                buffers.Add(buffer.Hash, buffer);
            }

            Console.WriteLine("{0} AGAINST {1}", expectedSize, size);
        }

        /// <summary>
        /// Write all buffers to the file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            size = 0;

            writer.Write((byte)version);
            writer.Write(buffers.Count);

            //need to make sure we update total size of buffers.
            for (int i = 0; i != buffers.Count; i++)
                size += (buffers.ElementAt(i).Value.Data.Length);

            writer.Write(size);

            for (int i = 0; i != buffers.Count; i++)
                buffers.ElementAt(i).Value.WriteToFile(writer);
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

        /// <summary>
        /// Construct a buffer with given hash.
        /// </summary>
        /// <param name="hash"></param>
        public VertexBuffer(ulong hash)
        {
            this.hash = hash;
        }

        /// <summary>
        /// Construct pool and read buffers.
        /// </summary>
        /// <param name="reader"></param>
        public VertexBuffer(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// Read buffer to file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            len = reader.ReadInt32();
            data = reader.ReadBytes(len);
        }

        /// <summary>
        /// Write buffer to file.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(len);
            writer.Write(data);
        }
    }
}
