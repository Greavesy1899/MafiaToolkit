using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SharpDX;
using Utils.Extensions;
using Utils.SharpDXExtensions;
using Utils.StringHelpers;

namespace ResourceTypes.Navigation
{
    //For AIWorld
    //Type 1: AI Group
    //Type 2: AI World Part
    //Type 3: AI Nav Point
    //Type 4: AI Cover Part
    //Type 5: AI Anim Point
    //Type 6: AI User Area
    //Type 7: AI Path Object
    //Type 8: AI Action Point
    //Type 9: AI Action Point 2?
    //Type 10: AI Action Point 3?
    //Type 11: AI Hiding Place
    //Type 12: AI Action Point 4?
    public class NAVData
    {
        //unk01_flags could be types; AIWORLDS seem to have 1005, while OBJDATA is 3604410608.
        FileInfo file;

        int fileSize; //size - 4;
        uint unk01_flags; //possibly flags?
        public object data;



        public NAVData(BinaryReader reader)
        {
            ReadFromFile(reader);
        }
        public NAVData(FileInfo info)
        {
            file = info;

            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            fileSize = reader.ReadInt32();
            unk01_flags = reader.ReadUInt32();

            //file name seems to be earlier.
            if (unk01_flags == 3604410608)
            {
                int nameSize = reader.ReadInt32();
                string fileName = new string(reader.ReadChars(nameSize));

                long start = reader.BaseStream.Position;
                string hpdString = new string(reader.ReadChars(11));
                reader.ReadByte();
                int hpdVersion = reader.ReadInt32();
                if (hpdString == "Kynogon HPD" && hpdVersion == 2)
                {
                    data = new HPDData(reader);
                }
                else
                {
                    reader.BaseStream.Seek(start, SeekOrigin.Begin);
                    data = new OBJData(reader);
                }
            }
            else if (unk01_flags == 1005)
            {
                data = new AIWorld(reader);
            }
            else
            {
                throw new Exception("Found unexpected type: " + unk01_flags);
            }
        }

        public class AIWorld
        {
            int unk02;
            int unk03;
            short unk04; //might always == 2
            string preFileName; //this comes before the actual filename.
            int unk05;
            string unkString1; //these both seem to be for their Kynapse.
            string unkString2; //sometimes it can be 1, or 2.
            string typeName; //always AIWORLDPART.
            long unk06; //between the AIWorldPart and the struct.
            AIChunk[] points;
            string originFile;
            byte[] trailingBytes;

            public AIWorld(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                unk02 = reader.ReadInt32();
                unk03 = reader.ReadInt32();
                unk04 = reader.ReadInt16();
                int nameSize = reader.ReadInt16();
                preFileName = new string(reader.ReadChars(nameSize));
                unk05 = reader.ReadInt32();
                nameSize = reader.ReadInt16();
                unkString1 = new string(reader.ReadChars(nameSize));
                nameSize = reader.ReadInt16();
                unkString2 = new string(reader.ReadChars(nameSize));
                nameSize = reader.ReadInt16();
                typeName = new string(reader.ReadChars(nameSize));

                unk06 = reader.ReadByte();

                if (unk06 != 1)
                    throw new Exception("unk06 was not 1");

                int unkCount = reader.ReadInt32();
                points = new AIChunk[unkCount];
                for (int i = 0; i != unkCount; i++)
                    points[i] = new AIChunk(reader);

                originFile = StringHelpers.ReadString(reader);
                trailingBytes = reader.ReadBytes(8);
            }

            private class AIChunk
            {
                private short type;
                private Type1 type1Data;
                private Type4 type4Data;
                private Type7 type7Data;
                private Type8 type8Data;
                private int type8Int;
                private Type11 type11Data;

