using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;
using Utils.Settings;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.Vehicles
{
    public class VehicleTableItem_M1 : IVehicleTableItem
    {
        public int Unk0 { get; set; }
        [PropertyForceAsAttribute]
        public int ID { get; set; }
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ETrafficCommonFlags_M1 CommonFlags { get; set; } //E_TrafficCommonFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ETrafficVehicleFlags_M1 VehicleFlags { get; set; } //E_TrafficVehicleFlags
        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public ETrafficVehicleLookFlags_M1 VehicleLookFlags { get; set; } //E_TrafficVehicleLookFlags
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

        public VehicleTableItem_M1()
        {
            ModelName = "";
            BrandNameUI = "";
            ModelNameUI = "";
            LogoNameUI = "";
        }

        public void ReadEntry(BinaryReader reader)
        {
            Unk0 = reader.ReadInt32();
            ID = reader.ReadInt32();
            CommonFlags = (ETrafficCommonFlags_M1)reader.ReadInt32();
            VehicleFlags = (ETrafficVehicleFlags_M1)reader.ReadInt32();
            VehicleLookFlags = (ETrafficVehicleLookFlags_M1)reader.ReadInt32();
            VehicleFunctionFlags = (EVehiclesTableFunctionFlags)reader.ReadInt32();
            ModelName = StringHelpers.ReadStringBuffer(reader, 32);
            SoundVehicleSwitch = StringHelpers.ReadStringBuffer(reader, 32);
            RadioRandom = (ERadioStation)reader.ReadInt32();
            RadioSoundQuality = reader.ReadSingle();
            TexID = reader.ReadInt32();
            TexHash = reader.ReadUInt64();
            BrandNameUI = StringHelpers.ReadStringBuffer(reader, 32);
            ModelNameUI = StringHelpers.ReadStringBuffer(reader, 32);
            LogoNameUI = StringHelpers.ReadStringBuffer(reader, 32);
            StealKoeficient = reader.ReadInt32();
            Price = reader.ReadInt32();
            MinDirt = reader.ReadSingle();
            MinRust = reader.ReadSingle();
            MaxDirt = reader.ReadSingle();
            MaxRust = reader.ReadSingle();
            RaceClass = (EVehicleRaceClass)reader.ReadInt32();
            TrunkDockOffsetX = reader.ReadSingle();
            TrunkDockOffsetY = reader.ReadSingle();
        }

        public void WriteEntry(XBinWriter writer)
        {
            writer.Write(Unk0);
            writer.Write(ID);
            writer.Write((int)CommonFlags);
            writer.Write((int)VehicleFlags);
            writer.Write((int)VehicleLookFlags);
            writer.Write((int)VehicleFunctionFlags);
            StringHelpers.WriteStringBuffer(writer, 32, ModelName);
            StringHelpers.WriteStringBuffer(writer, 32, SoundVehicleSwitch);
            writer.Write((int)RadioRandom);
            writer.Write(RadioSoundQuality);
            writer.Write(TexID);
            writer.Write(TexHash);
            StringHelpers.WriteStringBuffer(writer, 32, BrandNameUI);
            StringHelpers.WriteStringBuffer(writer, 32, ModelNameUI);
            StringHelpers.WriteStringBuffer(writer, 32, LogoNameUI);
            writer.Write(StealKoeficient);
            writer.Write(Price);
            writer.Write(MinDirt);
            writer.Write(MinRust);
            writer.Write(MaxDirt);
            writer.Write(MaxRust);
            writer.Write((int)RaceClass);
            writer.Write(TrunkDockOffsetX);
            writer.Write(TrunkDockOffsetY);
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", ID, ModelName);
        }
    }
}
