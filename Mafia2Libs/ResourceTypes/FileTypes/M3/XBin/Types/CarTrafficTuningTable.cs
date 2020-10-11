using ResourceTypes.FileTypes.M3.XBin;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;
using Utils.Settings;

namespace ResourceTypes.M3.XBin
{
    public class CarTrafficTuningItem
    {
        public int ID { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public int CollectionOffset { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public int CollectionCount1 { get; set; }
        [Browsable(false), PropertyIgnoreByReflector]
        public int CollectionCount2 { get; set; }
        public int[] TuningItems { get; set; }
        public int VehicleID { get; set; }
        public ETrafficVehicleFlags VehicleFlags { get; set; } //E_TrafficVehicleFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ETrafficVehicleLookFlags VehicleLookFlags { get; set; } //E_TrafficVehicleLookFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public float Weight { get; set; }
        public ulong NameHash { get; set; }

        public override string ToString()
        {
            return string.Format("ID = {0}", ID);
        }
    }

    public class CarTrafficTuningTable : BaseTable
    {
        private CarTrafficTuningItem[] traffic_tunings;
        private GamesEnumerator gameVersion;

        public CarTrafficTuningItem[] TrafficTuning {
            get { return traffic_tunings; }
            set { traffic_tunings = value; }
        }

        public CarTrafficTuningTable()
        {
            traffic_tunings = new CarTrafficTuningItem[0];
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            uint unknown = reader.ReadUInt32();
            traffic_tunings = new CarTrafficTuningItem[count0];

            for (int i = 0; i < count1; i++)
            {
                CarTrafficTuningItem item = new CarTrafficTuningItem();
                item.ID = reader.ReadInt32();
                item.CollectionOffset = reader.ReadInt32();
                item.CollectionCount1 = reader.ReadInt32();
                item.CollectionCount2 = reader.ReadInt32();
                item.VehicleID = reader.ReadInt32();
                item.VehicleFlags = (ETrafficVehicleFlags)reader.ReadInt32();
                item.VehicleLookFlags = (ETrafficVehicleLookFlags)reader.ReadInt32();
                item.Weight = reader.ReadSingle();
                item.NameHash = reader.ReadUInt64();

                traffic_tunings[i] = item;
            }

            for (int i = 0; i < count1; i++)
            {
                var item = traffic_tunings[i];
                item.TuningItems = new int[item.CollectionCount1];
                for (int z = 0; z < item.CollectionCount1; z++)
                {
                    item.TuningItems[z] = reader.ReadInt32();
                }
                traffic_tunings[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(traffic_tunings.Length);
            writer.Write(traffic_tunings.Length);
            writer.Write(0);

            int i = 0;
            int k = traffic_tunings.Length * 40 - 4; // 40 entry size
            foreach (var tuning in traffic_tunings)
            {
                CarTrafficTuningItem Item = traffic_tunings[i];
                //updating offset and count :)
                Item.CollectionOffset = k;
                Item.CollectionCount1 = Item.TuningItems.Length;
                Item.CollectionCount2 = Item.TuningItems.Length;
                writer.Write(Item.ID);
                writer.Write(Item.CollectionOffset);
                writer.Write(Item.TuningItems.Length);
                writer.Write(Item.TuningItems.Length);
                writer.Write(Item.VehicleID);
                writer.Write((int)Item.VehicleFlags);
                writer.Write((int)Item.VehicleLookFlags);
                writer.Write(Item.Weight);
                writer.Write(Item.NameHash);
                i++;
                k -= Item.TuningItems.Length * sizeof(int);
            }

            for (int j = 0; j < traffic_tunings.Length; j++)
            {
                CarTrafficTuningItem Item = traffic_tunings[j];
                for (int z = 0; z < Item.CollectionCount1; z++)
                {
                    writer.Write(Item.TuningItems[z]);
                }
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            CarTrafficTuningTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<CarTrafficTuningTable>(Root);
            this.traffic_tunings = TableInformation.traffic_tunings;
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

            foreach(var Item in TrafficTuning)
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
