using System;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_PlayVideo : ICommand
    {
        private readonly uint Magic = 0x22663242;

        public string VideoName { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            VideoName = StreamMapCoreUtils.ReadStringPtrWithOffset(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
