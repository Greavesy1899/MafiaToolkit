using System.IO;
namespace Mafia2 {
    public class VertexBufferPool {
        byte version;
        int numBuffers;
        int size;
        VertexBuffer[] buffers;

        public VertexBuffer[] Buffers {
            get { return buffers; }
            set { buffers = value; }
        }

        public VertexBufferPool(BinaryReader reader) {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader) {
            version = reader.ReadByte();
            numBuffers = reader.ReadInt32();
            size = reader.ReadInt32();
            buffers = new VertexBuffer[numBuffers];
            for(int i = 0; i != numBuffers; i++) {
                buffers[i] = new VertexBuffer(reader);
            }
        }
    }

    public class VertexBuffer {
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

        public VertexBuffer(BinaryReader reader) {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader) {
            hash = reader.ReadUInt64();
            len = reader.ReadInt32();
            data = reader.ReadBytes(len);
        }
    }
}
