﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Utils.Logging;
using Utils.StringHelpers;

namespace ResourceTypes.Misc
{
    public enum GroupTypes
    {
        Null = 0,
        City = 1,
        City_Ground = 2,
        City_Univers = 3,
        Shop = 4,
        Char_Universe = 5,
        Player = 6,
        H_Char = 7,
        L_Char = 8,
        Police_Char = 9,
        Car_Universe = 10,
        Car = 11,
        Car_Big = 12,
        Car_Police = 13,
        Car_Script = 14,
        Base_Anim = 15,
        Weapons = 16,
        GUI = 17,
        Sky = 18,
        Tables = 19,
        Default_Sound = 20,
        Particles = 21,
        Game_Script = 23,
        Mission_Script = 24,
        Script = 25,
        TwoH_Char_In_One = 26,
        FiveL_Char_In_One = 27,
        ThreeCar_In_One = 28,
        Char_Police = 29,
        Trees = 30,
        City_Crash = 31,
        Small = 33,
        Generate = 32,
        Script_Sounds = 34,
        Director_Lua = 35,
        Mapa = 36,
        Sound_City = 37,
        Anims_City = 38,
        Kyn_City_Part = 39,
        Kyn_City_Shop = 40,
        Generic_Speech_Normal = 41,
        Generic_Speech_Gangster = 42,
        Generic_Speeh_Various = 43,
        Generic_Speech_Story = 44,
        Big_Script = 45,
        Big_Mission_Script = 46,
        Generic_Speech_Police = 47,
        Text = 48,
        Ingame = 50,
        Ingame_GUI = 51,
        Dabing = 52,

    }
    public class StreamMapLoader
    {
        private FileInfo file;
        private StreamGroup[] groups;
        private string[] groupHeaders;
        private StreamLine[] lines;
        private StreamLoader[] loaders;
        private StreamBlock[] blocks;

        public StreamGroup[] Groups {
            get { return groups; }
            set { groups = value; }
        }
        public string[] GroupHeaders {
            get { return groupHeaders; }
            set { groupHeaders = value; }
        }
        public StreamLine[] Lines {
            get { return lines; }
            set { lines = value; }
        }
        public StreamLoader[] Loaders {
            get { return loaders; }
            set { loaders = value; }
        }
        public StreamBlock[] Blocks {
            get { return blocks; }
            set { blocks = value; }
        }

        //Only used after the StreamMap has been updated.
        private ulong[] upGroupHeaders;
        private string rawPool;


        public class StreamGroup
        {
            string name;
            public int nameIDX;
            GroupTypes type;
            int unk01;
            public int startOffset; //start
            public int endOffset; //end
            public int unk5;

