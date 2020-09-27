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
            CutsceneName = StreamMapCoreUtils.ReadStringPtrWithOffset(reader);
        }

        public void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
