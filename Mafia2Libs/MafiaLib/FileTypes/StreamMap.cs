using System;
using System.ComponentModel;
using System.Globalization;
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
        public StreamLoader[] loaders;
        public StreamBlock[] blocks;
        public ulong[] hashes;

        public class StreamGroup
        {
            string name;
            public int nameIDX;
            int type;
            int unk01;
            public int startOffset; //start
            public int endOffset; //end
            public int unk5;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public int Type {
                get { return type; }
                set { type = value; }
            }
            public int Unk01 {
                get { return unk01; }
                set { unk01 = value; }
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5}", nameIDX, type, unk01, startOffset, endOffset, unk5);
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
            int loadType;
            public int flagIDX;
            int unk5;
            ulong unk10;
            ulong unk11;
            int unk12;
            int unk13;
            int unk14;
            int unk15;
            public StreamLoader[] loadList;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public string Group {
                get { return group; }
                set { group = value; }
            }
            public int LoadType {
                get { return loadType; }
                set { loadType = value; }
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
            public StreamLoader[] LoadList {
                get { return loadList; }
                set { loadList = value; }
            }
            public int Unk5 {
                get { return unk5; }
                set { unk5 = value; }
            }
            public int Unk12 {
                get { return unk12; }
                set { unk12 = value; }
            }
            public int Unk13 {
                get { return unk13; }
                set { unk13 = value; }
            }
            public int Unk14 {
                get { return unk14; }
                set { unk14 = value; }
            }
            public int Unk15 {
                get { return unk15; }
                set { unk15 = value; }
            }


            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", nameIDX, lineID, groupID, loadType, flagIDX, unk5, unk10, unk11, unk12, unk13, unk14, unk15);
            }
        }

        [TypeConverter(typeof(StreamLoaderConverter))]
        public class StreamLoader
        {
            string path;
            string entity;
            public int start;
            public int end;
            public int type;
            public int loaderSubID;
            public int loaderID;
            public int loadType;
            public int pathIDX;
            public int entityIDX;
            bool isChild;

            public int LoadType {
                get { return loadType; }
                set { loadType = value; }
            }
            public string Path {
                get { return path; }
                set { path = value; }
            }
            public string Entity {
                get { return entity; }
                set { entity = value; }
            }
            [Browsable(false)]
            public int LoaderSubID {
                get { return loaderSubID; }
                set { loaderSubID = value; }
            }
            [Browsable(false)]
            public int LoaderID {
                get { return loaderID; }
                set { loaderID = value; }
            }
            public bool IsChild {
                get { return loaderSubID == 1 ? true : false; }
                set { isChild = value; }
            }

            public string ToEditorString()
            {
                return string.Format("{0}\t{1}\t{2}\t\t{3}", loadType, path, entity, IsChild);
            }
            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7}", start, end, type, loaderSubID, loaderID, loadType, pathIDX, entityIDX);
            }
        }

        public class StreamBlock
        {
            public int startOffset;
            public int endOffset;
            ulong[] hashes;

            public ulong[] Hashes {
                get { return hashes; }
                set { hashes = value; }
            }

            public override string ToString()
            {
                return string.Format("{0} {1}", startOffset, endOffset);
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
                if (reader.PeekChar() == '\0' && nHitTrail < 2)
                {
                    newString += reader.ReadChar();
                    nHitTrail++;
                }
                else if (reader.PeekChar() == '\0')
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

            int numGroups = reader.ReadInt32();
            int groupOffset = reader.ReadInt32();
            int numHeaders = reader.ReadInt32();
            int headerOffset = reader.ReadInt32();
            int numLines = reader.ReadInt32();
            int lineOffset = reader.ReadInt32();
            int numLoaders = reader.ReadInt32();
            int loadersOffset = reader.ReadInt32();
            int numBlocks = reader.ReadInt32();
            int blockOffset = reader.ReadInt32();
            int numHashes = reader.ReadInt32();
            int hashOffset = reader.ReadInt32();
            int poolSize = reader.ReadInt32();
            int poolOffset = reader.ReadInt32();

            if (reader.BaseStream.Position != groupOffset)
                throw new FormatException();

            groups = new StreamGroup[numGroups];
            for (int i = 0; i < numGroups; i++)
            {
                StreamGroup map = new StreamGroup();
                map.nameIDX = reader.ReadInt32();
                map.Name = ReadFromBuffer((long)((ulong)map.nameIDX + (ulong)poolOffset), reader.BaseStream.Position, reader);
                map.Type = reader.ReadInt32();
                map.Unk01 = reader.ReadInt32();
                map.startOffset = reader.ReadInt32();
                map.endOffset = reader.ReadInt32();
                map.unk5 = reader.ReadInt32();
                groups[i] = map;
            }

            if (reader.BaseStream.Position != headerOffset)
                throw new FormatException();

            groupHeaders = new string[numHeaders];

            for (int i = 0; i < numHeaders; i++)
                groupHeaders[i] = ReadFromBuffer((long)(reader.ReadUInt64() + (ulong)poolOffset), reader.BaseStream.Position, reader);

            if (reader.BaseStream.Position != lineOffset)
                throw new FormatException();

            lines = new StreamLine[numLines];

            for (int i = 0; i < numLines; i++)
            {
                StreamLine map = new StreamLine();
                map.nameIDX = reader.ReadInt32();
                map.lineID = reader.ReadInt32();
                map.groupID = reader.ReadInt32();
                map.LoadType = reader.ReadInt32();
                map.flagIDX = reader.ReadInt32();
                map.Unk5 = reader.ReadInt32();
                map.Unk10 = reader.ReadUInt64();
                map.Unk11 = reader.ReadUInt64();
                map.Unk12 = reader.ReadInt32();
                map.Unk13 = reader.ReadInt32();
                map.Unk14 = reader.ReadInt32();
                map.Unk15 = reader.ReadInt32();
                map.Name = ReadFromBuffer((long)((ulong)map.nameIDX + (ulong)poolOffset), reader.BaseStream.Position, reader);
                map.Flags = ReadBufferSpecial((long)((ulong)map.flagIDX + (ulong)poolOffset), reader.BaseStream.Position, reader).Replace('\0', '|');
                lines[i] = map;
            }

            if (reader.BaseStream.Position != loadersOffset)
                throw new FormatException();

            loaders = new StreamLoader[numLoaders];

            for (int i = 0; i < numLoaders; i++)
            {
                StreamLoader map = new StreamLoader();
                map.start = reader.ReadInt32();
                map.end = reader.ReadInt32();
                map.type = reader.ReadInt32(); 
                map.loaderSubID = reader.ReadInt32(); 
                map.loaderID = reader.ReadInt32();
                map.LoadType = reader.ReadInt32();
                map.pathIDX = reader.ReadInt32();
                map.entityIDX = reader.ReadInt32();
                map.Path = ReadFromBuffer((long)((ulong)map.pathIDX + (ulong)poolOffset), reader.BaseStream.Position, reader);
                map.Entity = ReadFromBuffer((long)((ulong)map.entityIDX + (ulong)poolOffset), reader.BaseStream.Position, reader);
                loaders[i] = map;
            }

            if (reader.BaseStream.Position != blockOffset)
                throw new FormatException();

            blocks = new StreamBlock[numBlocks];
            for (int i = 0; i < numBlocks; i++)
            {
                StreamBlock map = new StreamBlock();
                map.startOffset = reader.ReadInt32();
                map.endOffset = reader.ReadInt32();
                blocks[i] = map;
            }

            if (reader.BaseStream.Position != hashOffset)
                throw new FormatException();

            hashes = new ulong[numHashes];

            for (int i = 0; i < numHashes; i++)
                hashes[i] = reader.ReadUInt64();

            if (reader.BaseStream.Position != poolOffset)
                throw new FormatException();

            reader.BaseStream.Seek(poolSize, SeekOrigin.Current);

            if (reader.BaseStream.Position != reader.BaseStream.Length)
                throw new FormatException("Borked this up");
        }

        public class StreamLoaderConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                object result = null;
                string stringValue = value as string;

                if (!string.IsNullOrEmpty(stringValue))
                {
                    string[] split = stringValue.Split(' ');
                    StreamLoader container = new StreamLoader();
                    container.LoadType = int.Parse(split[0]);
                    container.Path = split[1];
                    container.Entity = split[2];
                    container.IsChild = bool.Parse(split[3]);
                }

                return result ?? base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                object result = null;
                StreamLoader container = (StreamLoader)value;

                if (destinationType == typeof(String))
                    result = container.ToEditorString();

                return result ?? base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}