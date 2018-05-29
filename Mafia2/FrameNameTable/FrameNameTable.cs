using System.IO;

namespace Mafia2
{
    public class FrameNameTable
    {
        int stringLength;
        string names;
        Data[] frameData;

        public void ReadFromFile(BinaryReader reader)
        {
            stringLength = reader.ReadInt32();
            names = new string(reader.ReadChars(stringLength));

            int count = reader.ReadInt32();
            frameData = new Data[count];

            for (int i = 0; i != frameData.Length; i++)
            {
                frameData[i] = new Data(reader);

                int pos = frameData[i].NamePos1;
                frameData[i].Name = names.Substring(pos, names.IndexOf('\0', pos) - pos);
            }
        }

        class Data
        {
            //more research required.
            string name;                   
            short unk01;                  //still unknown. OR this is the potential root.
            ushort namepos1;
            ushort namepos2;               //Sometimes ends up as 65535. null in the engine?
            int frameIndex;               //This index begins AFTER objects. Sometimes has massive values.

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public short Unk01 {
                get { return unk01; }
                set { unk01 = value; }
            }
            public ushort NamePos1 {
                get { return namepos1; }
                set { namepos1 = value; }
            }
            public ushort NamePos2 {
                get { return namepos2; }
                set { namepos2 = value; }
            }
            public int FrameIndex {
                get { return frameIndex; }
                set { frameIndex = value; }
            }

            public Data(BinaryReader reader)
            {
                ReadFromFile(reader);
            }

            public void ReadFromFile(BinaryReader reader)
            {
                unk01 = reader.ReadInt16();
                namepos1 = reader.ReadUInt16();
                namepos2 = reader.ReadUInt16();
                frameIndex = reader.ReadInt32();
            }

            public override string ToString()
            {
                return string.Format("{0}, {1}, Frame Index: {2}", unk01, name, frameIndex);
            }
        }
    }
}
