using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class PaintCombinationsTableItem
    {
        public int ID { get; set; }
        public int Unk01 { get; set; }
        public int MinOccurs { get; set; }
        public int MaxOccurs { get; set; }
        public string CarName { get; set; }
        public int[] ColorIndex { get; set; }

        public void WriteText(StreamWriter writer)
        {
            writer.WriteLine("Start --------------------------------");
            writer.WriteLine("ID: {0}", ID);
            writer.WriteLine("CarName: {0}", CarName);
            foreach (var index in ColorIndex)
            {
                writer.WriteLine("ColorIndex: {0}", index);
            }
            writer.WriteLine("End --------------------------------");
        }
    }

    public class PaintCombinationsTable
    {
        public PaintCombinationsTableItem[] PaintCombinations { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            PaintCombinations = new PaintCombinationsTableItem[count1];

            for (int i = 0; i < count1; i++)
            {
                PaintCombinationsTableItem item = new PaintCombinationsTableItem();
                item.ID = reader.ReadInt32();
                item.Unk01 = reader.ReadInt32();
                item.MinOccurs = reader.ReadInt32();
                item.MaxOccurs = reader.ReadInt32();
                item.CarName = StringHelpers.ReadStringBuffer(reader, 32).Trim('\0');
                PaintCombinations[i] = item;
            }

            for (int i = 0; i < count1; i++)
            {
                var item = PaintCombinations[i];
                item.ColorIndex = new int[item.MaxOccurs];
                for (int z = 0; z < item.MaxOccurs; z++)
                {
                    item.ColorIndex[z] = reader.ReadInt32();
                }
                PaintCombinations[i] = item;
            }
        }
    }
}
