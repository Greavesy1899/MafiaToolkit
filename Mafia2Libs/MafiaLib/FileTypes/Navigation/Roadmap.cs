using SharpDX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;
//roadmap research
//https://media.discordapp.net/attachments/464158725079564303/468180499806945310/unknown.png?width=1202&height=676
//https://media.discordapp.net/attachments/464158725079564303/468180681646931969/unknown.png?width=1442&height=474

//green = main road
//blue = parking
//yellow = optional road, the AI knows its there, but not direct.
//281
//311
//317

//195
//198
//199
//    1 = 
//2 = 
//4 = 
//8 = 
//16 = 
//32 = main road?
//64 = highway?

namespace ResourceTypes.Navigation
{
    public struct SplineDefinition
    {
        //12 bytes max!
        public uint offset;
        public byte unk0; //knowing 2k, just 128.
        public short NumSplines1; //should be equal to unk2
        public short NumSplines2;
        public float roadLength;
        public Vector3[] points;

        public SplineProperties toward;
        public SplineProperties backward;
        public bool hasToward;
        public bool hasBackward;
        public int idxOffset;

        public void SetSplineData(SplineProperties data, int offset)
        {
            if (data.Flags.HasFlag(RoadFlags.BackwardDirection))
            {
                backward = data;
                hasBackward = true;
            }
            else
            {
                toward = data;
                hasToward = true;
            }

            idxOffset = offset;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", offset, unk0, NumSplines1, NumSplines2, roadLength);
        }
    }

    public class SplineProperties
    {
        private ushort laneIDX0;
        private ushort laneIDX1;
        private uint offset0;
        private ushort laneSize0;
        private ushort laneSize1;
        private uint offset1;
        private ushort rangeSize0;
        private ushort rangeSize1;
        private RoadFlags flags;
        private ushort splineIDX;
        private LaneProperties[] lanes;
        private RangeProperties[] ranges;

        [Browsable(false)]
        public ushort LaneIDX0 {
            get { return laneIDX0; }
            set { laneIDX0 = value; }
        }
        [Browsable(false)]
        public ushort LaneIDX1 {
            get { return laneIDX1; }
            set { laneIDX1 = value; }
        }
        [Browsable(false)]
        public uint Offset0 {
            get { return offset0; }
            set { offset0 = value; }
        }
        [Browsable(false)]
        public ushort LaneSize0 {
            get { return laneSize0; }
            set { laneSize0 = value; }
        }
        [Browsable(false)]
        public ushort LaneSize1 {
            get { return laneSize1; }
            set { laneSize1 = value; }
        }
        [Browsable(false)]
        public uint Offset1 {
            get { return offset1; }
            set { offset1 = value; }
        }
        [Browsable(false)]
        public ushort RangeSize0 {
            get { return rangeSize0; }
            set { rangeSize0 = value; }
        }
        [Browsable(false)]
        public ushort RangeSize1 {
            get { return rangeSize1; }
            set { rangeSize1 = value; }
        }

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public RoadFlags Flags {
            get { return flags; }
            set { flags = value; }
        }
        [ReadOnly(true)]
        public ushort SplineIDX {
            get { return splineIDX; }
            set { splineIDX = value; }
        }
        public LaneProperties[] Lanes {
            get { return lanes; }
            set { lanes = value; }
        }
        public RangeProperties[] Ranges {
            get { return ranges; }
            set { ranges = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}, {6}, {7}, {8}, {9}", laneIDX0, laneIDX1, offset0, laneSize0, laneSize1, offset1, rangeSize0, rangeSize1, flags, splineIDX);
        }
    }

    public class LaneProperties
    {
        //16 bytes
        private float width;
        private LaneTypes flags;
        private ushort unk03;
        private uint rangeOffset;
        private ushort rangeSize0;
        private ushort rangeSize1;
        private RangeProperties[] ranges; 

