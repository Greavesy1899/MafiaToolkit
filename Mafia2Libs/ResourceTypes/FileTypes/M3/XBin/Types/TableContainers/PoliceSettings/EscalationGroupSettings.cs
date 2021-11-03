using ResourceTypes.XBin.Types;
using System;
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
        public class EscalationZoneGroup
        {

        }

        public class EscalationZoneBoatGroup
        {

        }

        public class EscalationGroupSettings
        {
            public EscalationZoneGroup[] YellowGroupList { get; set; }
            public EscalationZoneGroup[] RedGroupList { get; set; }
            public EscalationZoneBoatGroup[] YellowBoatGroupList { get; set; }
            public EscalationZoneBoatGroup[] RedBoatGroupList { get; set; }
            [Description("How many additional points from offences are needed to reach this escalation level")]
            public uint OffenceBarSize { get; set; }
            [Description("Multiplier of the zone size for merge overlap test")]
            public float MergeRadiusMult { get; set; }
            [Description("Which manoeuvres should be used by non-road blocking hunters")]
            public ECarHuntRoleFlags RedZoneCarHuntRoleFlags { get; set; }
            [Description("What should policemen shoot at, nothing = do not shoot")]
            public EPoliceCarShootFlags ShootTo { get; set; }
            public float MultiplierToHitFirstWheel { get; set; }
            public float MultiplierToHitSecondWheel { get; set; }
            public float MultiplierToHitThirdWheel { get; set; }
            public float MultiplierToHitFourthWheel { get; set; }
            public float MultiplierToHitFifthWheel { get; set; }
            public float MultiplierToHitSixthWheel { get; set; }
            public float MultiplierToHitOffender { get; set; }
            public float MultiplierToHitMotor { get; set; }
            [Description("Accuracy will be set to 0 for this amount of time [s] after hitting a wheel")]
            public float AfterDestroyWheelCooldown { get; set; }
            [Description("Accuracy will be set to 0 for this amount of time [s] after hitting a motor")]
            public float AfterDestroyMotor { get; set; }
            [Description("Name of the config in tactics_switching folder.")]
            public XBinHashName TacticsSwitchingConfigName { get; set; }
            [Description("Policeman on foot will not be spawned closer to player than this distance. Used for yellow zone.")]
            public float MinSpawnDistFromPlayerOnFoot { get; set; }
            [Description("Policeman in vehicle will not be spawned closer to player than this distance. Used for yellow zone.")]
            public float MinSpawnDistFromPlayerInVehicle { get; set; }
            [Description("Hunter Aggression (0-1 = SLOW, 1-2 = NORMAL, 2-3 = AGGRESSIVE, 3-4 = PIRATE)")]
            public float Aggression { get; set; }
            [Description("Delay before maneuvers [ms]")]
            public uint HuntManeuverDelay { get; set; }
            [Description("1 - 100% to regulate traffic car density (100% = no regulation)")]
            public float TrafficRegulation { get; set; }
            [Description("Timer started when police loses player to start cold search instead of regular battle search. [s]")]
            public float TimeToColdSearch { get; set; }
            [Description("Instead of white zone, cars will search through the whole zone")]
            public bool UseWholeZoneForVehicleSearch { get; set; }

            public EscalationGroupSettings() 
            {
                YellowGroupList = new EscalationZoneGroup[0];
                RedGroupList = new EscalationZoneGroup[0];
                YellowBoatGroupList = new EscalationZoneBoatGroup[0];
                RedBoatGroupList = new EscalationZoneBoatGroup[0];
                TacticsSwitchingConfigName = new XBinHashName();
            }

            public void ReadFromFile_Part0(BinaryReader reader)
            {
                // Read pointers + size of arrays
                uint YellowGroupListPtr = reader.ReadUInt32();
                uint NumYellowGroupList_0 = reader.ReadUInt32();
                uint NumYellowGroupList_1 = reader.ReadUInt32();
                uint RedGroupListPtr = reader.ReadUInt32();
                uint NumRedGroupList_0 = reader.ReadUInt32();
                uint NumRedGroupList_1 = reader.ReadUInt32();
                uint YellowBoatGroupListPtr = reader.ReadUInt32();
                uint NumYellowBoatGroupList_0 = reader.ReadUInt32();
                uint NumYellowBoatGroupList_1 = reader.ReadUInt32();
                uint RedBoatGroupListPtr = reader.ReadUInt32();
                uint NumRedBoatGroupList_0 = reader.ReadUInt32();
                uint NumRedBoatGroupList_1 = reader.ReadUInt32();

                // Read more class data
                OffenceBarSize = reader.ReadUInt32();
                MergeRadiusMult = reader.ReadSingle();
                RedZoneCarHuntRoleFlags = (ECarHuntRoleFlags)reader.ReadUInt32();
                ShootTo = (EPoliceCarShootFlags)reader.ReadUInt32();
                MultiplierToHitFirstWheel = reader.ReadSingle();
                MultiplierToHitSecondWheel = reader.ReadSingle();
                MultiplierToHitThirdWheel = reader.ReadSingle();
                MultiplierToHitFourthWheel = reader.ReadSingle();
                MultiplierToHitFifthWheel = reader.ReadSingle();
                MultiplierToHitSixthWheel = reader.ReadSingle();
                MultiplierToHitMotor = reader.ReadSingle();
                MultiplierToHitOffender = reader.ReadSingle();
                AfterDestroyWheelCooldown = reader.ReadSingle();
                AfterDestroyMotor = reader.ReadSingle();
                reader.ReadInt32(); // empty?? Check this
                TacticsSwitchingConfigName = XBinHashName.ConstructAndReadFromFile(reader);
                MinSpawnDistFromPlayerOnFoot = reader.ReadSingle();
                MinSpawnDistFromPlayerInVehicle = reader.ReadSingle();
                Aggression = reader.ReadSingle();
                HuntManeuverDelay = reader.ReadUInt32();
                TrafficRegulation = reader.ReadSingle();
                TimeToColdSearch = reader.ReadSingle();
                UseWholeZoneForVehicleSearch = Convert.ToBoolean(reader.ReadUInt32());
            }

            public void ReadFromFile_Part1(BinaryReader reader)
            {

            }

            public void WriteToFile_Part0(XBinWriter writer)
            {

            }

            public void WriteToFile_Part1(XBinWriter writer)
            {

            }
        }
    }
}
