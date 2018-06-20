using System;
using System.IO;

namespace Mafia2
{
    public class Prefab
    {
        int sizeOfFile; //-4 bytes
        int unk01; //possible count

        int sizeOfFile2; //-12 bytes
        long unk02; //possible hash


        public Prefab(string fileName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            sizeOfFile = reader.ReadInt32();
            unk01 = reader.ReadInt32();
            sizeOfFile2 = reader.ReadInt32();
            unk02 = reader.ReadInt64();

        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", 1, 2);
        }
    }
}
