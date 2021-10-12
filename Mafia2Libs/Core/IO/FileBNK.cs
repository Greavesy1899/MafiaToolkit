using MafiaToolkit;
using ResourceTypes.Actors;
using System;
using System.Collections.Generic;
using System.IO;

namespace Core.IO
{
    class FileBNK : FileBase
    {
        public FileBNK(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "BNK";
        }

        public override bool Open()
        {
            BNKEditor editor = new BNKEditor(file);
            return true;
        }
    }
}
