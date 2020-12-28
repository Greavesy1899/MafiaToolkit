using ResourceTypes.M3.XBin;
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
            VideoName = XBinCoreUtils.ReadStringPtrWithOffset(reader);
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.PushStringPtr(VideoName);
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
