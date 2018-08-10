using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using Gibbed.IO;

namespace Mafia2
{
    public class Collision
    {
        int version;
        int unk0;

        int count1;
        Placement[] placementData;

        int count2;
        NXSStruct[] nxsData;

        public NXSStruct[] NXSData
        {
            get { return nxsData; }
            set { nxsData = value; }
        }

        public Placement[] Placements
        {
            get { return placementData; }
            set { placementData = value; }
        }

        public Collision(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }

            for (int i = 0; i != placementData.Length; i++)
                placementData[i].Unk5 = 128;

            nxsData[1].Data.Num2 = 3;
            nxsData[1].Data.Num5 = nxsData[1].Data.Triangles.Length - 1;
            if (nxsData[1].Data.Triangles.Length < 256)
                nxsData[1].Data.UnkData = new byte[nxsData[1].Data.Triangles.Length];
            else
                nxsData[1].Data.UnkBytes = new short[nxsData[1].Data.Triangles.Length];

            nxsData[2].Data.Num2 = 3;
            nxsData[2].Data.Num5 = nxsData[1].Data.Triangles.Length - 1;
            if (nxsData[2].Data.Triangles.Length < 256)
                nxsData[2].Data.UnkData = new byte[nxsData[2].Data.Triangles.Length];
            else
                nxsData[2].Data.UnkBytes = new short[nxsData[2].Data.Triangles.Length];

            using (BinaryWriter writer = new BinaryWriter(File.Create(fileName)))
            {
                WriteToFile(writer);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            version = reader.ReadInt32();

            if (version != 17)
                throw new Exception("Unknown collision version");

            unk0 = reader.ReadInt32();
            count1 = reader.ReadInt32();

            placementData = new Placement[count1];
            for (int i = 0; i != count1; i++)
            {
                placementData[i] = new Placement(reader);
            }

            count2 = reader.ReadInt32();
            nxsData = new NXSStruct[count2];

            for (int i = 0; i != nxsData.Length; i++)
            {
                nxsData[i] = new NXSStruct(reader);
            }

        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(version);
            writer.Write(unk0);
            writer.Write(placementData.Length);
            for (int i = 0; i != placementData.Length; i++)
                placementData[i].WriteToFile(writer);

            writer.Write(nxsData.Length);
            for (int i = 0; i != nxsData.Length; i++)
                nxsData[i].WriteToFile(writer);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", 1, 2);
        }

        public class Placement
        {
            Vector3 position;
            Vector3 rotation;
            ulong hash;
            int unk4;
            byte unk5;

            public Vector3 Position
            {
                get { return position; }
                set { position = value; }
            }

            public Vector3 Rotation
            {
                get { return rotation; }
                set { rotation = value; }
            }

            public ulong Hash
            {
                get { return hash; }
                set { hash = value; }
            }

            public int Unk4
            {
                get { return unk4; }
                set { unk4 = value; }
            }

            public byte Unk5
            {
                get { return unk5; }
                set { unk5 = value; }
            }

            public Placement(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public Placement()
            {

            }

            /// <summary>
            /// Read data from file.
            /// </summary>
            /// <param name="reader">stream</param>
            public void ReadFromFile(BinaryReader reader)
            {
                position = new Vector3(reader);
                rotation = new Vector3(reader);
                rotation.ConvertToDegrees();
                hash = reader.ReadUInt64();
                unk4 = reader.ReadInt32();
                unk5 = reader.ReadByte();
            }

            /// <summary>
            /// Write Data to file
            /// </summary>
            /// <param name="writer">stream</param>
            public void WriteToFile(BinaryWriter writer)
            {
                position.WriteToFile(writer);
                rotation.ConvertToRadians();
                rotation.WriteToFile(writer);
                writer.Write(hash);
                writer.Write(unk4);
                writer.Write(unk5);
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", hash, unk4, unk5);
            }
        }

        public class NXSStruct
        {
            ulong hash;
            private int dataSize;
            private byte[] bytes;
            MeshData data;
            Section[] sections;

            public ulong Hash
            {
                get { return hash; }
                set { hash = value; }
            }

            public MeshData Data
            {
                get { return data; }
                set { data = value; }
            }

            public Section[] Sections
            {
                get { return sections; }
                set { sections = value; }
            }