        public float Width {
            get { return width; }
            set { width = value; }
        }

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public LaneTypes Flags {
            get { return flags; }
            set { flags = value; }
        }
        public ushort Unk03 {
            get { return unk03; }
            set { unk03 = value; }
        }
        [Browsable(false)]
        public uint RangeOffset {
            get { return rangeOffset; }
            set { rangeOffset = value; }
        }

        [Browsable(false)]
        public ushort RangeSize0 {
            get { return rangeSize0; }
            set { rangeSize0 = value; }
        }

        [Browsable(false)]
        public ushort RangeSize1 {
            get { return rangeSize1; }
            set { rangeSize1 = value; }
        }

        public RangeProperties[] Ranges {
            get { return ranges; }
            set { ranges = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", width, flags, unk03, rangeOffset, rangeSize0, rangeSize1);
        }
    }

    public class RangeProperties
    {
        //16 bytes
        private float unk01;
        private float unk02;
        private short unk03;
        private short unk04;
        private float unk05;

        public float Unk01 {
            get { return unk01; }
            set { unk01 = value; }
        }
        public float Unk02 {
            get { return unk02; }
            set { unk02 = value; }
        }
        public short Unk03 {
            get { return unk03; }
            set { unk03 = value; }
        }
        public short Unk04 {
            get { return unk04; }
            set { unk04 = value; }
        }
        public float Unk05 {
            get { return unk05; }
            set { unk05 = value; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", unk01, unk02, unk03, unk04, unk05);
        }
    }

    public struct JunctionDefinition
    {
        public Vector3 position;
        public uint offset0;
        public short junctionSize0;
        public short junctionSize1;
        public uint offset1;
        public short boundarySize0;
        public short boundarySize1;
        public int junctionIdx;
        public uint offset2;
        public short unk5;
        public short unk6;
        public Vector3[] boundaries; //lines, not a box.
        public JunctionSpline[] splines;
        public unkStruct2Sect2 dataSet2;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", offset0, junctionSize0, junctionSize1, offset1, boundarySize0, boundarySize1, junctionIdx, offset2, unk5, unk6);
        }
    }

    public struct JunctionSpline
    {
        public short unk0;
        public short unk1;
        public short unk2;
        public short unk3;
        public short unk4;
        public short unk5;
        public uint offset0;
        public short pathSize0;
        public short pathSize1;
        public float catmullMod;
        public Vector3[] path;
        public unkStruct2Sect2 unk11;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", unk0, unk1, unk2, unk3, unk4, unk5, unk4, offset0, unk5, pathSize0, pathSize1, catmullMod);
        }
    }

    public struct unkStruct2Sect2
    {
        public int unk0;
        public uint offset0;
        public short unk1;
        public short unk2;
        public int unk3;
        public uint offset1;
        public short unk4;
        public short unk5;
        public short unk6;
        public short unk7;
        public short unk8;
        public short unk9;
        public int unk10_0;
        public int unk10_1;
        public byte[] unk3Bytes;
    }

    public struct unkStruct3
    {
        public ushort unk0;
        public ushort unk1;

        public override string ToString()
        {
            return string.Format("{0} {1}", unk0, unk1);
        }
    }

    public class Roadmap
    {
        public int magic;
        public ushort splineCount;
        public uint splineOffset;
        public ushort splinePropertiesCount;
        public uint splinePropertiesOffset;
        public ushort junctionPropertiesCount;
        public uint junctionPropertiesOffset;
        public ushort unkDataSet3Count;
        public uint unkDataSet3Offset;
        public ushort unkDataSet4Count;
        public uint unkDataSet4Offset;
        public ushort unkDataSet5Count;
        public uint unkDataSet5Offset;
        public ushort unkDataSet6Count;

        public SplineDefinition[] splines;
        public SplineProperties[] splineData;
        public JunctionDefinition[] junctionData;
        public ushort[] unkSet3;
        public unkStruct3[] unkSet4;
        public ushort[] unkSet5;
        public ushort[] unkSet6;

        private FileInfo info;

