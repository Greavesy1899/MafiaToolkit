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
    public class PCK
    {
        public byte[] magic = { (byte)'A', (byte)'K', (byte)'P', (byte)'K' };
        public byte[] metaMagic = { (byte)'M', (byte)'E', (byte)'T', (byte)'A' };
        public uint headerLength;
        public uint unkn2 = 1;
        public uint languageLength;
        public uint bnkTableLength = 4;
        public uint wemATableLength;
        public uint wemBTableLength;
        public uint multiplier;
        public List<PCKString> pckStrings = new List<PCKString>();
        public List<Wem> WemList = new List<Wem>();
        public List<int> WemAList = new List<int>();
        public List<int> WemBList = new List<int>();
        public byte[] bnkTable;
        public int MetaData = 0;
        public PCK()
        {
            headerLength = 0;
            unkn2 = 1;
            languageLength = 0;
            bnkTableLength = 0;
            wemATableLength = 0;
            wemBTableLength = 0;
            multiplier = 16;
            pckStrings.Add(new PCKString(0, "sfx"));
        }
        public PCK(string fileName)
        {
            using (BinaryReader br = new BinaryReader(new FileStream(fileName, FileMode.Open), Encoding.ASCII))
            {
                char[] magicBytes = br.ReadChars(4);
                uint headerLen = br.ReadUInt32();
                headerLength = headerLen;
                unkn2 = br.ReadUInt32();
                languageLength = br.ReadUInt32();
                bnkTableLength = br.ReadUInt32();
                wemATableLength = br.ReadUInt32();
                wemBTableLength = br.ReadUInt32();
                long stringHeaderStart = br.BaseStream.Position;
                uint stringCount = br.ReadUInt32();

                for (int i = 0; i < stringCount; i++)
                {
                    PCKString stringData = new PCKString(br, stringHeaderStart);
                    pckStrings.Add(stringData);
                }

                br.BaseStream.Seek(stringHeaderStart + languageLength, SeekOrigin.Begin);

                uint bnksCount = br.ReadUInt32();

                uint wemACount = br.ReadUInt32();

                for (int i = 0; i < wemACount; i++)
                {
                    ulong id = br.ReadUInt32();
                    multiplier = br.ReadUInt32();
                    uint length = br.ReadUInt32();
                    uint offset = br.ReadUInt32() * multiplier;
                    uint languageEnum = br.ReadUInt32();
                    int workingOffset = (int)br.BaseStream.Position;
                    br.BaseStream.Seek(offset, SeekOrigin.Begin);
                    byte[] file = br.ReadBytes((int)length);
                    br.BaseStream.Seek(workingOffset, SeekOrigin.Begin);
                    string name = "Imported_Wem_" + i;
                    Wem newWem = new Wem(name, id, file, languageEnum, offset);
                    WemList.Add(newWem);
                }

                uint wemBCount = br.ReadUInt32();

                for (int i = 0; i < wemBCount; i++)
                {
                    ulong id = br.ReadUInt64();
                    multiplier = br.ReadUInt32();
                    int length = br.ReadInt32();
                    uint offset = br.ReadUInt32() * multiplier;
                    uint languageEnum = br.ReadUInt32();

                    int workingOffset = (int)br.BaseStream.Position;

                    br.BaseStream.Seek(offset, SeekOrigin.Begin);

                    byte[] file = br.ReadBytes(length);

                    br.BaseStream.Seek(workingOffset, SeekOrigin.Begin);

                    string name = "Imported_Wem_" + i;

                    Wem newWem = new Wem(name, id, file, languageEnum, offset);
                    WemList.Add(newWem);
                }
            }
        }

        public List<string> GetLanguages()
        {
            List<string> langList = new List<string>();
            for (int i = 0; i < pckStrings.Count; i++)
            {
                try
                {
                    langList.Insert((int)pckStrings[i].index, pckStrings[i].value);
                }
                catch (ArgumentOutOfRangeException)
                {
                    langList.Add(pckStrings[i].value);
                }
            }
            return langList;
        }

        public List<byte> GenerateLanguageBytes()
        {
            List<byte> languageBytes = new List<byte>();

            byte[] count = BitConverter.GetBytes(pckStrings.Count);
            for (int i = 0; i < count.Length; i++)
            {
                languageBytes.Add(count[i]);
            }
            int headerSize = 4 + (pckStrings.Count * 8);
            List<byte> stringTable = new List<byte>();
            int[] offsets = new int[pckStrings.Count];
            List<string> strings = new List<string>();
            for (int i = 0; i < pckStrings.Count; i++)
            {
                strings.Add(pckStrings[i].value);
            }
            strings.Sort();
            for (int i = 0; i < strings.Count; i++)
            {
                byte[] asBytes;
                asBytes = Encoding.Unicode.GetBytes(strings[i]);

                for (int j = 0; j < pckStrings.Count; j++)
                {
                    if (pckStrings[j].value == strings[i])
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
            for (int i = 0; i < pckStrings.Count; i++)
            {
                byte[] offset = BitConverter.GetBytes(offsets[i]);
                for (int j = 0; j < offset.Length; j++)
                {
                    languageBytes.Add(offset[j]);
                }
                byte[] index = BitConverter.GetBytes(pckStrings[i].index);
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

        public void WriteToFile(BinaryWriter bw)
        {
            if (multiplier == 0)
            {
                multiplier = 16;
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
                    wem.Offset = (uint)Math.Ceiling((decimal)MathHelpers.RoundUp16((int)(40 + WemAList.Count * 20 + WemBList.Count * 24 + languageLength), false) / multiplier);
                }
                else
                {
                    Wem lastWem = WemList[i - 1];
                    wem.Offset = (uint)Math.Ceiling((decimal)MathHelpers.RoundUp16((int)((lastWem.Offset * multiplier) + lastWem.file.Length), false) / multiplier);
                }
            }

            List<byte> languageBytes = GenerateLanguageBytes();
            wemATableLength = (uint)(WemAList.Count * 20) + 4;
            wemBTableLength = (uint)(WemBList.Count * 24) + 4;
            bnkTableLength = 4; //Const in Mafia games
            headerLength = (uint)(wemATableLength + 20 + languageBytes.Count + bnkTableLength + wemBTableLength);

            bw.Write(magic);
            bw.Write(headerLength);
            bw.Write(unkn2);
            bw.Write(languageBytes.Count);
            bw.Write(bnkTableLength);
            bw.Write(wemATableLength);
            bw.Write(wemBTableLength);

            for (int i = 0; i < languageBytes.Count; i++)
            {
                bw.Write(languageBytes[i]);
            }

            bw.Write(0);

            bw.Write(WemAList.Count);

            foreach (int id in WemAList)
            {
                Wem wem = WemList[id];

                bw.Write((uint)wem.ID);
                bw.Write(multiplier);
                bw.Write(wem.file.Length);
                bw.Write(wem.Offset);
                int workingOffset = (int)bw.BaseStream.Position;
                bw.Seek((int)(wem.Offset * multiplier), SeekOrigin.Begin);
                bw.Write(wem.file);
                bw.Seek(workingOffset, SeekOrigin.Begin);
                bw.Write(wem.LanguageEnum);
            }

            bw.Write(WemBList.Count);

            foreach (int id in WemBList)
            {
                Wem wem = WemList[id];

                bw.Write(wem.ID);
                bw.Write(multiplier);
                bw.Write(wem.file.Length);
                bw.Write(wem.Offset);
                int workingOffset = (int)bw.BaseStream.Position;
                bw.Seek((int)(wem.Offset * multiplier), SeekOrigin.Begin);
                bw.Write(wem.file);
                bw.Seek(workingOffset, SeekOrigin.Begin);
                bw.Write(wem.LanguageEnum);
            }

            bw.Close();
        }
    }
}
