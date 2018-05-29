using System.IO;

namespace Mafia2
{
    public class CityAreas
    {
        public string header;
        public int unk0_int;
        public int areaCount;
        public int namesLength;
        public string names;
        AreaData[] areaCollection;

        public AreaData[] AreaCollection {
            get { return areaCollection; }
            set { areaCollection = value; }
        }

        public CityAreas(BinaryReader reader)
        {
            ReadFromFile(reader);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            header = new string(reader.ReadChars(4));
            unk0_int = reader.ReadInt32();
            areaCount = reader.ReadInt32();
            namesLength = reader.ReadInt32();
            names = new string(reader.ReadChars(namesLength));
            reader.ReadBytes(0);
            areaCollection = new AreaData[areaCount];

            for(int i = 0; i != areaCollection.Length; i++)
            {
                areaCollection[i] = new AreaData(reader);
                int pos = areaCollection[i].Index1;
                areaCollection[i].IndexedString = names.Substring(pos, names.IndexOf('\0', pos) - pos);
                pos = areaCollection[i].Index2;

                if(pos != 65535)
                    areaCollection[i].IndexedString2 = names.Substring(pos, names.IndexOf('\0', pos) - pos);
            }
          
        }

        public class AreaData
        {
            string name;
            ushort index1;
            string indexedString1;
            ushort index2;
            string indexedString2;
            byte unkByte;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public ushort Index1 {
                get { return index1; }
                set { index1 = value; }
            }
            public string IndexedString {
                get { return indexedString1; }
                set { indexedString1 = value; }
            }
            public ushort Index2 {
                get { return index2; }
                set { index2 = value; }
            }
            public string IndexedString2 {
                get { return indexedString2; }
                set { indexedString2 = value; }
            }
            public byte UnkByte {
                get { return unkByte; }
                set { unkByte = value; }
            }

            public AreaData(BinaryReader reader)
            {
                name = readString(reader);
                reader.ReadByte();
                index1 = reader.ReadUInt16();
                index2 = reader.ReadUInt16();
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
