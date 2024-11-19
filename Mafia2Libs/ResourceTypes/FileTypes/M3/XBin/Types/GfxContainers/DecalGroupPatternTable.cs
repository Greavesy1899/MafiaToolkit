using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.GfxContainers
{
    public class DecalGroupPatternTable : BaseTable
    {
        public class DecalGroupPatternItem
        {
            public uint ID { get; set; }
            public uint MaxPC { get; set; }
            public uint MaxXBOX { get; set; }
            public uint MaxPS3 { get; set; }
            public float FadeOut { get; set; }
            public uint MaxDistance { get; set; }

            public DecalGroupPatternItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public DecalGroupPatternItem[] DecalGroupPattern { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            DecalGroupPattern = new DecalGroupPatternItem[count1];

            for (int i = 0; i < DecalGroupPattern.Length; i++)
            {
                DecalGroupPatternItem Item = new DecalGroupPatternItem();
                Item.ID = reader.ReadUInt32();
                Item.MaxPC = reader.ReadUInt32();
                Item.MaxXBOX = reader.ReadUInt32();
                Item.MaxPS3 = reader.ReadUInt32();
                Item.FadeOut = reader.ReadSingle();
                Item.MaxDistance = reader.ReadUInt32();
                DecalGroupPattern[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(DecalGroupPattern.Length);
            writer.Write(DecalGroupPattern.Length);

            for (int i = 0; i < DecalGroupPattern.Length; i++)
            {
                DecalGroupPatternItem Item = DecalGroupPattern[i];
                writer.Write(Item.ID);
                writer.Write(Item.MaxPC);
                writer.Write(Item.MaxXBOX);
                writer.Write(Item.MaxPS3);
                writer.Write(Item.FadeOut);
                writer.Write(Item.MaxDistance);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            DecalGroupPatternTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<DecalGroupPatternTable>(Root);
            this.DecalGroupPattern = TableInformation.DecalGroupPattern;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "DecalGroupPattern Table";

            foreach (var Item in DecalGroupPattern)
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
            DecalGroupPattern = new DecalGroupPatternItem[Root.Nodes.Count];

            for (int i = 0; i < DecalGroupPattern.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                DecalGroupPatternItem Entry = (DecalGroupPatternItem)ChildNode.Tag;
                DecalGroupPattern[i] = Entry;
            }
        }
    }
}