                public AIChunk(BinaryReader reader)
                {
                    type = reader.ReadInt16();

                    switch (type)
                    {
                        case 1:
                            type1Data = new Type1();
                            type1Data.ReadFromFile(reader);
                            break;
                        case 4:
                            type4Data = new Type4();
                            type4Data.ReadFromFile(reader);
                            break;
                        case 7:
                            type7Data = new Type7();
                            type7Data.ReadFromFile(reader);
                            break;
                        case 8:
                            type8Data = new Type8();
                            type8Data.ReadFromFile(reader);
                            type8Int = reader.ReadInt32();
                            break;
                        case 9:
                            type8Data = new Type8();
                            type8Data.ReadFromFile(reader);
                            break;
                        case 11:
                            type11Data = new Type11();
                            type11Data.ReadFromFile(reader);
                            break;
                        default:
                            throw new Exception("Unknown type");
                    }
                }

                private struct Type1
                {
                    byte unk06;
                    AIChunk[] points;

                    public void ReadFromFile(BinaryReader reader)
                    {
                        unk06 = reader.ReadByte();
                        int unkCount = reader.ReadInt32();
                        points = new AIChunk[unkCount];
                        for (int i = 0; i != unkCount; i++)
                            points[i] = new AIChunk(reader);
                    }
                }

                private struct Type4
                {
                    private byte unk0;
                    private Vector3 position;
                    private Vector3 rotation;
                    private int unk1;
                    private int unk2;
                    private int unk3;
                    private Vector3 unk4;
                    private int unk5;
                    private byte unk6;
                    private byte unk7;
                    private int[] unk8;
                    private int unk9;

                    public void ReadFromFile(BinaryReader reader)
                    {
                        unk0 = reader.ReadByte();
                        position = Vector3Extenders.ReadFromFile(reader);
                        rotation = Vector3Extenders.ReadFromFile(reader);
                        unk1 = reader.ReadInt32();
                        unk2 = reader.ReadInt32();
                        unk3 = reader.ReadInt32();
                        unk4 = Vector3Extenders.ReadFromFile(reader);
                        unk5 = reader.ReadInt32();
                        unk6 = reader.ReadByte();
                        unk7 = reader.ReadByte();
                        short count = reader.ReadInt16();
                        unk8 = new int[count];
                        for (int i = 0; i != count; i++)
                            unk8[i] = reader.ReadInt32();
                        unk9 = reader.ReadInt32();
                    }
                }

                private struct Type7
                {
                    private short unk0;
                    private Vector3 position;
                    private Vector3 rotation;
                    private Vector3 unk1;
                    private int unk2;

                    public void ReadFromFile(BinaryReader reader)
                    {
                        unk0 = reader.ReadInt16();
                        position = Vector3Extenders.ReadFromFile(reader);
                        rotation = Vector3Extenders.ReadFromFile(reader);
                        unk1 = Vector3Extenders.ReadFromFile(reader);
                        unk2 = reader.ReadInt32();
                    }
                }

                private struct Type8
                {
                    private byte unk0;
                    private int unk1;
                    private Vector3 unk2;
                    private float unk3;
                    private float unk4;
                    private int[] unk5;

                    public void ReadFromFile(BinaryReader reader)
                    {
                        unk0 = reader.ReadByte();
                        unk1 = reader.ReadInt32();
                        unk2 = Vector3Extenders.ReadFromFile(reader);
                        unk3 = reader.ReadSingle();
                        unk4 = reader.ReadSingle();
                        short count = reader.ReadInt16();
                        unk5 = new int[count];
                        for (int i = 0; i != count; i++)
                            unk5[i] = reader.ReadInt32();
                    }
                }

                private struct Type11
                {
                    private byte unk0;
                    private Vector3 unk1;
                    private Vector3 unk2;
                    private Vector3 unk3;
                    private float unk4;

                    public void ReadFromFile(BinaryReader reader)
                    {
                        unk0 = reader.ReadByte();
                        unk1 = Vector3Extenders.ReadFromFile(reader);
                        unk2 = Vector3Extenders.ReadFromFile(reader);
                        unk3 = Vector3Extenders.ReadFromFile(reader);
                        unk4 = reader.ReadSingle();
                    }
                }

