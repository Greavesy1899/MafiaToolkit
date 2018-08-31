using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class CityShops
    {
        public string header;   //HSTC
        public int fileVersion; //vanilla = 8; JA = 9;
        public int numAreas;
        public int bufferSize;
        public int numCount2;
        public int unk1;
        public int unk2;
        public int unk3;
        public Dictionary<int, string> names;

        AreaData[] areaData;

        public AreaData[] AreaDatas {
            get { return areaData; }
            set { areaData = value; }
        }

        public CityShops(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName + "2", FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            header = new string(reader.ReadChars(4));
            fileVersion = reader.ReadInt32();
            numAreas = reader.ReadInt32();
            bufferSize = reader.ReadInt32();
            numCount2 = reader.ReadInt32();
            unk1 = reader.ReadInt32();
            unk2 = reader.ReadInt32();
            unk3 = reader.ReadInt32();
            names = new Dictionary<int, string>();

            //this is updated version of areas, easier to use for shops.
            while(true)
            {
                int offset = (int)reader.BaseStream.Position - 32; // header is 32 bytes.

                if (offset == bufferSize)
                    break;

                string name = ReadString(reader); //read string
                names.Add(offset, name); //add offset as unique key and string
            }

            areaData = new AreaData[numAreas];

            for (int i = 0; i != numAreas; i++)
            {
                areaData[i] = new AreaData(reader);
                areaData[i].name = names[areaData[i].nameKey];
            }
        }

        private string ReadString(BinaryReader reader)
        {
            string newString = "";

            while (reader.PeekChar() != '\0')
            {
                newString += reader.ReadChar();
            }
            reader.ReadByte();
            return newString;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            //writer.Write("ratc".ToCharArray());
            //writer.Write(unk0_int);
            //writer.Write(areaCount);
            //writer.Write(namesLength);
            //writer.Write(names.ToCharArray());

            //for (int i = 0; i != areaCollection.Length; i++)
            //    areaCollection[i].WriteToFile(writer);
        }

        public class AreaData
        {
            public string string1;
            public string string2;
            public string name;
            public short nameKey;

            public AreaData(BinaryReader reader)
            {
                string1 = ReadString(reader);
                string2 = ReadString(reader);
                nameKey = reader.ReadInt16();
            }

            public void ReadFromFile(BinaryReader reader)
            {

            }

            public void WriteToFile(BinaryWriter writer)
            {
            }

            private string ReadString(BinaryReader reader)
            {
                string newString = "";

                while (reader.PeekChar() != '\0')
                {
                    newString += reader.ReadChar();
                }
                reader.ReadByte();
                return newString;
            }

            private void WriteString(BinaryWriter writer, string text)
            {
                writer.Write(text.ToCharArray());
                writer.Write('\0');
            }
        }
    }
}
