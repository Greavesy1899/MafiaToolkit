using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class PhoneBookTable : BaseTable
    {
        public class PhoneBookItem
        {
            public uint ID { get; set; }
            public int Number { get; set; }
            public int NameTextID { get; set; }
            public string Location { get; set; } // size = 32
            public string Dialogue { get; set; } // size = 64
            public string DialogueFSM { get; set; } // size = 32

            public PhoneBookItem()
            {
                Location = "";
                Dialogue = "";
                DialogueFSM = "";
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", Number, NameTextID);
            }
        }

        private uint unk0;
        public PhoneBookItem[] Items { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            Items = new PhoneBookItem[count1];

            for (int i = 0; i < Items.Length; i++)
            {
                PhoneBookItem Item = new PhoneBookItem();
                Item.ID = reader.ReadUInt32();
                Item.Number = reader.ReadInt32();
                Item.NameTextID = reader.ReadInt32();
                Item.Location = StringHelpers.ReadStringBuffer(reader, 32);
                Item.Dialogue = StringHelpers.ReadStringBuffer(reader, 64);
                Item.DialogueFSM = StringHelpers.ReadStringBuffer(reader, 32);
                Items[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(Items.Length);
            writer.Write(Items.Length);

            for (int i = 0; i < Items.Length; i++)
            {
                PhoneBookItem Item = Items[i];
                writer.Write(Item.ID);
                writer.Write(Item.Number);
                writer.Write(Item.NameTextID);
                StringHelpers.WriteStringBuffer(writer, 32, Item.Location);
                StringHelpers.WriteStringBuffer(writer, 64, Item.Dialogue);
                StringHelpers.WriteStringBuffer(writer, 32, Item.DialogueFSM);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            PhoneBookTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<PhoneBookTable>(Root);
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
            Root.Text = "PhoneBook Table";

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
            Items = new PhoneBookItem[Root.Nodes.Count];

            for (int i = 0; i < Items.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                PhoneBookItem Entry = (PhoneBookItem)ChildNode.Tag;
                Items[i] = Entry;
            }
        }
    }
}
