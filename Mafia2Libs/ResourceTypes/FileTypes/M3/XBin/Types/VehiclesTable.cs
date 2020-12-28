using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;
using Utils.Settings;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin
{
    public class VehicleTableItem
    {
        public int Unk0 { get; set; }
        [PropertyForceAsAttribute]
        public int ID { get; set; }
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ETrafficCommonFlags CommonFlags { get; set; } //E_TrafficCommonFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ETrafficVehicleFlags VehicleFlags { get; set; } //E_TrafficVehicleFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ETrafficVehicleLookFlags VehicleLookFlags { get; set; } //E_TrafficVehicleLookFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public EVehiclesTableFunctionFlags VehicleFunctionFlags { get; set; } //E_VehiclesTableFunctionFlags
        [PropertyForceAsAttribute]
        public string ModelName { get; set; }
        public string SoundVehicleSwitch { get; set; }
        public ERadioStation RadioRandom { get; set; } //E_RadioStation
        public float RadioSoundQuality { get; set; }
        public int TexID { get; set; }
        public ulong TexHash { get; set; } //maybe
        public string BrandNameUI { get; set; }
        public string ModelNameUI { get; set; }
        public string LogoNameUI { get; set; }
        public int StealKoeficient { get; set; }
        public int Price { get; set; }
        public float MinDirt { get; set; }
        public float MinRust { get; set; }
        public float MaxDirt { get; set; }
        public float MaxRust { get; set; }
        public EVehicleRaceClass RaceClass { get; set; } //E_VehicleRaceClass
        public float TrunkDockOffsetX { get; set; }
        public float TrunkDockOffsetY { get; set; }

        public VehicleTableItem()
        {
            ModelName = "";
            BrandNameUI = "";
            ModelNameUI = "";
            LogoNameUI = "";
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", ID, ModelName);
        }
    }

    public class VehicleTable : BaseTable
    {
        private uint unk0;
        private VehicleTableItem[] vehicles;
        private GamesEnumerator gameVersion;

        public VehicleTableItem[] Vehicles {
            get { return vehicles; }
            set { vehicles = value; }
        }

        public VehicleTable()
        {
            vehicles = new VehicleTableItem[0];
            gameVersion = GameStorage.Instance.GetSelectedGame().GameType;
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk0 = reader.ReadUInt32();
            uint count0 = reader.ReadUInt32();
            uint count1 = reader.ReadUInt32();
            vehicles = new VehicleTableItem[count0];

            for (int i = 0; i < count1; i++)
            {
                VehicleTableItem item = new VehicleTableItem();
                item.Unk0 = reader.ReadInt32();
                item.ID = reader.ReadInt32();
                item.CommonFlags = (ETrafficCommonFlags)reader.ReadInt32();
                item.VehicleFlags = (ETrafficVehicleFlags)reader.ReadInt32();
                item.VehicleLookFlags = (ETrafficVehicleLookFlags)reader.ReadInt32();
                item.VehicleFunctionFlags = (EVehiclesTableFunctionFlags)reader.ReadInt32();
                item.ModelName = StringHelpers.ReadStringBuffer(reader, 32).TrimEnd('\0');
                item.SoundVehicleSwitch = StringHelpers.ReadStringBuffer(reader, 32).TrimEnd('\0');
                item.RadioRandom = (ERadioStation)reader.ReadInt32();
                item.RadioSoundQuality = reader.ReadSingle();
                item.TexID = reader.ReadInt32();
                item.TexHash = reader.ReadUInt64();
                item.BrandNameUI = StringHelpers.ReadStringBuffer(reader, 32).TrimEnd('\0');
                item.ModelNameUI = StringHelpers.ReadStringBuffer(reader, 32).TrimEnd('\0');
                item.LogoNameUI = StringHelpers.ReadStringBuffer(reader, 32).TrimEnd('\0');
                item.StealKoeficient = reader.ReadInt32();
                item.Price = reader.ReadInt32();

                // No support in M3.
                if (gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    item.MinDirt = reader.ReadSingle();
                    item.MinRust = reader.ReadSingle();
                }

                item.MaxDirt = reader.ReadSingle();
                item.MaxRust = reader.ReadSingle();
                item.RaceClass = (EVehicleRaceClass)reader.ReadInt32();
                item.TrunkDockOffsetX = reader.ReadSingle();
                item.TrunkDockOffsetY = reader.ReadSingle();

                vehicles[i] = item;
            }
        }

        public void WriteToFile(XBinWriter writer)
        {
            writer.Write(unk0);
            writer.Write(vehicles.Length);
            writer.Write(vehicles.Length);

            foreach(var vehicle in vehicles)
            {
                writer.Write(vehicle.Unk0);
                writer.Write(vehicle.ID);
                writer.Write((int)vehicle.CommonFlags);
                writer.Write((int)vehicle.VehicleFlags);
                writer.Write((int)vehicle.VehicleLookFlags);
                writer.Write((int)vehicle.VehicleFunctionFlags);
                StringHelpers.WriteStringBuffer(writer, 32, vehicle.ModelName);
                StringHelpers.WriteStringBuffer(writer, 32, vehicle.SoundVehicleSwitch);
                writer.Write((int)vehicle.RadioRandom);
                writer.Write(vehicle.RadioSoundQuality);
                writer.Write(vehicle.TexID);
                writer.Write(vehicle.TexHash);
                StringHelpers.WriteStringBuffer(writer, 32, vehicle.BrandNameUI);
                StringHelpers.WriteStringBuffer(writer, 32, vehicle.ModelNameUI);
                StringHelpers.WriteStringBuffer(writer, 32, vehicle.LogoNameUI);
                writer.Write(vehicle.StealKoeficient);
                writer.Write(vehicle.Price);

                // No support in m3
                if (gameVersion == GamesEnumerator.MafiaI_DE)
                {
                    writer.Write(vehicle.MinDirt);
                    writer.Write(vehicle.MinRust);
                }

                writer.Write(vehicle.MaxDirt);
                writer.Write(vehicle.MaxRust);
                writer.Write((int)vehicle.RaceClass);
                writer.Write(vehicle.TrunkDockOffsetX);
                writer.Write(vehicle.TrunkDockOffsetY);
            }
        }

        public void ReadFromXML(string file)
        {
            XElement Root = XElement.Load(file);
            VehicleTable TableInformation = ReflectionHelpers.ConvertToPropertyFromXML<VehicleTable>(Root);
            this.vehicles = TableInformation.vehicles;
        }

        public void WriteToXML(string file)
        {
            XElement RootElement = ReflectionHelpers.ConvertPropertyToXML(this);
            RootElement.Save(file, SaveOptions.None);
        }

        public TreeNode GetAsTreeNodes()
        {
            TreeNode Root = new TreeNode();
            Root.Text = "Vehicles Table";

            foreach(var Item in Vehicles)
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
            Vehicles = new VehicleTableItem[Root.Nodes.Count];

            for (int i = 0; i < Vehicles.Length; i++)
            {
                TreeNode ChildNode = Root.Nodes[i];
                VehicleTableItem Entry = (VehicleTableItem)ChildNode.Tag;
                Vehicles[i] = Entry;
            }
        }
    }
}
