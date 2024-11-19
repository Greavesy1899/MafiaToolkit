using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.M3.XBin
{
    public class CarTrafficTuningItem
    {
        [PropertyForceAsAttribute]
        public int ID { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public int CollectionOffset { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public int CollectionCount1 { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public int CollectionCount2 { get; set; }
        public int[] TuningItems { get; set; }
        [PropertyForceAsAttribute]
        public int VehicleID { get; set; }
        public ETrafficVehicleFlags_M3 VehicleFlags { get; set; } //E_TrafficVehicleFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(UITypeEditor))]
        public ETrafficVehicleLookFlags_M3 VehicleLookFlags { get; set; } //E_TrafficVehicleLookFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(UITypeEditor))]
        public float Weight { get; set; }
        public XBinHashName Name { get; set; }

        public override string ToString()
        {
            return string.Format("ID = {0}", ID);
        }
    }

    public class CarTrafficTuningTable : BaseTable
    {
        private uint unk0;
        private CarTrafficTuningItem[] Items;

        public CarTrafficTuningItem[] TrafficTuning {
            get { return Items; }
            set { Items = value; }
        }

        public CarTrafficTuningTable()
        {
            Items = new CarTrafficTuningItem[0];
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint padding = reader.ReadUInt32();
            Items = new CarTrafficTuningItem[count0];

            for (int i = 0; i < count1; i++)
            {
                CarTrafficTuningItem item = new CarTrafficTuningItem();
                item.ID = reader.ReadInt32();
                item.CollectionOffset = reader.ReadInt32();
                item.CollectionCount1 = reader.ReadInt32();
                item.CollectionCount2 = reader.ReadInt32();
                item.VehicleID = reader.ReadInt32();
                item.VehicleFlags = (ETrafficVehicleFlags_M3)reader.ReadUInt32();
                item.VehicleLookFlags = (ETrafficVehicleLookFlags_M3)reader.ReadUInt32();
                item.Weight = reader.ReadSingle();
                item.Name = XBinHashName.ConstructAndReadFromFile(reader);

                Items[i] = item;
            }

            for (int i = 0; i < count1; i++)
            {
                var item = Items[i];
                item.TuningItems = new int[item.CollectionCount1];
                for (int z = 0; z < item.CollectionCount1; z++)
                {
                    item.TuningItems[z] = reader.ReadInt32();
                }
                Items[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(Items.Length);
            writer.Write(Items.Length);
            writer.Write(0); // padding

            int i = 0;
            long[] offsets = new long[Items.Length];
            foreach (var tuning in Items)
            {
                CarTrafficTuningItem Item = Items[i];
                Item.CollectionCount1 = Item.TuningItems.Length;
                Item.CollectionCount2 = Item.TuningItems.Length;
                writer.Write(Item.ID);
                offsets[i] = writer.BaseStream.Position;
                writer.Write(0xDEADBEEF); //placeholder
                writer.Write(Item.TuningItems.Length);
                writer.Write(Item.TuningItems.Length);
                writer.Write(Item.VehicleID);
                writer.Write((int)Item.VehicleFlags);
                writer.Write((int)Item.VehicleLookFlags);
                writer.Write(Item.Weight);
                Item.Name.WriteToFile(writer);
                i++;
            }

            for (int j = 0; j < Items.Length; j++)
            {
                CarTrafficTuningItem Item = Items[j];
                uint thisPosition = (uint)(writer.BaseStream.Position);

                for (int z = 0; z < Item.CollectionCount1; z++)
                {
                    writer.Write(Item.TuningItems[z]);
                }

                long currentPosition = writer.BaseStream.Position;
                writer.BaseStream.Position = offsets[j];
                var offset = (uint)(thisPosition - offsets[j]);
                writer.Write(offset);
                writer.BaseStream.Position = currentPosition;
            }
            offsets = new long[0];
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarTrafficTuningTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarTrafficTuningTable>(Root);
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
            Root.Text = "CarTrafficTuningTable";

            foreach (var Item in TrafficTuning)
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
            TrafficTuning = new CarTrafficTuningItem[Root.Nodes.Count];

            for (int i = 0; i < TrafficTuning.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                CarTrafficTuningItem Entry = (CarTrafficTuningItem)ChildNode.Tag;
                TrafficTuning[i] = Entry;
            }
        }
    }
}
