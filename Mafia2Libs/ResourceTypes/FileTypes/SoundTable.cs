using System;
using System.IO;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;

namespace ResourceTypes.SoundTable
{
    public class Entry0
    {
        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public float[] Unk3 { get; set; }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            Unk0 = reader.ReadByte8();
            Unk1 = reader.ReadByte8();

            Unk3 = new float[(Unk1 * 2)];
            for(int i = 0; i < Unk3.Length; i++)
            {
                Unk3[i] = reader.ReadSingle(isBigEndian);
            }
        }

        public void WriteToFile(MemoryStream OutStream, bool bIsBigEndian)
        {
            OutStream.WriteByte(Unk0);

            OutStream.WriteByte((byte)(Unk3.Length / 2));
            foreach(float Value in Unk3)
            {
                OutStream.Write(Value, bIsBigEndian);
            }
        }
    }

    public class Entry1
    {
        public byte Unk0 { get; set; }
        public byte Unk1 { get; set; }
        public float Unk2 { get; set; }
        public float Unk3 { get; set; }
        public float Unk4 { get; set; }
        public float Unk5 { get; set; }
        public byte Unk6 { get; set; }
        public float Unk7 { get; set; }
        public float Unk8 { get; set; }
        public float Unk9 { get; set; }
        public byte Unk10 { get; set; }

        public void ReadFromFile(MemoryStream Reader, bool bIsBigEndian, int Index)
        {
            Unk0 = Reader.ReadByte8();
            Unk1 = Reader.ReadByte8();
            Unk2 = Reader.ReadSingle(bIsBigEndian);
            Unk3 = Reader.ReadSingle(bIsBigEndian);
            Unk4 = Reader.ReadSingle(bIsBigEndian);
            Unk5 = Reader.ReadSingle(bIsBigEndian);

            Unk6 = Reader.ReadByte8();
            if (Unk1 == 13 || Unk1 == 5 || Unk1 == 11 || Unk1 == 9 || Unk1 == 15 || Unk1 == 27)
            {
                Unk7 = Reader.ReadSingle(bIsBigEndian);
                Unk8 = Reader.ReadSingle(bIsBigEndian);
                Unk9 = Reader.ReadSingle(bIsBigEndian);

                if(Unk1 == 27)
                {
                    Unk10 = Reader.ReadByte8();
                }
            }
            else if (Unk1 == 3 || Unk1 == 1)
            {

            }
            else
            {

            }
        }

        public void WriteToFile(MemoryStream OutStream, bool bIsBigEndian)
        {
            OutStream.WriteByte(Unk0);
            OutStream.WriteByte(Unk1);
            OutStream.Write(Unk2, bIsBigEndian);
            OutStream.Write(Unk3, bIsBigEndian);
            OutStream.Write(Unk4, bIsBigEndian);
            OutStream.Write(Unk5, bIsBigEndian);

            OutStream.WriteByte(Unk6);
            if (Unk1 == 13 || Unk1 == 5 || Unk1 == 11 || Unk1 == 9 || Unk1 == 15 || Unk1 == 27)
            {
                OutStream.Write(Unk7, bIsBigEndian);
                OutStream.Write(Unk8, bIsBigEndian);
                OutStream.Write(Unk9, bIsBigEndian);

                if (Unk1 == 27)
                {
                    OutStream.WriteByte(Unk10);
                }
            }
            else if (Unk1 == 3 || Unk1 == 1)
            {
                // Nothing right now
            }
            else
            {
                // Nothing right now
            }
        }
    }

    public class Entry2
    {
        public uint Unk0 { get; set; }
        public ushort Unk1 { get; set; }
    }

    public class Entry3
    {
        public uint ID { get; set; }
        public float Unk0 { get; set; }
        public uint Count { get; set; }
        public Entry3_1[] FSBGroup { get; set; }
        public void ReadFromFile(MemoryStream Stream, bool bIsBigEndian)
        {
            ID = Stream.ReadUInt32(bIsBigEndian);
            Unk0 = Stream.ReadSingle(bIsBigEndian);

            Count = Stream.ReadUInt32(bIsBigEndian);
            FSBGroup = new Entry3_1[Count];
            for (int a = 0; a < Count; a++)
            {
                Entry3_1 FSB = new Entry3_1();
                FSB.ReadFromFile(Stream, bIsBigEndian);
                FSBGroup[a] = FSB;
            }
        }
    }

    public class Entry3_1
    {
        public uint ID { get; set; } // (ID starts at 1)
        public ushort Unk2 { get; set; } // The Entry1 to use
        public Entry4[] FSBs { get; set; }

        public void ReadFromFile(MemoryStream Stream, bool bIsBigEndian)
        {
            ID = Stream.ReadUInt32(bIsBigEndian);
            Unk2 = Stream.ReadUInt16(bIsBigEndian);

            byte FSBCount = Stream.ReadByte8();
            FSBs = new Entry4[FSBCount];
            for (int i = 0; i < FSBCount; i++)
            {
                Entry4 FSBEntry = new Entry4();
                FSBEntry.ReadFromFile(Stream, bIsBigEndian);
                FSBs[i] = FSBEntry;
            }
        }
    }

    public class Entry4
    {
        public string FSBName { get; set; }
        public byte Unk0 { get; set; }  // 255?
        public float Unk1 { get; set; }

        public void ReadFromFile(MemoryStream Stream, bool bIsBigEndian)
        {
            FSBName = Stream.ReadString8(bIsBigEndian);
            Unk0 = Stream.ReadByte8();
            Unk1 = Stream.ReadSingle(bIsBigEndian);
        }

        public void WriteToFile(MemoryStream OutStream, bool bIsBigEndian)
        {
            OutStream.WriteString8(FSBName, bIsBigEndian);
            OutStream.WriteByte(Unk0);
            OutStream.Write(Unk1, bIsBigEndian);
        }
    }

    public class Entry5
    {
        public string Name { get; set; }
        public string[] FSBList { get; set; }

        public void ReadFromFile(MemoryStream Stream, bool bIsBigEndian)
        {
            Name = Stream.ReadString8(bIsBigEndian);

            uint FSBCount = Stream.ReadUInt32(bIsBigEndian);
            FSBList = new string[FSBCount];
            for (int i = 0; i < FSBCount; i++)
            {
                FSBList[i] = Stream.ReadString8(bIsBigEndian);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - Count: {1}", Name, FSBList.Length);
        }
    }

    public class FSBGroup
    {
        public uint ID { get; set; }
        public string Name { get; set; }
        public float Unk0 { get; set; }
        public FSBFile[] Variants { get; set; }
    }

    public class FSBFile
    {
        public string Name { get; set; }
        public Entry4[] Files { get; set; }
        public uint ID { get; set; } // (ID starts at 1)
        public ushort Unk2 { get; set; } // The Entry1 to use
    }

    public class SoundTable
    {
        public Entry0[] Entry0s { get; set; }
        public Entry1[] Entry1s { get; set; }
        public Entry2[] Entry2s { get; set; }
        public FSBGroup[] FSBGroups { get; set; }

        public void ReadFromFile(MemoryStream reader, bool isBigEndian)
        {
            // Load Curve Data?
            uint NumEntries = reader.ReadUInt32(isBigEndian);
            Entry0s = new Entry0[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                Entry0 NewEntry = new Entry0();
                NewEntry.ReadFromFile(reader, isBigEndian);
                Entry0s[i] = NewEntry;
            }

            // I don't know what this is.
            // It's linked to group and variant data.
            NumEntries = reader.ReadUInt32(isBigEndian);
            Entry1s = new Entry1[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                Entry1 NewEntry = new Entry1();
                NewEntry.ReadFromFile(reader, isBigEndian, i);
                Entry1s[i] = NewEntry;
            }

            // I don't know what this is.
            NumEntries = reader.ReadUInt32(isBigEndian);
            Entry2s = new Entry2[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                Entry2 NewEntry = new Entry2();
                NewEntry.Unk0 = reader.ReadUInt32(isBigEndian);
                NewEntry.Unk1 = reader.ReadUInt16(isBigEndian);
                Entry2s[i] = NewEntry;
            }

            // Includes group and variant data
            NumEntries = reader.ReadUInt32(isBigEndian);
            Entry3[] Entry3s = new Entry3[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                Entry3 NewEntry = new Entry3();
                NewEntry.ReadFromFile(reader, isBigEndian);
                Entry3s[i] = NewEntry;
            }

            // Load the size of the string buffer
            uint StringBufferSize = reader.ReadUInt32(isBigEndian);

            // Load strings for group & variants
            NumEntries = reader.ReadUInt32(isBigEndian);
            Entry5[] Entry5s = new Entry5[NumEntries];
            for (int i = 0; i < NumEntries; i++)
            {
                Entry5 NewEntry = new Entry5();
                NewEntry.ReadFromFile(reader, isBigEndian);
                Entry5s[i] = NewEntry;
            }

            // Build groups
            FSBGroups = new FSBGroup[Entry5s.Length];
            for(int i = 0; i < FSBGroups.Length; i++)
            {
                FSBGroup NewGroup = new FSBGroup();
                NewGroup.ID = Entry3s[i].ID;
                NewGroup.Unk0 = Entry3s[i].Unk0;
                NewGroup.Name = Entry5s[i].Name;
                NewGroup.Variants = new FSBFile[Entry5s[i].FSBList.Length];

                // TODO: These files 
                for(int z = 0; z < NewGroup.Variants.Length; z++)
                {
                    FSBFile NewFile = new FSBFile();
                    NewFile.Name = Entry5s[i].FSBList[z];
                    NewFile.ID = Entry3s[i].FSBGroup[z].ID;
                    NewFile.Unk2 = Entry3s[i].FSBGroup[z].Unk2;
                    NewFile.Files = Entry3s[i].FSBGroup[z].FSBs;

                    NewGroup.Variants[z] = NewFile;
                }

                FSBGroups[i] = NewGroup;
            }

            // Write to file
            using(MemoryStream OutStream = new MemoryStream())
            {
                WriteToFile(OutStream, false);
                File.WriteAllBytes("SoundTable_OutTest.stbl", OutStream.ToArray());
            }
        }

        public void WriteToFile(MemoryStream OutStream, bool bIsBigEndian)
        {
            // Curves?
            OutStream.Write(Entry0s.Length, bIsBigEndian); // This array includes two floats
            foreach(Entry0 Entry in Entry0s)
            {
                Entry.WriteToFile(OutStream, bIsBigEndian);
            }

            // Linked via variant data
            OutStream.Write(Entry1s.Length, bIsBigEndian);
            foreach (Entry1 Entry in Entry1s)
            {
                Entry.WriteToFile(OutStream, bIsBigEndian);
            }

            // I don't know what this is
            OutStream.Write(Entry2s.Length, bIsBigEndian);
            foreach (Entry2 Entry in Entry2s)
            {
                OutStream.Write(Entry.Unk0, bIsBigEndian);
                OutStream.Write(Entry.Unk1, bIsBigEndian);
            }

            // Write group data and variant data
            OutStream.Write(FSBGroups.Length, bIsBigEndian);
            foreach(FSBGroup Group in FSBGroups)
            {
                OutStream.Write(Group.ID, bIsBigEndian);
                OutStream.Write(Group.Unk0, bIsBigEndian);

                OutStream.Write(Group.Variants.Length, bIsBigEndian);
                foreach(FSBFile File in Group.Variants)
                {
                    OutStream.Write(File.ID, bIsBigEndian);
                    OutStream.Write(File.Unk2, bIsBigEndian);

                    OutStream.WriteByte((byte)File.Files.Length);
                    foreach(Entry4 Entry in File.Files)
                    {
                        Entry.WriteToFile(OutStream, bIsBigEndian);
                    }
                }
            }

            // Temporarily set size buffer
            long SizeOffset = OutStream.Position;
            OutStream.Write(0, bIsBigEndian); // Temp, replaced later.

            // Write strings, group names and variant names
            OutStream.Write(FSBGroups.Length, bIsBigEndian);
            foreach (FSBGroup Group in FSBGroups)
            {
                OutStream.WriteString8(Group.Name, bIsBigEndian);

                OutStream.Write(Group.Variants.Length, bIsBigEndian);
                foreach(FSBFile File in Group.Variants)
                {
                    OutStream.WriteString8(File.Name, bIsBigEndian);
                }
            }

            // Write size buffer
            OutStream.Position = SizeOffset;
            uint Size = (uint)(OutStream.Length - OutStream.Position);
            OutStream.Write(Size - 4, bIsBigEndian); // NB: Exlude this UInt32
        }
    }
}
