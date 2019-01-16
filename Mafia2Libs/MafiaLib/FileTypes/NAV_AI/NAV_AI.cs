using System;
using System.IO;
using SharpDX;

namespace Mafia2
{
    public class NAVData
    {
        //unk01_flags could be types; AIWORLDS seem to have 1005, while OBJDATA is 3604410608.
        FileInfo file;

        int fileSize; //size - 4;
        uint unk01_flags; //possibly flags?
        object data;



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
            if(unk01_flags == 3604410608)
            {
                data = new OBJData(reader);
            }
            else if(unk01_flags == 1005)
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
            AISegment[] segments;
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

                if(unk06 != 1)
                    throw new Exception("unk06 was not 1");

                int unkCount = reader.ReadInt32();
                segments = new AISegment[unkCount];
                for(int i = 0; i != unkCount; i++)
                {
                    segments[i] = new AISegment(reader);
                }

                originFile = Functions.ReadString(reader);
                trailingBytes = reader.ReadBytes(8);
            }

            private class AISegment
            {
                short unk0;
                byte unk1;
                AIChunk[] chunks;

                public AISegment(BinaryReader reader)
                {
                    unk0 = reader.ReadInt16();
                    unk1 = reader.ReadByte();
                    int unkCount = reader.ReadInt32();
                    chunks = new AIChunk[unkCount];
                    for (int i = 0; i != unkCount; i++)
                    {
                        chunks[i] = new AIChunk(reader);
                    }
                }

                private class AIChunk
                {
                    private short type;
                    private Type4 type4Data;
                    private Type7 type7Data;

                    public AIChunk(BinaryReader reader)
                    {
                        type = reader.ReadInt16();

                        switch(type)
                        {
                            case 4:
                                type4Data = new Type4();
                                type4Data.ReadFromFile(reader);
                                break;
                            case 7:
                                type7Data = new Type7();
                                type7Data.ReadFromFile(reader);
                                break;
                            default:
                                throw new Exception("Unknown type");
                        }
                    }

                    private struct Type4
                    {
                        private byte unk0;
                        private Vector3 position;
                        private Vector3 rotation;
                        private short[] unks1;
                        private Vector3 unkVector;
                        private byte[] unkHalfs; //ACTUALLY HALF DATA.
                        private int[] unkData;
                        private int trailingUnk;

                        public void ReadFromFile(BinaryReader reader)
                        {
                            unk0 = reader.ReadByte();
                            position = Vector3Extenders.ReadFromFile(reader);
                            rotation = Vector3Extenders.ReadFromFile(reader);

                            unks1 = new short[6];
                            for (int i = 0; i != 6; i++)
                                unks1[i] = reader.ReadInt16();

                            unkVector = Vector3Extenders.ReadFromFile(reader);
                            unkHalfs = reader.ReadBytes(6);

                            short unkCount = reader.ReadInt16();

                            unkData = new int[unkCount];
                            for (int i = 0; i != unkCount; i++)
                                unkData[i] = reader.ReadInt32();

                            trailingUnk = reader.ReadInt32();
                        }
                    }

                    private struct Type7
                    {
                        private short unk0;
                        private Vector3 position;
                        private Vector3 rotation;
                        private byte[] data;

                        public void ReadFromFile(BinaryReader reader)
                        {
                            unk0 = reader.ReadInt16();
                            position = Vector3Extenders.ReadFromFile(reader);
                            rotation = Vector3Extenders.ReadFromFile(reader);
                            data = reader.ReadBytes(16);
                        }
                    }

                    public override string ToString()
                    {
                        return string.Format("Chunk: {0}", type);
                    }
                }
            }         
        }

        public class OBJData
        {
            string fileName;

            public OBJData(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                int nameSize = reader.ReadInt32();
                fileName = new string(reader.ReadChars(nameSize));
            }
        }
    }
}