            public string Name {
                get { return name; }
                set { name = value; }
            }
            public GroupTypes Type {
                get { return type; }
                set { type = value; }
            }
            public int Unk01 {
                get { return unk01; }
                set { unk01 = value; }
            }
            public int Unk05 {
                get { return unk5; }
                set { unk5 = value; }
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

            [Description("Name of line, this will be used in LUA scripts.")]
            public string Name {
                get { return name; }
                set { name = value; }
            }
            [Browsable(false)]
            public string Group {
                get { return group; }
                set { group = value; }
            }
            [Description("0 - 6, unknown what they do.")]
            public int LoadType {
                get { return loadType; }
                set { loadType = value; }
            }
            [Description("Flags, referencing SDSConfig.bin")]
            public string Flags {
                get { return flags; }
                set { flags = value; }
            }
            [Description("Hash #1")]
            public ulong Unk10 {
                get { return unk10; }
                set { unk10 = value; }
            }
            [Description("Hash #2")]
            public ulong Unk11 {
                get { return unk11; }
                set { unk11 = value; }
            }
            [Description("The assets which will loaded when this line is activated.")]
            public StreamLoader[] LoadList {
                get { return loadList; }
                set { loadList = value; }
            }
            public int Unk5 {
                get { return unk5; }
                set { unk5 = value; }
            }
            [Browsable(false)]
            public int Unk12 {
                get { return unk12; }
                set { unk12 = value; }
            }
            [Browsable(false)]
            public int Unk13 {
                get { return unk13; }
                set { unk13 = value; }
            }
            [Browsable(false)]
            public int Unk14 {
                get { return unk14; }
                set { unk14 = value; }
            }
            [Browsable(false)]
            public int Unk15 {
                get { return unk15; }
                set { unk15 = value; }
            }

            public StreamLine()
            {
                name = "New Line";
                loadList = new StreamLoader[0];
            }

            public StreamLine(StreamLine other)
            {
                name = other.name + "_duplicated";
                group = other.group;
                loadType = other.loadType;
                flags = other.flags;
                unk10 = other.unk10;
                unk11 = other.unk11;
                unk5 = other.unk5;
                unk12 = other.unk12;
                unk13 = other.unk13;
                unk14 = other.unk14;
                unk15 = other.unk15;

                loadList = new StreamLoader[other.loadList.Length];
                for (int i = 0; i < other.loadList.Length; i++)
                {
                    loadList[i] = new StreamLoader(other.loadList[i]);
                }
            }


            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", nameIDX, lineID, groupID, loadType, flagIDX, unk5, unk10, unk11, unk12, unk13, unk14, unk15);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class StreamLoader
        {
            string path;
            string entity;
            public int start;
            public int end;
            private GroupTypes type;
            private int loaderSubID;
            private int loaderID;
            private int loadType;
            public int pathIDX;
            public int entityIDX;
            private string preferredGroup;
            private string assignedGroup;

            [Description("Loading type, 0 - 6.")]
            public int LoadType {
                get { return loadType; }
                set { loadType = value; }
            }
            [Description("Path to the asset.")]
            public string Path {
                get { return path; }
                set { path = value; }
            }
            [Description("Entity name to use in scripts or 'City-1'")]
            public string Entity {
                get { return entity; }
                set { entity = value; }
            }
            public int LoaderSubID {
                get { return loaderSubID; }
                set { loaderSubID = value; }
            }
            [Browsable(false)]
            public int LoaderID {
                get { return loaderID; }
                set { loaderID = value; }
            }
            [Browsable(false)]
            public int GroupID { get; set; }
            [ReadOnly(true), Description("The Assigned group for this line. This is determined when saving the Stream Map file.")]
            public string AssignedGroup {
                get { return assignedGroup; }
                set { assignedGroup = value; preferredGroup = ""; }
            }
            [Description("When this is set, the 'Type' is ignored and the editor will automatically assign this line to the preferred group. The list is available under the 'Stream Groups' Tab.")]
            public string PreferredGroup {
                set { preferredGroup = value; assignedGroup = ""; }
                get { return preferredGroup; }
            }
            [Description("The group this asset is under, every group can be seen under 'Stream Groups'")]
            public GroupTypes Type {
                get { return type; }
                set { type = value; preferredGroup = ""; }
            }


            public StreamLoader()
            {
                GroupID = -1;
            }
            public StreamLoader(StreamLoader other)
            {
                path = other.path;
                entity = other.entity;
                GroupID = other.GroupID;
                loadType = other.loadType;
                loaderSubID = other.loaderSubID;
                AssignedGroup = other.AssignedGroup;
                type = other.type;
            }

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3}", LoadType, path, entity, Type);
            }

            public override int GetHashCode()
            {
                var hashCode = 1843194125;
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(path);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(entity);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AssignedGroup);
                hashCode = hashCode * -1521134295 + loadType.GetHashCode();
                return hashCode;
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

            public StreamBlock()
            {
                startOffset = 0;
                endOffset = 0;
                hashes = new ulong[endOffset - startOffset];
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
            {
                return;
            }

            if (reader.ReadInt32() != 0x6)
            {
                return;
            }

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

            ToolkitAssert.Ensure(reader.BaseStream.Position == groupOffset, "Failed to reach the starting offset for group declaration!");

            groups = new StreamGroup[numGroups];
            for (int i = 0; i < numGroups; i++)
            {
                StreamGroup map = new StreamGroup();
                map.nameIDX = reader.ReadInt32();
                map.Name = ReadFromBuffer((long)((ulong)map.nameIDX + (ulong)poolOffset), reader.BaseStream.Position, reader);
                map.Type = (GroupTypes)reader.ReadInt32();
                map.Unk01 = reader.ReadInt32();
                map.startOffset = reader.ReadInt32();
                map.endOffset = reader.ReadInt32();
                map.unk5 = reader.ReadInt32();
                groups[i] = map;
            }

            ToolkitAssert.Ensure(reader.BaseStream.Position == headerOffset, "Did not reach the header starting offset");

            groupHeaders = new string[numHeaders];
            ulong[] ulongHeaders = new ulong[numHeaders];

            for (int i = 0; i < numHeaders; i++)
            {
                ulongHeaders[i] = reader.ReadUInt64();
                groupHeaders[i] = ReadFromBuffer((long)(ulongHeaders[i] + (ulong)poolOffset), reader.BaseStream.Position, reader);
            }

            ToolkitAssert.Ensure(reader.BaseStream.Position == lineOffset, "Did not reach the line data starting offset!");

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
                map.Group = groupHeaders[map.groupID];
                map.Name = ReadFromBuffer((long)((ulong)map.nameIDX + (ulong)poolOffset), reader.BaseStream.Position, reader);
                map.Flags = ReadBufferSpecial((long)((ulong)map.flagIDX + (ulong)poolOffset), reader.BaseStream.Position, reader).TrimEnd('\0').Replace('\0', '|');
                lines[i] = map;
            }

