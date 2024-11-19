using ResourceTypes.XBin.Types;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers
{
    public partial class PoliceSettingsTable
    {
        public class PoliceSettingsItem
        {
            public uint ID { get; set; }
            [Description("This is curve of yellow zone size (y) in wanted level (x).")]
            public XBinVector2[] YellowZoneGrowth { get; set; }
            [Description("This is curve of yellow zone size (y) in wanted level (x).")]
            public XBinVector2[] YellowZoneGrowthInCar { get; set; }
            [Description("This is curve of red zone size (y) in wanted level (x).")]
            public XBinVector2[] RedZoneGrowth { get; set; }
            [Description("This is curve of red zone size (y) in wanted level (x).")]
            public XBinVector2[] RedZoneGrowthInCar { get; set; }
            [Description("This is curve of yellow zone time to decay (y) in wanted level (x)")]
            public XBinVector2[] YellowZoneDecayTime { get; set; }
            [Description("This is curve of red zone time to decay (y) in wanted level (x).")]
            public XBinVector2[] RedZoneDecayTime { get; set; }
            [Description("Points in bar begin to decay after this delay [s]. Won't start unless no offence was committed in this zone")]
            public XBinVector2[] YellowZonePointsDecayDelay { get; set; }
            [Description("Speed in points/sec. Will start after DecayDelay")]
            public XBinVector2[] YellowZonePointsDecaySpeed { get; set; }
            [Description("City name")]
            public XBinHashName City { get; set; }
            [Description("Voice for police dispatch")]
            public XBinHashName DispatchVoice { get; set; }
            [Description("Voice for transmitter")]
            public XBinHashName TransmitterVoice { get; set; }
            [Description("Max distance for spotted by disciplinary offence multiplier to be applied")]
            public float MaxSpotDistance { get; set; }
            [Description("Multiplier for spotted by disciplinary offence when player furthest from policeman")]
            public float SpottedDistMultMin { get; set; }
            [Description("Multiplier for spotted by disciplinary offence when player nearest to policeman")]
            public float SpottedDistMultMax { get; set; }

            public void ReadFromFile(BinaryReader reader)
            {
                ID = reader.ReadUInt32();

                // Read offsets and size of arrays.
                uint YellowZoneGrowthPtr = reader.ReadUInt32();
                uint NumYellowZoneGrowth_0 = reader.ReadUInt32();
                uint NumYellowZoneGrowth_1 = reader.ReadUInt32();
                uint YellowZoneGrowthInCarPtr = reader.ReadUInt32();
                uint NumYellowZoneGrowthInCar_0 = reader.ReadUInt32();
                uint NumYellowZoneGrowthInCar_1 = reader.ReadUInt32();
                uint RedZoneGrowthPtr = reader.ReadUInt32();
                uint NumRedZoneGrowth_0 = reader.ReadUInt32();
                uint NumRedZoneGrowth_1 = reader.ReadUInt32();
                uint RedZoneGrowthInCarPtr = reader.ReadUInt32();
                uint NumRedZoneGrowthInCar_0 = reader.ReadUInt32();
                uint NumRedZoneGrowthInCar_1 = reader.ReadUInt32();
                uint YellowZoneDecayTimePtr = reader.ReadUInt32();
                uint NumYellowZoneDecayTime_0 = reader.ReadUInt32();
                uint NumYellowZoneDecayTime_1 = reader.ReadUInt32();
                uint RedZoneDecayTimePtr = reader.ReadUInt32();
                uint NumRedZoneDecayTime_0 = reader.ReadUInt32();
                uint NumRedZoneDecayTime_1 = reader.ReadUInt32();
                uint YellowZonePointsDecayDelayPtr = reader.ReadUInt32();
                uint NumYellowZonePointsDecayDelay_0 = reader.ReadUInt32();
                uint NumYellowZonePointsDecayDelay_1 = reader.ReadUInt32();
                uint YellowZonePointsDecaySpeedPtr = reader.ReadUInt32();
                uint NumYellowZonePointsDecaySpeed_0 = reader.ReadUInt32();
                uint NumYellowZonePointsDecaySpeed_1 = reader.ReadUInt32();
                uint GroupDefinitionsPtr = reader.ReadUInt32();
                uint NumGroupDefinitions_0 = reader.ReadUInt32();
                uint NumGroupDefinitions_1 = reader.ReadUInt32();

                // Read rest of PoliceSettings
                City = XBinHashName.ConstructAndReadFromFile(reader);
                DispatchVoice = XBinHashName.ConstructAndReadFromFile(reader);
                TransmitterVoice = XBinHashName.ConstructAndReadFromFile(reader);
                MaxSpotDistance = reader.ReadSingle();
                SpottedDistMultMin = reader.ReadSingle();
                SpottedDistMultMax = reader.ReadSingle();

                // padding
                reader.ReadInt32();

                // Start reading array data
                YellowZoneGrowth = ReadVector2Array(reader, NumYellowZoneGrowth_0);
                YellowZoneGrowthInCar = ReadVector2Array(reader, NumYellowZoneGrowthInCar_0);
                RedZoneGrowth = ReadVector2Array(reader, NumRedZoneGrowth_0);
                RedZoneGrowthInCar = ReadVector2Array(reader, NumRedZoneGrowthInCar_0);
                YellowZoneDecayTime = ReadVector2Array(reader, NumYellowZoneDecayTime_0);
                RedZoneDecayTime = ReadVector2Array(reader, NumRedZoneDecayTime_0);
                YellowZonePointsDecayDelay = ReadVector2Array(reader, NumYellowZonePointsDecayDelay_0);
                YellowZonePointsDecaySpeed = ReadVector2Array(reader, NumYellowZonePointsDecaySpeed_0);

                // more padding
                reader.ReadInt32();

                EscalationGroupSettings[] EscalationGroups = new EscalationGroupSettings[NumGroupDefinitions_0];
                for(int i = 0; i < NumGroupDefinitions_0; i++)
                {
                    EscalationGroupSettings EscalationSetting = new EscalationGroupSettings();
                    EscalationSetting.ReadFromFile_Part0(reader);
                    EscalationGroups[i] = EscalationSetting;
                }

                for (int i = 0; i < NumGroupDefinitions_0; i++)
                {
                    EscalationGroups[i].ReadFromFile_Part1(reader);
                }
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(ID);
            }

            private XBinVector2[] ReadVector2Array(BinaryReader reader, uint Length)
            {
                XBinVector2[] VectorArr = new XBinVector2[Length];
                for(int i = 0; i < Length; i++)
                {
                    VectorArr[i] = new XBinVector2();
                    VectorArr[i].ReadFromFile(reader);
                }

                return VectorArr;
            }
        }
    }
}
