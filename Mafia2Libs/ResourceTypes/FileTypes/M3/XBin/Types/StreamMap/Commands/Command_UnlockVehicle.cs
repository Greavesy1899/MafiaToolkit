using ResourceTypes.M3.XBin;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_UnlockVehicle : ICommand
    {
        private readonly uint Magic = 0x3B3DD38A;

        public uint GUID { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            GUID = reader.ReadUInt32();
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(GUID);
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
