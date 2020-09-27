using System;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_UnlockSDS : ICommand
    {
        private readonly uint Magic = 0x687DAD8B;

        public string SDSName { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            SDSName = StreamMapCoreUtils.ReadStringPtrWithOffset(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
