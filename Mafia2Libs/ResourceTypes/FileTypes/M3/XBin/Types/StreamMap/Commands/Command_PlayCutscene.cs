using ResourceTypes.M3.XBin;
using System;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public class Command_PlayCutscene: ICommand
    {
        private readonly uint Magic = 0x90ACE5D5;

        public string CutsceneName { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            CutsceneName = XBinCoreUtils.ReadStringPtrWithOffset(reader);
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.PushStringPtr(CutsceneName);
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
