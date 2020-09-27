using System.IO;

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

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(SeasonID);
        }
    }
}
