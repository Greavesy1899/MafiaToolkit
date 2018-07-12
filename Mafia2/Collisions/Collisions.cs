using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

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

        public NXSStruct[] NXSData {
            get { return nxsData; }
            set { nxsData = value; }
        }
        public Placement[] Placements {
            get { return placementData; }
            set { placementData = value; }
        }

        public Collision(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
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
            for(int i = 0; i != count1; i++)
            {
                placementData[i] = new Placement(reader);
            }

            count2 = reader.ReadInt32();
            nxsData = new NXSStruct[count2];
            for(int i = 0; i != nxsData.Length; i++)
            {
                nxsData[i] = new NXSStruct(reader);
            }

            using (BinaryWriter writer = new BinaryWriter(File.Create("exported/frame.edd")))
            {
                writer.Write(count1);

                for (int i = 0; i != count1; i++)
                {
                    writer.Write(placementData[i].Hash.ToString());

                    writer.Write(placementData[i].Position.X);
                    writer.Write(placementData[i].Position.Y);
                    writer.Write(placementData[i].Position.Z);
                    //writer.Write(nxsData[c].definition.rotation.X);
                    //writer.Write(nxsData[c].definition.rotation.Y);
                    writer.Write(0f);
                    writer.Write(0f);
                    writer.Write(placementData[i].Rotation.Z);

                }
            }
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

            public Vector3 Position {
                get { return position; }
                set { position = value; }
            }
            public Vector3 Rotation {
                get { return rotation; }
                set { rotation = value; }
            }
            public ulong Hash {
                get { return hash; }
                set { hash = value; }
            }
            public int Unk4 {
                get { return unk4; }
                set { unk4 = value; }
            }
            public byte Unk5 {
                get { return unk5; }
                set { unk5 = value; }
            }

            public Placement(BinaryReader reader)
            {
                position = new Vector3(reader);
                rotation = new Vector3(reader);
                rotation.ConvertToDegrees();
                hash = reader.ReadUInt64();
                unk4 = reader.ReadInt32();
                unk5 = reader.ReadByte();
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, {2}", hash, unk4, unk5);
            }
        }

        public class NXSStruct
        {
            ulong hash;
            MeshData data;
            Section[] sections;

            public ulong Hash {
                get { return hash; }
                set { hash = value; }
            }
            public MeshData Data {
                get { return data; }
                set { data = value; }
            }
            public Section[] Sections {
                get { return sections; }
                set { sections = value; }
            }

            public NXSStruct(BinaryReader reader)
            {
                hash = reader.ReadUInt64();

                int byteSize = reader.ReadInt32();
                long pos = reader.BaseStream.Position;

                reader.BaseStream.Position += byteSize;

                int length = reader.ReadInt32();
                sections = new Section[length];
                for (int i = 0; i != sections.Length; i++)
                {
                    sections[i] = new Section(reader);
                }
                long pos2 = reader.BaseStream.Position;

                reader.BaseStream.Position = pos;
                data = new MeshData(reader, sections);
                reader.BaseStream.Position = pos2;

                CustomEDM EDM = new CustomEDM(hash.ToString(), 1);
                using (BinaryWriter writer = new BinaryWriter(File.Create("exported/" + EDM.Name + ".edm")))
                {
                    EDM.Parts[0] = new CustomEDM.Part(Data.Vertices, Data.Triangles, "Collision_Mesh");
                    EDM.WriteToFile(writer);
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
                int finish;

                public float UnkSmall {
                    get { return unkSmall; }
                    set { unkSmall = value; }
                }
                public int Num1 {
                    get { return num1; }
                    set { num1 = value; }
                }
                public int Num2 {
                    get { return num2; }
                    set { num2 = value; }
                }
                public int Num3 {
                    get { return num3; }
                    set { num3 = value; }
                }
                public int Num4 {
                    get { return num4; }
                    set { num4 = value; }
                }
                public Vector3[] Vertices {
                    get { return points; }
                    set { points = value; }
                }
                public Int3[] Triangles {
                    get { return triangles; }
                    set { triangles = value; }
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
                        unkShorts[i] = (CollisionMaterials)reader.ReadInt16();
                    }

                    num5 = reader.ReadInt32();

                    if(num5 == 2)
                    {
                        unk0 = reader.ReadInt32();

                        if (unk0 != 1)
                        {
                            unkData = new byte[unk0];
                            for (int i = 0; i != unk0; i++)
                            {
                                unkData[i] = reader.ReadByte();
                            }
                        }
                    }
                    else
                    {
                        if (num2 == 3)
                            unkBytes = new short[num5];
                        else
                            unkBytes = new short[nTriangles];

                        for (int i = 0; i != unkBytes.Length; i++)
                        {
                            unkBytes[i] = reader.ReadInt16();
                        }
                    }

                    if (num2 == 3)
                    {
                        unk1 = reader.ReadInt32();
                        unk2 = reader.ReadInt32();
                    }

                    for (int i = 0; i != sections.Length; i++)
                    {
                        sections[i].EdgeData = reader.ReadBytes(sections[i].NumEdges);
                    }
                    finish = reader.ReadInt32();

                    bool hasHit = false;

                    //string opc = new string(reader.ReadChars(3));
                    //if (opc == "OPC")
                     //   hasHit = true;

                    Console.WriteLine("MeshData: {0}, {1}, {2}, {3}, {4}, {5}, POS: {6}", num1, num2, unk1, unk2, hasHit, reader.BaseStream.Position, num3);
                }
            }

            public class Section
            {
                int start;
                int numEdges;
                int unk1;
                int unk2;
                byte[] edgeData;

                public int Start {
                    get { return start; }
                    set { start = value; }
                }
                public int NumEdges {
                    get { return numEdges; }
                    set { numEdges = value; }
                }
                public int Unk1 {
                    get { return unk1; }
                    set { unk1 = value; }
                }
                public int Unk2 {
                    get { return unk2; }
                    set { unk2 = value; }
                }
                public byte[] EdgeData {
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
            }
        }
    }
}
