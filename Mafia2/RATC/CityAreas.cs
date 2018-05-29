using System.IO;

namespace Mafia2
{
    public class CityAreas
    {
        public string header;
        public int unk0_int;
        public int unk1_int;
        public int unk2_int;
        public string[] collection;
        public AreaStruct[] areaCollection;

        public CityAreas(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            header = new string(reader.ReadChars(4));

            unk0_int = reader.ReadInt32();
            unk1_int = reader.ReadInt32();
            unk2_int = reader.ReadInt32();

            collection = new string(reader.ReadChars(unk2_int)).Split('\0');

            reader.ReadBytes(0);

            areaCollection = new AreaStruct[unk1_int];

            for(int i = 0; i != areaCollection.Length; i++)
            {
                areaCollection[i] = new AreaStruct(reader);
                areaCollection[i].indexedString1 = collection[areaCollection[i].index1];
                areaCollection[i].indexedString2 = collection[areaCollection[i].index2];
            }
          
        }

        public class AreaStruct
        {
            public string name;

            public short index1;
            public string indexedString1;

            public short index2;
            public string indexedString2;

            public byte unkByte;


            public AreaStruct(BinaryReader reader)
            {
                name = readString(reader);
                reader.ReadByte();
                index1 = reader.ReadInt16();
                index2 = reader.ReadInt16();
                unkByte = reader.ReadByte();
            }

            private string readString(BinaryReader reader)
            {
                string newString = "";

                while(reader.PeekChar() != '\0')
                {
                    newString += reader.ReadChar();
                }
                return newString;
            }

            public override string ToString()
            {
                return string.Format("{0}", name);
            }
        }
    }
}
