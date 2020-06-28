using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ResourceTypes.M3.XBin;

namespace Core
{
    public class XBinFile
    {
        private XBin xbin;

        public static string GetExtensionUpperInvariant()
        {
            return ".XBIN";
        }

        public static string GetExtensionLowerInvariant()
        {
            return ".xbin";
        }

        public void Open(FileInfo info)
        {
            xbin = new XBin();
            using(BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                xbin.ReadFromFile(reader);
            }
            Write(info);
        }

        public void Write(FileInfo info)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(info.FullName, FileMode.Create)))
            {
                xbin.WriteToFile(writer);
            }
        }
    }
}
