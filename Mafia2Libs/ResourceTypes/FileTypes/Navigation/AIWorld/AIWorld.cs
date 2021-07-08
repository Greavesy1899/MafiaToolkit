using System;
using System.IO;
using System.Numerics;
using Utils.StringHelpers;
using Utils.VorticeUtils;

namespace ResourceTypes.Navigation
{
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
                    position = Vector3Utils.ReadFromFile(reader);
                    rotation = Vector3Utils.ReadFromFile(reader);
                    unk1 = reader.ReadInt32();
                    unk2 = reader.ReadInt32();
                    unk3 = reader.ReadInt32();
                    unk4 = Vector3Utils.ReadFromFile(reader);
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
                    position = Vector3Utils.ReadFromFile(reader);
                    rotation = Vector3Utils.ReadFromFile(reader);
                    unk1 = Vector3Utils.ReadFromFile(reader);
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
                    unk2 = Vector3Utils.ReadFromFile(reader);
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
                    unk1 = Vector3Utils.ReadFromFile(reader);
                    unk2 = Vector3Utils.ReadFromFile(reader);
                    unk3 = Vector3Utils.ReadFromFile(reader);
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
