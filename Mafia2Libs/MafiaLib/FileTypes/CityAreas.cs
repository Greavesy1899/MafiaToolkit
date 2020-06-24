using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.City
{
    public class CityAreas
    {
        public int areaCount;
        public int namesLength;
        public string names;
        List<AreaData> areaCollection;

        public List<AreaData> AreaCollection {
            get { return areaCollection; }
            set { areaCollection = value; }
        }

        public CityAreas(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != 1668571506)
            {
                return;
            }

            if (reader.ReadInt32() != 1)
            {
                return;
            }

            areaCount = reader.ReadInt32();
            namesLength = reader.ReadInt32();
            names = new string(reader.ReadChars(namesLength));
            areaCollection = new List<AreaData>();

            for(int i = 0; i != areaCount; i++)
            {
                AreaData areaData = new AreaData();
                areaData.ReadFromFile(reader);
                int pos = areaData.Index1;

                areaData.AreaName1 = names.Substring(pos, names.IndexOf('\0', pos) - pos);
                pos = areaData.Index2;

                if (pos != 65535)
                {
                    areaData.AreaName2 = names.Substring(pos, names.IndexOf('\0', pos) - pos);
                }

                areaCollection.Add(areaData);
            }         
        }

        public void RebuildNames()
        {
            List<string> addedNames = new List<string>();
            List<ushort> addedNamesPos = new List<ushort>();
            string namePool = "";

            foreach(AreaData area in areaCollection)
            {
                area.Index1 = 0;
                area.Index2 = 0;
                int index = -1;
                //do area1 stuff.
                if (!string.IsNullOrEmpty(area.AreaName1))
                {
                    index = addedNames.IndexOf(area.AreaName1);
                    if (index == -1)
                    {
                        //update city area data first.
                        area.Index1 = (ushort)namePool.Length;
                        namePool += area.AreaName1;
                        namePool += '\0';

                        //make sure it doesn't happen again.
                        addedNames.Add(area.AreaName1);
                        addedNamesPos.Add(area.Index1);
                    }
                    else
                    {
                        area.Index1 = addedNamesPos[index];
                        area.AreaName1 = addedNames[index];
                    }
                }
                else if (string.IsNullOrEmpty(area.AreaName1))
                {
                    area.Index1 = 65535;
                }

                //do area 2 stuff.
                if (!string.IsNullOrEmpty(area.AreaName2))
                {
                    index = addedNames.IndexOf(area.AreaName2);
                    if (index == -1)
                    {
                        //update city area data first.
                        area.Index2 = (ushort)namePool.Length;
                        namePool += area.AreaName2;
                        namePool += '\0';

                        //make sure it doesn't happen again.
                        addedNames.Add(area.AreaName2);
                        addedNamesPos.Add(area.Index2);
                    }
                    else
                    {
                        area.Index2 = addedNamesPos[index];
                        area.AreaName2 = addedNames[index];
                    }
                }
                else if(string.IsNullOrEmpty(area.AreaName2))
                {
                    area.Index2 = 65535;
                }
            }

            names = namePool;
            namesLength = names.Length;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            //update DB data.
            RebuildNames();
            areaCount = areaCollection.Count;

            //now we shall write.
            writer.Write(1668571506);
            writer.Write(1);
            writer.Write(areaCount);
            writer.Write(namesLength);
            writer.Write(names.ToCharArray());

            for (int i = 0; i != areaCollection.Count; i++)
            {
                areaCollection[i].WriteToFile(writer);
            }
        }

        public class AreaData
        {
            string name;
            ushort index1;
            string areaString1;
            ushort index2;
            string areaString2;
            bool unkByte;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            [Browsable(false)]
            public ushort Index1 {
                get { return index1; }
                set { index1 = value; }
            }
            public string AreaName1 {
                get { return areaString1; }
                set { areaString1 = value; }
            }
            [Browsable(false)]
            public ushort Index2 {
                get { return index2; }
                set { index2 = value; }
            }
            public string AreaName2 {
                get { return areaString2; }
                set { areaString2 = value; }
            }
            public bool UnkByte {
                get { return unkByte; }
                set { unkByte = value; }
            }

            public void Create()
            {
                name = "NEW_AREA";
            }

            public void ReadFromFile(BinaryReader reader)
            {
                name = StringHelpers.ReadString(reader);
                index1 = reader.ReadUInt16();
                index2 = reader.ReadUInt16();
                unkByte = reader.ReadBoolean();
            }

            public void WriteToFile(BinaryWriter writer)
            {
                StringHelpers.WriteString(writer, name);
                writer.Write(index1);
                writer.Write(index2);
                writer.Write(unkByte);
            }

            public override string ToString()
            {
                return string.Format("{0}", name);
            }
        }
    }
}
