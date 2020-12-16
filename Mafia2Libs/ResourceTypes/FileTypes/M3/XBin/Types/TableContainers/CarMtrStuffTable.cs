using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class CarMtrStuffTable : BaseTable
    {
        public class CarMtrStuffItem
        {
            public uint ID { get; set; }
            public string MaterialName { get; set; }
            public ECarMtrStuffFlags Flags { get; set; }
            public float DirtSpeedMin { get; set; }
            public float DirtSpeedMax { get; set; }
            public float DirtCoeff { get; set; }
            public float TemperaturePercentCoeff { get; set; }

            public CarMtrStuffItem()
            {
                MaterialName = "";
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", ID, MaterialName);
            }
        }

        private uint unk0;
        public CarMtrStuffItem[] CarWindowTints { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            CarWindowTints = new CarMtrStuffItem[count1];

            for (int i = 0; i < CarWindowTints.Length; i++)
            {
                CarMtrStuffItem Item = new CarMtrStuffItem();
                Item.ID = reader.ReadUInt32();
                Item.MaterialName = StringHelpers.ReadStringBuffer(reader, 32);
                Item.Flags = (ECarMtrStuffFlags)reader.ReadUInt32();
                Item.DirtSpeedMin = reader.ReadSingle();
                Item.DirtSpeedMax = reader.ReadSingle();
                Item.DirtCoeff = reader.ReadSingle();
                Item.TemperaturePercentCoeff = reader.ReadSingle();
                CarWindowTints[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(CarWindowTints.Length);
            writer.Write(CarWindowTints.Length);

            for (int i = 0; i < CarWindowTints.Length; i++)
            {
                CarMtrStuffItem Item = CarWindowTints[i];
                writer.Write(Item.ID);
                StringHelpers.WriteStringBuffer(writer, 32, Item.MaterialName);
                writer.Write((uint)Item.Flags);
                writer.Write(Item.DirtSpeedMin);
                writer.Write(Item.DirtSpeedMax);
                writer.Write(Item.DirtCoeff);
                writer.Write(Item.TemperaturePercentCoeff);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarMtrStuffTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarMtrStuffTable>(Root);
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
            Root.Text = "CarMtrStuff Table";

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
            CarWindowTints = new CarMtrStuffItem[Root.Nodes.Count];

            for (int i = 0; i < CarWindowTints.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarMtrStuffItem Entry = (CarMtrStuffItem)ChildNode.Tag;
                CarWindowTints[i] = Entry;
            }
        }
    }
}
