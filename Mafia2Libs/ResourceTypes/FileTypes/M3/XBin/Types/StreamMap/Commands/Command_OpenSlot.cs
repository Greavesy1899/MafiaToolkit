using ResourceTypes.M3.XBin;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{ 
    public class Command_OpenSlot : ICommand
    {
        private readonly uint Magic = 0xD7C10363;

        public uint TypeID { get; set; }
        public uint SlotID { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            TypeID = reader.ReadUInt32();
            SlotID = reader.ReadUInt32();
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(TypeID);
            writer.Write(SlotID);
        }

        public int GetSize()
        {
            return 8;
        }

        public uint GetMagic()
        {
            return Magic;
        }
    }
}
