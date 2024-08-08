using ResourceTypes.Animation2;
using System;
using System.IO;

namespace Core.IO
{
    public class FileAnimation2 : FileBase
    {
        public FileAnimation2(FileInfo info) : base(info)
        {

        }

        public override bool Open()
        {
            Animation2 anim = new(file.FullName);
            return true;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return "AN2";
        }
    }
}
