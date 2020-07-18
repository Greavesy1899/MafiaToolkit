using Mafia2Tool;
using System.IO;

namespace Core.IO
{
    public class FileXML : FileBase
    {
        public FileXML(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "XML";
        }

        public override bool Open()
        {
            var filename = GetNameWithoutExtension();

            if (filename.Equals("SDSContent"))
            {
                SDSContentEditor editor = new SDSContentEditor(file);
                return true;
            }

            return false;
        }
    }
}
