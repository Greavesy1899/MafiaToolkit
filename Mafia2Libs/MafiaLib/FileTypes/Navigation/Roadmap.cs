using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Navigation
{
    struct unkStruct0
    {
        //12 bytes max!
        public ushort offset;
        public byte chunkIDX; //for some reason when the offset goes over 65536, then this is increased. if you want to access spline data AFTER the ushort max value, just add offset to short.maxvalue
        public byte unk0; //knowing 2k, just 128.
        public short NumSplines1; //should be equal to unk2
        public short NumSplines2;
        public float unk3;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", offset, chunkIDX, unk0, NumSplines1, NumSplines2, unk3);
        }
    }

    struct Spline
    {
        public Vector3[] points;
    }

    struct unkStruct1
    {
        public ushort unk0;
        public ushort unk1;
        public int int3_unk1;
        public ushort unk2;
        public ushort unk3;
        public int int3_unk2;
        public ushort unk4;
        public ushort unk5;
        public int unk6;
        public unkStruct1Sect1[] data1;
        public unkStruct1Sect2[] data2;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}, {6}, {7}, {8}", unk0, unk1, int3_unk1, unk2, unk3, int3_unk2, unk4, unk5, unk6);
        }
    }

    struct unkStruct1Sect1
    {
        //16 bytes
        public float unk01;
        public ushort unk02;
        public ushort unk03;
        public int unk04;
        public ushort unk05;
        public ushort unk06;
        public unkStruct1Sect1[] children; //SOMETIMES this is the case.

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5}", unk01, unk02, unk03, unk04, unk05, unk06);
        }
    }

    struct unkStruct1Sect2
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

    struct unkStruct2
    {
        public Vector3 position;
        public int offset0;
        public short unk0;
        public short unk1;
        public int offset1;
        public short unk2;
        public short unk3;
        public int unk4;
        public int offset2;
        public short unk5;
        public short unk6;
        public unkStruct2Sect1[] dataSet1;
        public unkStruct2Sect2[] dataSet2;

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", offset0, unk0, unk1, offset1, unk2, unk3, unk4, offset2, unk5, unk6);
        }
    }

    public struct unkStruct2Sect1
    {
        public short unk0;
        public short unk1;
        public short unk2;
        public short unk3;
        public short unk4;
        public short unk5;
        public int unk6;
        public short unk7;
        public short unk8;
        public float unk9;
        public float[] unk10;
        public unkStruct2Sect2[] unk11;
    }

    public struct unkStruct2Sect2
    {
        public byte[] data;
    }

    public class Roadmap
    {
        public Roadmap(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int magic = reader.ReadInt32();
            short splineCount1 = reader.ReadInt16();
            short splineCount2 = reader.ReadInt16();
            int splineFinishOffset = reader.ReadInt24();
            reader.ReadByte();
            short unkDataSet1Count1 = reader.ReadInt16();
            short unkDataSet1Count2 = reader.ReadInt16();
            int unkDataSet1Offset = reader.ReadInt24();
            reader.ReadByte();
            short unkDataSet2Count1 = reader.ReadInt16();
            short unkDataSet2Count2 = reader.ReadInt16();
            int unkDataSet2Offset = reader.ReadInt24();
            reader.ReadByte();
            short unkDataSet3Count1 = reader.ReadInt16();
            short unkDataSet3Count2 = reader.ReadInt16();
            int unkDataSet3Offset = reader.ReadInt24();
            reader.ReadByte();
            short unkDataSet4Count1 = reader.ReadInt16();
            short unkDataSet4Count2 = reader.ReadInt16();
            int unkDataSet4Offset = reader.ReadInt24();
            reader.ReadByte();
            short unkDataSet5Count1 = reader.ReadInt16();
            short unkDataSet5Count2 = reader.ReadInt16();
            int unkDataSet5Offset = reader.ReadInt24();
            reader.ReadByte();
            short unkDataSet6Count1 = reader.ReadInt16();
            short unkDataSet6Count2 = reader.ReadInt16();

            int count1 = reader.ReadInt32();
            unkStruct0[] data1 = new unkStruct0[splineCount1];

            for (int i = 0; i != splineCount1; i++)
            {
                unkStruct0 data = new unkStruct0();
                data.offset = reader.ReadUInt16();
                data.chunkIDX = reader.ReadByte();
                data.unk0 = reader.ReadByte();
                data.NumSplines1 = reader.ReadInt16();
                data.NumSplines2 = reader.ReadInt16();
                data.unk3 = reader.ReadSingle();
                data1[i] = data;
                Console.WriteLine("POTENTIAL NODE:  " + data.ToString());
            }

            Spline[] data2 = new Spline[splineCount1];

            List<string> splines = new List<string>();

            for (int i = 0; i != splineCount1; i++)
            {
                Spline splineData = new Spline();
                splineData.points = new Vector3[data1[i].NumSplines1];

                //if (data1[i].chunkIDX == 1)
                //    Console.WriteLine("STOP");

                reader.BaseStream.Position = (data1[i].chunkIDX != 0 ? data1[i].offset + (65536 * data1[i].chunkIDX) : data1[i].offset) - 4;
                splines.Add("Spline " + i);
                splines.Add(data1[i].NumSplines1.ToString());

                for (int y = 0; y != data1[i].NumSplines1; y++)
                {
                    splineData.points[y] = Vector3Extenders.ReadFromFile(reader);
                    splines.Add(string.Format("X: {0} Y: {1} Z: {2}", splineData.points[y].X, splineData.points[y].Y, splineData.points[y].Z));
                }
                splines.Add(" ");
                data2[i] = splineData;
            }

            unkStruct1[] data3 = new unkStruct1[unkDataSet1Count1];

            for (int i = 0; i != data3.Length; i++)
            {
                unkStruct1 data = new unkStruct1();
                data.unk0 = reader.ReadUInt16();
                data.unk1 = reader.ReadUInt16();
                data.int3_unk1 = reader.ReadInt24();
                reader.ReadByte();
                data.unk2 = reader.ReadUInt16();
                data.unk3 = reader.ReadUInt16();
                data.int3_unk2 = reader.ReadInt24();
                reader.ReadByte();
                data.unk4 = reader.ReadUInt16();
                data.unk5 = reader.ReadUInt16();
                data.unk6 = reader.ReadInt32();
                Console.WriteLine(data.ToString());
                data3[i] = data;
            }

            for (int i = 0; i != unkDataSet1Count1; i++)
            {
                unkStruct1 data = data3[i];

                if (reader.BaseStream.Position == data.int3_unk1 - 4)
                {
                    Console.WriteLine(i + " working fine");
                }
                else
                {
                    Console.WriteLine(i + " not working fine");
                }

                if (i == 234)
                    Console.WriteLine(i);

                data.data1 = new unkStruct1Sect1[data.unk3];

                for (int y = 0; y != data.unk3; y++)
                {
                    unkStruct1Sect1 sect = new unkStruct1Sect1();
                    sect.unk01 = reader.ReadSingle();
                    sect.unk02 = reader.ReadUInt16();
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
                        child.unk02 = reader.ReadUInt16();
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
                            child2.unk02 = reader.ReadUInt16();
                            child2.unk03 = reader.ReadUInt16();
                            child2.unk04 = reader.ReadInt24();
                            reader.ReadByte();
                            child2.unk05 = reader.ReadUInt16();
                            child2.unk06 = reader.ReadUInt16();
                            child.children[z] = child2;
                        }
                        sect.children[x] = child;
                    }
                    data.data1[y] = sect;
                }

                data.data2 = new unkStruct1Sect2[data.unk4];

                for (int y = 0; y != data.unk4; y++)
                {
                    unkStruct1Sect2 sect = new unkStruct1Sect2();
                    sect.unk01 = reader.ReadSingle();
                    sect.unk02 = reader.ReadSingle();
                    sect.unk03 = reader.ReadInt16();
                    sect.unk04 = reader.ReadInt16();
                    sect.unk05 = reader.ReadSingle();
                    data.data2[y] = sect;
                }
            }

            unkStruct2[] data4 = new unkStruct2[unkDataSet2Count1];
            for (int i = 0; i != unkDataSet2Count1; i++)
            {
                unkStruct2 data = new unkStruct2();
                data.position = Vector3Extenders.ReadFromFile(reader);
                data.offset0 = reader.ReadInt24();
                reader.ReadByte();
                data.unk0 = reader.ReadInt16();
                data.unk1 = reader.ReadInt16();
                data.offset1 = reader.ReadInt24();
                reader.ReadByte();
                data.unk2 = reader.ReadInt16();
                data.unk3 = reader.ReadInt16();
                data.unk4 = reader.ReadInt32();
                data.offset2 = reader.ReadInt24();
                reader.ReadByte();
                data.unk5 = reader.ReadInt16();
                data.unk6 = reader.ReadInt16();
                data4[i] = data;
            }

            for (int i = 0; i != unkDataSet2Count1; i++)
            {
                data4[i].dataSet1 = new unkStruct2Sect1[data4[i].unk0];
                unkStruct2Sect1 data4Sect = new unkStruct2Sect1();

                for (int y = 0; y != data4[i].unk0; y++)
                {
                    data4Sect.unk0 = reader.ReadInt16();
                    data4Sect.unk1 = reader.ReadInt16();
                    data4Sect.unk2 = reader.ReadInt16();
                    data4Sect.unk3 = reader.ReadInt16();
                    data4Sect.unk4 = reader.ReadInt16();
                    data4Sect.unk5 = reader.ReadInt16();
                    data4Sect.unk6 = reader.ReadInt24();
                    reader.ReadByte();
                    data4Sect.unk7 = reader.ReadInt16();
                    data4Sect.unk8 = reader.ReadInt16();
                    data4Sect.unk9 = reader.ReadSingle();
                    data4[i].dataSet1[y] = data4Sect;
                }
                for (int y = 0; y != data4[i].unk0; y++)
                {
                    data4Sect.unk10 = new float[12];

                    for (int z = 0; z != 12; z++)
                    {
                        data4Sect.unk10[z] = reader.ReadSingle();
                    }
                    data4[i].dataSet1[y] = data4Sect;
                }

                data4[i].dataSet2 = new unkStruct2Sect2[data4[i].unk5];

                for (int z = 0; z != data4[i].unk5; z++)
                {
                    unkStruct2Sect2 data4Sect2 = new unkStruct2Sect2();
                    data4Sect2.data = reader.ReadBytes(16);
                    data4[i].dataSet2[z] = data4Sect2;
                }
                long position = reader.BaseStream.Position;
                Console.WriteLine(position);
            }
            File.WriteAllLines("NEW_SPLINES_JA", splines.ToArray());
        }
    }
}
