using System.IO;
using Gibbed.IO;
using Gibbed.Illusion.FileFormats;

namespace Gibbed.Mafia2.ResourceFormats
{
    //also known as SystemObjectDatabase
    public class XBinResource : IResourceType
    {
        //header
        public ulong Unk01;
        public int Size;
        public byte[] Data;

        //near the end
        public uint Unk02;
        public ulong Unk03;
        public string Unk04;
        public ulong Hash;
        public string Name;
        public byte[] XMLData;

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            Unk01 = input.ReadValueU64(endian);
            Size = input.ReadValueS32(endian);
            Data = input.ReadBytes(Size);
            Unk02 = input.ReadValueU32(endian);
            Unk03 = input.ReadValueU64(endian);
            Unk04 = input.ReadStringU32(endian);
            Hash = input.ReadValueU64(endian);
            Name = input.ReadStringU32(endian);

            if(input.Position != input.Length)
            {
                var xmlSize = input.ReadValueU64(endian);
                XMLData = input.ReadBytes((int)xmlSize);
            }
        }

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            //output.WriteStringU16(FileName, endian);
            //output.WriteValueU64(Hash);
            //output.WriteStringU16(Name, endian);
            //output.WriteValueU32(Size, endian);
            //output.Write(Data, 0, (int)Size);
        }
    }
}
