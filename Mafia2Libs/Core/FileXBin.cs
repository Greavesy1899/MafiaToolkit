using System.IO;
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

            return true;
        }
        public override void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                xbin.WriteToFile(writer);
            }
        }
    }
}
