using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Extensions;

namespace ResourceTypes.BufferPools
{
    public class VertexBufferManager
    {
        List<FileInfo> loadedPoolNames;
        Dictionary<ulong, VertexBuffer> buffers;
        DirectoryInfo root;

        public Dictionary<ulong, VertexBuffer> Buffers {
            get { return buffers; }
        }

        public List<FileInfo> LoadedPoolNames {
            get { return loadedPoolNames; }
        }

        public VertexBufferManager(List<FileInfo> files, bool isBigEndian = false)
        {
            loadedPoolNames = files;
            buffers = new Dictionary<ulong, VertexBuffer>();
            if (LoadedPoolNames.Count > 0)
            {
                root = loadedPoolNames[0].Directory;
            }
            ReadFiles(isBigEndian);
        }

        public bool AddBuffer(VertexBuffer buffer)
        {
            if (!HasBuffer(buffer))
            {
                buffers.Add(buffer.Hash, buffer);
                return true;
            }
            return false;
        }

        public bool HasBuffer(ulong hash)
        {
            return buffers.ContainsKey(hash);
        }

        public bool HasBuffer(VertexBuffer buffer)
        {
            return HasBuffer(buffer.Hash);
        }

        public bool RemoveBuffer(VertexBuffer buffer)
        {
            if (HasBuffer(buffer))
            {
                buffers.Remove(buffer.Hash);
                return true;
            }
            return false;
        }

        public bool RemoveBuffer(ulong hash)
        {
            if (HasBuffer(hash))
            {
                buffers.Remove(hash);
                return true;
            }
            return false;
        }

        public bool ReplaceBuffer(VertexBuffer buffer)
        {
            if (HasBuffer(buffer))
            {
                buffers[buffer.Hash] = buffer;
                return true;
            }
            return false;
        }

        public void ReadFiles(bool isBigEndian)
        {
            for (int i = 0; i != loadedPoolNames.Count; i++)
            {
                VertexBufferPool pool = null;
                using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(loadedPoolNames[i].FullName), false))
                {
                    pool = new VertexBufferPool(stream, isBigEndian);
                }

                foreach (var buff in pool.Buffers)
                {
                    if (!buffers.ContainsKey(buff.Key))
                    {
                        buffers.Add(buff.Key, buff.Value);
                    }
                    else
                    {
                        Console.WriteLine("Skipped a buffer {0}", buff.Key);
                    }
                }
            }
        }

        public VertexBuffer GetBuffer(ulong vertexRef)
        {
            return buffers.ContainsKey(vertexRef) ? buffers[vertexRef] : null;
        }

        public void WriteToFile(bool isBigEndian = false)
        {
            int numPool = 0;
            int poolSize = 0;
            VertexBufferPool pool = new VertexBufferPool();

            foreach (var loaded in loadedPoolNames)
            {
                File.Delete(loaded.FullName);
            }
            loadedPoolNames.Clear();
            var buffArray = buffers.Values.ToArray();
            //var sorted = buffArray.OrderBy(buff => buff.Data.Length);
            foreach (var buffer in buffArray)
            {
                int prePoolSize = (poolSize) + (buffer.Data.Length);
                if (pool.Buffers.Count == 128 || prePoolSize > 20900000)
                {
                    string name = Path.Combine(root.FullName, string.Format("VertexBufferPool_{0}.vbp", numPool));
                    SavePool(pool, name, isBigEndian);
                    pool = new VertexBufferPool();
                    numPool++;
                    prePoolSize = 0;
                }

                pool.Buffers.Add(buffer.Hash, buffer);
                poolSize = prePoolSize;
            }

            if (pool != null)
            {
                string name = Path.Combine(root.FullName, string.Format("VertexBufferPool_{0}.vbp", numPool));
                SavePool(pool, name, isBigEndian);
            }
        }

        private void SavePool(VertexBufferPool pool, string name, bool isBigEndian)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                pool.WriteToFile(stream, isBigEndian);
                File.WriteAllBytes(name, stream.ToArray());
                loadedPoolNames.Add(new FileInfo(name));
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
