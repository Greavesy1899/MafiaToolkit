using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using ResourceTypes.M3.XBin;
using ResourceTypes.M3.XBin.PaintCombinations;
using Utils.Helpers.Reflection;
using Utils.Logging;
using Utils.Settings;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class PaintCombinationsTableItem
    {
        [PropertyForceAsAttribute]
        public int ID { get; set; }
        [PropertyForceAsAttribute]
        public string CarName { get; set; }
        public IPaintCombinationsTableItem_Elm[] Elements { get; set; }

        public static readonly uint NUM_ELEMENTS = 14;

        public PaintCombinationsTableItem()
        {
            Elements = new IPaintCombinationsTableItem_Elm[NUM_ELEMENTS];
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", ID, CarName);
        }
    }

    public class PaintCombinationsTable : BaseTable
    {
        private uint unk0;
        private GamesEnumerator gameVersion;

        public PaintCombinationsTableItem[] PaintCombinations { get; set; }

        public PaintCombinationsTable()
        {
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

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
                reader.BaseStream.Position += 4; // Skip array offset, not required
                int MinOccurs = reader.ReadInt32();
                int MaxOccurs = reader.ReadInt32();
                ToolkitAssert.Ensure(MinOccurs == 14 && MaxOccurs == 14, "Would expect Min and Max Occurs to be 14, not {0} and {1} respectively.", MinOccurs, MaxOccurs);
                item.CarName = StringHelpers.ReadStringBuffer(reader, 32).Trim('\0');
                PaintCombinations[i] = item;
            }

            for (int i = 0; i < count1; i++)
            {
                var item = PaintCombinations[i];
                item.Elements = new IPaintCombinationsTableItem_Elm[PaintCombinationsTableItem.NUM_ELEMENTS];
                for (int z = 0; z < PaintCombinationsTableItem.NUM_ELEMENTS; z++)
                {
                    IPaintCombinationsTableItem_Elm Element = null;
                    if(gameVersion == GamesEnumerator.MafiaIII)
                    {
                        Element = new PaintCombinationsTableItem_Elm_M3();
                    }
                    else if(gameVersion == GamesEnumerator.MafiaI_DE)
                    {
                        Element = new PaintCombinationsTableItem_Elm_MDE();
                    }

                    Element.ReadEntry(reader);

                    item.Elements[z] = Element;
                }

                PaintCombinations[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(PaintCombinations.Length);
            writer.Write(PaintCombinations.Length);

            for (int i = 0; i < PaintCombinations.Length; i++)
            {
                PaintCombinationsTableItem Item = PaintCombinations[i];
                writer.Write(Item.ID);
                writer.PushObjectPtr(string.Format("Entry_{0}", i));
                writer.Write(PaintCombinationsTableItem.NUM_ELEMENTS);
                writer.Write(PaintCombinationsTableItem.NUM_ELEMENTS);
                StringHelpers.WriteStringBuffer(writer, 32, Item.CarName);
            }

            for (int i = 0; i < PaintCombinations.Length; i++)
            {
                PaintCombinationsTableItem Item = PaintCombinations[i];
                writer.FixUpObjectPtr(string.Format("Entry_{0}", i));
                for (int z = 0; z < PaintCombinationsTableItem.NUM_ELEMENTS; z++)
                {
                    IPaintCombinationsTableItem_Elm Element = Item.Elements[z];
                    Element.WriteEntry(writer);
                }
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            PaintCombinationsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<PaintCombinationsTable>(Root);
            PaintCombinations = TableInformation.PaintCombinations;
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
