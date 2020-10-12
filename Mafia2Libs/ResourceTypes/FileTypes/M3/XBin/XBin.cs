using ResourceTypes.FileTypes.M3.XBin;
using System.IO;

namespace ResourceTypes.M3.XBin
{
    public class XBin
    {
        private ulong hash;
        private int version;
        private int numTables;
        private int offset;

        private int unk0; //could be with the table;
        public BaseTable TableInformation { get; set; }
        private int unk1; // Unknown.

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64(); // I *think* this is to UInt32's molded together.
            version = reader.ReadInt32();
            numTables = reader.ReadInt32();
            offset = reader.ReadInt32();

            if (numTables > 0)
            {
                for(int i = 0; i < numTables; i++)
                {
                    uint offset = reader.ReadUInt32();
                    uint size = reader.ReadUInt32();
                }
            }
            else
            {
                unk0 = reader.ReadInt32();
                TableInformation = XBinFactory.ReadXBin(reader, hash);
            }
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
            writer.Write(unk0);
            TableInformation.WriteToFile(writer);
            writer.Write(unk1);
        }
    }
}
