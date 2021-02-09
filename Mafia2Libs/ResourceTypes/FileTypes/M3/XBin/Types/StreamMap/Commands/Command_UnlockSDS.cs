using ResourceTypes.M3.XBin;
using System;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_UnlockSDS : ICommand
    {
        private readonly uint Magic = 0x687DAD8B;

        public string SDSName { get; set; }

        public Command_UnlockSDS()
        {
            SDSName = "";
        }

        public void ReadFromFile(BinaryReader reader)
        {
            SDSName = XBinCoreUtils.ReadStringPtrWithOffset(reader);
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.PushStringPtr(SDSName);
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
