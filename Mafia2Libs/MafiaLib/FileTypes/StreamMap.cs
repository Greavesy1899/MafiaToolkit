using System;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Misc
{
    public class StreamMapLoader
    {
        private FileInfo file;
        public StreamGroup[] groups;
        public string[] groupHeaders;
        public StreamLine[] lines;
        public StreamMap3[] loaders;
        private StreamMap4[] s5;
        private ulong[] s6;

        public struct StreamGroup
        {
            public string name;
            public int nameIDX;
            public int unk1;
            public int unk2;
            public int startOffset; //start
            public int endOffset; //end
            public int unk5;

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5}", nameIDX, unk1, unk2, startOffset, endOffset, unk5);
            }
        }

        public class StreamLine
        {
            string name;
            string group;
            string flags;
            public int nameIDX;
            public int lineID;
            public int groupID;
            public int unk3;
            public int flagIDX;
            public int unk5;
            ulong unk10;
            ulong unk11;
            public int unk12;
            public int unk13;
            public int unk14;
            public int unk15;
            public string[] toLoad;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public string Group {
                get { return group; }
                set { group = value; }
            }
            public string Flags {
                get { return flags; }
                set { flags = value; }
            }
            public ulong Unk10 {
                get { return unk10; }
                set { unk10 = value; }
            }
            public ulong Unk11 {
                get { return unk11; }
                set { unk11 = value; }
            }

            public string[] ToLoad {
                get { return toLoad; }
                set { ToLoad = value; }
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", nameIDX, lineID, groupID, unk3, flagIDX, unk5, unk10, unk11, unk12, unk13, unk14, unk15);
            }
        }

        public struct StreamMap3
        {
            public string path;
            public int unk0;
            public int unk1;
            public int unk2;
            public int unk3;
            public int unk4;
            public int unk5;
            public int pathIDX;
            public int unk7;

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", unk0, unk1, unk2, unk3, unk4, unk5, pathIDX, unk7);
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

        private string ReadBufferSpecial(long start, long end, BinaryReader reader)
        {
            reader.BaseStream.Position = start;
            string newString = "";
            int nHitTrail = 0;

            while (nHitTrail != 2)
            {
                if(reader.PeekChar() == '\0' && nHitTrail < 2)
                {
                    newString += reader.ReadChar();
                    nHitTrail++;
                }
                else if(reader.PeekChar() == '\0')
                {
                    nHitTrail++;
                    reader.ReadByte();
                }
                else
                {
                    newString += reader.ReadChar();
                    nHitTrail = 0;
                }
            }
            reader.BaseStream.Position = end;
            return newString;
        }

        private string ReadFromBuffer(long start, long end, BinaryReader reader)
        {
            reader.BaseStream.Position = start;
            string result = StringHelpers.ReadString(reader);
            reader.BaseStream.Position = end;
            return result;
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

            groups = new StreamGroup[unk1];
            for (int i = 0; i < unk1; i++)
            {
                StreamGroup map = new StreamGroup();
                map.nameIDX = reader.ReadInt32();
                map.name = ReadFromBuffer((long)((ulong)map.nameIDX + (ulong)unkOffset7), reader.BaseStream.Position, reader);
                map.unk1 = reader.ReadInt32();
                map.unk2 = reader.ReadInt32();
                map.startOffset = reader.ReadInt32();
                map.endOffset = reader.ReadInt32();
                map.unk5 = reader.ReadInt32();
                groups[i] = map;
            }

            if (reader.BaseStream.Position != unkOffset2)
                throw new FormatException();

            groupHeaders = new string[unk2];

            for (int i = 0; i < unk2; i++)
                groupHeaders[i] = ReadFromBuffer((long)(reader.ReadUInt64() + (ulong)unkOffset7), reader.BaseStream.Position, reader);

            if (reader.BaseStream.Position != unkOffset3)
                throw new FormatException();

            lines = new StreamLine[unk3];

            for (int i = 0; i < unk3; i++)
            {
                StreamLine map = new StreamLine();
                map.nameIDX = reader.ReadInt32();
                map.lineID = reader.ReadInt32();
                map.groupID = reader.ReadInt32();
                map.unk3 = reader.ReadInt32();
                map.flagIDX = reader.ReadInt32();
                map.unk5 = reader.ReadInt32();
                map.Unk10 = reader.ReadUInt64();
                map.Unk11 = reader.ReadUInt64();
                map.unk12 = reader.ReadInt32();
                map.unk13 = reader.ReadInt32();
                map.unk14 = reader.ReadInt32();
                map.unk15 = reader.ReadInt32();
                map.Name = ReadFromBuffer((long)((ulong)map.nameIDX + (ulong)unkOffset7), reader.BaseStream.Position, reader);
                map.Flags = ReadBufferSpecial((long)((ulong)map.flagIDX + (ulong)unkOffset7), reader.BaseStream.Position, reader).Replace('\0', '|');
                map.toLoad = new string[2];
                map.toLoad[0] = "/sds/car/ascot_whatever.sds";
                map.toLoad[1] = "/sds/car/shove_it_up_your_butt.sds";
                lines[i] = map;
            }

            if (reader.BaseStream.Position != unkOffset4)
                throw new FormatException();

            loaders = new StreamMap3[unk4];

            for(int i = 0; i < unk4; i++)
            {
                StreamMap3 map = new StreamMap3();
                map.unk0 = reader.ReadInt32();
                map.unk1 = reader.ReadInt32();
                map.unk2 = reader.ReadInt32();
                map.unk3 = reader.ReadInt32();
                map.unk4 = reader.ReadInt32();
                map.unk5 = reader.ReadInt32();
                map.pathIDX = reader.ReadInt32();
                map.unk7 = reader.ReadInt32();
                map.path = ReadFromBuffer((long)((ulong)map.pathIDX + (ulong)unkOffset7), reader.BaseStream.Position, reader);
                loaders[i] = map;
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

            reader.BaseStream.Seek(unk7, SeekOrigin.Current);

            if (reader.BaseStream.Position != reader.BaseStream.Length)
                throw new FormatException("Borked this up");
        }
    }
}