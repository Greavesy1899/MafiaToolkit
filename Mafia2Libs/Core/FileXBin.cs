using System.IO;
using ResourceTypes.FileTypes.M3.XBin;
using ResourceTypes.M3.XBin;

namespace Core.IO
{
    public class FileXBin : FileBase
    {
        private XBin xbin;

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
            xbin = new XBin();
            using(BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                xbin.ReadFromFile(reader);
            }

            Save();

            return true;
        }
        public override void Save()
        {
            File.Copy(file.FullName, file.FullName + ".back");
            using (XBinWriter writer = new XBinWriter(File.Open(file.FullName, FileMode.Create)))
            {
                xbin.WriteToFile(writer);
            }
        }
    }
}
