using System.IO;

namespace Mafia2
{
   public class FrameNameTable
    {
        string[] names;
        unk01Struct[] unk01arr;

        public void ReadFromFile(BinaryReader reader)
        {
            int stringLength = reader.ReadInt32();
            string sceneName = new string(reader.ReadChars(stringLength));
            names = sceneName.Split('\0');

            int count = reader.ReadInt32();
            unk01arr = new unk01Struct[count];

            for (int i = 0; i != unk01arr.Length; i++)
            {
                unk01arr[i].unk01 = reader.ReadInt16();
                unk01arr[i].unk02 = reader.ReadInt16();
                unk01arr[i].unk03 = reader.ReadInt16();
                unk01arr[i].unk04 = reader.ReadInt32();
            }
        }

        struct unk01Struct
        {
            public short unk01;
            public short unk02;
            public short unk03;
            public int unk04;

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3}", unk01, unk02, unk03, unk04);
            }
        }
    }
}
