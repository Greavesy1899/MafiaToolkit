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

        List<Area> areas;
        List<AreaData> areaDatas;


        public List<Area> Areas {
            get { return areas; }
            set
            {
                areas = value;
                numAreas = areas.Count;
            }
        }
        public List<AreaData> AreaDatas {
            get { return areaDatas; }
            set 
            {
                areaDatas = value;
                numDatas = areaDatas.Count;
            }
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

            areas = new List<Area>();
            areaDatas = new List<AreaData>();

            for (int i = 0; i != numAreas; i++)
            {
                Area area = new Area();
                area.ReadFromFile(reader, fileVersion);
                area.Name = names[area.NameKey];
                areas.Add(area);
            }
            for (int i = 0; i != numDatas; i++)
            {
                AreaData areaData = new AreaData();
                areaData.ReadFromFile(reader, fileVersion);
                areaData.TranslokatorName = names[areaData.TranslokatorNameKey];
                for (int y = 0; y != areaData.Translokators.Count; y++)
                    areaData.Translokators[y].Name = names[areaData.Translokators[y].NameKey];
                areaDatas.Add(areaData);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(1668576104);
            writer.Write(fileVersion);
            writer.Write(areas.Count);
            //gotta fix the buffer!
            buildUpdateKeyStringBuffer();
            //now we can continue;
            writer.Write(m_buffer.Length);
            writer.Write(areaDatas.Count);
            writer.Write(unk1);
            writer.Write(unk2);
            writer.Write(unk3);
            writer.Write(m_buffer.ToCharArray());

            for (int i = 0; i != areas.Count; i++)
                areas[i].WriteToFile(writer, fileVersion);

            for (int i = 0; i != areaDatas.Count; i++)
                areaDatas[i].WriteToFile(writer, fileVersion);
        }

        public void PopulateTranslokatorEntities()
        {
            for (int i = 0; i != areaDatas.Count; i++)
            {
                for (int y = 0; y != areaDatas[i].Translokators.Count; y++)
                {
                    bool fix = false;
                    if(areaDatas[i].Translokators[y].EntityProperties == null)
                        fix = true;
                    else if(areaDatas[i].Translokators[y].EntityProperties.Count != areaDatas[i].Entries.Count)
                        fix = true;

                    if(fix)
                    {
                        areaDatas[i].Translokators[y].EntityProperties = new List<short>();

                        for (int z = 0; z != areaDatas[i].Entries.Count; z++)
                            areaDatas[i].Translokators[y].EntityProperties.Add(1023);
                    }
                }
            }
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

                for (int y = 0; y != areaDatas[i].Translokators.Count; y++)
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

            public Area()
            {
            }

            public void ReadFromFile(BinaryReader reader, int version)
            {
                string1 = Functions.ReadString(reader);
                string2 = Functions.ReadString(reader);

                if (version == 9)
                    reader.ReadByte();

                nameKey = reader.ReadUInt16();
            }

            public void WriteToFile(BinaryWriter writer, int version)
            {
                Functions.WriteString(writer, string1);
                Functions.WriteString(writer, string2);

                if (version == 9)
                    writer.Write((byte)0);

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
            List<string> entries;
            int numTranslokators;
            List<TranslokatorData> translokators;

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
            public List<string> Entries {
                get { return entries; }
                set { entries = value; }
            }
            public List<TranslokatorData> Translokators {
                get { return translokators; }
                set { translokators = value; }
            }

            public void ReadFromFile(BinaryReader reader, int fileVersion)
            {
                name = Functions.ReadString(reader);
                translokatorNameKey = reader.ReadUInt16();
                actorFile = Functions.ReadString(reader);
                description = Functions.ReadString(reader);
                unk1 = reader.ReadInt16();
                unk2 = reader.ReadInt32();
                unk3 = reader.ReadInt32();
                numEntities = reader.ReadInt32();
                entries = new List<string>();
                
                for(int i = 0; i != numEntities; i++)
                {
                    entries.Add(Functions.ReadString(reader));
                }

                numTranslokators = reader.ReadInt32();
                translokators = new List<TranslokatorData>();

                for (int i = 0; i != numTranslokators; i++)
                {
                    TranslokatorData translokator = new TranslokatorData();
                    translokator.ReadFromFile(reader, numEntities, fileVersion);
                    translokators.Add(translokator);
                }
            }

            public void WriteToFile(BinaryWriter writer, int fileVersion)
            {
                Functions.WriteString(writer, name);
                writer.Write(translokatorNameKey);
                Functions.WriteString(writer, actorFile);
                Functions.WriteString(writer, description);
                writer.Write(unk1);
                writer.Write(unk2);
                writer.Write(unk3);
                writer.Write(entries.Count);

                for (int i = 0; i != numEntities; i++)
                    Functions.WriteString(writer, entries[i]);

                writer.Write(translokators.Count);
                for (int i = 0; i != translokators.Count; i++)
                {
                    translokators[i].WriteToFile(writer, fileVersion);
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
                List<short> entityProperties;

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
                public List<short> EntityProperties {
                    get { return entityProperties; }
                    set { entityProperties = value; }
                }

                public void ReadFromFile(BinaryReader reader, int numEntities, int fileVersion)
                {
                    nameKey = reader.ReadUInt16();
                    positionX = reader.ReadSingle();
                    positionY = reader.ReadSingle();
                    mapMarkerIconID = reader.ReadInt32();
                    mapMarkerStringID = reader.ReadInt32();

                    if(fileVersion == 9)
                        reader.ReadByte();

                    entityProperties = new List<short>();
                    for (int i = 0; i != numEntities; i++)
                        entityProperties.Add(reader.ReadInt16());
                }

                public void WriteToFile(BinaryWriter writer, int fileVersion)
                {
                    writer.Write(nameKey);
                    writer.Write(positionX);
                    writer.Write(positionY);
                    writer.Write(mapMarkerIconID);
                    writer.Write(mapMarkerStringID);

                    if (fileVersion == 9)
                        writer.Write((byte)0);

                    for (int i = 0; i != entityProperties.Count; i++)
                        writer.Write(entityProperties[i]);
                }
            }
        }
    }
}
