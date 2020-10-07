using ResourceTypes.FileTypes.M3.XBin;
using System.IO;
using System.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class XBin
    {
        private ulong hash;
        private int version;
        private int numTables;
        private int offset;

        private int unk0; //could be with the table;

        // TODO: Sort this out; make a factory.
        private VehicleTable vehicles; // test;
        private PaintCombinationsTable paintCombinations;
        private StringTable tables;
        private StreamMapTable streamMap;

        private int unk1;

        public void ReadFromFile(BinaryReader reader)
        {
            //float pp = 1.525879E-05f;
            hash = reader.ReadUInt64();
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

                //tables = new StringTable();
                //tables.ReadFromFile(reader);
                //tables.WriteToXML();
                //tables.ReadFromXML();

                streamMap = new StreamMapTable();
                streamMap.ReadFromFile(reader);

                //tables = new StringTable();
               // tables.ReadFromFile(reader);
                //tables.WriteToXML();

                //vehicles = new VehicleTable();
                //vehicles.ReadFromXML();
                //vehicles.ReadFromFile(reader);
                //vehicles.WriteToXML();
                //unk1 = reader.ReadInt32();
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(hash);
            writer.Write(version);
            writer.Write(numTables);
            writer.Write(offset);
            writer.Write(unk0);
            streamMap.WriteToFile(writer);
            writer.Write(unk1);
        }
    }
}
