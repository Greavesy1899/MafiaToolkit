using System.IO;
using Gibbed.Illusion.FileFormats;
using Gibbed.Illusion.FileFormats.Hashing;
using Gibbed.IO;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class FlashResource : IResourceType
    {
        public string FileName;
        public ulong Hash;
        public string Name;
        public uint Size;
        public byte[] Data;

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            FileName = input.ReadStringU16(endian);
            Hash = input.ReadValueU64(endian);
            Name = input.ReadStringU16(endian);
            Size = input.ReadValueU32(endian);
            Data = input.ReadBytes((int)Size);
        }

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            output.WriteStringU16(FileName, endian);
            output.WriteValueU64(FNV64.Hash(Name), endian);
            output.WriteStringU16(Name, endian);
            output.WriteValueS32(Data.Length, endian);
            output.WriteBytes(Data);
        }
    }
}
