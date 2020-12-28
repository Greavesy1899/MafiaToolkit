using ResourceTypes.M3.XBin;
using System.IO;
using Utils.StringHelpers;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_Save : ICommand
    {
        private readonly uint Magic = 0xEA186B6;

        public ESaveType SaveType { get; set; }
        public string SaveId { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            SaveType = (ESaveType)reader.ReadUInt32();
            SaveId = StringHelpers.ReadStringBuffer(reader, 64);
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write((uint)SaveType);
            StringHelpers.WriteStringBuffer(writer, 64, SaveId);
        }
        public int GetSize()
        {
            return 68;
        }

        public uint GetMagic()
        {
            return Magic;
        }
    }
}
