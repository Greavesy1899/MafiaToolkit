using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ResourceTypes.Misc
{
    public class FrameProps
    {
        public const int Signature = 1718775152;
        public int version;
        public int[] unks;
        public int[] unks2;
        public string[] unks3;
        public ulong[] unks4;
        public uint[] unks5;

        public FrameProps(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != Signature)
                return;

            version = reader.ReadInt32();
            unks = new int[5];
            for (int i = 0; i != 5; i++)
                unks[i] = reader.ReadInt32();

            unks2 = new int[unks[0]];
            unks3 = new string[unks[0]];
            unks4 = new ulong[unks[2]];
            unks5 = new uint[unks[2]];

            for (int i = 0; i != unks2.Length; i++)
                unks2[i] = reader.ReadInt32();


            for (int i = 0; i != unks3.Length; i++)
                unks3[i] = ReadString(reader);

            for (int i = 0; i != unks4.Length; i++)
                unks4[i] = reader.ReadUInt64();

            for (int i = 0; i != unks5.Length; i++)
                unks5[i] = reader.ReadUInt32();

            WriteToText();
        }

        private void WriteToText()
        {
            List<string> file = new List<string>();
            for(int i = 0; i != unks.Length; i++)
                file.Add(i + " " + unks[i].ToString());
            file.Add("");
            for (int i = 0; i != unks2.Length; i++)
                file.Add(i + " " + unks2[i].ToString());
            file.Add("");
            for (int i = 0; i != unks3.Length; i++)
                file.Add(i + " " + unks3[i].ToString());
            file.Add("");
            for (int i = 0; i != unks4.Length; i++)
                file.Add(i + " " + unks4[i].ToString());
            file.Add("");
            for (int i = 0; i != unks5.Length; i++)
                file.Add(i + " " + unks5[i].ToString());

            File.WriteAllLines("FrameProps.txt", file.ToArray());
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
    }
}
