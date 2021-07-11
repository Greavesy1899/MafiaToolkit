using Mafia2Tool;
using ResourceTypes.Actors;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.IO
{
    class FilePck : FileBase
    {
        public FilePck(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "PCK";
        }

        public override bool Open()
        {
            PckEditor editor = new PckEditor(file);
            return true;
        }
    }
}