            public NXSStruct(BinaryReader reader)
            {
                ReadFromFile(reader);

            }

            public void ReadFromFile(BinaryReader reader)
            {
                hash = reader.ReadUInt64();

                dataSize = reader.ReadInt32();
                long pos = reader.BaseStream.Position;

                bytes = reader.ReadBytes(dataSize);

                int length = reader.ReadInt32();
                sections = new Section[length];
                for (int i = 0; i != sections.Length; i++)
                    sections[i] = new Section(reader);

                long pos2 = reader.BaseStream.Position;

                reader.BaseStream.Position = pos;
                data = new MeshData(reader, sections);
                reader.BaseStream.Position = pos2;
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(hash);
                writer.Write(data.GetMeshSize());
                data.WriteToFile(writer);
                writer.Write(sections.Length);

                for (int i = 0; i != sections.Length; i++)
                    sections[i].WriteToFile(writer);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class MeshData
        {
            string nxs;
            string mesh;
            int num1;
            int num2;
            float unkSmall;
            int num3;
            int num4;

            int nPoints;
            int nTriangles;

            Vector3[] points;
            Int3[] triangles; //or some linking thing
            CollisionMaterials[] unkShorts; //COULD be materialIDs

            int num5;

            short[] unkBytes;

            int unk0;
            byte[] unkData;

            int unk1;
            int unk2;

            private int opcSize; //possible.
            private int opcVersion; //1
            private int opcType; //if this is 4; there is no data. If 3, data is here.
            private int opcCount; //only available if type is 3.
            private UnkOPCData[] opcData; //8 halfs, with a long value.
            private float[] opcFloats; //6 floats.
            private int hbmVersion; //0
            private int hbmOffset; //potentially opcCount+1;
            private int hbmMaxOffset;
            private byte[] hbmOffsetData;
            private int hbmNumRefs;
            private float[] hbmUnkFloats;
            private float[] hbmUnkFloats2;
            private int unkSize;
            private byte[] unkSizeData;

            private Section[] sections;


            public float UnkSmall
            {
                get { return unkSmall; }
                set { unkSmall = value; }
            }

            public int Num1
            {
                get { return num1; }
                set { num1 = value; }
            }

            public int Num2
            {
                get { return num2; }
                set { num2 = value; }
            }

            public int Num3
            {
                get { return num3; }
                set { num3 = value; }
            }

            public int Num4
            {
                get { return num4; }
                set { num4 = value; }
            }

            public Vector3[] Vertices
            {
                get { return points; }
                set { points = value; }
            }

            public Int3[] Triangles
            {
                get { return triangles; }
                set { triangles = value; }
            }

            //TEST
            public int Num5
            {
                get { return num5; }
                set { num5 = value; }
            }

            public byte[] UnkData
            {
                get { return unkData; }
                set { unkData = value; }
            }

            public short[] UnkBytes
            {
                get { return unkBytes; }
                set { unkBytes = value; }
            }


            public MeshData(BinaryReader reader, Section[] sections)
            {
                nxs = new string(reader.ReadChars(4));
                mesh = new string(reader.ReadChars(4));
                num1 = reader.ReadInt32();
                num2 = reader.ReadInt32();
                unkSmall = reader.ReadSingle();
                num3 = reader.ReadInt32();
                num4 = reader.ReadInt32();

                nPoints = reader.ReadInt32();
                nTriangles = reader.ReadInt32();

                points = new Vector3[nPoints];
                triangles = new Int3[nTriangles];
                unkShorts = new CollisionMaterials[nTriangles];

                for (int i = 0; i != points.Length; i++)
                {
                    points[i] = new Vector3(reader);
                }

                for (int i = 0; i != triangles.Length; i++)
                {
                    triangles[i] = new Int3(reader);
                }

                for (int i = 0; i != unkShorts.Length; i++)
                {
                    unkShorts[i] = (CollisionMaterials) reader.ReadInt16();
                }

                if (num2 == 3)
                {
                    num5 = reader.ReadInt32();
                    if (num5 != nTriangles - 1)
                    {
                        if (nTriangles < 256)
                        {
                            unkData = new byte[nTriangles];
                            for (int i = 0; i != nTriangles; i++)
                                unkData[i] = reader.ReadByte();
                        }
                        else
                        {
                            unkBytes = new short[nTriangles];
                            for (int i = 0; i != nTriangles; i++)
                                unkBytes[i] = reader.ReadInt16();
                        }
                    }
                    else
                    {
                        if (nTriangles < 256)
                        {
                            unkData = new byte[nTriangles];
                            for (int i = 0; i != nTriangles; i++)
                                unkData[i] = reader.ReadByte();
                        }
                        else
                        {
                            unkBytes = new short[nTriangles];
                            for (int i = 0; i != nTriangles; i++)
                                unkBytes[i] = reader.ReadInt16();
                        }
                    }
                }

                unk0 = reader.ReadInt32();
                unk1 = reader.ReadInt32();

                //sections for 2 is 186811
                for (int i = 0; i != sections.Length; i++)
                    sections[i].EdgeData = reader.ReadBytes(sections[i].NumEdges);

                opcSize = reader.ReadInt32();

                bool hasHit = false;

                //BEGIN OPC/HBM SECTION.
                string opc = new string(reader.ReadChars(4));
                if (opc == "OPC\0")
                    hasHit = true;

                opcVersion = reader.ReadInt32();
                opcType = reader.ReadInt32();

                if (opcType == 3)
                {
                    opcCount = reader.ReadInt32();
                    opcData = new UnkOPCData[opcCount];
                    for (int i = 0; i != opcData.Length; i++)
                        opcData[i] = new UnkOPCData(reader);

                    opcFloats = new float[6];
                    for (int i = 0; i != 6; i++)
                        opcFloats[i] = reader.ReadSingle();
                }

                string hbm = new string(reader.ReadChars(4));
                if (hbm == "HBM\0")
                    hasHit = true;

                hbmVersion = reader.ReadInt32();
                hbmOffset = reader.ReadInt32();
                hbmMaxOffset = reader.ReadInt32();

                if (hbmOffset > 1)
                {
                    hbmOffsetData = new byte[hbmOffset];
                    for (int i = 0; i != hbmOffset; i++)
                        hbmOffsetData[i] = reader.ReadByte();

                    hbmNumRefs = reader.ReadInt32();
                    if (hbmNumRefs != 0)
                        throw new NotImplementedException();
                }

                hbmUnkFloats = new float[24];
                for (int i = 0; i != 24; i++)
                    hbmUnkFloats[i] = reader.ReadSingle();

                unkSize = reader.ReadInt32();
                unkSizeData = reader.ReadBytes(unkSize);

                this.sections = sections;
                Console.WriteLine("MeshData: {0}, {1}, {2}, {3}, {4}, {5}, POS: {6}", num1, num2, unk1, unk2, hasHit,
                    reader.BaseStream.Position, num3);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(22239310);
                writer.Write(1213416781);
                writer.Write(num1);
                writer.Write(num2);
                writer.Write(unkSmall);
                writer.Write(num3);
                writer.Write(num4);
                writer.Write(nPoints);
                writer.Write(nTriangles);

                for (int i = 0; i != points.Length; i++)
                    points[i].WriteToFile(writer);

                for (int i = 0; i != triangles.Length; i++)
                    triangles[i].WriteToFile(writer);

                for (int i = 0; i != unkShorts.Length; i++)
                    writer.Write((short) CollisionMaterials.Plaster);

                if (num2 == 3)
                {
                    writer.Write(num5);
                    if (num5 != nTriangles - 1)
                    {
                        if (nTriangles < 256)
                        {
                            writer.Write(unkData);
                        }
                        else
                        {
                            for (int i = 0; i != nTriangles; i++)
                                writer.Write(unkBytes[i]);
                        }
                    }
                    else
                    {
                        if (nTriangles < 256)
                        {
                            writer.Write(unkData);
                        }
                        else
                        {
                            for (int i = 0; i != nTriangles; i++)
                                writer.Write(unkBytes[i]);
                        }
                    }
                }

                writer.Write(unk0);
                writer.Write(unk1);

                //sections for 2 is 186811
                for (int i = 0; i != sections.Length; i++)
                    writer.Write(sections[i].EdgeData);

                writer.Write(opcSize);

                bool hasHit = false;

                //BEGIN OPC/HBM SECTION.
                writer.Write(21188687);

                writer.Write(opcVersion);
                writer.Write(opcType);

                if (opcType == 3)
                {
                    writer.Write(opcCount);
                    for (int i = 0; i != opcData.Length; i++)
                        opcData[i].WriteToFile(writer);

                    for (int i = 0; i != 6; i++)
                        writer.Write(opcFloats[i]);
                }

                writer.Write(21840456);

                writer.Write(hbmVersion);
                writer.Write(hbmOffset);
                writer.Write(hbmMaxOffset);

                if (hbmOffset > 1)
                {
                    writer.Write(hbmOffsetData);

                    writer.Write(hbmNumRefs);
                    if (hbmNumRefs != 0)
                        throw new NotImplementedException();
                }

                for (int i = 0; i != 24; i++)
                    writer.Write(hbmUnkFloats[i]);

                writer.Write(unkSize);
                writer.Write(unkSizeData);
            }

            private struct UnkOPCData
            {
                private short[] unkHalfs;
                private int unkInt;

                public UnkOPCData(BinaryReader reader)
                {
                    unkHalfs = new short[8];
                    for (int i = 0; i != unkHalfs.Length; i++)
                        unkHalfs[i] = reader.ReadInt16();

                    unkInt = reader.ReadInt32();
                }

                public void WriteToFile(BinaryWriter writer)
                {
                    for (int i = 0; i != unkHalfs.Length; i++)
                        writer.Write(unkHalfs[i]);

                    writer.Write(unkInt);
                }
            }

            public int GetMeshSize()
            {
                int size = 0;
                size += 4;
                size += 4;
                size += 4;
                size += 4;
                size += 4;
                size += 4;
                size += 4;
                size += 4;
                size += 4;

                for (int i = 0; i != points.Length; i++)
                    size += 12;

                for (int i = 0; i != triangles.Length; i++)
                    size += 12;

                for (int i = 0; i != unkShorts.Length; i++)
                    size += 2;

                if (num2 == 3)
                {
                    size += 4;
                    if (num5 != nTriangles - 1)
                    {
                        if (nTriangles < 256)
                        {
                            size += unkData.Length;
                        }
                        else
                        {
                            for (int i = 0; i != nTriangles; i++)
                                size += 2;
                        }
                    }
                    else
                    {
                        if (nTriangles < 256)
                        {
                            size += unkData.Length;
                        }
                        else
                        {
                            for (int i = 0; i != nTriangles; i++)
                                size += 2;
                        }
                    }
                }

                size += 4;
                size += 4;

                for (int i = 0; i != sections.Length; i++)
                    size += sections[i].NumEdges;

                size += 4;

                bool hasHit = false;

                //BEGIN OPC/HBM SECTION.
                size += 4;

                size += 4;
                size += 4;

                if (opcType == 3)
                {
                    size += 4;

                    for (int i = 0; i != opcData.Length; i++)
                        size += 20;

                    for (int i = 0; i != 6; i++)
                        size += 4;
                }

                size += 4;
                size += 4;
                size += 4;
                size += 4;

                if (hbmOffset > 1)
                {
                    size += hbmOffsetData.Length;

                    size += 4;
                    if (hbmNumRefs != 0)
                        throw new NotImplementedException();
                }

                for (int i = 0; i != 24; i++)
                    size += 4;

                size += 4;
                size += unkSize;
                return size;

            }
        }

        public class Section
        {
            int start;
            int numEdges;
            int unk1;
            int unk2;
            byte[] edgeData;

            public int Start
            {
                get { return start; }
                set { start = value; }
            }

            public int NumEdges
            {
                get { return numEdges; }
                set { numEdges = value; }
            }

            public int Unk1
            {
                get { return unk1; }
                set { unk1 = value; }
            }

            public int Unk2
            {
                get { return unk2; }
                set { unk2 = value; }
            }

            public byte[] EdgeData
            {
                get { return edgeData; }
                set { edgeData = value; }
            }

            public Section(BinaryReader reader)
            {
                start = reader.ReadInt32();
                numEdges = reader.ReadInt32();
                unk1 = reader.ReadInt32();
                unk2 = reader.ReadInt32();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(start);
                writer.Write(numEdges);
                writer.Write(unk1);
                writer.Write(unk2);

            }
        }
    }
}