                public override string ToString()
                {
                    return string.Format("Chunk: {0}", type);
                }
            }
        }
    }
    public class HPDData
    {
        int unk0;
        int unk1;
        byte[] remainingHeader; //132
        public unkStruct[] unkData;
        string unk2;
        int unk3;
        int unk4;

        public struct unkStruct
        {
            public int id;
            public Vector3 unk0;
            public Vector3 unk1;
            public byte[] unkData;

            public override string ToString()
            {
                return string.Format("{0} {1} {2}", id, unk0.ToString(), unk1.ToString());
            }
        }

        public HPDData(BinaryReader reader)
        {
            unk0 = reader.ReadInt32();
            unk1 = reader.ReadInt32();
            remainingHeader = reader.ReadBytes(132);

            unkData = new unkStruct[unk1];

            for (int i = 0; i != unkData.Length; i++)
            {
                unkStruct data = new unkStruct();
                data.id = reader.ReadInt32();
                data.unk0 = Vector3Extenders.ReadFromFile(reader);
                data.unk1 = Vector3Extenders.ReadFromFile(reader);
                data.unkData = reader.ReadBytes(20);
                unkData[i] = data;
            }

            unk2 = StringHelpers.ReadString(reader);
            unk3 = reader.ReadInt32();
            unk4 = reader.ReadInt32();
        }
    }
    public class OBJData
    {
        public struct VertexStruct
        {
            public uint unk7;
            public Vector3 position;
            public float unk0;
            public float unk1;
            public int unk2;
            public short unk3;
            public short unk4;
            public int unk5;
            public int unk6;

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6}", unk7, unk0, unk2, unk3, unk4, unk5, unk6);
            }
        }

        public struct Unk10DataSet
        {
            public BoundingBox B1;
            public int UnkOffset;
            public int Unk20;
        }

        public struct Unk12DataSet
        {
            public BoundingBox B1;
            public int Unk01; //-1?
            public int Unk02; //-1?
            public float Unk03;
            public float Unk04;
            public float Unk05;
        }

        public struct Unk18DataSet
        {
            public int Unk0;
            public float Unk1;
            public float Unk2;
            public float Unk3;
            public int Unk4;
            public byte[] Unk5;
            public BoundingBox B1;
            public BoundingBox B2;
            public BoundingBox B3;
        }

        public struct UnkSet0
        {
            public float X;
            public float Y;
            public int Offset;

            //This data is after each segment is listed.
            public int cellUnk0;
            public int cellUnk1;
            public int cellUnk2;
            public int cellUnk3;
            public float cellUnk4;
            public float cellUnk5;
            public float cellUnk6;
            public float cellUnk7;
            public int cellUnk8;
            public int cellUnk9;
            public int cellUnk10;
            public int cellUnk11;
            public int cellUnk12;
            public int cellUnk13;
            public int cellUnk14;
            public int cellUnk15;
            public int cellUnk16;
            public int cellUnk17;
            public int cellUnk18;
            public int cellUnk19;
            public Unk10DataSet[] unk10Boxes;
            public Unk12DataSet[] unk12Boxes;
            public int[] unk14Offsets;
            public int unk14End;
            //public BoundingBox[] unk14Boxes;
            public byte[] unk14Data;
            public int[] unk16Offsets;
            public int unk16End;
            public BoundingBox[] unk16Boxes;
            public Unk18DataSet[] unk18Set;

        }

        string fileName;
        int unk0;
        byte unk1;
        int unk2;
        int unk3;
        int unk4;
        int vertSize;
        int triSize;
        int unk5;
        public VertexStruct[] vertices;
        int unk6;
        int unk7;
        short unk8;
        short unk9;
        public uint[] indices;

        public OBJData(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            StreamWriter writer = File.CreateText("NAV_AI_OBJ_DATA.txt");

            unk0 = reader.ReadInt32();
            unk2 = reader.ReadInt32();
            unk3 = reader.ReadInt32();
            unk4 = reader.ReadInt32();
            vertSize = reader.ReadInt32();
            triSize = reader.ReadInt32();
            //writer.WriteLine(string.Format("{0}, )

            //List<string> data = new List<string>();
            vertices = new VertexStruct[vertSize];
            for (int i = 0; i < vertSize; i++)
            {
                VertexStruct vertex = new VertexStruct();
                vertex.unk7 = reader.ReadUInt32() & 0x7FFFFFFF;
                vertex.position = Vector3Extenders.ReadFromFile(reader);
                //float pos = vertex.position.Y;
                //vertex.position.Y = vertex.position.Z;
                //vertex.position.Z = pos;
                vertex.unk0 = reader.ReadSingle();
                vertex.unk1 = reader.ReadSingle();
                vertex.unk2 = reader.ReadInt32();
                vertex.unk3 = reader.ReadInt16();
                vertex.unk4 = reader.ReadInt16();
                vertex.unk5 = reader.ReadInt32();
                vertex.unk6 = reader.ReadInt32();
                //data.Add(string.Format("v {0} {1} {2}", vertex.position.X, vertex.position.Z, vertex.position.Y));
                vertices[i] = vertex;
            }
            //data.Add("");
            //data.Add("g mesh");
            indices = new uint[triSize * 3];
            int index = 0;
            for (int i = 0; i < triSize; i++)
            {
                indices[index] = reader.ReadUInt32() & 0x7FFFFFFF;
                indices[index + 1] = reader.ReadUInt32() & 0x7FFFFFFF;
                indices[index + 2] = reader.ReadUInt32() & 0x7FFFFFFF;
                //data.Add(string.Format("f {0} {1} {2}", indices[index] + 1, indices[index + 1] + 1, indices[index + 2] + 1));
                index += 3;
            }

            //KynogonRuntimeMesh
            long mesh_pos = reader.BaseStream.Position;
            string magicName = new string(reader.ReadChars(18));
            if(magicName != "KynogonRuntimeMesh")
            {
                throw new FormatException("Did not find KynogonRuntimeMesh");
            }

            short mesh_unk0 = reader.ReadInt16();
            int magicVersion = reader.ReadInt32();

            if(magicVersion != 2)
            {
                throw new FormatException("Version did not equal 2");
            }

            int mesh_unk1 = reader.ReadInt32();
            int mesh_unk2 = reader.ReadInt32();
            BoundingBox bbox = BoundingBoxExtenders.ReadFromFile(reader);
            int cellSizeX = reader.ReadInt32();
            int cellSizeY = reader.ReadInt32();
            float radius = reader.ReadSingle();
            int map_unk3 = reader.ReadInt32();
            int height = reader.ReadInt32();
            int offset = reader.ReadInt32(); //this is a potential offset;
            int[] grid = new int[(cellSizeX * cellSizeY) + 1];
            List<UnkSet0[]> gridSets = new List<UnkSet0[]>();

            for(int i = 0; i < grid.Length; i++)
            {
                grid[i] = reader.ReadInt32();
            }

            for(int i = 0; i < grid.Length; i++)
            {
                if (i + 1 >= grid.Length)
                    break;

                if (i == 189)
                    Console.WriteLine("st");

                int numSet0 = reader.ReadInt32();
                UnkSet0[] set0s = new UnkSet0[numSet0];
                gridSets.Add(set0s);

                writer.WriteLine("-----------------------");
                writer.WriteLine(string.Format("{0} {1}", i, numSet0));
                writer.WriteLine("");

                if (numSet0 == 0)
                    continue;

                //NOTE: EVERY OFFSET IN UNKSET0 BEGINS FROM MESH_POS HIGHER IN THE CODE FILE.
                int offset0 = reader.ReadInt32();
                
                for(int x = 0; x < numSet0; x++)
                {
                    UnkSet0 set = new UnkSet0();
                    set.X = reader.ReadSingle();
                    set.Y = reader.ReadSingle();
                    set.Offset = reader.ReadInt32();
                    set0s[x] = set;
                }

                //NOTE: EVERY BLOCK OF DATA SEEMS TO START WITH 96, THIS IS HOWEVER UNCONFIRMED.
                for (int z = 0; z < numSet0; z++)
                {
                    UnkSet0 set = set0s[z];
                    //NOTE: MOST OF THE OFFSETS BEGIN HERE, JUST AFTER THE SETS HAVE BEEN DEFINED ABOVE. 
                    set.cellUnk0 = reader.ReadInt32();

                    if (set.cellUnk0 != mesh_unk2)
                        throw new FormatException();
                    writer.WriteLine("");
                    set.cellUnk1 = reader.ReadInt32();
                    set.cellUnk2 = reader.ReadInt32();
                    set.cellUnk3 = reader.ReadInt32();
                    set.cellUnk4 = reader.ReadSingle();
                    set.cellUnk5 = reader.ReadSingle();
                    set.cellUnk6 = reader.ReadSingle();
                    set.cellUnk7 = reader.ReadSingle();
                    set.cellUnk8 = reader.ReadInt32();
                    set.cellUnk9 = reader.ReadInt32(); //-1?
                    set.cellUnk10 = reader.ReadInt32(); //1;
                    set.cellUnk11 = reader.ReadInt32();
                    set.cellUnk12 = reader.ReadInt32(); //0
                    set.cellUnk13 = reader.ReadInt32(); //-1;
                    set.cellUnk14 = reader.ReadInt32(); //0
                    set.cellUnk15 = reader.ReadInt32(); //-1;
                    set.cellUnk16 = reader.ReadInt32(); //8;
                    set.cellUnk17 = reader.ReadInt32(); //112;
                    set.cellUnk18 = reader.ReadInt32(); //0;
                    set.cellUnk19 = reader.ReadInt32(); //-1;
                    writer.WriteLine(string.Format("Unk1: {0}", set.cellUnk1));
                    writer.WriteLine(string.Format("Unk2: {0}", set.cellUnk2));
                    writer.WriteLine(string.Format("Unk3: {0}", set.cellUnk3));
                    writer.WriteLine(string.Format("Unk4: {0}", set.cellUnk4));
                    writer.WriteLine(string.Format("Unk5: {0}", set.cellUnk5));
                    writer.WriteLine(string.Format("Unk6: {0}", set.cellUnk6));
                    writer.WriteLine(string.Format("Unk7: {0}", set.cellUnk7));
                    writer.WriteLine(string.Format("Unk8: {0}", set.cellUnk8));
                    writer.WriteLine(string.Format("Unk9: {0}", set.cellUnk9));
                    writer.WriteLine(string.Format("Unk10: {0}", set.cellUnk10));
                    writer.WriteLine(string.Format("Unk11: {0}", set.cellUnk11));
                    writer.WriteLine(string.Format("Unk12: {0}", set.cellUnk12));
                    writer.WriteLine(string.Format("Unk13: {0}", set.cellUnk13));
                    writer.WriteLine(string.Format("Unk14: {0}", set.cellUnk14));
                    writer.WriteLine(string.Format("Unk15: {0}", set.cellUnk15));
                    writer.WriteLine(string.Format("Unk16: {0}", set.cellUnk16));
                    writer.WriteLine(string.Format("Unk17: {0}", set.cellUnk17));
                    writer.WriteLine(string.Format("Unk18: {0}", set.cellUnk18));
                    writer.WriteLine(string.Format("Unk19: {0}", set.cellUnk19));
                    writer.WriteLine("");
                    //THIS BIT IS UNKNOWN, UPTO CELLUNK20
                    set.unk10Boxes = new Unk10DataSet[set.cellUnk10];
                    writer.WriteLine("Unk10 Boxes");
                    for (int x = 0; x < set.cellUnk10; x++)
                    {
                        Unk10DataSet unk10Set = new Unk10DataSet();
                        unk10Set.B1 = BoundingBoxExtenders.ReadFromFile(reader);
                        unk10Set.UnkOffset = reader.ReadInt32();
                        unk10Set.Unk20 = reader.ReadInt32();
                        set.unk10Boxes[x] = unk10Set;
                        writer.WriteLine(string.Format("Minimum: {0} ", unk10Set.B1.Minimum.ToString()));
                        writer.WriteLine(string.Format("Maximum: {0} ", unk10Set.B1.Maximum.ToString()));
                        writer.WriteLine(string.Format("UnkOffset: {0} ", unk10Set.UnkOffset));
                        writer.WriteLine(string.Format("Unk20: {0} ", unk10Set.Unk20));
                        writer.WriteLine("");
                    }
                    //END OF CONFUSING BIT.


                    //THIS BIT IS UNKNOWN, BUT IS CELLUNK12
                    set.unk12Boxes = new Unk12DataSet[set.cellUnk12];
                    writer.WriteLine("Unk12 Boxes");
                    for (int x = 0; x < set.cellUnk12; x++)
                    {
                        Unk12DataSet unk12Set = new Unk12DataSet();
                        unk12Set.B1 = BoundingBoxExtenders.ReadFromFile(reader);
                        unk12Set.Unk01 = reader.ReadInt32();
                        unk12Set.Unk02 = reader.ReadInt32();
                        unk12Set.Unk03 = reader.ReadSingle();
                        unk12Set.Unk04 = reader.ReadSingle();
                        unk12Set.Unk05 = reader.ReadSingle();
                        set.unk12Boxes[x] = unk12Set;
                        writer.WriteLine(string.Format("Minimum: {0} ", unk12Set.B1.Minimum.ToString()));
                        writer.WriteLine(string.Format("Maximum: {0} ", unk12Set.B1.Maximum.ToString()));
                        writer.WriteLine(string.Format("Unk01: {0} ", unk12Set.Unk01));
                        writer.WriteLine(string.Format("Unk02: {0} ", unk12Set.Unk02));
                        writer.WriteLine(string.Format("Unk03: {0} ", unk12Set.Unk03));
                        writer.WriteLine(string.Format("Unk04: {0} ", unk12Set.Unk04));
                        writer.WriteLine(string.Format("Unk05: {0} ", unk12Set.Unk05));
                        writer.WriteLine("");
                    }

                    //END OF CONFUSING BIT.

                    //THIS LOOPS THROUGH OFFSETS TO BBOX'S
                    writer.WriteLine("Unk14 Offsets");
                    set.unk14Offsets = new int[set.cellUnk14];
                    for (int x = 0; x < set.cellUnk14; x++)
                    {
                        set.unk14Offsets[x] = reader.ReadInt32();
                        writer.WriteLine(string.Format("{0} ", set.unk14Offsets[x]));
                    }

                    //ALWAYS A 4-BYTE INTEGER WHICH DENOTES THE END OF THE BATCH
                    if (set.cellUnk14 > 0)
                        set.unk14End = reader.ReadInt32();

                    if (set.cellUnk14 > 0)
                    {
                        set.unk14Data = reader.ReadBytes(set.unk14End - set.unk14Offsets[0]);
                    }
                    writer.WriteLine("");

                    //set.unk14Boxes = new BoundingBox[set.cellUnk14];
                    //writer.WriteLine("Unk14 Boxes");
                    //for (int x = 0; x < set.cellUnk14; x++)
                    //{
                    //    set.unk14Boxes[x] = BoundingBoxExtenders.ReadFromFile(reader);
                    //    writer.WriteLine(string.Format("{0} ", set.unk14Boxes[x].ToString()));
                    //}
                    //writer.WriteLine("");



                    //CONTINUE ONTO THE NEXT BATCH
                    set.unk16Offsets = new int[set.cellUnk16];
                    writer.WriteLine("Unk16 Offsets");
                    for (int x = 0; x < set.cellUnk16; x++)
                    {
                        set.unk16Offsets[x] = reader.ReadInt32();
                        writer.WriteLine(string.Format("{0} ", set.unk16Offsets[x]));
                    }
                    writer.WriteLine("");
                    //ALWAYS A 4-BYTE INTEGER WHICH DENOTES THE END OF THE BATCH
                    if (set.cellUnk16 > 0)
                        set.unk16End = reader.ReadInt32();

                    set.unk16Boxes = new BoundingBox[set.cellUnk16];
                    writer.WriteLine("Unk16 Boxes");
                    for (int x = 0; x < set.cellUnk16; x++)
                    {
                        set.unk16Boxes[x] = BoundingBoxExtenders.ReadFromFile(reader);
                        writer.WriteLine(string.Format("{0} ", set.unk16Boxes[x]));
                    }
                    writer.WriteLine("");
                    if (set.cellUnk18 > 1)
                        throw new FormatException();

                    set.unk18Set = new Unk18DataSet[set.cellUnk18];
                    writer.WriteLine("Unk18 Boxes");
                    for (int x = 0; x < set.cellUnk18; x++)
                    {
                        //THIS COULD BE AN OFFSET LIST WITH SOMEKIND OF FLOAT/ROTATION DATA
                        Unk18DataSet dataSet = new Unk18DataSet();
                        dataSet.Unk0 = reader.ReadInt32();
                        dataSet.Unk1 = reader.ReadSingle();
                        dataSet.Unk2 = reader.ReadSingle();
                        dataSet.Unk3 = reader.ReadSingle();

                        //THIS COULD BE THE FINAL AREA WITH THE 12 BYTES SIMPLY PADDING OUT
                        dataSet.Unk4 = reader.ReadInt32();
                        dataSet.Unk5 = reader.ReadBytes(12);

                        //BOUNDING BOX FOR THIS KIND OF DATA.
                        dataSet.B1 = BoundingBoxExtenders.ReadFromFile(reader);
                        dataSet.B2 = BoundingBoxExtenders.ReadFromFile(reader);
                        dataSet.B3 = BoundingBoxExtenders.ReadFromFile(reader);
                        set.unk18Set[x] = dataSet;

                        
                        writer.WriteLine(string.Format("Unk01: {0} ", dataSet.Unk1));
                        writer.WriteLine(string.Format("Unk02: {0} ", dataSet.Unk2));
                        writer.WriteLine(string.Format("Unk03: {0} ", dataSet.Unk3));
                        writer.WriteLine(string.Format("Unk04: {0} ", dataSet.Unk4));
                        writer.WriteLine(string.Format("Unk05: {0} ", dataSet.Unk5));
                        writer.WriteLine(string.Format("Minimum: {0} ", dataSet.B1.Minimum.ToString()));
                        writer.WriteLine(string.Format("Maximum: {0} ", dataSet.B1.Maximum.ToString()));
                        writer.WriteLine(string.Format("Minimum: {0} ", dataSet.B2.Minimum.ToString()));
                        writer.WriteLine(string.Format("Maximum: {0} ", dataSet.B2.Maximum.ToString()));
                        writer.WriteLine(string.Format("Minimum: {0} ", dataSet.B3.Minimum.ToString()));
                        writer.WriteLine(string.Format("Maximum: {0} ", dataSet.B3.Maximum.ToString()));
                    }
                    writer.WriteLine("");
                    set0s[z] = set;
                }
                Console.WriteLine("Completed: " + i);
                //byte[] data = reader.ReadBytes(size);
                //File.WriteAllBytes("grid_" + i + ".bin", data);
            }


            //File.WriteAllLines("model.obj", data.ToArray());

        }
    }
}