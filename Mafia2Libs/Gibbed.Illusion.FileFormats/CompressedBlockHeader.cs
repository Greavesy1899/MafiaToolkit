using Gibbed.IO;
using System.IO;

namespace Gibbed.Illusion.FileFormats
{
    public struct CompressedBlockHeader
    {
        public uint UncompressedSize;
        public uint HeaderSize;
        public short ChunkSize;
        public short ChunkCount;
        public short Unknown0C;
        public uint CompressedSize;
        public byte Unknown0E;
        public byte Unknown0F;
        public ushort[] Chunks;

        public static CompressedBlockHeader Read(Stream input, Endian endian)
        {
            CompressedBlockHeader instance;
            instance.UncompressedSize = input.ReadValueU32(endian);
            instance.HeaderSize = input.ReadValueU32(endian);
            instance.ChunkSize = input.ReadValueS16(endian);
            instance.ChunkCount = input.ReadValueS16(endian);

            // For Zlib, this is 15. For Oodle, this is 0.
            instance.Unknown0C = input.ReadValueS16(endian);

            // This is usually 1.
            instance.Unknown0E = input.ReadValueU8();

            // This could very well be the number of available chunks. 
            // (As in how many slots are in the array.)
            instance.Unknown0F = input.ReadValueU8();

            instance.Chunks = new ushort[8];
            instance.CompressedSize = 0;
            for (int i = 0; i < 8; ++i)
            {
                instance.Chunks[i] = input.ReadValueU16(endian);
                instance.CompressedSize += instance.Chunks[i];
            }

            return instance;
        }

        public void SetZlibPreset()
        {
            HeaderSize = 32;
            Unknown0C = 1;
            Unknown0E = 15;
            Unknown0F = 8;
            ChunkCount = 1;
            ChunkSize = 0;
            Chunks = new ushort[8];
        }

        public void SetOodlePreset()
        {
            HeaderSize = 128;
            Unknown0C = 0;
            Unknown0E = 1;
            Unknown0F = 0;
            ChunkCount = 1;
            ChunkSize = 1;
            Chunks = new ushort[8];
        }

        public void Write(Stream output, Endian endian)
        {
            output.WriteValueU32(this.UncompressedSize, endian);
            output.WriteValueU32(this.HeaderSize, endian);
            output.WriteValueS16(this.ChunkSize, endian);
            output.WriteValueS16(this.ChunkCount, endian);
            output.WriteValueS16(this.Unknown0C, endian);
            output.WriteValueU8(this.Unknown0E);
            output.WriteValueU8(this.Unknown0F);
            for (int i = 0; i < 8; i++)
            {
                output.WriteValueU16(this.Chunks[i], endian);
            }
        }
    }
}
