using System.Collections.Generic;
using System.ComponentModel;
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

        private string m_buffer;

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
            //gotta fix the buffer!
            buildUpdateKeyStringBuffer();
            //now we can continue;
            writer.Write(m_buffer.Length);
            writer.Write(areaDatas.Length);
            writer.Write(unk1);
            writer.Write(unk2);
            writer.Write(unk3);
            writer.Write(m_buffer.ToCharArray());

            for (int i = 0; i != areas.Length; i++)
                areas[i].WriteToFile(writer);

            for (int i = 0; i != areaDatas.Length; i++)
                areaDatas[i].WriteToFile(writer);
        }

        private void buildUpdateKeyStringBuffer()
        {
            //fix this
            List<string> addedNames = new List<string>();
            List<ushort> addedPos = new List<ushort>();
            m_buffer = "";

            for (int i = 0; i != numAreas; i++)
            {
                int index = addedNames.IndexOf(areas[i].Name);
                if (index == -1)
                {
                    addedNames.Add(areas[i].Name);
                    addedPos.Add((ushort)m_buffer.Length);
                    areas[i].NameKey = (ushort)m_buffer.Length;
                    m_buffer += areas[i].Name + "\0";
                }
                else
                {
                    areas[i].NameKey = addedPos[index];
                }
            }
            for (int i = 0; i != numDatas; i++)
            {
                int index = addedNames.IndexOf(areaDatas[i].TranslokatorName);
                if (index == -1)
                {
                    addedNames.Add(areaDatas[i].TranslokatorName);
                    addedPos.Add((ushort)m_buffer.Length);
                    areaDatas[i].TranslokatorNameKey = (ushort)m_buffer.Length;
                    m_buffer += areaDatas[i].TranslokatorName + "\0";
                }
                else
                {
                    areaDatas[i].TranslokatorNameKey = addedPos[index];
                }

                for (int y = 0; y != areaDatas[i].Translokators.Length; y++)
                {
                    index = addedNames.IndexOf(areaDatas[i].Translokators[y].Name);
                    if (index == -1)
                    {
                        addedNames.Add(areaDatas[i].Translokators[y].Name);
                        addedPos.Add((ushort)m_buffer.Length);
                        areaDatas[i].Translokators[y].NameKey = (ushort)m_buffer.Length;
                        m_buffer += areaDatas[i].Translokators[y].Name + "\0";
                    }
                    else
                    {
                        areaDatas[i].Translokators[y].NameKey = addedPos[index];
                    }
                }
            }
        }

        public class Area
        {
            string string1;
            string string2;
            string name;
            ushort nameKey;

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
            [ReadOnly(true)]
            public ushort NameKey {
                get { return nameKey; }
                set { nameKey = value; }
            }

            public Area(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                string1 = Functions.ReadString(reader);
                string2 = Functions.ReadString(reader);
                nameKey = reader.ReadUInt16();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                Functions.WriteString(writer, string1);
                Functions.WriteString(writer, string2);
                writer.Write(nameKey);
            }
        }

        public class AreaData
        {
            string name;
            ushort translokatorNameKey;
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
            [ReadOnly(true)]
            public ushort TranslokatorNameKey {
                get { return translokatorNameKey; }
               set { translokatorNameKey = value; }
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
                translokatorNameKey = reader.ReadUInt16();
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

            public void WriteToFile(BinaryWriter writer)
            {
                Functions.WriteString(writer, name);
                writer.Write(translokatorNameKey);
                Functions.WriteString(writer, actorFile);
                Functions.WriteString(writer, description);
                writer.Write(unk1);
                writer.Write(unk2);
                writer.Write(unk3);
                writer.Write(entries.Length);

                for (int i = 0; i != numEntities; i++)
                    Functions.WriteString(writer, entries[i]);

                writer.Write(translokators.Length);
                for (int i = 0; i != translokators.Length; i++)
                {
                    translokators[i].WriteToFile(writer);
                }
            }

            

            public class TranslokatorData
            {
                ushort nameKey;
                string name;
                float positionX;
                float positionY;
                int mapMarkerIconID;
                int mapMarkerStringID;
                short[] unkData;

                [ReadOnly(true)]
                public ushort NameKey {
                    get { return nameKey; }
                    set { nameKey = value; }
                }
                public string Name {
                    get { return name; }
                    set { name = value; }
                }
                public float PositionX {
                    get { return positionX; }
                    set { positionX = value; }
                }
                public float PositionY {
                    get { return positionY; }
                    set { positionY = value; }
                }
                public int MapMarkerIconID {
                    get { return mapMarkerIconID; }
                    set { mapMarkerIconID = value; }
                }
                public int MapMarkerStringID {
                    get { return mapMarkerStringID; }
                    set { mapMarkerStringID = value; }
                }
                public short[] UnkData {
                    get { return unkData; }
                    set { unkData = value; }
                }

                public void ReadFromFile(BinaryReader reader, int numEntities)
                {
                    nameKey = reader.ReadUInt16();
                    positionX = reader.ReadSingle();
                    positionY = reader.ReadSingle();
                    mapMarkerIconID = reader.ReadInt32();
                    mapMarkerStringID = reader.ReadInt32();

                    unkData = new short[numEntities];
                    for (int i = 0; i != numEntities; i++)
                        unkData[i] = reader.ReadInt16();
                }

                public void WriteToFile(BinaryWriter writer)
                {
                    writer.Write(nameKey);
                    writer.Write(positionX);
                    writer.Write(positionY);
                    writer.Write(mapMarkerIconID);
                    writer.Write(mapMarkerStringID);
                    for (int i = 0; i != UnkData.Length; i++)
                        writer.Write(unkData[i]);
                }
            }
        }
    }
}
