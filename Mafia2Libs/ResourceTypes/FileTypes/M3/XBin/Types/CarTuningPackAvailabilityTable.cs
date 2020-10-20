using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class CarTuningPackAvailabilityItem
    {
        public int ID { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public uint OverrideTuningItemsOffset { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public int TuningItemsCount1 { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public int TuningItemsCount2 { get; set; }
        public int[] TuningItems { get; set; }
        public int VehicleID { get; set; }
        public int Zero { get; set; }
        public ulong PackageName { get; set; }

        public override string ToString()
        {
            return string.Format("ID = {0}", ID);
        }
    }

    public class CarTuningPackAvailabilityTable : BaseTable
    {
        private uint unk0;
        private CarTuningPackAvailabilityItem[] availabilitys;

        public CarTuningPackAvailabilityItem[] PackAvailability {
            get { return availabilitys; }
            set { availabilitys = value; }
        }

        public CarTuningPackAvailabilityTable()
        {
            availabilitys = new CarTuningPackAvailabilityItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint CarTuningPackAvailabilityValue = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint unknown = reader.ReadUInt32();
            availabilitys = new CarTuningPackAvailabilityItem[count0];

            for (int i = 0; i < count1; i++)
            {
                CarTuningPackAvailabilityItem Item = new CarTuningPackAvailabilityItem();
                Item.ID = reader.ReadInt32();
                Item.OverrideTuningItemsOffset = reader.ReadUInt32();
                Item.TuningItemsCount1 = reader.ReadInt32();
                Item.TuningItemsCount2 = reader.ReadInt32();
                Item.VehicleID = reader.ReadInt32();
                Item.Zero = reader.ReadInt32();
                Item.PackageName = reader.ReadUInt64();

                availabilitys[i] = Item;
            }

            for (int i = 0; i < count1; i++)
            {
                var Item = availabilitys[i];
                Item.TuningItems = new int[Item.TuningItemsCount1];
                for (int z = 0; z < Item.TuningItemsCount1; z++)
                {
                    Item.TuningItems[z] = reader.ReadInt32();
                }
                availabilitys[i] = Item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(16);
            writer.Write(availabilitys.Length);
            writer.Write(availabilitys.Length);
            writer.Write(0);

            int i = 0;
            long[] offsets = new long[availabilitys.Length];
            foreach (var availability in availabilitys)
            {
                CarTuningPackAvailabilityItem Item = availabilitys[i];
                writer.Write(Item.ID);
                offsets[i] = writer.BaseStream.Position;
                writer.Write(0); //placeholder
                writer.Write(Item.TuningItemsCount1);
                writer.Write(Item.TuningItemsCount2);
                writer.Write(Item.VehicleID);
                writer.Write(Item.Zero);
                writer.Write(Item.PackageName);
                i++;
            }

            for (int j = 0; j < availabilitys.Length; j++)
            {
                CarTuningPackAvailabilityItem Item = availabilitys[j];

                if (Item.TuningItemsCount1 != 0)
                {
                    uint thisPosition = (uint)(writer.BaseStream.Position);
                    for (int z = 0; z < Item.TuningItemsCount1; z++)
                    {
                        writer.Write(Item.TuningItems[z]);
                    }

                    long currentPosition = writer.BaseStream.Position;
                    writer.BaseStream.Position = offsets[j];
                    var offset = (uint)(thisPosition - offsets[j]);
                    writer.Write(offset);
                    writer.BaseStream.Position = currentPosition;
                }
            }
            offsets = new long[0];
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarTuningPackAvailabilityTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarTuningPackAvailabilityTable>(Root);
            this.availabilitys = TableInformation.availabilitys;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "CarTuningPackAvailability Table";

            foreach(var Item in PackAvailability)
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
            PackAvailability = new CarTuningPackAvailabilityItem[Root.Nodes.Count];

            for (int i = 0; i < PackAvailability.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarTuningPackAvailabilityItem Entry = (CarTuningPackAvailabilityItem)ChildNode.Tag;
                PackAvailability[i] = Entry;
            }
        }
    }
}
