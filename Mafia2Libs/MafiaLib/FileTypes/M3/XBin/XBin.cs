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
        private VehicleTable vehicles; // test;

        private int unk1;

        public void ReadFromFile(BinaryReader reader)
        {
            hash = reader.ReadUInt64();
            version = reader.ReadInt32();
            numTables = reader.ReadInt32();
            offset = reader.ReadInt32();
            unk0 = reader.ReadInt32();

            vehicles = new VehicleTable();
            vehicles.ReadFromFile(reader);
            unk1 = reader.ReadInt32();
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(version);
            writer.Write(numTables);
            writer.Write(offset);
            writer.Write(unk0);
            vehicles.WriteToFile(writer);
            writer.Write(unk1);
        }
    }
}
