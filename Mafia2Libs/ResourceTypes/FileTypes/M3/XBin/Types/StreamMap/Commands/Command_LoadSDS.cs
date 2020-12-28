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
            SDSName = XBinCoreUtils.ReadStringPtrWithOffset(reader);
            QuotaID = XBinCoreUtils.ReadStringPtrWithOffset(reader);
            LoadFlags = reader.ReadUInt32();
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write((uint)SlotType);
            writer.PushStringPtr(SDSName);
            writer.PushStringPtr(QuotaID);
            writer.Write(LoadFlags);
        }

        public int GetSize()
        {
            return 16;
        }

        public uint GetMagic()
        {
            return Magic;
        }
    }
}
