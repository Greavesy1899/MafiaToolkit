using SharpDX;
using System;
using System.Collections.Generic;
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

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", offset, unk0, NumSplines1, NumSplines2, roadLength);
        }
    }

    public struct SplineProperties
    {
        public ushort unk0;
        public ushort unk1;
        public uint offset0;
        public ushort laneSize0;
        public ushort laneSize1;
        public uint offset1;
        public ushort rangeSize0;
        public ushort rangeSize1;
        public RoadFlags flags;
        public ushort unk3;
        public unkStruct1Sect1[] lanes;
        public unkStruct1Sect2[] ranges;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}, {6}, {7}, {8}, {9}", unk0, unk1, offset0, laneSize0, laneSize1, offset1, rangeSize0, rangeSize1, flags, unk3);
        }
    }

    public struct unkStruct1Sect1
    {
        //16 bytes
        public float unk01;
        public LaneTypes flags;
        public ushort unk03;
        public uint unk04;
        public ushort unk05;
        public ushort unk06;
        public unkStruct1Sect1[] children; //SOMETIMES this is the case.

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", unk01, flags, unk03, unk04, unk05, unk06);
        }
    }

    public struct unkStruct1Sect2
    {
        //16 bytes
        public float unk01;
        public float unk02;
        public short unk03;
        public short unk04;
        public float unk05;


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

        public SplineDefinition[] data1;
        public SplineProperties[] data3;
        public JunctionDefinition[] data4;
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
            data1 = new SplineDefinition[splineCount];

            for (int i = 0; i != splineCount; i++)
            {
                SplineDefinition data = new SplineDefinition();
                data.offset = reader.ReadInt24();
                data.unk0 = reader.ReadByte();
                data.NumSplines1 = reader.ReadInt16();
                data.NumSplines2 = reader.ReadInt16();
                data.roadLength = reader.ReadSingle();
                data1[i] = data;
            }

            for (int i = 0; i != splineCount; i++)
            {
                SplineDefinition data = data1[i];
                data.points = new Vector3[data1[i].NumSplines1];
                reader.BaseStream.Position = data1[i].offset - 4;

                for (int y = 0; y != data1[i].NumSplines1; y++)
                    data.points[y] = Vector3Extenders.ReadFromFile(reader);

                data1[i] = data;
            }

            data3 = new SplineProperties[splinePropertiesCount];
            for (int i = 0; i != data3.Length; i++)
            {
                SplineProperties data = new SplineProperties();
                data.unk0 = reader.ReadUInt16();
                data.unk1 = reader.ReadUInt16();
                data.offset0 = reader.ReadInt24();
                reader.ReadByte();
                data.laneSize0 = reader.ReadUInt16();
                data.laneSize1 = reader.ReadUInt16();
                data.offset1 = reader.ReadInt24();
                reader.ReadByte();
                data.rangeSize0 = reader.ReadUInt16();
                data.rangeSize1 = reader.ReadUInt16();
                data.flags = (RoadFlags)reader.ReadInt16();
                data.unk3 = reader.ReadUInt16();
                data3[i] = data;
            }

            for (int i = 0; i != splinePropertiesCount; i++)
            {
                SplineProperties data = data3[i];
                data.lanes = new unkStruct1Sect1[data.laneSize1];

                for (int y = 0; y != data.laneSize1; y++)
                {
                    unkStruct1Sect1 sect = new unkStruct1Sect1();
                    sect.unk01 = reader.ReadSingle();
                    sect.flags = (LaneTypes)reader.ReadUInt16();
                    sect.unk03 = reader.ReadUInt16();
                    sect.unk04 = reader.ReadInt24();
                    reader.ReadByte();
                    sect.unk05 = reader.ReadUInt16();
                    sect.unk06 = reader.ReadUInt16();

                    sect.children = new unkStruct1Sect1[sect.unk05];
                    for (int x = 0; x != sect.unk05; x++)
                    {
                        unkStruct1Sect1 child = new unkStruct1Sect1();
                        child.unk01 = reader.ReadSingle();
                        child.flags = (LaneTypes)reader.ReadUInt16();
                        child.unk03 = reader.ReadUInt16();
                        child.unk04 = reader.ReadInt24();
                        reader.ReadByte();
                        child.unk05 = reader.ReadUInt16();
                        child.unk06 = reader.ReadUInt16();

                        child.children = new unkStruct1Sect1[child.unk05];
                        for (int z = 0; z != child.unk05; z++)
                        {
                            unkStruct1Sect1 child2 = new unkStruct1Sect1();
                            child2.unk01 = reader.ReadSingle();
                            child2.flags = (LaneTypes)reader.ReadUInt16();
                            child2.unk03 = reader.ReadUInt16();
                            child2.unk04 = reader.ReadInt24();
                            reader.ReadByte();
                            child2.unk05 = reader.ReadUInt16();
                            child2.unk06 = reader.ReadUInt16();
                            child.children[z] = child2;
                        }
                        sect.children[x] = child;
                    }
                    data.lanes[y] = sect;
                }

                data.ranges = new unkStruct1Sect2[data.rangeSize0];

                for (int y = 0; y != data.rangeSize0; y++)
                {
                    unkStruct1Sect2 sect = new unkStruct1Sect2();
                    sect.unk01 = reader.ReadSingle();
                    sect.unk02 = reader.ReadSingle();
                    sect.unk03 = reader.ReadInt16();
                    sect.unk04 = reader.ReadInt16();
                    sect.unk05 = reader.ReadSingle();
                    data.ranges[y] = sect;
                }

                data3[i] = data;
            }

            data4 = new JunctionDefinition[junctionPropertiesCount];
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
                data4[i] = data;
            }

            for (int i = 0; i != junctionPropertiesCount; i++)
            {
                data4[i].splines = new JunctionSpline[data4[i].junctionSize0];

                for (int y = 0; y != data4[i].junctionSize0; y++)
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
                    data4[i].splines[y] = data4Sect;
                }
                for (int y = 0; y != data4[i].junctionSize0; y++)
                {
                    data4[i].splines[y].path = new Vector3[data4[i].splines[y].pathSize0];

                    for (int z = 0; z != data4[i].splines[y].pathSize1; z++)
                    {
                        data4[i].splines[y].path[z] = Vector3Extenders.ReadFromFile(reader);
                    }
                }

                data4[i].boundaries = new Vector3[data4[i].boundarySize0];
                for (int y = 0; y != data4[i].boundarySize0; y++)
                {
                    data4[i].boundaries[y] = Vector3Extenders.ReadFromFile(reader);
                }

                if (data4[i].unk5 >= 2)
                {
                    data4[i].dataSet2 = new unkStruct2Sect2();
                    data4[i].dataSet2.unk0 = reader.ReadInt32();
                    data4[i].dataSet2.offset0 = reader.ReadInt24();
                    reader.ReadByte();
                    data4[i].dataSet2.unk1 = reader.ReadInt16();
                    data4[i].dataSet2.unk2 = reader.ReadInt16();
                    data4[i].dataSet2.unk3 = reader.ReadInt32();
                    data4[i].dataSet2.offset1 = reader.ReadInt24();
                    reader.ReadByte();
                    data4[i].dataSet2.unk4 = reader.ReadInt16();
                    data4[i].dataSet2.unk5 = reader.ReadInt16();
                    data4[i].dataSet2.unk6 = reader.ReadInt16();
                    data4[i].dataSet2.unk7 = reader.ReadInt16();
                    data4[i].dataSet2.unk8 = reader.ReadInt16();
                    data4[i].dataSet2.unk9 = reader.ReadInt16();

                    if (data4[i].dataSet2.unk1 > 2 && data4[i].dataSet2.unk2 > 2)
                    {
                        if (data4[i].dataSet2.unk1 == 4)
                        {
                            data4[i].dataSet2.unk10_0 = reader.ReadInt32();
                            data4[i].dataSet2.unk10_1 = -1;
                        }
                        else
                        {
                            data4[i].dataSet2.unk10_0 = reader.ReadInt32();
                            data4[i].dataSet2.unk10_1 = reader.ReadInt32();
                        }
                    }


                    if (data4[i].unk5 == 3)
                        data4[i].dataSet2.unk3Bytes = reader.ReadBytes(16);
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
            using (BinaryWriter writer = new BinaryWriter(File.Open(info.FullName + "_dupe", FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }
        public void WriteToFile(BinaryWriter writer)
        {
            splineCount = (ushort)data1.Length;
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
                SplineDefinition data = data1[i];
                positions[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.offset);
                writer.Write(data.NumSplines1);
                writer.Write(data.NumSplines2);

                float curDistance = 0.0f;
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

                writer.Write(curDistance/=2);
            }

            for (int i = 0; i < splineCount; i++)
            {
                long position = writer.BaseStream.Position;
                writer.BaseStream.Seek(positions[i], SeekOrigin.Begin);
                writer.WriteInt24((uint)position + 4);
                writer.BaseStream.Seek(position, SeekOrigin.Begin);
                SplineDefinition splineData = data1[i];
                for (int y = 0; y != splineData.points.Length; y++)
                    Vector3Extenders.WriteToFile(splineData.points[y], writer);
            }

            WriteUpdatedOffset(writer, 8);
            positions = new long[data3.Length]; //lane offset
            long[] pos2 = new long[data3.Length]; //range offset
            int laneIDX = 0;
            int curSpline = 0;
            int previousSpline = 0;
            //unk0
            //unk1
            for (int i = 0; i != data3.Length; i++)
            {
                SplineProperties data = data3[i];
                laneIDX += (data.laneSize0-1);
                writer.Write(data.unk0);
                writer.Write(data.unk1);
                positions[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.offset0);
                writer.Write(data.laneSize0);
                writer.Write(data.laneSize1);
                pos2[i] = writer.BaseStream.Position;
                writer.WriteInt24(data.offset1);
                writer.Write(data.rangeSize0);
                writer.Write(data.rangeSize1);
                writer.Write((short)data.flags);
                writer.Write(data.unk3);
                previousSpline = curSpline;
            }

            for (int i = 0; i < splinePropertiesCount; i++)
            {
                SplineProperties data = data3[i];
                //update lane position
                long curPosition = writer.BaseStream.Position;
                writer.BaseStream.Seek(positions[i], SeekOrigin.Begin);
                writer.WriteInt24((uint)curPosition + 4);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished
                for (int y = 0; y < data.laneSize1; y++)
                {
                    unkStruct1Sect1 sect = data3[i].lanes[y];
                    writer.Write(sect.unk01);
                    writer.Write((ushort)sect.flags);
                    writer.Write(sect.unk03);
                    writer.WriteInt24(sect.unk04);
                    writer.Write(sect.unk05);
                    writer.Write(sect.unk06);

                    for (int x = 0; x < sect.unk05; x++)
                    {
                        unkStruct1Sect1 child = sect.children[x];
                        writer.Write(child.unk01);
                        writer.Write((ushort)child.flags);
                        writer.Write(child.unk03);
                        writer.WriteInt24(child.unk04);
                        writer.Write(child.unk05);
                        writer.Write(child.unk06);

                        for (int z = 0; z < child.unk05; z++)
                        {
                            unkStruct1Sect1 child2 = child.children[z];
                            writer.Write(child2.unk01);
                            writer.Write((ushort)child.flags);
                            writer.Write(child2.unk03);
                            writer.WriteInt24(child2.unk04);
                            writer.Write(child2.unk05);
                            writer.Write(child2.unk06);
                        }
                    }
                }

                //update range position
                curPosition = writer.BaseStream.Position;
                uint laneOffset = (uint)(data.rangeSize0 > 0 ? curPosition + 4 : 0);
                writer.BaseStream.Seek(pos2[i], SeekOrigin.Begin);
                writer.WriteInt24(laneOffset);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished

                for (int y = 0; y < data.rangeSize0; y++)
                {
                    unkStruct1Sect2 sect = data.ranges[y];
                    writer.Write(sect.unk01);
                    writer.Write(sect.unk02);
                    writer.Write(sect.unk03);
                    writer.Write(sect.unk04);
                    writer.Write(sect.unk05);
                }
            }

            WriteUpdatedOffset(writer, 16);

            positions = new long[junctionPropertiesCount];
            pos2 = new long[junctionPropertiesCount];
            long[] pos3 = new long[junctionPropertiesCount];
            for (int i = 0; i < junctionPropertiesCount; i++)
            {
                JunctionDefinition data = data4[i];
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
                uint junctionOffset = (uint)(data4[i].junctionSize0 > 0 ? curPosition + 4 : 0);
                writer.BaseStream.Seek(positions[i], SeekOrigin.Begin);
                writer.WriteInt24(junctionOffset);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished

                for (int y = 0; y < data4[i].junctionSize0; y++)
                {
                    JunctionSpline data4Sect = data4[i].splines[y];
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
                for (int y = 0; y < data4[i].junctionSize0; y++)
                {
                    for (int z = 0; z != data4[i].splines[y].pathSize1; z++)
                    {
                        Vector3Extenders.WriteToFile(data4[i].splines[y].path[z], writer);
                    }
                }

                //update boundary position
                curPosition = writer.BaseStream.Position;
                uint boundaryOffset = (uint)(data4[i].boundarySize0 > 0 ? curPosition + 4 : 0);
                writer.BaseStream.Seek(pos2[i], SeekOrigin.Begin);
                writer.WriteInt24(boundaryOffset);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished

                for (int y = 0; y < data4[i].boundarySize0; y++)
                {
                    Vector3Extenders.WriteToFile(data4[i].boundaries[y], writer);
                }

                //update unk position
                curPosition = writer.BaseStream.Position;
                uint unkOffset = (uint)(data4[i].unk5 > 0 ? curPosition + 4 : 0);
                writer.BaseStream.Seek(pos3[i], SeekOrigin.Begin);
                writer.WriteInt24(unkOffset);
                writer.BaseStream.Seek(curPosition, SeekOrigin.Begin);
                //finished

                if (data4[i].unk5 >= 2)
                {
                    unkStruct2Sect2 data = data4[i].dataSet2;

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


                    if (data4[i].unk5 == 3)
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
    }
}
