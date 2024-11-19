using System.IO;
using Gibbed.IO;
using Gibbed.Mafia2.ResourceFormats;

namespace Gibbed.Illusion.ResourceFormats
{
    // We do not actually use this yet; We just use it to get the names.
    public class HavokResource : IResourceType
    {
        // Serialized
        public uint Unk01;
        public ulong FileHash;
        public uint Unk02;
        public byte[] Data;

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            output.WriteValueU32(Unk01, endian);
            output.WriteValueU64(FileHash, endian);
            output.WriteValueU32(Unk02);
            output.WriteBytes(Data);
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            Unk01 = input.ReadValueU32(endian);
            FileHash = input.ReadValueU64(endian);
            Unk02 = input.ReadValueU32(endian);
            Data = input.ReadBytes((int)(input.Length - input.Position));
        }
    }
}
