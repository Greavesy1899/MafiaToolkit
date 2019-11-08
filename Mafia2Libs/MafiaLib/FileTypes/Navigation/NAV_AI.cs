using System;
using System.Collections.Generic;
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
            unk0 = reader.ReadInt32();
            unk2 = reader.ReadInt32();
            unk3 = reader.ReadInt32();
            unk4 = reader.ReadInt32();
            vertSize = reader.ReadInt32();
            triSize = reader.ReadInt32();

            List<string> data = new List<string>();
            vertices = new VertexStruct[vertSize];
            for (int i = 0; i < vertSize; i++)
            {
                VertexStruct vertex = new VertexStruct();
                vertex.unk7 = reader.ReadUInt32() & 0x7FFFFFFF;
                vertex.position = Vector3Extenders.ReadFromFile(reader);
                //float pos = vertex.position.Y; //fuck the third var thing
                //vertex.position.Y = vertex.position.Z;
                //vertex.position.Z = pos;
                vertex.unk0 = reader.ReadSingle();
                vertex.unk1 = reader.ReadSingle();
                vertex.unk2 = reader.ReadInt32();
                vertex.unk3 = reader.ReadInt16();
                vertex.unk4 = reader.ReadInt16();
                vertex.unk5 = reader.ReadInt32();
                vertex.unk6 = reader.ReadInt32();
                data.Add(string.Format("v {0} {1} {2}", vertex.position.X, vertex.position.Z, vertex.position.Y));
                vertices[i] = vertex;
            }
            data.Add("");
            data.Add("g mesh");
            indices = new uint[triSize*3];
            int index = 0;
            for(int i = 0; i < triSize; i++)
            {
                indices[index] = reader.ReadUInt32() & 0x7FFFFFFF;
                indices[index+1] = reader.ReadUInt32() & 0x7FFFFFFF;
                indices[index+2] = reader.ReadUInt32() & 0x7FFFFFFF;
                data.Add(string.Format("f {0} {1} {2}", indices[index] + 1, indices[index + 1] + 1, indices[index + 2] + 1));
                index += 3;
            }
            File.WriteAllLines("model.obj", data.ToArray());


            


        }
    }
}