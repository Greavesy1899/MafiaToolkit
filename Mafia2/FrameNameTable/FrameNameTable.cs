using System.IO;

namespace Mafia2
{
    public class FrameNameTable
    {
        int stringLength;
        string names;
        Data[] frameData;

        public FrameNameTable(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            stringLength = reader.ReadInt32();
            names = new string(reader.ReadChars(stringLength));

            int size = reader.ReadInt32();
            frameData = new Data[size];

            for (int i = 0; i != frameData.Length; i++)
            {
                frameData[i] = new Data(reader);

                int pos = frameData[i].NamePos1;
                frameData[i].Name = names.Substring(pos, names.IndexOf('\0', pos) - pos);
                pos = frameData[i].Parent;
                frameData[i].ParentName = names.Substring(pos, names.IndexOf('\0', pos) - pos);
            }
        }

        class Data
        {
            string parentName;
            string name;                   
            short parent;  
            ushort namepos1;
            ushort namepos2;
            short frameIndex;
            short flags;

            public string ParentName {
                get { return parentName; }
                set { parentName = value; }
            }
            public string Name {
                get { return name; }
                set { name = value; }
            }
            public short Parent {
                get { return parent; }
                set { parent = value; }
            }
            public ushort NamePos1 {
                get { return namepos1; }
                set { namepos1 = value; }
            }
            public ushort NamePos2 {
                get { return namepos2; }
                set { namepos2 = value; }
            }
            public short FrameIndex {
                get { return frameIndex; }
                set { frameIndex = value; }
            }
            public short Flags {
                get { return flags; }
                set { flags = value; }
            }

            public Data(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                parent = reader.ReadInt16();
                namepos1 = reader.ReadUInt16();
                namepos2 = reader.ReadUInt16();
                frameIndex = reader.ReadInt16();
                flags = reader.ReadInt16();
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, Frame Index: {2}", parentName, name, frameIndex);
            }
        }
    }
}
