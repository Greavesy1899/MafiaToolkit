using ResourceTypes.M3.XBin;
using System.IO;
using Utils.Helpers.Reflection;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_OpenTraffic : ICommand
    {
        private readonly uint Magic = 0xD4F4F264;

        public uint SeasonID { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            SeasonID = reader.ReadUInt32();
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(SeasonID);
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
