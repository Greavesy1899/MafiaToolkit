using Mafia2Tool.MafiaLib.FileTypes.M3.XBin.Types;
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

        // TODO: Sort this out; make a factory.
        private VehicleTable vehicles; // test;
        private PaintCombinationsTable paintCombinations;
        private StringTable tables;

        private int unk1;

        public void ReadFromFile(BinaryReader reader)
        {
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

                StreamMapTable map = new StreamMapTable();
                map.ReadFromFile(reader);

                //vehicles = new VehicleTable();
                //vehicles.ReadFromFile(reader);
                //unk1 = reader.ReadInt32();
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(hash);
            writer.Write(version);
            writer.Write(numTables);
            writer.Write(offset);
            writer.Write(unk0);
            tables.WriteToFile(writer);
            //vehicles.WriteToFile(writer);
            writer.Write(unk1);
        }
    }
}