            ToolkitAssert.Ensure(reader.BaseStream.Position == loadersOffset, "Did not reach the loader data starting offset!");

            loaders = new StreamLoader[numLoaders];

            for (int i = 0; i < numLoaders; i++)
            {
                StreamLoader map = new StreamLoader();
                map.start = reader.ReadInt32();
                map.end = reader.ReadInt32();
                map.Type = (GroupTypes)reader.ReadInt32();
                
                map.LoaderSubID = reader.ReadInt32();
                map.LoaderID = reader.ReadInt32();
                map.LoadType = reader.ReadInt32();
                map.pathIDX = reader.ReadInt32();
                map.entityIDX = reader.ReadInt32();
                map.Path = ReadFromBuffer((long)((ulong)map.pathIDX + (ulong)poolOffset), reader.BaseStream.Position, reader);
                map.Entity = ReadBufferSpecial((long)((ulong)map.entityIDX + (ulong)poolOffset), reader.BaseStream.Position, reader).TrimEnd('\0').Replace('\0', '|');
                loaders[i] = map;
            }

            ToolkitAssert.Ensure(reader.BaseStream.Position == blockOffset, "Did not reach the block declaration starting offset!");

            blocks = new StreamBlock[numBlocks];
            for (int i = 0; i < numBlocks; i++)
            {
                StreamBlock map = new StreamBlock();
                map.startOffset = reader.ReadInt32();
                map.endOffset = reader.ReadInt32();
                map.Hashes = new ulong[map.endOffset - map.startOffset];
                blocks[i] = map;
            }

            ToolkitAssert.Ensure(reader.BaseStream.Position == hashOffset, "Did not reach the block hashes starting offset!");

            ulong[] hashes = new ulong[numHashes];

            for (int i = 0; i < numHashes; i++)
            {
                hashes[i] = reader.ReadUInt64();                                                                                              
            }

            for(int i = 0; i < numBlocks; i++)
            {
                var block = blocks[i];
                Array.Copy(hashes, block.startOffset, block.Hashes, 0, block.Hashes.Length);
            }

            ToolkitAssert.Ensure(reader.BaseStream.Position == poolOffset, "Did not reach the buffer pool starting offset!");

            reader.BaseStream.Seek(poolSize, SeekOrigin.Current);

            ToolkitAssert.Ensure(reader.BaseStream.Position == reader.BaseStream.Length, "Did not reach the end of the file!");
        }

        private void Update()
        {
            Dictionary<string, int> pool = new Dictionary<string, int>();
            int size = 0;
            rawPool = "";

            int loaderIDX = 0;
            foreach (var group in groups)
            {
                int idx = -1;
                if (!pool.TryGetValue(group.Name, out idx))
                {
                    idx = size;
                    pool.Add(group.Name, size);
                    size += group.Name.Length + 2;
                    rawPool += (group.Name + '\0' + '\0');
                }

                group.nameIDX = idx;
                for (int x = group.startOffset; x < group.startOffset + group.endOffset; x++)
                {
                    loaderIDX++;
                    var loader = loaders[x];
                    if (loader.LoaderSubID == 1)
                    {
                        loader.LoaderID = x != 0 ? loaders[x - 1].LoaderID : 1;
                    }
                    else
                    {
                        loader.LoaderID = loaderIDX;
                    }

                    idx = -1;
                    if (!pool.TryGetValue(loader.Path, out idx))
                    {
                        idx = size;
                        pool.Add(loader.Path, size);
                        size += loader.Path.Length + 2;
                        rawPool += (loader.Path + '\0' + '\0');
                    }
                    loader.pathIDX = idx;
                    if (!pool.TryGetValue(loader.Entity, out idx))
                    {
                        idx = size;
                        pool.Add(loader.Entity, size);
                        string[] splits = loader.Entity.Split('|');
                        string entity = loader.Entity.Replace('|', '\0');

                        rawPool += (entity + '\0' + '\0');
                        size += loader.Entity.Length + 2;
                        if (splits.Length > 5)
                        {
                            rawPool += ('\0');
                            size++;
                        }
                    }
                    loader.entityIDX = idx;

                }
            }

            List<string> newGH = new List<string>();
            List<ulong> hashGH = new List<ulong>();
            string groupName = "";
            foreach (var line in lines)
            {
                int idx = -1;

                if (groupName != line.Group)
                {
                    groupName = line.Group;
                    if (!pool.TryGetValue(line.Group, out idx))
                    {
                        pool.Add(line.Group, size);
                        line.groupID = newGH.Count;
                        newGH.Add(line.Group);
                        hashGH.Add((ulong)size);
                        size += line.Group.Length + 2;
                        rawPool += (line.Group + '\0' + '\0');
                    }
                    else
                    {
                        line.groupID = newGH.Count;
                        newGH.Add(line.Group);
                        hashGH.Add((ulong)idx);
                    }
                }
                else
                {
                    line.groupID = newGH.Count - 1;
                }
                if (!pool.TryGetValue(line.Name, out idx))
                {
                    idx = size;
                    pool.Add(line.Name, size);
                    size += line.Name.Length + 2;
                    rawPool += (line.Name + '\0' + '\0');
                }
                line.nameIDX = idx;
                if (!pool.TryGetValue(line.Flags, out idx))
                {
                    idx = size;
                    pool.Add(line.Flags, size);
                    size += line.Flags.Length + 3;
                    string flags = line.Flags.Replace('|', '\0');
                    rawPool += (flags + '\0' + '\0' + '\0');
                }
                line.flagIDX = idx;
            }
            groupHeaders = newGH.ToArray();
            upGroupHeaders = hashGH.ToArray();
        }

