using System;
using System.IO;

namespace Core.IO
{
    public class FileTextureDDS : FileBase
    {
        public FileTextureDDS(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "DDS";
        }

        public override void Save()
        {
            throw new NotImplementedException("This functionality is not supported for this filetype.");
        }
    }
}
