using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.GuiContainers
{
    public class GuiLanguageMapTable : BaseTable
    {
        public class GuiLanguageMapItem
        {
            public string LangCode { get; set; }
            [Browsable(false), PropertyIgnoreByReflector]
            public uint DisplayNameOffset { get; set; }
            public string DisplayName { get; set; } //utf-8
            public int HasAudioLayer { get; set; } //bool

            public GuiLanguageMapItem()
            {
                DisplayName = "";
            }

            public override string ToString()
            {
                return string.Format("{0}", DisplayName);
            }
        }

        public GuiLanguageMapItem[] GuiLanguageMap { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            GuiLanguageMap = new GuiLanguageMapItem[count1];

            for (int i = 0; i < GuiLanguageMap.Length; i++)
            {
                GuiLanguageMapItem Item = new GuiLanguageMapItem();
                Item.LangCode = StringHelpers.ReadStringBuffer(reader, 32);
                Item.DisplayNameOffset = reader.ReadUInt32();
                Item.HasAudioLayer = reader.ReadInt32();
                GuiLanguageMap[i] = Item;
            }

            for (int i = 0; i < count1; i++)
            {
                var Item = GuiLanguageMap[i];
                Item.DisplayName = StringHelpers.ReadStringEncoded(reader).TrimEnd('\0');
                GuiLanguageMap[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(GuiLanguageMap.Length);
            writer.Write(GuiLanguageMap.Length);

            long[] offsets = new long[GuiLanguageMap.Length];
            for (int i = 0; i < GuiLanguageMap.Length; i++)
            {
                GuiLanguageMapItem Item = GuiLanguageMap[i];
                StringHelpers.WriteStringBuffer(writer, 32, Item.LangCode);
                offsets[i] = writer.BaseStream.Position;
                writer.Write(0xDEADC0DE); // placeholder
                writer.Write(Item.HasAudioLayer);
            }

            for (int j = 0; j < GuiLanguageMap.Length; j++)
            {
                GuiLanguageMapItem Item = GuiLanguageMap[j];
                uint thisPosition = (uint)(writer.BaseStream.Position);
                StringHelpers.WriteString(writer, Item.DisplayName);

                long currentPosition = writer.BaseStream.Position;
                writer.BaseStream.Position = offsets[j];
                var offset = (uint)(thisPosition - offsets[j]);
                writer.Write(offset);
                writer.BaseStream.Position = currentPosition;
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            GuiLanguageMapTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<GuiLanguageMapTable>(Root);
            this.GuiLanguageMap = TableInformation.GuiLanguageMap;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "GuiLanguageMap Table";

            foreach (var Item in GuiLanguageMap)
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
            GuiLanguageMap = new GuiLanguageMapItem[Root.Nodes.Count];

            for (int i = 0; i < GuiLanguageMap.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                GuiLanguageMapItem Entry = (GuiLanguageMapItem)ChildNode.Tag;
                GuiLanguageMap[i] = Entry;
            }
        }
    }
}
