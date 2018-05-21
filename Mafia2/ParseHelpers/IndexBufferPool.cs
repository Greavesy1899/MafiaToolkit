using System.IO;

namespace Mafia2 {
    public class IndexBufferPool {
        private byte version;
        private int numBuffers;
        private int size;
        private IndexBuffer[] buffers;

        public IndexBuffer[] Buffers {
            get { return buffers; }
            set { buffers = value; }
        }

        public IndexBufferPool(BinaryReader reader) {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader) {
            version = reader.ReadByte();
            numBuffers = reader.ReadInt32();
            size = reader.ReadInt32();
            buffers = new IndexBuffer[numBuffers];
            for(int i = 0; i < numBuffers; i++) {
                buffers[i] = new IndexBuffer(reader);
            }

        }
    }

    public class IndexBuffer {
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
            set { data = value; }
        }

        public IndexBuffer(BinaryReader reader) {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader) {
            hash = reader.ReadUInt64();
            u = reader.ReadInt32();
            len = reader.ReadInt32();
            data = new ushort[len / 2];

            int num = 0;
            int index = 0;

            while(num < len) {
                data[index] = reader.ReadUInt16();
                num += 2;
                ++index;
            }

        }
    }
}
