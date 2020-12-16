using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class CarWindowTintTable : BaseTable
    {
        public class CarWindowTintItem
        {
            public uint ID { get; set; }
            public string Description { get; set; }
            public byte Red { get; set; }
            public byte Green { get; set; }
            public byte Blue { get; set; }
            public byte Alpha { get; set; }

            public CarWindowTintItem()
            {
                Description = "";
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", ID, Description);
            }
        }

        private uint unk0;
        public CarWindowTintItem[] CarWindowTints { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            CarWindowTints = new CarWindowTintItem[count1];

            for (int i = 0; i < CarWindowTints.Length; i++)
            {
                CarWindowTintItem CarWindowTint = new CarWindowTintItem();
                CarWindowTint.ID = reader.ReadUInt32();
                CarWindowTint.Description = StringHelpers.ReadStringBuffer(reader, 32);
                CarWindowTint.Red = reader.ReadByte();
                CarWindowTint.Green = reader.ReadByte();
                CarWindowTint.Blue = reader.ReadByte();
                CarWindowTint.Alpha = reader.ReadByte();
                CarWindowTints[i] = CarWindowTint;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(CarWindowTints.Length);
            writer.Write(CarWindowTints.Length);

            for (int i = 0; i < CarWindowTints.Length; i++)
            {
                CarWindowTintItem Item = CarWindowTints[i];
                writer.Write(Item.ID);
                StringHelpers.WriteStringBuffer(writer, 32, Item.Description);
                writer.Write(Item.Red);
                writer.Write(Item.Green);
                writer.Write(Item.Blue);
                writer.Write(Item.Alpha);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarWindowTintTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarWindowTintTable>(Root);
            this.CarWindowTints = TableInformation.CarWindowTints;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CarWindowTint Table";

            foreach (var Item in CarWindowTints)
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
            CarWindowTints = new CarWindowTintItem[Root.Nodes.Count];

            for (int i = 0; i < CarWindowTints.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarWindowTintItem Entry = (CarWindowTintItem)ChildNode.Tag;
                CarWindowTints[i] = Entry;
            }
        }
    }
}
