using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class MaterialsShotsTable : BaseTable
    {
        public class MaterialsShotsItem
        {
            public uint ID { get; set; }
            public string MaterialName { get; set; }
            public uint GUID_Part0 { get; set; }
            public uint GUID_Part1 { get; set; }
            public XBinAkHashName SoundSwitch { get; set; }
            public float Penetration { get; set; }
            public uint Particle { get; set; }
            public uint Decal { get; set; }
            public uint DecalCold { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1}", ID, MaterialName);
            }
        }

        public MaterialsShotsItem[] Items { get; set; }

        private uint unk0;

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();

            Items = new MaterialsShotsItem[count1];
            for (int i = 0; i < Items.Length; i++)
            {
                MaterialsShotsItem Item = new MaterialsShotsItem();
                Item.ID = reader.ReadUInt32();
                Item.MaterialName = StringHelpers.ReadStringBuffer(reader, 32);
                Item.GUID_Part0 = reader.ReadUInt32();
                Item.GUID_Part1 = reader.ReadUInt32();
                Item.SoundSwitch = XBinAkHashName.ConstructAndReadFromFile(reader);
                Item.Penetration = reader.ReadSingle();
                Item.Particle = reader.ReadUInt32();
                Item.Decal = reader.ReadUInt32();
                Item.DecalCold = reader.ReadUInt32();
                Items[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(Items.Length);
            writer.Write(Items.Length);

            foreach (var Item in Items)
            {
                writer.Write(Item.ID);
                StringHelpers.WriteStringBuffer(writer, 32, Item.MaterialName);
                writer.Write(Item.GUID_Part0);
                writer.Write(Item.GUID_Part1);
                Item.SoundSwitch.WriteToFile(writer);
                writer.Write(Item.Penetration);
                writer.Write(Item.Particle);
                writer.Write(Item.Decal);
                writer.Write(Item.DecalCold);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            MaterialsShotsTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<MaterialsShotsTable>(Root);
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
            Root.Text = "MaterialsShots Table";

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
            Items = new MaterialsShotsItem[Items.Length];
            for (int i = 0; i < Items.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                MaterialsShotsItem Entry = (MaterialsShotsItem)ChildNode.Tag;
                Items[i] = Entry;
            }
        }
    }
}
