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

        public CityAreas(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName+"2", FileMode.Create)))
            {
                WriteToFile(writer);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            header = new string(reader.ReadChars(4));
            unk0_int = reader.ReadInt32();
            areaCount = reader.ReadInt32();
            namesLength = reader.ReadInt32();
            names = new string(reader.ReadChars(namesLength));
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

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write("ratc".ToCharArray());
            writer.Write(unk0_int);
            writer.Write(areaCount);
            writer.Write(namesLength);
            writer.Write(names.ToCharArray());

            for (int i = 0; i != areaCollection.Length; i++)
                areaCollection[i].WriteToFile(writer);
        }

        public class AreaData
        {
            string name;
            byte unkByte0;
            ushort index1;
            string indexedString1;
            ushort index2;
            string indexedString2;
            byte unkByte1;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public byte UnkByte0 {
                get { return unkByte0; }
                set { unkByte0 = value; }
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
            public byte UnkByte1 {
                get { return unkByte1; }
                set { unkByte1 = value; }
            }

            public AreaData(BinaryReader reader)
            {
                name = ReadString(reader);
                reader.ReadByte();
                index1 = reader.ReadUInt16();
                index2 = reader.ReadUInt16();
                unkByte1 = reader.ReadByte();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                WriteString(writer, name);
                writer.Write(index1);
                writer.Write(index2);
                writer.Write(unkByte1);
            }

            private string ReadString(BinaryReader reader)
            {
                string newString = "";

                while(reader.PeekChar() != '\0')
                {
                    newString += reader.ReadChar();
                }
                return newString;
            }

            private void WriteString(BinaryWriter writer, string text)
            {
                writer.Write(text.ToCharArray());
                writer.Write('\0');
            }

            public override string ToString()
            {
                return string.Format("{0}", name);
            }
        }
    }
}
