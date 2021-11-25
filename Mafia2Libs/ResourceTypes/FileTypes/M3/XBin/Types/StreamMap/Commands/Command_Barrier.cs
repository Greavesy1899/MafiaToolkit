using ResourceTypes.M3.XBin;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    // Essentially an empty command; holds no data.
    // (This is witnessed on M3.)
    public class Command_Barrier : ICommand
    {
        private readonly uint Magic = 0x31247C78;

        public uint Unk0 { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            Unk0 = reader.ReadUInt32();
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(Unk0);
        }

        public int GetSize()
        {
            return 4;
        }

        public uint GetMagic()
        {
            return Magic;
        }
    }
}
