using SharpDX;
using System.Collections.Generic;
using System.IO;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Sound
{
    struct unkStruct1
    {
        public byte unk0;
        public byte unk1;
        public byte numFloats;
        public Vector4[] floats;
        public short numShorts;
        public short[] shorts;
        public int unk2;
        public float unk3;
        public string name;
        public short unk4;
        public int unk5;
    }

    struct unkStruct2
    {
        public string name;
        public float[] floats;
        public string name2;
        public byte unk01;
        public string name3;
        public byte unk02;
        public float unk03;
        public string name4;
        public byte unk04;
        public byte unk05;
        public byte unk06;
    }

    public class SoundSectorLoader
    {
        ulong[] actorHashes;
        ushort[] unksData2;
        string fileName;
        int numActorhashes;
        int unk01;
        byte unk02;
        short count1;
        int parent;
        int unk03;
        string soundPrimary;
        short parentIdx;
        short sUnk04;
        short count2;
        unkStruct1[] data;
        byte sUnk06;
        int sUnk07;
        unkStruct2[] data1;

        public SoundSectorLoader(FileInfo info)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(info.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            fileName = new string(reader.ReadChars(reader.ReadByte()));
            numActorhashes = reader.ReadInt32();
            actorHashes = new ulong[numActorhashes];

            for (int i = 0; i != actorHashes.Length; i++)
            {
                actorHashes[i] = reader.ReadUInt64();
            }

            unk01 = reader.ReadInt32();
            unk02 = reader.ReadByte();

            count1 = reader.ReadInt16();
            unksData2 = new ushort[count1];

            for (int i = 0; i != count1; i++)
            {
                unksData2[i] = reader.ReadUInt16();
            }

            parent = reader.ReadInt32();
            unk03 = reader.ReadInt32();
            soundPrimary = new string(reader.ReadChars(reader.ReadByte()));
            parentIdx = reader.ReadInt16();
            sUnk04 = reader.ReadInt16();
            count2 = reader.ReadInt16();

            data = new unkStruct1[count2];
            for (int i = 0; i != count2; i++)
            {
                unkStruct1 subData = new unkStruct1();
                subData.unk0 = reader.ReadByte();
                subData.unk1 = reader.ReadByte();
                subData.numFloats = reader.ReadByte();

                subData.floats = new Vector4[subData.numFloats];

                for (int x = 0; x != subData.numFloats; x++)
                {
                    subData.floats[x] = Vector4Extenders.ReadFromFile(reader);
                }

                subData.numShorts = reader.ReadInt16();

                subData.shorts = new short[subData.numShorts];

                for (int x = 0; x != subData.numShorts; x++)
                {
                    subData.shorts[x] = reader.ReadInt16();
                }

                subData.unk2 = reader.ReadInt32();
                subData.unk3 = reader.ReadSingle();
                subData.name = new string(reader.ReadChars(reader.ReadByte()));
                subData.unk4 = reader.ReadInt16();
                subData.unk5 = reader.ReadInt32();
                data[i] = subData;
            }

            sUnk06 = reader.ReadByte();
            sUnk07 = reader.ReadInt32();
            data1 = new unkStruct2[sUnk07];
            for (int i = 0; i != sUnk07; i++)
            {
                byte sZero = reader.ReadByte();
                unkStruct2 subData = new unkStruct2();
                subData.name = new string(reader.ReadChars(reader.ReadByte()));
                subData.floats = new float[5];
                for (int x = 0; x != 5; x++)
                {
                    subData.floats[x] = reader.ReadSingle();
                }
                subData.name2 = new string(reader.ReadChars(reader.ReadByte()));
                subData.unk01 = reader.ReadByte();
                subData.name3 = new string(reader.ReadChars(reader.ReadByte()));
                subData.unk02 = reader.ReadByte();
                subData.unk03 = reader.ReadSingle();
                subData.name4 = new string(reader.ReadChars(reader.ReadByte()));
                subData.unk04 = reader.ReadByte();
                subData.unk05 = reader.ReadByte();

                if (subData.unk05 == 1)
                    subData.unk06 = reader.ReadByte();

                data1[i] = subData;
            }
             DumpToSound();
        }

        private void DumpToSound()
        {
            List<string> dump = new List<string>();
            dump.Add(string.Format("File Name: {0}", fileName));
            dump.Add("");
            dump.Add("Sound hashes (Links to sounds in actors)");
            dump.Add("========================================");
            dump.Add(string.Format("Count: {0}", numActorhashes));

            for (int i = 0; i != actorHashes.Length; i++)
                dump.Add(actorHashes[i].ToString());

            dump.Add(string.Format("Unk01: {0}", unk01));
            dump.Add(string.Format("Unk02: {0}", unk02));
            dump.Add("");
            dump.Add("Unknown Data Set:");
            dump.Add("========================================");
            dump.Add(string.Format("Count: {0}", count1));
            for (int i = 0; i != unksData2.Length; i++)
                dump.Add(unksData2[i].ToString());
            dump.Add("");
            dump.Add(string.Format("Parent: {0}", parent));
            dump.Add(string.Format("Unk03: {0}", unk03));
            dump.Add(string.Format("SoundPrimary: {0}", soundPrimary));
            dump.Add(string.Format("Parent Index: {0}", parentIdx));
            dump.Add(string.Format("Unk04: {0}", sUnk04));
            dump.Add("");
            dump.Add("Unknown Data Set:");
            dump.Add("========================================");
            dump.Add(string.Format("Count: {0}", count2));
            for (int i = 0; i != count2; i++)
            {
                dump.Add("Next Set: " + i.ToString());
                dump.Add("========================================");
                dump.Add(string.Format("Unk00: {0}", data[i].unk0));
                dump.Add(string.Format("Unk01: {0}", data[i].unk1));
                dump.Add("");
                dump.Add(string.Format("Vector4 Count: {0}", data[i].numFloats));
                for (int x = 0; x != data[i].numFloats; x++)
                    dump.Add(string.Format(data[i].floats[x].ToString()));
                dump.Add("");
                dump.Add(string.Format("Short Count: {0}", data[i].numShorts));
                for (int x = 0; x != data[i].numShorts; x++)
                    dump.Add(string.Format(data[i].shorts[x].ToString()));
                dump.Add("");
                dump.Add(string.Format("Unk02: {0}", data[i].unk2));
                dump.Add(string.Format("Unk03: {0}", data[i].unk3));
                dump.Add(string.Format("Name: {0}", data[i].name));
                dump.Add(string.Format("Unk04: {0}", data[i].unk4));
                dump.Add(string.Format("Unk05: {0}", data[i].unk5));
                dump.Add("========================================");
                dump.Add("");
                dump.Add("");
            }
            dump.Add("");
            dump.Add(string.Format("sUnk06: {0}", sUnk06));
            dump.Add("");
            dump.Add("Unknown Data Set:");
            dump.Add("========================================");
            dump.Add(string.Format("Count 3: {0}", sUnk07));
            for (int i = 0; i != sUnk07; i++)
            {
                dump.Add("Next Set: " + i.ToString());
                dump.Add("========================================");
                dump.Add(string.Format("Name: {0}", data1[i].name));
                dump.Add(string.Format("Floats (First three position): {0}", 5));
                for (int x = 0; x != 5; x++)
                    dump.Add(string.Format(data1[i].floats[x].ToString()));
                dump.Add(string.Format("Name2: {0}", data1[i].name2));
                dump.Add(string.Format("Unk01: {0}", data1[i].unk01));
                dump.Add(string.Format("Unk02: {0}", data1[i].name3));
                dump.Add(string.Format("Name3: {0}", data1[i].unk02));
                dump.Add(string.Format("Unk03: {0}", data1[i].unk03));
                dump.Add(string.Format("Name4: {0}", data1[i].name4));
                dump.Add(string.Format("Unk04: {0}", data1[i].unk04));
                dump.Add(string.Format("Unk05: {0}", data1[i].unk05));
                dump.Add(string.Format("Unk06: {0}", data1[i].unk06));
                dump.Add("========================================");
                dump.Add("");
                dump.Add("");
            }
            //File.WriteAllLines("SoundSectors/"+(fileName.Replace("/", "").Trim('\0')) + ".txt", dump.ToArray());
        }

        public void WriteToFile(BinaryWriter writer)
        {
            writer.Write((byte)fileName.Length);
            writer.Write(fileName.ToCharArray());
            writer.Write(numActorhashes);

            for (int i = 0; i != numActorhashes; i++)
                writer.Write(actorHashes[i]);

            writer.Write(unk01);
            writer.Write(unk02);
            writer.Write(count1);

            for (int i = 0; i != count1; i++)
                writer.Write(unksData2[i]);

            writer.Write(parent);
            writer.Write(unk03);
            writer.Write((byte)soundPrimary.Length);
            writer.Write(soundPrimary.ToCharArray());
            writer.Write(parentIdx);
            writer.Write(sUnk04);
            writer.Write(count2);

            for (int i = 0; i != count2; i++)
            {
                unkStruct1 subData = data[i];
                writer.Write(subData.unk0);
                writer.Write(subData.unk1);
                writer.Write(subData.numFloats);

                for (int x = 0; x != subData.numFloats; x++)
                {
                    Vector4Extenders.WriteToFile(subData.floats[x], writer);
                }

                writer.Write(subData.numShorts);

                for (int x = 0; x != subData.numShorts; x++)
                    writer.Write(subData.shorts[x]);

                writer.Write(subData.unk2);
                writer.Write(subData.unk3);
                writer.Write((byte)subData.name.Length);
                writer.Write(subData.name.ToCharArray());
                writer.Write(subData.unk4);
                writer.Write(subData.unk5);
            }

            writer.Write(sUnk06);
            writer.Write(sUnk07);

            for (int i = 0; i != sUnk07; i++)
            {
                writer.Write((byte)0);
                unkStruct2 subData = data1[i];
                writer.Write((byte)subData.name.Length);
                writer.Write(subData.name.ToCharArray());
                for (int x = 0; x != 5; x++)
                    writer.Write(subData.floats[x]);

                writer.Write((byte)subData.name2.Length);
                writer.Write(subData.name2.ToCharArray());
                writer.Write(subData.unk01);
                writer.Write((byte)subData.name3.Length);
                writer.Write(subData.name3.ToCharArray());
                writer.Write(subData.unk02);
                writer.Write(subData.unk03);
                writer.Write((byte)subData.name4.Length);
                writer.Write(subData.name4.ToCharArray());
                writer.Write(subData.unk04);
                writer.Write(subData.unk05);

                if (subData.unk05 == 1)
                    writer.Write(subData.unk06);
            }
        }
    }
}
