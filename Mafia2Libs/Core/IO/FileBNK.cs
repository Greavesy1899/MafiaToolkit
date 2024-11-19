using System.IO;
using Mafia2Tool;

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
