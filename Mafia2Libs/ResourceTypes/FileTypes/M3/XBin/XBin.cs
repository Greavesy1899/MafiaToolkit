using Gibbed.Illusion.FileFormats.Hashing;
using System.Collections.Generic;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.M3.XBin
{
    public class XBin
    {
        private ulong hash;
        private int version;
        private int numTables;
        private int offset;

        public BaseTable TableInformation { get; set; }

        private Dictionary<ulong, string> XBinHashes;

        public void ReadFromFile(BinaryReader reader)
        {
            ConstructHashPool();

            hash = reader.ReadUInt64(); // I *think* this is to UInt32's molded together.
            version = reader.ReadInt32();
            numTables = reader.ReadInt32();
            offset = reader.ReadInt32();

            TableInformation = XBinFactory.ReadXBin(reader, this, hash);
        }

        public void WriteToFile(FileInfo file)
        {
            using(XBinWriter writer = new XBinWriter(File.Open(file.FullName, FileMode.Create)))
            {
                InternalWriteToFile(writer);
            }
        }

        private void InternalWriteToFile(XBinWriter writer)
        {
            writer.Write(hash);
            writer.Write(version);
            writer.Write(numTables);
            writer.Write(offset);
            TableInformation.WriteToFile(writer);
        }

        private void ConstructHashPool()
        {
            string[] Lines = File.ReadAllLines("Resources/GameData/XBin_Hashes.txt");

            XBinHashes = new Dictionary<ulong, string>();

            foreach(var Line in Lines)
            {
                ulong Hash = FNV64.Hash(Line);
                XBinHashes.TryAdd(Hash, Line);
            }
        }

        public string GetFromHashPool(ulong Hash)
        {
            return XBinHashes.TryGet(Hash);
        }
    }
}
