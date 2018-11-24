using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class CustomEDD
    {
        string header = "ED10";
        int entryCount;
        List<Entry> entries;

        public int EntryCount {
            get { return entryCount; }
            set { entryCount = value; }
        }
        public List<Entry> Entries {
            get { return entries; }
            set { entries = value; }
        }

        public CustomEDD()
        {
            entries = new List<Entry>();
        }

        public CustomEDD(FileInfo info)
        {
            entries = new List<Entry>();

            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(header.ToCharArray());
            writer.Write(entryCount);
            for (int i = 0; i != entryCount; i++)
                entries[i].WriteToFile(writer);
        }

        public void ReadFromFile(BinaryReader reader)
        {
            reader.BaseStream.Position += 4;
            entryCount = reader.ReadInt32();

            for (int i = 0; i != entryCount; i++)
                entries.Add(new Entry(reader));
        }

        public class Entry
        {
            int lodCount;
            Vector3 position;
            Matrix33 rotation;
            string[] lodNames;

            public int LodCount {
                get { return lodCount; }
                set { lodCount = value; }
            }
            public Vector3 Position {
                get { return position; }
                set { position = value; }
            }
            public Matrix33 Rotation {
                get { return rotation; }
                set { rotation = value; }
            }
            public string[] LODNames {
                get { return lodNames; }
                set { lodNames = value; }
            }

            public Entry()
            {
                lodCount = 0;
                position = new Vector3(0);
                rotation = new Matrix33();
                lodNames = new string[lodCount];
            }

            public Entry(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(lodCount);
                position.WriteToFile(writer);
                rotation.WriteToFile(writer);
                for (int i = 0; i != lodCount; i++)
                    writer.Write(lodNames[i]);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                lodCount = reader.ReadInt32();
                position = new Vector3(reader);

                Vector3 vec1 = new Vector3(reader);
                Vector3 vec2 = new Vector3(reader);
                Vector3 vec3 = new Vector3(reader);
                rotation = new Matrix33(vec1, vec2, vec3, false);

                lodNames = new string[lodCount];
                for (int i = 0; i != lodCount; i++)
                    lodNames[i] = reader.ReadString();
            }
        }

    }
}
