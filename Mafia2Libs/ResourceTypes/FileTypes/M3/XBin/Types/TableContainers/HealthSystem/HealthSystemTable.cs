using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin.TableContainers.HealthSystem
{
    public partial class HealthSystemTable : BaseTable
    {
        private uint unk0; // 16
        private uint unk1; // 0
        
        public HealthSystemItem[] Items { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint Count0 = reader.ReadUInt32();
            uint Count1 = reader.ReadUInt32();
            unk1 = reader.ReadUInt32();

            Items = new HealthSystemItem[Count0];
            for(int i = 0; i < Count0; i++)
            {
                HealthSystemItem NewItem = new HealthSystemItem();
                NewItem.ReadFromFile(reader);
                Items[i] = NewItem;
            }

            for(int i = 0; i < Count0; i++)
            {
                HealthSystemItem SystemItem = Items[i];
                SystemItem.ReadHealthSegments(reader);
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            throw new NotImplementedException();
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            HealthSystemTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<HealthSystemTable>(Root);
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
            Root.Text = "HealthSystems Table";

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
            Items = new HealthSystemItem[Root.Nodes.Count];

            for (int i = 0; i < Items.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                HealthSystemItem Entry = (HealthSystemItem)ChildNode.Tag;
                Items[i] = Entry;
            }
        }
    }
}
