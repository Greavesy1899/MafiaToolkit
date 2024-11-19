using Toolkit.Forms;
using System.IO;

namespace Core.IO
{
    public class FileFxAnimSet : FileBase
    {
        public FileFxAnimSet(FileInfo info) : base(info) { }

        public override string GetExtensionUpper()
        {
            return "FAS";
        }

        public override bool Open()
        {
            FxAnimSetEditor AnimSetEditor = new FxAnimSetEditor(GetUnderlyingFileInfo());
            return true;
        }
    }
}
