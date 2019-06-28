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
        private StreamMap1[] s1;
        private ulong[] s2;
        private StreamMap2[] s3;
        private StreamMap3[] s4;
        private StreamMap4[] s5;
        private ulong[] s6;
        private string s7;

        public struct StreamMap1
        {
            public int unk0;
            public int unk1;
            public int unk2;
            public int unk3;
            public int unk4;
            public int unk5;

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5}", unk0, unk1, unk2, unk3, unk4, unk5);
            }
        }

        public struct StreamMap2
        {
            public int unk0;
            public int unk1;
            public int unk2;
            public int unk3;
            public int unk4;
            public int unk5;
            public ulong unk10;
            public ulong unk11;
            public int unk12;
            public int unk13;
            public int unk14;
            public int unk15;

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", unk0, unk1, unk2, unk3, unk4, unk5, unk10, unk11, unk12, unk13, unk14, unk15);
            }
        }

        public struct StreamMap3
        {
            public int unk0;
            public int unk1;
            public int unk2;
            public int unk3;
            public int unk4;
            public int unk5;
            public int unk6;
            public int unk7;

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", unk0, unk1, unk2, unk3, unk4, unk5, unk6, unk7);
            }
        }

        public struct StreamMap4
        {
            public int unk0;
            public int unk1;

            public override string ToString()
            {
                return string.Format("{0} {1}", unk0, unk1);
            }
        }
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

            int unk1 = reader.ReadInt32();
            int unkOffset1 = reader.ReadInt32();
            int unk2 = reader.ReadInt32();
            int unkOffset2 = reader.ReadInt32();
            int unk3 = reader.ReadInt32();
            int unkOffset3 = reader.ReadInt32();
            int unk4 = reader.ReadInt32();
            int unkOffset4 = reader.ReadInt32();
            int unk5 = reader.ReadInt32();
            int unkOffset5 = reader.ReadInt32();
            int unk6 = reader.ReadInt32();
            int unkOffset6 = reader.ReadInt32();
            int unk7 = reader.ReadInt32();
            int unkOffset7 = reader.ReadInt32();

            if (reader.BaseStream.Position != unkOffset1)
                throw new FormatException();

            s1 = new StreamMap1[unk1];
            for (int i = 0; i < unk1; i++)
            {
                StreamMap1 map = new StreamMap1();
                map.unk0 = reader.ReadInt32();
                map.unk1 = reader.ReadInt32();
                map.unk2 = reader.ReadInt32();
                map.unk3 = reader.ReadInt32();
                map.unk4 = reader.ReadInt32();
                map.unk5 = reader.ReadInt32();
                s1[i] = map;
            }

            if (reader.BaseStream.Position != unkOffset2)
                throw new FormatException();

            s2 = new ulong[unk2];

            for (int i = 0; i < unk2; i++)
                s2[i] = reader.ReadUInt64();

            if (reader.BaseStream.Position != unkOffset3)
                throw new FormatException();

            s3 = new StreamMap2[unk3];

            for (int i = 0; i < unk3; i++)
            {
                StreamMap2 map = new StreamMap2();
                map.unk0 = reader.ReadInt32();
                map.unk1 = reader.ReadInt32();
                map.unk2 = reader.ReadInt32();
                map.unk3 = reader.ReadInt32();
                map.unk4 = reader.ReadInt32();
                map.unk5 = reader.ReadInt32();
                map.unk10 = reader.ReadUInt64();
                map.unk11 = reader.ReadUInt64();
                map.unk12 = reader.ReadInt32();
                map.unk13 = reader.ReadInt32();
                map.unk14 = reader.ReadInt32();
                map.unk15 = reader.ReadInt32();
                s3[i] = map;
            }

            if (reader.BaseStream.Position != unkOffset4)
                throw new FormatException();

            s4 = new StreamMap3[unk4];

            for(int i = 0; i < unk4; i++)
            {
                StreamMap3 map = new StreamMap3();
                map.unk0 = reader.ReadInt32();
                map.unk1 = reader.ReadInt32();
                map.unk2 = reader.ReadInt32();
                map.unk3 = reader.ReadInt32();
                map.unk4 = reader.ReadInt32();
                map.unk5 = reader.ReadInt32();
                map.unk6 = reader.ReadInt32();
                map.unk7 = reader.ReadInt32();
                s4[i] = map;
            }

            if (reader.BaseStream.Position != unkOffset5)
                throw new FormatException();

            s5 = new StreamMap4[unk5];
            for (int i = 0; i < unk5; i++)
            {
                StreamMap4 map = new StreamMap4();
                map.unk0 = reader.ReadInt32();
                map.unk1 = reader.ReadInt32();
                s5[i] = map;
            }

            if (reader.BaseStream.Position != unkOffset6)
                throw new FormatException();

            s6 = new ulong[unk6];

            for (int i = 0; i < unk6; i++)
                s6[i] = reader.ReadUInt64();

            if (reader.BaseStream.Position != unkOffset7)
                throw new FormatException();

            s7 = new string(reader.ReadChars(unk7));
        }
    }
}