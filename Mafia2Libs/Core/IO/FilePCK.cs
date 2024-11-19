using System.IO;
using Mafia2Tool;

namespace Core.IO
{
    class FilePCK : FileBase
    {
        public FilePCK(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "PCK";
        }

        public override bool Open()
        {
            PCKEditor editor = new PCKEditor(file);
            return true;
        }
    }
}
