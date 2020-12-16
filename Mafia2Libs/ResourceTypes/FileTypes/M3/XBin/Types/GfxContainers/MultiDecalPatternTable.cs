using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.GfxContainers
{
    public class MultiDecalPatternTable : BaseTable
    {
        public class MultiDecalPatternItem
        {
            public uint ID { get; set; }
            public float Probability { get; set; }
            public EMultiDecalFlags Flags { get; set; }
            public uint NumDecals { get; set; }
            public float MaxRightShift { get; set; }
            public float MaxUpShift { get; set; }
            public float ScaleFactor { get; set; }
            public float ScaleRand { get; set; }

            public MultiDecalPatternItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public MultiDecalPatternItem[] MultiDecalPattern { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            MultiDecalPattern = new MultiDecalPatternItem[count1];

            for (int i = 0; i < MultiDecalPattern.Length; i++)
            {
                MultiDecalPatternItem Item = new MultiDecalPatternItem();
                Item.ID = reader.ReadUInt32();
                Item.Probability = reader.ReadSingle();
                Item.Flags = (EMultiDecalFlags)reader.ReadUInt32();
                Item.NumDecals = reader.ReadUInt32();
                Item.MaxRightShift = reader.ReadSingle();
                Item.MaxUpShift = reader.ReadSingle();
                Item.ScaleFactor = reader.ReadSingle();
                Item.ScaleRand = reader.ReadSingle();
                MultiDecalPattern[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(MultiDecalPattern.Length);
            writer.Write(MultiDecalPattern.Length);

            for (int i = 0; i < MultiDecalPattern.Length; i++)
            {
                MultiDecalPatternItem Item = MultiDecalPattern[i];
                writer.Write(Item.ID);
                writer.Write(Item.Probability);
                writer.Write((uint)Item.Flags);
                writer.Write(Item.NumDecals);
                writer.Write(Item.MaxRightShift);
                writer.Write(Item.MaxUpShift);
                writer.Write(Item.ScaleFactor);
                writer.Write(Item.ScaleRand);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            MultiDecalPatternTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<MultiDecalPatternTable>(Root);
            this.MultiDecalPattern = TableInformation.MultiDecalPattern;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "MultiDecalPattern Table";

            foreach (var Item in MultiDecalPattern)
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
            MultiDecalPattern = new MultiDecalPatternItem[Root.Nodes.Count];

            for (int i = 0; i < MultiDecalPattern.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                MultiDecalPatternItem Entry = (MultiDecalPatternItem)ChildNode.Tag;
                MultiDecalPattern[i] = Entry;
            }
        }
    }
}
