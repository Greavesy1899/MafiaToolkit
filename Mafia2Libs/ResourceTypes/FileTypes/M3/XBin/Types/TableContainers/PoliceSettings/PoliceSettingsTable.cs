using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public partial class PoliceSettingsTable : BaseTable
    {
        private uint unk0;
        public PoliceSettingsItem[] Items { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            Items = new PoliceSettingsItem[count1];

            for (int i = 0; i < Items.Length; i++)
            {
                PoliceSettingsItem Item = new PoliceSettingsItem();
                Item.ReadFromFile(reader);
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(Items.Length);
            writer.Write(Items.Length);

            for (int i = 0; i < Items.Length; i++)
            {
                PoliceSettingsItem Item = Items[i];
                writer.Write(Item.ID);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            PoliceSettingsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<PoliceSettingsTable>(Root);
            this.Items = TableInformation.Items;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "PoliceSettings Table";

            foreach (var Item in Items)
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
            Items = new PoliceSettingsItem[Root.Nodes.Count];

            for (int i = 0; i < Items.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                PoliceSettingsItem Entry = (PoliceSettingsItem)ChildNode.Tag;
                Items[i] = Entry;
            }
        }
    }
}
