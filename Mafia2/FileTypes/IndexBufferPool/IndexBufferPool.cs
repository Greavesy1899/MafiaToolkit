using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mafia2
{
    public class IndexBufferManager
    {
        IndexBufferPool[] bufferPools;
        List<FileInfo> loadedPoolNames;

        public IndexBufferPool[] BufferPools {
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
        public IndexBufferManager(List<FileInfo> files)
        {
            loadedPoolNames = files;
            ReadFiles();
        }

        /// <summary>
        /// Search pool for buffer.
        /// </summary>
        /// <param name="indexRef"></param>
        /// <returns></returns>
        public BufferLocationStruct SearchBuffer(ulong indexRef)
        {
            for (int i = 0; i != bufferPools.Length; i++)
            {
                int c = 0;
                foreach (KeyValuePair<ulong, IndexBuffer> entry in bufferPools[i].Buffers)
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
        public void AddBuffer(IndexBuffer buffer)
        {
            for (int i = 0; i != bufferPools.Length; i++)
            {
                if (bufferPools[i].Buffers.Count != 128)
                {
                    bufferPools[i].Buffers.Add(buffer.Hash, buffer);
                    return;
                }
            }
        }

        /// <summary>
        /// Remove buffer if found.
        /// </summary>
        /// <param name="buffer"></param>
        public void RemoveBuffer(IndexBuffer buffer)
        {
            for (int i = 0; i != bufferPools.Length; i++)
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
            bufferPools = new IndexBufferPool[loadedPoolNames.Count];
            for (int i = 0; i != bufferPools.Length; i++)
            {
                using (BinaryReader reader = new BinaryReader(File.Open(loadedPoolNames[i].FullName, FileMode.Open)))
                    bufferPools[i] = new IndexBufferPool(reader);
            }
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
                foreach (KeyValuePair<ulong, IndexBuffer> entry in bufferPools[i].Buffers)
                {
                    if (entry.Key == indexRef)
                        return entry.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// writer pools to their files.
        /// </summary>
        public void WriteToFile()
        {
            for(int i = 0; i != bufferPools.Length; i++)
            {
                using(BinaryWriter writer = new BinaryWriter(File.Open(loadedPoolNames[i].FullName, FileMode.Create)))
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
        private Dictionary<ulong, IndexBuffer> buffers = new Dictionary<ulong, IndexBuffer>();

        public Dictionary<ulong, IndexBuffer> Buffers {
            get { return buffers; }
            set { buffers = value; }
        }

        /// <summary>
        /// Construct pool and read buffers.
        /// </summary>
        /// <param name="reader"></param>
        public IndexBufferPool(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        /// <summary>
        /// read all buffers from the file.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadFromFile(BinaryReader reader)
        {
            version = (BufferType)reader.ReadByte();
            numBuffers = reader.ReadInt32();
            size = reader.ReadInt32();

            for (int i = 0; i != numBuffers; i++)
            {
                IndexBuffer buffer = new IndexBuffer(reader);
                buffers.Add(buffer.Hash, buffer);
            }
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
            {
                int usage = buffers.ElementAt(i).Value.Data.Length*2;
                size += usage;
            }
            size += 128;
            writer.Write(size);

            for (int i = 0; i != buffers.Count; i++)
                buffers.ElementAt(i).Value.WriteToFile(writer);
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

        /// <summary>
        /// Construct a buffer with given hash.
        /// </summary>
        /// <param name="hash"></param>
        public IndexBuffer(ulong hash)
        {
            this.hash = hash;
            u = 1;
        }

        /// <summary>
        /// Construct buffer and read data.
        /// </summary>
        /// <param name="reader"></param>
        public IndexBuffer(BinaryReader reader)
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
        
        /// <summary>
        /// Write buffer to file.
        /// </summary>
        /// <param name="writer"></param>
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
