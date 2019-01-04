using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class CityShops
    {
        public int fileVersion; //vanilla = 8; JA = 9;
        public int numAreas;
        public int bufferSize;
        public int numDatas;
        public int unk1;
        public int unk2;
        public int unk3;
        public Dictionary<int, string> names;
        public Dictionary<int, string> newNames;

        Area[] areas;
        AreaData[] areaDatas;


        public Area[] Areas {
            get { return areas; }
            set { areas = value; }
        }
        public AreaData[] AreaDatas {
            get { return areaDatas; }
            set { areaDatas = value; }
        }

        public CityShops(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != 1668576104)
                return;

            fileVersion = reader.ReadInt32();
            numAreas = reader.ReadInt32();
            bufferSize = reader.ReadInt32();
            numDatas = reader.ReadInt32();
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

                string name = Functions.ReadString(reader); //read string
                names.Add(offset, name); //add offset as unique key and string
            }

            areas = new Area[numAreas];
            areaDatas = new AreaData[numDatas];

            for (int i = 0; i != numAreas; i++)
            {
                areas[i] = new Area(reader);
                areas[i].Name = names[areas[i].NameKey];
            }
            for (int i = 0; i != numDatas; i++)
            {
                areaDatas[i] = new AreaData(reader);
                areaDatas[i].TranslokatorName = names[areaDatas[i].TranslokatorNameKey];

                for (int y = 0; y != areaDatas[i].Translokators.Length; y++)
                    areaDatas[i].Translokators[y].Name = names[areaDatas[i].Translokators[y].NameKey];
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(1668576104);
            writer.Write(fileVersion);
            writer.Write(areas.Length);

        }

        public class Area
        {
            string string1;
            string string2;
            string name;
            short nameKey;

            public string String1 {
                get { return string1; }
                set { string1 = value; }
            }
            public string String2 {
                get { return string2; }
                set { string2 = value; }
            }
            public string Name {
                get { return name; }
                set { name = value; }
            }
            public short NameKey {
                get { return nameKey; }
            }

            public Area(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                string1 = ReadString(reader);
                string2 = ReadString(reader);
                nameKey = reader.ReadInt16();
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

        public class AreaData
        {
            string name;
            short translokatorNameKey;
            string translokatorName;
            string actorFile;
            string description;
            short unk1;
            int unk2;
            int unk3;
            int numEntities;
            string[] entries;
            int numTranslokators;
            TranslokatorData[] translokators;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public short TranslokatorNameKey {
                get { return translokatorNameKey; }
            }
            public string TranslokatorName {
                get { return translokatorName; }
                set { translokatorName = value; }
            }
            public string ActorFile {
                get { return actorFile; }
                set { actorFile = value; }
            }
            public string Description {
                get { return description; }
                set { description = value; }
            }
            public short Unk1 {
                get { return unk1; }
                set { unk1 = value; }
            }
            public int Unk2 {
                get { return unk2; }
                set { unk2 = value; }
            }
            public int Unk3 {
                get { return unk3; }
                set { unk3 = value; }
            }
            public string[] Entries {
                get { return entries; }
                set { entries = value; }
            }
            public TranslokatorData[] Translokators {
                get { return translokators; }
                set { translokators = value; }
            }

            public AreaData(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                name = Functions.ReadString(reader);
                translokatorNameKey = reader.ReadInt16();
                actorFile = Functions.ReadString(reader);
                description = Functions.ReadString(reader);
                unk1 = reader.ReadInt16();
                unk2 = reader.ReadInt32();
                unk3 = reader.ReadInt32();
                numEntities = reader.ReadInt32();
                entries = new string[numEntities];
                
                for(int i = 0; i != numEntities; i++)
                {
                    entries[i] = Functions.ReadString(reader);
                }

                numTranslokators = reader.ReadInt32();
                translokators = new TranslokatorData[numTranslokators];

                for (int i = 0; i != numTranslokators; i++)
                {
                    translokators[i] = new TranslokatorData();
                    translokators[i].ReadFromFile(reader, numEntities);
                }
            }

            public class TranslokatorData
            {
                short nameKey;
                string name;
                float unk0;
                float unk1;
                int unk2;
                int unk3;
                short[] unkData;

                public short NameKey {
                    get { return nameKey; }
                }
                public string Name {
                    get { return name; }
                    set { name = value; }
                }
                public float Unk0 {
                    get { return unk0; }
                    set { unk0 = value; }
                }
                public float Unk1 {
                    get { return unk1; }
                    set { unk1 = value; }
                }
                public int Unk2 {
                    get { return unk2; }
                    set { unk2 = value; }
                }
                public int Unk3 {
                    get { return unk3; }
                    set { unk3 = value; }
                }
                public short[] UnkData {
                    get { return unkData; }
                    set { unkData = value; }
                }

                public void ReadFromFile(BinaryReader reader, int numEntities)
                {
                    nameKey = reader.ReadInt16();
                    unk0 = reader.ReadSingle();
                    unk1 = reader.ReadSingle();
                    unk2 = reader.ReadInt32();
                    unk3 = reader.ReadInt32();

                    unkData = new short[numEntities];
                    for (int i = 0; i != numEntities; i++)
                        unkData[i] = reader.ReadInt16();
                }
            }
        }
    }
}
