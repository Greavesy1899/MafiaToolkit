using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
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

        public override string ToString()
        {
            return string.Format("{0} {1}", ID, CarName);
        }
    }

    public class PaintCombinationsTable : BaseTable
    {
        private uint unk0;

        public PaintCombinationsTableItem[] PaintCombinations { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
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

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(PaintCombinations.Length);
            writer.Write(PaintCombinations.Length);

            for(int i = 0; i < PaintCombinations.Length; i++)
            {
                PaintCombinationsTableItem Item = PaintCombinations[i];
                writer.Write(Item.ID);
                writer.Write(Item.Unk01);
                writer.Write(Item.MinOccurs);
                writer.Write(Item.MaxOccurs);
                StringHelpers.WriteString32(writer, Item.CarName);
            }

            for (int i = 0; i < PaintCombinations.Length; i++)
            {
                PaintCombinationsTableItem Item = PaintCombinations[i];
                for (int z = 0; z < Item.MaxOccurs; z++)
                {
                    writer.Write(Item.ColorIndex[z]);
                }
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            PaintCombinationsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<PaintCombinationsTable>(Root);
            this.PaintCombinations = TableInformation.PaintCombinations;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "Paint Combinations Table";

            foreach (var Item in PaintCombinations)
            {
                TreeNode ChildNode = new TreeNode();
                ChildNode.Tag = Item;
                ChildNode.Text = Item.ToString();
                Root.Nodes.Add(ChildNode);
            }

            return Root;
        }

        public void SetFromTreeNodes(TreeNode Root)
        {
            PaintCombinations = new PaintCombinationsTableItem[Root.Nodes.Count];

            for (int i = 0; i < PaintCombinations.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                PaintCombinationsTableItem Entry = (PaintCombinationsTableItem)ChildNode.Tag;
                PaintCombinations[i] = Entry;
            }
        }
    }
}
