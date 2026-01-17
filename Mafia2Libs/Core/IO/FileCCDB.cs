using System.IO;
using Mafia2Tool;

namespace Core.IO
{
    public class FileCCDB : FileBase
    {
        public FileCCDB(FileInfo info) : base(info)
        {
        }

        public static string GetExtensionUpperInvariant()
        {
            return ".CCDB";
        }

        public static string GetExtensionLowerInvariant()
        {
            return ".ccdb";
        }

        public override bool Open()
        {
            CCDBEditor editor = new CCDBEditor(file);
            return true;
        }

        public override void Save()
        {
            // Save is handled by the editor
        }
    }
}
