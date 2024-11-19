using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceTypes.XBin.Types;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.TableContainers.HealthSystem
{
    public partial class HealthSystemTable : BaseTable
    {
        public class HealthSegment
        {
            public float[] Regeneration { get; set; }
            public float[] DamageTimeout { get; set; }
            public float[] Size { get; set; }
            public bool[] DamageGoesOver { get; set; }
            public float[] NoDmgTimeAfterSegmentDown { get; set; }
            public XBinHashName[] EffectName { get; set; }
            public float[] EffectMaxOpacity { get; set; }
            public bool IsArmour { get; set; }

            public HealthSegment()
            {
                Regeneration = new float[0];
                DamageTimeout = new float[0];
                Size = new float[0];
                DamageGoesOver = new bool[0];
                NoDmgTimeAfterSegmentDown = new float[0];
                EffectName = new XBinHashName[0];
                EffectMaxOpacity = new float[0];
            }
            public void ReadFromFile(BinaryReader reader)
            {
                uint RegenerationOffset = reader.ReadUInt32();
                uint NumRegeneration = reader.ReadUInt32();
                uint NumRegeneration2 = reader.ReadUInt32();
                Regeneration = new float[NumRegeneration];

                uint DamageTimeoutOffset = reader.ReadUInt32();
                uint NumDamageTimeout0 = reader.ReadUInt32();
                uint NumDamageTimeout1 = reader.ReadUInt32();
                DamageTimeout = new float[NumDamageTimeout0];

                uint SizeOffset = reader.ReadUInt32();
                uint NumSizeElements0 = reader.ReadUInt32();
                uint NumSizeElements1 = reader.ReadUInt32();
                Size = new float[NumSizeElements0];

                uint DamageGoesOverOffset = reader.ReadUInt32();
                uint NumDamageGoesOver = reader.ReadUInt32();
                uint NumDamageGoesOver2 = reader.ReadUInt32();
                DamageGoesOver = new bool[NumDamageGoesOver];

                uint NoDmgTimeAfterSegmentDownOffset = reader.ReadUInt32();
                uint NumNoDmgTimeAfterSegmentDown0 = reader.ReadUInt32();
                uint NumNoDmgTimeAfterSegmentDown1 = reader.ReadUInt32();
                NoDmgTimeAfterSegmentDown = new float[NumNoDmgTimeAfterSegmentDown0];

                uint EffectNameOffset = reader.ReadUInt32();
                uint NumEffectName0 = reader.ReadUInt32();
                uint NumEffectName1 = reader.ReadUInt32();
                EffectName = new XBinHashName[NumEffectName0];

                uint EffectMaxOpacityOffset = reader.ReadUInt32();
                uint NumEffectMaxOpacity0 = reader.ReadUInt32();
                uint NumEffectMaxOpacity1 = reader.ReadUInt32();
                EffectMaxOpacity = new float[NumEffectMaxOpacity0];

                IsArmour = Convert.ToBoolean(reader.ReadUInt32());
            }

            public void ReadArrayData(BinaryReader reader)
            {
                // store the data from the file
                for (int i = 0; i < Regeneration.Length; i++)
                {

                }

                for (int i = 0; i < DamageTimeout.Length; i++)
                {

                }

                for (int i = 0; i < Size.Length; i++)
                {
                    Size[i] = reader.ReadSingle();
                }

                for (int i = 0; i < DamageGoesOver.Length; i++)
                {

                }

                for (int i = 0; i < NoDmgTimeAfterSegmentDown.Length; i++)
                {

                }

                for (int i = 0; i < EffectName.Length; i++)
                {
                    XBinHashName NewHashName = new XBinHashName();
                    NewHashName.ReadFromFile(reader);
                    EffectName[i] = NewHashName;
                }

                for (int i = 0; i < EffectMaxOpacity.Length; i++)
                {
                    EffectMaxOpacity[i] = reader.ReadSingle();
                }
            }

            public override string ToString()
            {
                return "Health Segment";
            }
        }

        public class HealthSystemItem
        {
            public HealthSegment[] HealthSegments { get; set; }
            public XBinVector2[] DamageLimiterGraph { get; set; }
            public XBinVector2[] DangerBarDistanceMults { get; set; }
            public float DefaultRegeneration { get; set; }
            public float DefaultCriticalSize { get; set; }
            public float DefaultDamageTimeout { get; set; }
            public float DefaultSize { get; set; }
            public bool DefaultDamageGoesOver { get; set; }
            public float DefaultNoDmgTimeAfterSegmentDown { get; set; }
            public string Name { get; set; }
            public float RegOnTakedownAmount { get; set; }
            public float RegOnTakedownTime { get; set; }
            public bool RegOnTakedownExceedSegment { get; set; }
            public ushort SegmentsCntToChangeColor { get; set; }
            public XBinHashName DefaultEffectName { get; set; }
            public float DefaultEffectMaxOpacity { get; set; }
            public float DangerBarSize { get; set; }
            public float DangerBarRegenerationDelay { get; set; }
            public float DangerBarRegenerationTime { get; set; }
            public float LethalDmgClampMultiplier { get; set; }
            public float AfterClampNoDmgTime { get; set; }

            public HealthSystemItem()
            {
                HealthSegments = new HealthSegment[0];
                DamageLimiterGraph = new XBinVector2[0];
                DangerBarDistanceMults = new XBinVector2[0];
                Name = "";
                DefaultEffectName = new XBinHashName();
            }

            public void ReadFromFile(BinaryReader Reader)
            {
                uint SegmentOffset = Reader.ReadUInt32();
                uint NumSegments = Reader.ReadUInt32();
                uint NumSegments2 = Reader.ReadUInt32();
                HealthSegments = new HealthSegment[NumSegments];

                uint DamageLimiterGraphOffset = Reader.ReadUInt32();
                uint NumGraphElements0 = Reader.ReadUInt32();
                uint NumGraphElements1 = Reader.ReadUInt32();
                DamageLimiterGraph = new XBinVector2[NumGraphElements0];

                uint DamageBarDistanceOffset = Reader.ReadUInt32();
                uint NumDamageBarDistanceElements0 = Reader.ReadUInt32();
                uint NumDamageBarDistanceElements1 = Reader.ReadUInt32();
                DangerBarDistanceMults = new XBinVector2[NumDamageBarDistanceElements0];

                DefaultRegeneration = Reader.ReadSingle();
                DefaultCriticalSize = Reader.ReadSingle();
                DefaultDamageTimeout = Reader.ReadSingle();
                DefaultSize = Reader.ReadSingle();
                DefaultDamageGoesOver = Convert.ToBoolean(Reader.ReadInt32());
                DefaultNoDmgTimeAfterSegmentDown = Reader.ReadSingle();
                Name = StringHelpers.ReadStringBuffer(Reader, 32).TrimEnd('\0');
                RegOnTakedownAmount = Reader.ReadSingle();
                RegOnTakedownTime = Reader.ReadSingle();
                RegOnTakedownExceedSegment = Convert.ToBoolean(Reader.ReadInt16());
                SegmentsCntToChangeColor = Reader.ReadUInt16();
                DefaultEffectName.ReadFromFile(Reader);
                DefaultEffectMaxOpacity = Reader.ReadSingle();
                DangerBarSize = Reader.ReadSingle();
                DangerBarRegenerationDelay = Reader.ReadSingle();
                DangerBarRegenerationTime = Reader.ReadSingle();
                LethalDmgClampMultiplier = Reader.ReadSingle();
                AfterClampNoDmgTime = Reader.ReadSingle();
            }

            public void ReadHealthSegments(BinaryReader reader)
            {
                for(int i = 0; i < HealthSegments.Length; i++)
                {
                    HealthSegment SegmentItem = new HealthSegment();
                    SegmentItem.ReadFromFile(reader);
                    HealthSegments[i] = SegmentItem;
                }

                for (int i = 0; i < HealthSegments.Length; i++)
                {
                    HealthSegment SegmentItem = HealthSegments[i];
                    SegmentItem.ReadArrayData(reader);
                }

                for(int i = 0; i < DamageLimiterGraph.Length; i++)
                {
                    XBinVector2 NewVector = new XBinVector2();
                    NewVector.ReadFromFile(reader);
                    DamageLimiterGraph[i] = NewVector;
                }

                for (int i = 0; i < DangerBarDistanceMults.Length; i++)
                {
                    XBinVector2 NewVector = new XBinVector2();
                    NewVector.ReadFromFile(reader);
                    DangerBarDistanceMults[i] = NewVector;
                }
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
