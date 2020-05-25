using System.IO;
using Gibbed.IO;
using Gibbed.Illusion.FileFormats;

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
            output.WriteValueU64(Hash);
            output.WriteStringU16(Name, endian);
            output.WriteValueU32(Size, endian);
            output.Write(Data, 0, (int)Size);
        }
    }
}
