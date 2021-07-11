using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utils.StringHelpers;

namespace ResourceTypes.Wwise
{
    public class PCK
    {
        private byte[] magic = { (byte)'A', (byte)'K', (byte)'P', (byte)'K' };
        private uint HeaderLength;
        private uint Unkn2 = 1;
        private uint LanguageLength;
        private uint BnkTableLength;
        private uint WemTableLength;
        private uint UnknStructLength;
        private uint Multiplier;
        public List<PCKString> PckStrings = new List<PCKString>();
        public List<Wem> WemList = new List<Wem>();
        string FileName;
        public PCK(string file)
        {
            FileName = file;
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            char[] MagicBytes = reader.ReadChars(4);
            HeaderLength = reader.ReadUInt32();
            Unkn2 = reader.ReadUInt32();
            LanguageLength = reader.ReadUInt32();
            BnkTableLength = reader.ReadUInt32();
            WemTableLength = reader.ReadUInt32();
            UnknStructLength = reader.ReadUInt32();
            long StringHeaderStart = reader.BaseStream.Position;
            uint StringCount = reader.ReadUInt32();

            for (int i = 0; i < StringCount; i++)
            {
                PCKString StringData = new PCKString(reader, StringHeaderStart);
                PckStrings.Add(StringData);
            }

            reader.BaseStream.Seek(StringHeaderStart + LanguageLength, SeekOrigin.Begin);

            for (int i = 0; i < BnkTableLength / 4; i++)
            {
                reader.ReadUInt32();
            }

            uint WemCount = reader.ReadUInt32();

            for (int i = 0; i < WemCount; i++)
            {
                uint Id = reader.ReadUInt32();
                Multiplier = reader.ReadUInt32();
                int Length = reader.ReadInt32();
                uint Offset = reader.ReadUInt32() * Multiplier;
                int LanguageEnum = reader.ReadInt32();
                int WorkingOffset = (int)reader.BaseStream.Position;
                reader.BaseStream.Seek(Offset, SeekOrigin.Begin);
                byte[] File = reader.ReadBytes(Length);
                reader.BaseStream.Seek(WorkingOffset, SeekOrigin.Begin);
                string Name = "Imported Wem " + i; //Would need connection to some global Wem Hash -> Name database
                Wem newWem = new Wem(Name, Id, File, PckStrings[LanguageEnum].value, Offset);
                WemList.Add(newWem);
            }
        }

        public void WriteToFile(BinaryWriter writer)
        {
            WemTableLength = (uint)(WemList.Count * 20) + 4;
            List<byte> LanguageBytes = GenerateLanguageBytes();
            HeaderLength = (uint)(WemTableLength + 20 + LanguageBytes.Count + BnkTableLength + UnknStructLength);
            writer.Write(magic);
            writer.Write(HeaderLength);
            writer.Write(Unkn2);
            writer.Write(LanguageBytes.Count);
            writer.Write(BnkTableLength); //Const
            writer.Write(WemTableLength);
            writer.Write(UnknStructLength); //Const

            for (int i = 0; i < LanguageBytes.Count; i++)
            {
                writer.Write(LanguageBytes[i]);
            }

            writer.Write((int)0);
            writer.Write(WemList.Count);

            foreach (Wem wem in WemList)
            {
                writer.Write(wem.Id);
                writer.Write(Multiplier);
                writer.Write(wem.File.Length);
                writer.Write(wem.Offset / Multiplier);
                int workingOffset = (int)writer.BaseStream.Position;
                writer.Seek((int)wem.Offset, SeekOrigin.Begin);
                writer.Write(wem.File);
                writer.Seek(workingOffset, SeekOrigin.Begin);
                uint LangEnum = 0;

                foreach (PCKString str in PckStrings)
                {
                    if (str.value == wem.Language)
                    {
                        LangEnum = str.index;
                    }
                }

                writer.Write(LangEnum);
            }
        }

        public void WriteWemToFile(BinaryWriter writer, int Index)
        {
            writer.Write(WemList[Index].File);
        }

        public List<string> GetLanguages()
        {
            List<string> langList = new List<string>();
            for (int i = 0; i < PckStrings.Count; i++)
            {
                try
                {
                    langList.Insert((int)PckStrings[i].index, PckStrings[i].value);
                }
                catch (ArgumentOutOfRangeException)
                {
                    langList.Add(PckStrings[i].value);
                }
            }
            return langList;
        }

        public List<byte> GenerateLanguageBytes()
        {
            List<byte> languageBytes = new List<byte>();

            byte[] count = BitConverter.GetBytes(PckStrings.Count);
            for (int i = 0; i < count.Length; i++)
            {
                languageBytes.Add(count[i]);
            }
            int headerSize = 4 + (PckStrings.Count * 8);
            List<byte> stringTable = new List<byte>();
            int[] offsets = new int[PckStrings.Count];
            List<string> strings = new List<string>();
            for (int i = 0; i < PckStrings.Count; i++)
            {
                strings.Add(PckStrings[i].value);
            }
            strings.Sort();
            for (int i = 0; i < strings.Count; i++)
            {
                byte[] asBytes;
                asBytes = System.Text.Encoding.Unicode.GetBytes(strings[i]);

                for (int j = 0; j < PckStrings.Count; j++)
                {
                    if (PckStrings[j].value == strings[i])
                    {
                        offsets[j] = Math.Max(0, headerSize + stringTable.Count);
                        for (int k = 0; k < asBytes.Length; k++)
                        {
                            stringTable.Add(asBytes[k]);
                        }
                        stringTable.Add(0);
                        stringTable.Add(0);
                    }
                }
            }
            for (int i = 0; i < PckStrings.Count; i++)
            {
                byte[] offset = BitConverter.GetBytes(offsets[i]);
                for (int j = 0; j < offset.Length; j++)
                {
                    languageBytes.Add(offset[j]);
                }
                byte[] index = BitConverter.GetBytes(PckStrings[i].index);
                for (int j = 0; j < index.Length; j++)
                {
                    languageBytes.Add(index[j]);
                }
            }
            for (int i = 0; i < stringTable.Count; i++)
            {
                languageBytes.Add(stringTable[i]);
            }
            while (languageBytes.Count % 4 != 0)
            {
                languageBytes.Add(0);
            }

            return languageBytes;
        }
    }

    public class PCKString
    {
        public string value;
        public uint index;

        public PCKString(BinaryReader br, long start)
        {
            uint offset = br.ReadUInt32();
            index = br.ReadUInt32();
            long retval = br.BaseStream.Position;
            br.BaseStream.Seek(start + offset, SeekOrigin.Begin);
            value = StringHelpers.ReadUniNullTerminatedString(br);
            br.BaseStream.Seek(retval, SeekOrigin.Begin);

        }
        public PCKString(uint Index, string Value)
        {
            index = Index;
            value = Value;
        }
    }
}
