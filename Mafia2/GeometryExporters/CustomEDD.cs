using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class CustomEDD
    {
        string header = "ED10";
        int entryCount;
        Entry[] entries;

        public int EntryCount {
            get { return entryCount; }
            set { entryCount = value; }
        }
        public Entry[] Entries {
            get { return entries; }
            set { entries = value; }
        }

        public CustomEDD() { }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write(header.ToCharArray());
            writer.Write(entryCount);
            for (int i = 0; i != entryCount; i++)
                entries[i].WriteToFile(writer);
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

            public void WriteToFile(BinaryWriter writer)
            {
                writer.Write(lodCount);
                position.WriteToFile(writer);
                rotation.WriteToFile(writer);
                for (int i = 0; i != lodCount; i++)
                    writer.Write(lodNames[i]);
            }
        }

    }
}
