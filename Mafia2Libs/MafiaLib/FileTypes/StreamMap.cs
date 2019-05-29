using SharpDX;
using System;
using System.IO;
using Utils.SharpDXExtensions;
using Utils.Types;

namespace ResourceTypes.Misc
{
    public class StreamMapLoader
    {
        private FileInfo file;

        public StreamMapLoader(FileInfo info)
        {
            file = info;
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            if (reader.ReadInt32() != 1299346515)
                return;

            if (reader.ReadInt32() != 0x6)
                return;

            int fileSize = reader.ReadInt32();
            int unk0 = reader.ReadInt32();
            int[] unkInts = new int[14];

            for (int i = 0; i < unkInts.Length; i++)
                unkInts[i] = reader.ReadInt32();

            int[] chunk0 = new int[unkInts[0]];

        }
    }
}