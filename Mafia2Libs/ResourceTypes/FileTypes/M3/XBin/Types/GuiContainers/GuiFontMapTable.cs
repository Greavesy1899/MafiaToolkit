using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.GuiContainers
{
    public class GuiFontMapTable : BaseTable
    {
        public class GuiFontMapItem
        {
            public int ID { get; set; }
            public string Alias { get; set; }
            public string Name { get; set; }
            public EFontMapFlags Flags { get; set; }
            public float Scale { get; set; }
            public float OffsetX { get; set; }
            public float OffsetY { get; set; }
            public EFontMapPlatform Platform { get; set; }
            public EFontMapLanguage Language { get; set; }

            public GuiFontMapItem()
            {
                ID = 0;
            }

            public override string ToString()
            {
                return string.Format("ID = {0}", ID);
            }
        }

        public GuiFontMapItem[] GuiFontMap { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            GuiFontMap = new GuiFontMapItem[count1];

            for (int i = 0; i < GuiFontMap.Length; i++)
            {
                GuiFontMapItem Item = new GuiFontMapItem();
                Item.ID = i;
                Item.Alias = StringHelpers.ReadStringBuffer(reader, 32);
                Item.Name = StringHelpers.ReadStringBuffer(reader, 32);
                Item.Flags = (EFontMapFlags)reader.ReadUInt32();
                Item.Scale = reader.ReadSingle();
                Item.OffsetX = reader.ReadSingle();
                Item.OffsetY = reader.ReadSingle();
                Item.Platform = (EFontMapPlatform)reader.ReadUInt32();
                Item.Language = (EFontMapLanguage)reader.ReadUInt32();
                GuiFontMap[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(GuiFontMap.Length);
            writer.Write(GuiFontMap.Length);

            for (int i = 0; i < GuiFontMap.Length; i++)
            {
                GuiFontMapItem Item = GuiFontMap[i];
                StringHelpers.WriteStringBuffer(writer, 32, Item.Alias);
                StringHelpers.WriteStringBuffer(writer, 32, Item.Name);
                writer.Write((uint)Item.Flags);
                writer.Write(Item.Scale);
                writer.Write(Item.OffsetX);
                writer.Write(Item.OffsetY);
                writer.Write((uint)Item.Platform);
                writer.Write((uint)Item.Language);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GuiFontMapTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<GuiFontMapTable>(Root);
            this.GuiFontMap = TableInformation.GuiFontMap;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GuiFontMap Table";

            foreach (var Item in GuiFontMap)
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
            GuiFontMap = new GuiFontMapItem[Root.Nodes.Count];

            for (int i = 0; i < GuiFontMap.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                GuiFontMapItem Entry = (GuiFontMapItem)ChildNode.Tag;
                GuiFontMap[i] = Entry;
            }
        }
    }
}
