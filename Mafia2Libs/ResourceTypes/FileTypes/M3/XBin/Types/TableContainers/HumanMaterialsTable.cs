using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class HumanMaterialsTable : BaseTable
    {
        public class HumanMaterialsItem
        {
            public uint ID { get; set; }
            public string MaterialName { get; set; }
            public EHumanMaterialsTableItemFlags Flags { get; set; }
            public uint SoundMaterialSwitch { get; set; } // Actually C_AKHashName
            public uint StepParticles { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", ID, MaterialName, Flags);
            }
        }

        public HumanMaterialsItem[] Items { get; set; }

        private uint unk0;

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();

            Items = new HumanMaterialsItem[count1];
            for(int i = 0; i < Items.Length; i++)
            {
                HumanMaterialsItem Item = new HumanMaterialsItem();
                Item.ID = reader.ReadUInt32();
                Item.MaterialName = StringHelpers.ReadStringBuffer(reader, 32);
                Item.Flags = (EHumanMaterialsTableItemFlags)reader.ReadUInt32();
                Item.SoundMaterialSwitch = reader.ReadUInt32();
                Item.StepParticles = reader.ReadUInt32();
                Items[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(Items.Length);
            writer.Write(Items.Length);

            foreach(var Item in Items)
            {
                writer.Write(Item.ID);
                StringHelpers.WriteStringBuffer(writer, 32, Item.MaterialName);
                writer.Write((uint)Item.Flags);
                writer.Write(Item.SoundMaterialSwitch);
                writer.Write(Item.StepParticles);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            HumanMaterialsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<HumanMaterialsTable>(Root);
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
            Root.Text = "HumanMaterials Table";

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
            Items = new HumanMaterialsItem[Items.Length];
            for (int i = 0; i < Items.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                HumanMaterialsItem Entry = (HumanMaterialsItem)ChildNode.Tag;
                Items[i] = Entry;
            }
        }
    }
}
