using Mafia2Tool;
using ResourceTypes.Actors;
using ResourceTypes.SoundTable;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.IO
{
    public class FileSoundTable : FileBase
    {
        public FileSoundTable(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "STBL";
        }

        public override bool Open()
        {
            // TODO: Make editor
            using (MemoryStream ReaderStream = new MemoryStream(File.ReadAllBytes(file.FullName)))
            {
                SoundTable Table = new SoundTable();
                Table.ReadFromFile(ReaderStream, false);
            }

            return true;
        }
    }
}
