using System;
using System.IO;

namespace Mafia2
{
    public class Cutscene
    {
        private int unk01; // seems to be always 1.
        private string cutsceneName; //begins with a length; short data type.
        //5 empty bytes;

        private int gcsSize; //size of GCS! data.
        private GCSData gcsData; //GCS! data.

        private SPDData spdData; //SPD! data.


        public Cutscene()
        {
            //throw new NotImplementedException();
        }

        public void ReadFromFile(BinaryReader reader)
        {
            unk01 = reader.ReadInt32();

            if (unk01 != 1)
                throw new FormatException("Unk01 should equal 1");

            short length = reader.ReadInt16();
            cutsceneName = new string(reader.ReadChars(length));
            byte[] unkBytes1 = reader.ReadBytes(5);
            gcsSize = reader.ReadInt32();


            byte[] unkBytes = reader.ReadBytes(3);
        }

        public void ReadFromFile2(BinaryReader reader)
        {
            unk01 = reader.ReadInt32();

            if (unk01 != 1)
                throw new FormatException("Unk01 should equal 1");

            short length = reader.ReadInt16();
            cutsceneName = new string(reader.ReadChars(length));
            byte[] unkBytes1 = reader.ReadBytes(5);
            gcsSize = reader.ReadInt32();

            long start = reader.BaseStream.Position;
            gcsData = new GCSData();
            gcsData.ReadFromFile(reader);
            reader.BaseStream.Seek(start, SeekOrigin.Begin);
            byte[] unkBytes2 = reader.ReadBytes(gcsSize);

            //go back three for size of SPD.
            reader.BaseStream.Position -= 3;
            int spdSize = reader.ReadInt32();
            start = reader.BaseStream.Position;
            spdData = new SPDData();
            spdData.ReadFromFile(reader);
            reader.BaseStream.Seek(start, SeekOrigin.Begin);
            byte[] unkBytes3 = reader.ReadBytes(spdSize);

        }

        public class GCSData
        {
            private string header; //usually equals !GCS.
            private long unk02; //cutscene flags? 
            //next 4 bytes is 0x1 and then 0xFF.

            private int unk03; //100000. This COULD be long with the next 8 bytes.
            private int unk04; //various data found here. potential size for this section.
            private int numStrings; //in fmv0108, i found 2 strings; this block could be for faceFX.
            private string[] strings; //size is numstrings.
            //3 empty bytes.

            //possible new beginning of data.
            private int size2;

            public void ReadFromFile(BinaryReader reader)
            {
                header = new string(reader.ReadChars(4));
                unk02 = reader.ReadInt64();
                byte[] unkBytes2 = reader.ReadBytes(4);
                unk03 = reader.ReadInt32();
                unk04 = reader.ReadInt32();
                numStrings = reader.ReadInt32();
                strings = new string[numStrings];
                for (int i = 0; i != numStrings; i++)
                {
                    short len = reader.ReadInt16();
                    strings[i] = new string(reader.ReadChars(len));
                }
            }
        }

        public class SPDData
        {
            private string header;
            private int unk01; //i think this is the same as unk02 in GCSData.
            private int unk02; //possible empty? or unk01 & unk02 are just a long.
            private UnkStruct1[] unkIntData; //size at the start as an int, then three ints following. (possible struct with 3 ints)

            public void ReadFromFile(BinaryReader reader)
            {
                header = new string(reader.ReadChars(4)); //header
                unk01 = reader.ReadInt32();
                unk02 = reader.ReadInt32();

                int sizeOfInts = reader.ReadInt32();
                unkIntData = new UnkStruct1[sizeOfInts];

                for (int i = 0; i != unkIntData.Length; i++)
                {
                    unkIntData[i] = new UnkStruct1(reader);
                }
            }

            private struct UnkStruct1
            {
                private int unk01;
                private int unk02;
                private int unk03;
                private short unk04;

                public UnkStruct1(BinaryReader reader)
                {
                    unk01 = reader.ReadInt32();
                    unk02 = reader.ReadInt32();
                    unk03 = reader.ReadInt32();
                    unk04 = reader.ReadInt16();
                }
            }
        }
    }
}