        public void WriteToFile()
        {
            Update();
            var oldString = file.FullName.Remove(file.FullName.Length - 4, 4);
            oldString += "_old.bin";
            File.Copy(file.FullName, oldString, true);
            using (BinaryWriter writer = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                InternalWriteToFile(writer);
            }
        }
        private void InternalWriteToFile(BinaryWriter writer)
        {
            int groupOffset = 0;
            int headerOffset = 0;
            int lineOffset = 0;
            int loadersOffset = 0;
            int blockOffset = 0;
            int hashOffset = 0;
            int poolOffset = 0;

            writer.Write(new byte[72]);

            groupOffset = 72;

            foreach (var group in groups)
            {
                writer.Write(group.nameIDX);
                writer.Write((int)group.Type);
                writer.Write(group.Unk01);
                writer.Write(group.startOffset);
                writer.Write(group.endOffset);
                writer.Write(group.unk5);
            }

            headerOffset = (int)writer.BaseStream.Position;

            foreach (var value in upGroupHeaders)
            {
                writer.Write(value);
            }

            lineOffset = (int)writer.BaseStream.Position;

            foreach (var line in lines)
            {
                writer.Write(line.nameIDX);
                writer.Write(line.lineID);
                writer.Write(line.groupID);
                writer.Write(line.LoadType);
                writer.Write(line.flagIDX);
                writer.Write(line.Unk5);
                writer.Write(line.Unk10);
                writer.Write(line.Unk11);
                writer.Write(line.Unk12);
                writer.Write(line.Unk13);
                writer.Write(line.Unk14);
                writer.Write(line.Unk15);
            }

            loadersOffset = (int)writer.BaseStream.Position;

            foreach (var loader in loaders)
            {
                writer.Write(loader.start);
                writer.Write(loader.end);
                writer.Write((int)loader.Type);
                writer.Write(loader.LoaderSubID);
                writer.Write(loader.LoaderID);
                writer.Write(loader.LoadType);
                writer.Write(loader.pathIDX);
                writer.Write(loader.entityIDX);
            }

            blockOffset = (int)writer.BaseStream.Position;

            List<ulong> hashesArray = new List<ulong>();
            foreach (var block in blocks)
            {
                // add and update offsets
                writer.Write(hashesArray.Count); // Start Offset
                hashesArray.AddRange(block.Hashes);
                writer.Write(hashesArray.Count); // End Offset
            }

            hashOffset = (int)writer.BaseStream.Position;

            foreach (var value in hashesArray)
            {
                writer.Write(value);
            }

            poolOffset = (int)writer.BaseStream.Position;

            writer.Write(rawPool.ToCharArray());

            writer.BaseStream.Seek(0, SeekOrigin.Begin);
            writer.Write(1299346515);
            writer.Write(0x6);
            writer.Write((uint)writer.BaseStream.Length);
            writer.Write(0);
            writer.Write(groups.Length);
            writer.Write(groupOffset);
            writer.Write(groupHeaders.Length);
            writer.Write(headerOffset);
            writer.Write(lines.Length);
            writer.Write(lineOffset);
            writer.Write(loaders.Length);
            writer.Write(loadersOffset);
            writer.Write(blocks.Length);
            writer.Write(blockOffset);
            writer.Write(hashesArray.Count);
            writer.Write(hashOffset);
            writer.Write(rawPool.Length);
            writer.Write(poolOffset);
        }
    }
}