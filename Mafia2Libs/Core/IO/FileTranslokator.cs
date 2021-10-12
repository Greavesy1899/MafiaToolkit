using Toolkit.Forms;
using System.IO;

namespace Core.IO
{
    class FileTranslokator : FileBase
    {
        public FileTranslokator(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "TRA";
        }

        public override bool Open()
        {
            TranslokatorEditor editor = new TranslokatorEditor(file);
            return true;
        }
    }
}
