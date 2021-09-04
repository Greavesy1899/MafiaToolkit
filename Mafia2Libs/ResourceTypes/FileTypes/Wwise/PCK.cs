using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Utils.StringHelpers;
using Utils.MathHelpers;
using System.ComponentModel;

namespace ResourceTypes.Wwise
{
    public class PCKString
    {
        public string Value;
        public uint Index;

        public PCKString(BinaryReader br, long start)
        {
            uint Offset = br.ReadUInt32();
            Index = br.ReadUInt32();
            long retval = br.BaseStream.Position;
            br.BaseStream.Seek(start + Offset, SeekOrigin.Begin);
            Value = StringHelpers.ReadUniNullTerminatedString(br);
            br.BaseStream.Seek(retval, SeekOrigin.Begin);

        }
        public PCKString(uint index, string value)
        {
            Index = index;
            Value = value;
        }
    }
    public class PCK
    {
        public byte[] Magic = { (byte)'A', (byte)'K', (byte)'P', (byte)'K' };
        public uint HeaderLength;
        public uint Version = 1;
        public uint LanguageLength;
        public uint BnkTableLength = 4;
        public uint WemATableLength;
        public uint WemBTableLength;
        public uint Multiplier;
        public List<PCKString> PckStrings = new List<PCKString>();
        public List<Wem> WemList = new List<Wem>();
        public List<int> WemAList = new List<int>();
        public List<int> WemBList = new List<int>();
        public byte[] BnkTable;
        public BNK LoadedBNK;
        public PCK()
        {
            HeaderLength = 0;
            Version = 1;
            LanguageLength = 0;
            BnkTableLength = 0;
            WemATableLength = 0;
            WemBTableLength = 0;
            Multiplier = 16;
            PckStrings.Add(new PCKString(0, "sfx"));
        }
        public PCK(string fileName)
        {
            using (BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open), Encoding.ASCII))
            {
                char[] MagicBytes = br.ReadChars(4);
                uint headerLen = br.ReadUInt32();
                HeaderLength = headerLen;
                Version = br.ReadUInt32();
                LanguageLength = br.ReadUInt32();
                BnkTableLength = br.ReadUInt32();
                WemATableLength = br.ReadUInt32();
                WemBTableLength = br.ReadUInt32();
                long stringHeaderStart = br.BaseStream.Position;
                uint stringCount = br.ReadUInt32();

                for (int i = 0; i < stringCount; i++)
                {
                    PCKString stringData = new PCKString(br, stringHeaderStart);
                    PckStrings.Add(stringData);
                }

                br.BaseStream.Seek(stringHeaderStart + LanguageLength, SeekOrigin.Begin);

                uint BnksCount = br.ReadUInt32();

                uint wemACount = br.ReadUInt32();

                for (int i = 0; i < wemACount; i++)
                {
                    ulong ID = br.ReadUInt32();
                    Multiplier = br.ReadUInt32();
                    uint Length = br.ReadUInt32();
                    uint Offset = br.ReadUInt32() * Multiplier;
                    uint languageEnum = br.ReadUInt32();
                    int workingOffset = (int)br.BaseStream.Position;
                    br.BaseStream.Seek(Offset, SeekOrigin.Begin);
                    byte[] file = br.ReadBytes((int)Length);
                    br.BaseStream.Seek(workingOffset, SeekOrigin.Begin);
                    string name = "Imported_Wem_" + i;
                    Wem newWem = new Wem(name, ID, file, languageEnum, Offset);
                    WemList.Add(newWem);
                }

                uint wemBCount = br.ReadUInt32();

                for (int i = 0; i < wemBCount; i++)
                {
                    ulong ID = br.ReadUInt64();
                    Multiplier = br.ReadUInt32();
                    int Length = br.ReadInt32();
                    uint Offset = br.ReadUInt32() * Multiplier;
                    uint languageEnum = br.ReadUInt32();

                    int workingOffset = (int)br.BaseStream.Position;

                    br.BaseStream.Seek(Offset, SeekOrigin.Begin);

                    byte[] file = br.ReadBytes(Length);

                    br.BaseStream.Seek(workingOffset, SeekOrigin.Begin);

                    string name = "Imported_Wem_" + i;

                    Wem newWem = new Wem(name, ID, file, languageEnum, Offset);
                    WemList.Add(newWem);
                }
            }
        }

        public List<string> GetLanguages()
        {
            List<string> langList = new List<string>();
            for (int i = 0; i < PckStrings.Count; i++)
            {
                try
                {
                    langList.Insert((int)PckStrings[i].Index, PckStrings[i].Value);
                }
                catch (ArgumentOutOfRangeException)
                {
                    langList.Add(PckStrings[i].Value);
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
            int[] Offsets = new int[PckStrings.Count];
            List<string> strings = new List<string>();
            for (int i = 0; i < PckStrings.Count; i++)
            {
                strings.Add(PckStrings[i].Value);
            }
            strings.Sort();
            for (int i = 0; i < strings.Count; i++)
            {
                byte[] asBytes;
                asBytes = Encoding.Unicode.GetBytes(strings[i]);

                for (int j = 0; j < PckStrings.Count; j++)
                {
                    if (PckStrings[j].Value == strings[i])
                    {
                        Offsets[j] = Math.Max(0, headerSize + stringTable.Count);
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
                byte[] Offset = BitConverter.GetBytes(Offsets[i]);
                for (int j = 0; j < Offset.Length; j++)
                {
                    languageBytes.Add(Offset[j]);
                }
                byte[] index = BitConverter.GetBytes(PckStrings[i].Index);
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

        public void WriteToFile(BinaryWriter bw, string BnkPath)
        {
            if (LoadedBNK != null)
            {
                File.Copy(BnkPath, BnkPath + "_old", true);
                using (BinaryWriter bnkWrite = new BinaryWriter(File.Open(BnkPath, FileMode.Create)))
                {
                    LoadedBNK.WriteToFile(bnkWrite);
                }
            }

            if (Multiplier == 0)
            {
                Multiplier = 16;
            }

            WemList = new List<Wem>(WemList.OrderBy(i => i.ID));

            WemAList = new List<int>();
            WemBList = new List<int>();


            foreach (Wem wem in WemList)
            {
                if (wem.ID == (uint)wem.ID)
                {
                    WemAList.Add(WemList.IndexOf(wem));
                }
                else
                {
                    WemBList.Add(WemList.IndexOf(wem));
                }
            }

            for (int i = 0; i < WemList.Count; i++)
            {
                Wem wem = WemList[i];

                if (i == 0)
                {
                    wem.Offset = (uint)Math.Ceiling((decimal)MathHelpers.RoundUp16((int)(40 + WemAList.Count * 20 + WemBList.Count * 24 + LanguageLength), false) / Multiplier);
                }
                else
                {
                    Wem lastWem = WemList[i - 1];
                    wem.Offset = (uint)Math.Ceiling((decimal)MathHelpers.RoundUp16((int)((lastWem.Offset * Multiplier) + lastWem.File.Length), false) / Multiplier);
                }
            }

            List<byte> languageBytes = GenerateLanguageBytes();
            WemATableLength = (uint)(WemAList.Count * 20) + 4;
            WemBTableLength = (uint)(WemBList.Count * 24) + 4;
            BnkTableLength = 4; //Const in Mafia games
            HeaderLength = (uint)(WemATableLength + 20 + languageBytes.Count + BnkTableLength + WemBTableLength);

            bw.Write(Magic);
            bw.Write(HeaderLength);
            bw.Write(Version);
            bw.Write(languageBytes.Count);
            bw.Write(BnkTableLength);
            bw.Write(WemATableLength);
            bw.Write(WemBTableLength);

            for (int i = 0; i < languageBytes.Count; i++)
            {
                bw.Write(languageBytes[i]);
            }

            bw.Write(0);

            bw.Write(WemAList.Count);

            foreach (int ID in WemAList)
            {
                Wem wem = WemList[ID];

                bw.Write((uint)wem.ID);
                bw.Write(Multiplier);
                bw.Write(wem.File.Length);
                bw.Write(wem.Offset);
                int workingOffset = (int)bw.BaseStream.Position;
                bw.Seek((int)(wem.Offset * Multiplier), SeekOrigin.Begin);
                bw.Write(wem.File);
                bw.Seek(workingOffset, SeekOrigin.Begin);
                bw.Write(wem.LanguageEnum);
            }

            bw.Write(WemBList.Count);

            foreach (int ID in WemBList)
            {
                Wem wem = WemList[ID];

                bw.Write(wem.ID);
                bw.Write(Multiplier);
                bw.Write(wem.File.Length);
                bw.Write(wem.Offset);
                int workingOffset = (int)bw.BaseStream.Position;
                bw.Seek((int)(wem.Offset * Multiplier), SeekOrigin.Begin);
                bw.Write(wem.File);
                bw.Seek(workingOffset, SeekOrigin.Begin);
                bw.Write(wem.LanguageEnum);
            }

            bw.Close();
        }
    }
}
