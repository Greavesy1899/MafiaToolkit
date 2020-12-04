using Mafia2Tool;
using System.Diagnostics;
using System.IO;
using Utils.Settings;

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

            if (filename.Equals("SDSContent") && !ToolkitSettings.UseSDSToolFormat)
            {
                SDSContentEditor editor = new SDSContentEditor(file);
                return true;
            }
            else
            {
                return (Process.Start(file.FullName) != null ? true : false);
            }
        }
    }
}