        public Roadmap(FileInfo info)
        {
            this.info = info;
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            WriteToFile();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            magic = reader.ReadInt32();
            splineCount = reader.ReadUInt16();
            short splineCount2 = reader.ReadInt16();
            splineOffset = reader.ReadInt24();
            reader.ReadByte();
            splinePropertiesCount = reader.ReadUInt16();
            short splinePropertiesCount2 = reader.ReadInt16();
            splinePropertiesOffset = reader.ReadInt24();
            reader.ReadByte();
            junctionPropertiesCount = reader.ReadUInt16();
            short junctionPropertiesCount2 = reader.ReadInt16();
            junctionPropertiesOffset = reader.ReadInt24();
            reader.ReadByte();
            unkDataSet3Count = reader.ReadUInt16();
            short unkDataSet3Count2 = reader.ReadInt16();
            unkDataSet3Offset = reader.ReadInt24();
            reader.ReadByte();
            unkDataSet4Count = reader.ReadUInt16();
            short unkDataSet4Count2 = reader.ReadInt16();
            unkDataSet4Offset = reader.ReadInt24();
            reader.ReadByte();
            unkDataSet5Count = reader.ReadUInt16();
            short unkDataSet5Count2 = reader.ReadInt16();
            unkDataSet5Offset = reader.ReadInt24();
            reader.ReadByte();
            unkDataSet6Count = reader.ReadUInt16();
            short unkDataSet6Count2 = reader.ReadInt16();

            int count1 = reader.ReadInt32();
            splines = new SplineDefinition[splineCount];

            for (int i = 0; i != splineCount; i++)
            {
                SplineDefinition data = new SplineDefinition();
                data.offset = reader.ReadInt24();
                data.unk0 = reader.ReadByte();
                data.NumSplines1 = reader.ReadInt16();
                data.NumSplines2 = reader.ReadInt16();
                data.roadLength = reader.ReadSingle();
                splines[i] = data;
            }

            for (int i = 0; i != splineCount; i++)
            {
                SplineDefinition data = splines[i];
                data.points = new Vector3[splines[i].NumSplines1];
                reader.BaseStream.Position = splines[i].offset - 4;

                for (int y = 0; y != splines[i].NumSplines1; y++)
                    data.points[y] = Vector3Extenders.ReadFromFile(reader);

                splines[i] = data;
            }

            splineData = new SplineProperties[splinePropertiesCount];
            for (int i = 0; i != splineData.Length; i++)
            {
                SplineProperties data = new SplineProperties();
                data.LaneIDX0 = reader.ReadUInt16();
                data.LaneIDX1 = reader.ReadUInt16();
                data.Offset0 = reader.ReadInt24();
                reader.ReadByte();
                data.LaneSize0 = reader.ReadUInt16();
                data.LaneSize1 = reader.ReadUInt16();
                data.Offset1 = reader.ReadInt24();
                reader.ReadByte();
                data.RangeSize0 = reader.ReadUInt16();
                data.RangeSize1 = reader.ReadUInt16();
                data.Flags = (RoadFlags)reader.ReadInt16();
                data.SplineIDX = reader.ReadUInt16();
                splineData[i] = data;
            }

            for (int i = 0; i != splinePropertiesCount; i++)
            {
                SplineProperties data = splineData[i];
                data.Lanes = new LaneProperties[data.LaneSize1];

                for (int y = 0; y != data.LaneSize1; y++)
                {
                    LaneProperties sect = new LaneProperties();
                    sect.Width = reader.ReadSingle();
                    sect.Flags = (LaneTypes)reader.ReadUInt16();
                    sect.Unk03 = reader.ReadUInt16();
                    sect.RangeOffset = reader.ReadInt24();
                    reader.ReadByte();
                    sect.RangeSize0 = reader.ReadUInt16();
                    sect.RangeSize1 = reader.ReadUInt16();

                    data.Lanes[y] = sect;
                }

                for (int y = 0; y != data.LaneSize1; y++)
                {
                    if (data.Lanes[y].RangeSize0 > 0)
                        data.Lanes[y].Ranges = new RangeProperties[data.Lanes[y].RangeSize0];

                    for(int x = 0; x != data.Lanes[y].RangeSize0; x++)
                    {
                        RangeProperties sect = new RangeProperties();
                        sect.Unk01 = reader.ReadSingle();
                        sect.Unk02 = reader.ReadSingle();
                        sect.Unk03 = reader.ReadInt16();
                        sect.Unk04 = reader.ReadInt16();
                        sect.Unk05 = reader.ReadSingle();
                        data.Lanes[y].Ranges[x] = sect;
                    }
                }

                data.Ranges = new RangeProperties[data.RangeSize0];

                for (int y = 0; y != data.RangeSize0; y++)
                {
                    RangeProperties sect = new RangeProperties();
                    sect.Unk01 = reader.ReadSingle();
                    sect.Unk02 = reader.ReadSingle();
                    sect.Unk03 = reader.ReadInt16();
                    sect.Unk04 = reader.ReadInt16();
                    sect.Unk05 = reader.ReadSingle();
                    data.Ranges[y] = sect;
                }

                splineData[i] = data;

                if (data.SplineIDX > 4096 && data.SplineIDX < 4128)
                {
                    splines[data.SplineIDX - 4096].SetSplineData(data, 4096);
                }
                else if (data.SplineIDX > 24576 && data.SplineIDX < 25332)
                {
                    splines[data.SplineIDX - 24576].SetSplineData(data, 24576);
                }
                else if (data.SplineIDX > 16384 && data.SplineIDX < 16900)
                {
                    splines[data.SplineIDX - 16384].SetSplineData(data, 16384);
                }
                else if (data.SplineIDX > 32768 && data.SplineIDX < 36864)
                {
                    splines[data.SplineIDX - 32768].SetSplineData(data, 32768);
                }
                else if (data.SplineIDX > 36864)
                {
                    splines[data.SplineIDX - 36864].SetSplineData(data, 36864);
                }
                else
                {
                    splines[data.SplineIDX].SetSplineData(data, 0);
                }
            }

            junctionData = new JunctionDefinition[junctionPropertiesCount];
            for (int i = 0; i != junctionPropertiesCount; i++)
            {
                JunctionDefinition data = new JunctionDefinition();
                data.position = Vector3Extenders.ReadFromFile(reader);
                data.offset0 = reader.ReadInt24();
                reader.ReadByte();
                data.junctionSize0 = reader.ReadInt16();
                data.junctionSize1 = reader.ReadInt16();
                data.offset1 = reader.ReadInt24();
                reader.ReadByte();
                data.boundarySize0 = reader.ReadInt16();
                data.boundarySize1 = reader.ReadInt16();
                data.junctionIdx = reader.ReadInt32();
                data.offset2 = reader.ReadInt24();
                reader.ReadByte();
                data.unk5 = reader.ReadInt16();
                data.unk6 = reader.ReadInt16();
                junctionData[i] = data;
            }

            for (int i = 0; i != junctionPropertiesCount; i++)
            {
                junctionData[i].splines = new JunctionSpline[junctionData[i].junctionSize0];

                for (int y = 0; y != junctionData[i].junctionSize0; y++)
                {
                    JunctionSpline data4Sect = new JunctionSpline();
                    data4Sect.unk0 = reader.ReadInt16();
                    data4Sect.unk1 = reader.ReadInt16();
                    data4Sect.unk2 = reader.ReadInt16();
                    data4Sect.unk3 = reader.ReadInt16();
                    data4Sect.unk4 = reader.ReadInt16();
                    data4Sect.unk5 = reader.ReadInt16();
                    data4Sect.offset0 = reader.ReadInt24();
                    reader.ReadByte();
                    data4Sect.pathSize0 = reader.ReadInt16();
                    data4Sect.pathSize1 = reader.ReadInt16();
                    data4Sect.catmullMod = reader.ReadSingle();
                    junctionData[i].splines[y] = data4Sect;
                }
                for (int y = 0; y != junctionData[i].junctionSize0; y++)
                {
                    junctionData[i].splines[y].path = new Vector3[junctionData[i].splines[y].pathSize0];

                    for (int z = 0; z != junctionData[i].splines[y].pathSize1; z++)
                    {
                        junctionData[i].splines[y].path[z] = Vector3Extenders.ReadFromFile(reader);
                    }
                }

                junctionData[i].boundaries = new Vector3[junctionData[i].boundarySize0];
                for (int y = 0; y != junctionData[i].boundarySize0; y++)
                {
                    junctionData[i].boundaries[y] = Vector3Extenders.ReadFromFile(reader);
                }

                if (junctionData[i].unk5 >= 2)
                {
                    junctionData[i].dataSet2 = new unkStruct2Sect2();
                    junctionData[i].dataSet2.unk0 = reader.ReadInt32();
                    junctionData[i].dataSet2.offset0 = reader.ReadInt24();
                    reader.ReadByte();
                    junctionData[i].dataSet2.unk1 = reader.ReadInt16();
                    junctionData[i].dataSet2.unk2 = reader.ReadInt16();
                    junctionData[i].dataSet2.unk3 = reader.ReadInt32();
                    junctionData[i].dataSet2.offset1 = reader.ReadInt24();
                    reader.ReadByte();
                    junctionData[i].dataSet2.unk4 = reader.ReadInt16();
                    junctionData[i].dataSet2.unk5 = reader.ReadInt16();
                    junctionData[i].dataSet2.unk6 = reader.ReadInt16();
                    junctionData[i].dataSet2.unk7 = reader.ReadInt16();
                    junctionData[i].dataSet2.unk8 = reader.ReadInt16();
                    junctionData[i].dataSet2.unk9 = reader.ReadInt16();

                    if (junctionData[i].dataSet2.unk1 > 2 && junctionData[i].dataSet2.unk2 > 2)
                    {
                        if (junctionData[i].dataSet2.unk1 == 4)
                        {
                            junctionData[i].dataSet2.unk10_0 = reader.ReadInt32();
                            junctionData[i].dataSet2.unk10_1 = -1;
                        }
                        else
                        {
                            junctionData[i].dataSet2.unk10_0 = reader.ReadInt32();
                            junctionData[i].dataSet2.unk10_1 = reader.ReadInt32();
                        }
                    }


                    if (junctionData[i].unk5 == 3)
                        junctionData[i].dataSet2.unk3Bytes = reader.ReadBytes(16);
                }
            }

            unkSet3 = new ushort[unkDataSet3Count];
            unkSet4 = new unkStruct3[unkDataSet4Count];
            unkSet5 = new ushort[unkDataSet5Count];
            unkSet6 = new ushort[unkDataSet6Count];

            for (int i = 0; i < unkDataSet3Count; i++)
                unkSet3[i] = reader.ReadUInt16();

            for (int i = 0; i < unkDataSet4Count; i++)
            {
                unkSet4[i].unk0 = reader.ReadUInt16();
                unkSet4[i].unk1 = reader.ReadUInt16();
            }

            for (int i = 0; i < unkDataSet5Count; i++)
                unkSet5[i] = reader.ReadUInt16();

            for (int i = 0; i < unkDataSet6Count; i++)
                unkSet6[i] = reader.ReadUInt16();

            int highest = -1;

            for (int i = 0; i < unkDataSet3Count; i++)
            {
                if (unkSet3[i] > highest)
                    highest = unkSet3[i];
            }

            int highestp1 = -1;
            int highestp2 = -1;
            int idxp2 = -1;

            for (int i = 0; i < unkDataSet4Count; i++)
            {
                if (unkSet4[i].unk0 > highestp1)
                    highestp1 = unkSet4[i].unk0;

                if (unkSet4[i].unk1 > highestp2)
                {
                    highestp2 = unkSet4[i].unk1;
                    idxp2 = i;
                }
            }
        }

