using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.GfxContainers
{
    public class GfxGlassMatTemplateTable : BaseTable
    {
        public class GfxGlassMatTemplateItem
        {
            public uint ID { get; set; }
            public uint OriginalTemplate_Part0 { get; set; }
            public uint OriginalTemplate_Part1 { get; set; }
            public uint DamagedTemplate_Part0 { get; set; }
            public uint DamagedTemplate_Part1 { get; set; }
            public int Type { get; set; }
            public uint GlassBreakType { get; set; }
            public string Desc { get; set; }

            public GfxGlassMatTemplateItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public GfxGlassMatTemplateItem[] GfxGlassMatTemplate { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            GfxGlassMatTemplate = new GfxGlassMatTemplateItem[count1];

            for (int i = 0; i < GfxGlassMatTemplate.Length; i++)
            {
                GfxGlassMatTemplateItem Item = new GfxGlassMatTemplateItem();
                Item.ID = reader.ReadUInt32();
                Item.OriginalTemplate_Part0 = reader.ReadUInt32();
                Item.OriginalTemplate_Part1 = reader.ReadUInt32();
                Item.DamagedTemplate_Part0 = reader.ReadUInt32();
                Item.DamagedTemplate_Part1 = reader.ReadUInt32();
                Item.Type = reader.ReadInt32();
                Item.GlassBreakType = reader.ReadUInt32();
                Item.Desc = StringHelpers.ReadStringBuffer(reader, 32);
                GfxGlassMatTemplate[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(GfxGlassMatTemplate.Length);
            writer.Write(GfxGlassMatTemplate.Length);

            for (int i = 0; i < GfxGlassMatTemplate.Length; i++)
            {
                GfxGlassMatTemplateItem Item = GfxGlassMatTemplate[i];
                writer.Write(Item.ID);
                writer.Write(Item.OriginalTemplate_Part0);
                writer.Write(Item.OriginalTemplate_Part1);
                writer.Write(Item.DamagedTemplate_Part0);
                writer.Write(Item.DamagedTemplate_Part1);
                writer.Write(Item.Type);
                writer.Write(Item.GlassBreakType);
                StringHelpers.WriteStringBuffer(writer, 32, Item.Desc);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GfxGlassMatTemplateTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<GfxGlassMatTemplateTable>(Root);
            this.GfxGlassMatTemplate = TableInformation.GfxGlassMatTemplate;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GfxGlassMatTemplate Table";

            foreach (var Item in GfxGlassMatTemplate)
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
            GfxGlassMatTemplate = new GfxGlassMatTemplateItem[Root.Nodes.Count];

            for (int i = 0; i < GfxGlassMatTemplate.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                GfxGlassMatTemplateItem Entry = (GfxGlassMatTemplateItem)ChildNode.Tag;
                GfxGlassMatTemplate[i] = Entry;
            }
        }
    }
}
