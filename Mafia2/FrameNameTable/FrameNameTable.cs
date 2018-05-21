using System.IO;

namespace Mafia2
{
   public class FrameNameTable
    {
        public void ReadFromFile(BinaryReader reader)
        {
            string[] strings;
            unk01Struct[] unk01Struct;

            int stringLength = reader.ReadInt32();
            string sceneName = new string(reader.ReadChars(stringLength));
            strings = sceneName.Split('\0');

            int count = reader.ReadInt32();
            unk01Struct = new unk01Struct[count];

            for (int i = 0; i != unk01Struct.Length; i++)
            {
                unk01Struct[i].unk01 = reader.ReadInt16();
                unk01Struct[i].unk02 = reader.ReadInt16();
                unk01Struct[i].unk03 = reader.ReadInt16();
                unk01Struct[i].unk04 = reader.ReadInt32();
            }
        }

        struct unk01Struct
        {
            public short unk01;
            public short unk02;
            public short unk03;
            public int unk04;
        }
    }
}
