using ResourceTypes.M3.XBin;
using SharpDX;
using System;
using System.IO;
using Utils.SharpDXExtensions;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_LoadSDS : ICommand
    {
        private readonly uint Magic = 0x22663242;

        public ESlotType SlotType { get; set; }
        public string SDSName { get; set; }
        public string QuotaID { get; set; }
        public uint LoadFlags { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            SlotType = (ESlotType)reader.ReadUInt32();
            SDSName = StreamMapCoreUtils.ReadStringPtrWithOffset(reader);
            QuotaID = StreamMapCoreUtils.ReadStringPtrWithOffset(reader);
            LoadFlags = reader.ReadUInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
