using Toolkit.Forms;
using System.IO;

namespace Core.IO
{
    class FileATP : FileBase
    {
        public FileATP(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "ATP";
        }

        public override bool Open()
        {
            ATPEditor editor = new ATPEditor(file);
            return true;
        }
    }
}
