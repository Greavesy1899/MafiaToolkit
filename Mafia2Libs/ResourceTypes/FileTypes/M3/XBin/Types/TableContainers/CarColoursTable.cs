using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public class CarColoursTable : BaseTable
    {
        public class CarColoursItem
        {
            public uint ID { get; set; }
            public string ColorNameIndex { get; set; }
            public byte Red { get; set; }
            public byte Green { get; set; }
            public byte Blue { get; set; }
            public string ColorName { get; set; }
            public string SpeechDarkLight { get; set; }
            public string PoliceComm { get; set; }
            public byte Unk0 { get; set; } // Unknown, maybe always zero? Could be for padding too.

            public CarColoursItem()
            {
                ColorNameIndex = "";
                ColorName = "";
                SpeechDarkLight = "";
                PoliceComm = "";
            }

            public override string ToString()
            {
                return string.Format("{0} - {1}", ID, ColorNameIndex);
            }
        }

        private int unk0;
        public CarColoursItem[] CarColours { get; set; }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadInt32();
            uint count1 = reader.ReadUInt32();
            uint count2 = reader.ReadUInt32();
            CarColours = new CarColoursItem[count1];

            for(int i = 0; i < CarColours.Length; i++)
            {
                CarColoursItem CarColour = new CarColoursItem();
                CarColour.ID = reader.ReadUInt32();
                CarColour.ColorNameIndex = StringHelpers.ReadStringBuffer(reader, 8);
                CarColour.Red = reader.ReadByte();
                CarColour.Green = reader.ReadByte();
                CarColour.Blue = reader.ReadByte();
                CarColour.ColorName = StringHelpers.ReadStringBuffer(reader, 16);
                CarColour.SpeechDarkLight = StringHelpers.ReadStringBuffer(reader, 32);
                CarColour.PoliceComm = StringHelpers.ReadStringBuffer(reader, 32);
                CarColour.Unk0 = reader.ReadByte();
                CarColours[i] = CarColour;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(CarColours.Length);
            writer.Write(CarColours.Length);

            for(int i = 0; i < CarColours.Length; i++)
            {
                CarColoursItem Item = CarColours[i];
                writer.Write(Item.ID);
                StringHelpers.WriteStringBuffer(writer, 8, Item.ColorNameIndex);
                writer.Write(Item.Red);
                writer.Write(Item.Green);
                writer.Write(Item.Blue);
                StringHelpers.WriteStringBuffer(writer, 16, Item.ColorName);
                StringHelpers.WriteStringBuffer(writer, 32, Item.SpeechDarkLight);
                StringHelpers.WriteStringBuffer(writer, 32, Item.PoliceComm);
                writer.Write(Item.Unk0);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarColoursTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarColoursTable>(Root);
            this.CarColours = TableInformation.CarColours;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CarColours Table";

            foreach (var Item in CarColours)
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
            CarColours = new CarColoursItem[Root.Nodes.Count];

            for (int i = 0; i < CarColours.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarColoursItem Entry = (CarColoursItem)ChildNode.Tag;
                CarColours[i] = Entry;
            }
        }
    }
}
