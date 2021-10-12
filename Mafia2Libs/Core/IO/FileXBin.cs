using System.IO;
using MafiaToolkit;

namespace Core.IO
{
    public class FileXBin : FileBase
    {
        public FileXBin(FileInfo info) : base(info)
        {

        }

        public static string GetExtensionUpperInvariant()
        {
            return ".XBIN";
        }

        public static string GetExtensionLowerInvariant()
        {
            return ".xbin";
        }

        public override bool Open()
        {
            XBinEditor editor = new XBinEditor(file);
            return true;
        }
        public override void Save()
        {
            // guhh
        }
    }
}
