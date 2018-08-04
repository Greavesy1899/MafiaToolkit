using System;
using System.IO;

namespace Mafia2
{
    public class NAVData
    {
        //unk01_flags could be types; AIWORLDS seem to have 1005, while OBJDATA is 3604410608.


        int fileSize; //size - 4;
        uint unk01_flags; //possibly flags?
        object data;



        public NAVData(BinaryReader reader)
        {
            ReadFromFile(reader);
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
            byte unk06; //potential bool.

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
                    throw new Exception("unk06 WAS NOT 1");


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