        public void WriteToFile()
        {
            DebugCheckLineIDX();
            UpdateData();
            using (BinaryWriter writer = new BinaryWriter(File.Open(info.FullName, FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }
        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(magic);
            writer.Write(splineCount);
            writer.Write(splineCount);
            writer.WriteInt24(splineOffset);
            writer.Write(splinePropertiesCount);
            writer.Write(splinePropertiesCount);
            writer.WriteInt24(splinePropertiesOffset);
            writer.Write(junctionPropertiesCount);
            writer.Write(junctionPropertiesCount);
            writer.WriteInt24(junctionPropertiesOffset);
            writer.Write(unkDataSet3Count);
            writer.Write(unkDataSet3Count);
            writer.WriteInt24(unkDataSet3Offset);
            writer.Write(unkDataSet4Count);
            writer.Write(unkDataSet4Count);
            writer.WriteInt24(unkDataSet4Offset);
            writer.Write(unkDataSet5Count);
            writer.Write(unkDataSet5Count);
            writer.WriteInt24(unkDataSet5Offset);
            writer.Write(unkDataSet6Count);
            writer.Write(unkDataSet6Count);
            writer.Write((int)unkDataSet3Count);

            long[] positions = new long[splineCount];
            for (int i = 0; i < splineCount; i++)
            {
                SplineDefinition data = splines[i];
                positions[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.offset);
                writer.Write(data.NumSplines1);
                writer.Write(data.NumSplines2);
                writer.Write(data.roadLength);
            }

            List<SplineProperties> splineList = new List<SplineProperties>();

            for (int i = 0; i < splineCount; i++)
            {
                long position = writer.BaseStream.Position;
                writer.BaseStream.Seek(positions[i], SeekOrigin.Begin);
                writer.WriteInt24((uint)position + 4);
                writer.BaseStream.Seek(position, SeekOrigin.Begin);
                SplineDefinition splineData = splines[i];
                for (int y = 0; y != splineData.points.Length; y++)
                    Vector3Extenders.WriteToFile(splineData.points[y], writer);

                if (splineData.hasToward)
                    splineList.Add(splineData.toward);

                if (splineData.hasBackward)
                    splineList.Add(splineData.backward);
            }

            WriteUpdatedOffset(writer, 8);
            positions = new long[splineData.Length]; //lane offset
            long[] pos2 = new long[splineData.Length]; //range offset

            for (int i = 0; i != splineList.Count; i++)
            {
                SplineProperties data = splineData[i];
                writer.Write(data.LaneIDX0);
                writer.Write(data.LaneIDX1);
                positions[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.Offset0);
                writer.Write(data.LaneSize0);
                writer.Write(data.LaneSize1);
                pos2[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.Offset1);
                writer.Write(data.RangeSize0);
                writer.Write(data.RangeSize1);
                writer.Write((short)data.Flags);
                writer.Write(data.SplineIDX);
            }

            long[] pos3;

            for (int i = 0; i < splineList.Count; i++)
            {
                SplineProperties data = splineData[i];
                //update lane position
                long curPosition = writer.BaseStream.Position;
                writer.BaseStream.Seek(positions[i], SeekOrigin.Begin);
                writer.WriteInt24((uint)curPosition + 4);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished
                //also do rangeoffset
                pos3 = new long[data.LaneSize0];

                for (int y = 0; y < data.LaneSize1; y++)
                {
                    LaneProperties sect = splineData[i].Lanes[y];
                    writer.Write(sect.Width);
                    writer.Write((ushort)sect.Flags);
                    writer.Write(sect.Unk03);
                    //writer.Write((ushort)0);
                    pos3[y] = writer.BaseStream.Position;
                    writer.WriteInt24(sect.RangeOffset);
                    writer.Write(sect.RangeSize0);
                    writer.Write(sect.RangeSize1);
                }

                for (int y = 0; y < data.LaneSize0; y++)
                {
                    ////update range for this lane
                    curPosition = writer.BaseStream.Position;
                    uint rangeOffset = (uint)(data.Lanes[y].RangeSize0 > 0 ? curPosition + 4 : 0);
                    writer.BaseStream.Seek(pos3[y], SeekOrigin.Begin);
                    writer.WriteInt24(rangeOffset);
                    writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                    //finished

                    for (int x = 0; x < data.Lanes[y].RangeSize0; x++)
                    {
                        RangeProperties sect = data.Lanes[y].Ranges[x];
                        writer.Write(sect.Unk01);
                        writer.Write(sect.Unk02);
                        writer.Write(sect.Unk03);
                        writer.Write(sect.Unk04);
                        writer.Write(sect.Unk05);
                    }
                }

                //update range position
                curPosition = writer.BaseStream.Position;
                uint laneOffset = (uint)(data.RangeSize0 > 0 ? curPosition + 4 : 0);
                writer.BaseStream.Seek(pos2[i], SeekOrigin.Begin);
                writer.WriteInt24(laneOffset);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished

                for (int y = 0; y < data.RangeSize0; y++)
                {
                    RangeProperties sect = data.Ranges[y];
                    writer.Write(sect.Unk01);
                    writer.Write(sect.Unk02);
                    writer.Write(sect.Unk03);
                    writer.Write(sect.Unk04);
                    writer.Write(sect.Unk05);
                }
            }

            WriteUpdatedOffset(writer, 16);

            positions = new long[junctionPropertiesCount];
            pos2 = new long[junctionPropertiesCount];
            pos3 = new long[junctionPropertiesCount];
            for (int i = 0; i < junctionPropertiesCount; i++)
            {
                JunctionDefinition data = junctionData[i];
                Vector3Extenders.WriteToFile(data.position, writer);
                positions[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.offset0);
                writer.Write(data.junctionSize0);
                writer.Write(data.junctionSize1);
                pos2[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.offset1);
                writer.Write(data.boundarySize0);
                writer.Write(data.boundarySize1);
                writer.Write(data.junctionIdx);
                pos3[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.offset2);
                writer.Write(data.unk5);
                writer.Write(data.unk6);
            }

            for (int i = 0; i < junctionPropertiesCount; i++)
            {
                //update junction position
                long curPosition = writer.BaseStream.Position;
                uint junctionOffset = (uint)(junctionData[i].junctionSize0 > 0 ? curPosition + 4 : 0);
                writer.BaseStream.Seek(positions[i], SeekOrigin.Begin);
                writer.WriteInt24(junctionOffset);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished

                for (int y = 0; y < junctionData[i].junctionSize0; y++)
                {
                    JunctionSpline data4Sect = junctionData[i].splines[y];
                    writer.Write(data4Sect.unk0);
                    writer.Write(data4Sect.unk1);
                    writer.Write(data4Sect.unk2);
                    writer.Write(data4Sect.unk3);
                    writer.Write(data4Sect.unk4);
                    writer.Write(data4Sect.unk5);
                    writer.WriteInt24(data4Sect.offset0);
                    writer.Write(data4Sect.pathSize0);
                    writer.Write(data4Sect.pathSize1);
                    writer.Write(data4Sect.catmullMod);
                }
                for (int y = 0; y < junctionData[i].junctionSize0; y++)
                {
                    for (int z = 0; z != junctionData[i].splines[y].pathSize1; z++)
                    {
                        Vector3Extenders.WriteToFile(junctionData[i].splines[y].path[z], writer);
                    }
                }

                //update boundary position
                curPosition = writer.BaseStream.Position;
                uint boundaryOffset = (uint)(junctionData[i].boundarySize0 > 0 ? curPosition + 4 : 0);
                writer.BaseStream.Seek(pos2[i], SeekOrigin.Begin);
                writer.WriteInt24(boundaryOffset);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished

                for (int y = 0; y < junctionData[i].boundarySize0; y++)
                {
                    Vector3Extenders.WriteToFile(junctionData[i].boundaries[y], writer);
                }

                //update unk position
                curPosition = writer.BaseStream.Position;
                uint unkOffset = (uint)(junctionData[i].unk5 > 0 ? curPosition + 4 : 0);
                writer.BaseStream.Seek(pos3[i], SeekOrigin.Begin);
                writer.WriteInt24(unkOffset);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished

                if (junctionData[i].unk5 >= 2)
                {
                    unkStruct2Sect2 data = junctionData[i].dataSet2;

                    writer.Write(data.unk0);
                    writer.WriteInt24(data.offset0);
                    writer.Write(data.unk1);
                    writer.Write(data.unk2);
                    writer.Write(data.unk3);
                    writer.WriteInt24(data.offset1);
                    writer.Write(data.unk4);
                    writer.Write(data.unk5);
                    writer.Write(data.unk6);
                    writer.Write(data.unk7);
                    writer.Write(data.unk8);
                    writer.Write(data.unk9);

                    if (data.unk1 > 2 && data.unk2 > 2)
                    {
                        if (data.unk1 == 4)
                        {
                            writer.Write(data.unk10_0);
                        }
                        else
                        {
                            writer.Write(data.unk10_0);
                            writer.Write(data.unk10_1);
                        }
                    }


                    if (junctionData[i].unk5 == 3)
                        writer.Write(data.unk3Bytes);
                }
            }

            WriteUpdatedOffset(writer, 24);

            //related to junctions.
            for (int i = 0; i < unkDataSet3Count; i++)
                writer.Write(unkSet3[i]);

            WriteUpdatedOffset(writer, 32);

            for (int i = 0; i < unkDataSet4Count; i++)
            {
                writer.Write(unkSet4[i].unk0);
                writer.Write(unkSet4[i].unk1);
            }

            WriteUpdatedOffset(writer, 40);

            for (int i = 0; i < unkDataSet5Count; i++)
                writer.Write(unkSet5[i]);

            WriteUpdatedOffset(writer, 48);

            for (int i = 0; i < unkDataSet6Count; i++)
                writer.Write(unkSet6[i]);
        }

        private void WriteUpdatedOffset(BinaryWriter writer, long pos, bool isZero = false)
        {
            long jump = writer.BaseStream.Position;
            writer.BaseStream.Seek(pos, SeekOrigin.Begin);
            writer.WriteInt24(isZero ? 0 : (uint)jump + 4);
            writer.BaseStream.Seek(jump, SeekOrigin.Begin);
        }

        public void DebugCheckLineIDX()
        {
            int correct = 0;
            for(int i = 0; i != splineData.Length; i++)
            {
                int count = 0;
                SplineProperties data = splineData[i];
                
                foreach(var lane in data.Lanes)
                {
                    if (lane.Flags != 0)
                        count++;
                    else if (data.Flags.HasFlag(RoadFlags.flag_16))
                        count++;
                }

                if(count == Math.Abs(data.LaneIDX0 - data.LaneIDX1))
                {
                    correct++;
                }
                else
                {
                    Console.WriteLine("[" + i + "] " + count + " vs " + Math.Abs(data.LaneIDX0 - data.LaneIDX1));
                }
            }
            Console.WriteLine(correct + "/" + splineData.Length + " = " + Convert.ToSingle(correct / splineData.Length));
        }

        public void UpdateData()
        {
            for (int i = 0; i < splines.Length; i++)
            {
                SplineDefinition data = splines[i];
                float curDistance = 0.0f;

                //int idx1 = -1;
                //int idx2 = -1;

                //if(data.hasToward && data.hasBackward)
                //{
                //    idx2 += (data.toward.laneSize0 - 1);
                //    data.toward.laneIDX0 = (ushort)idx1;
                //    data.toward.laneIDX1 = (ushort)idx2;
                //}
                //else
                //{
                //    idx1 += (data.toward.laneSize0 - 1);
                //    data.toward.laneIDX0 = (ushort)idx1;
                //    data.toward.laneIDX1 = ushort.MaxValue;
                //}

                for (int x = 0; x < data.NumSplines1; x++)
                {
                    float temp = 0.0f;

                    if (x < data.NumSplines1 - 1)
                    {
                        temp += Vector3.Distance(data.points[x], data.points[x + 1]);
                    }
                    if (x > 0)
                    {
                        temp += Vector3.Distance(data.points[x], data.points[x - 1]);
                    }
                    curDistance += temp;
                }
                data.roadLength = curDistance/2;
                splines[i] = data;
            }

            splineCount = (ushort)splines.Length;


        }
    }
}
