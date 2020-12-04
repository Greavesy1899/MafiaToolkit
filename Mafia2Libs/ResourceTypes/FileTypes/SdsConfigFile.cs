using System;
using System.Collections.Generic;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.SDSConfig
{
    public class SdsConfigFile
    {
        public struct Unk1Struct
        {
            public Unk2Struct[] unk2;
            public Unk3Struct[] unk3;
        }
        public struct Unk2Struct
        {
            public int unk3;
            public short unk4;
            public short unk5;
            public short unk6;
            public int unk7;
            public int unk8;
        }
        public struct Unk3Struct
        {

        }
        public const uint Signature = 0x73647370; // 'sdsp'
        Dictionary<int, string> names = new Dictionary<int, string>();
        Unk1Struct[] data;

        public void ReadFromFile(FileInfo file)
        {
            using(BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                ReadFromFile(reader);
            }
        }

        public void ReadFromFile(BinaryReader reader)
        {
            int magic = reader.ReadInt32();
            int version = reader.ReadInt32();

            if (version != 2 && magic != 0x73647370)
                throw new FormatException("unsupported SDS config version");

            uint stringTableSize = reader.ReadUInt32();
            string str = "";
            for (int i = 0; i < stringTableSize; i++)
            {
                int b = (byte)~reader.ReadByte();
                str += Convert.ToChar(b);
            }

            ushort unk0 = reader.ReadUInt16();
            data = new Unk1Struct[unk0];
            for (ushort i = 0; i < unk0; i++)
            {
                Unk1Struct data1 = new Unk1Struct();

                short unk1 = reader.ReadInt16();
                //var name1 = stringTable.ToStringUTF8Z(unk1);

                ushort unk2 = reader.ReadUInt16();
                data1.unk2 = new Unk2Struct[unk2];
                for (ushort j = 0; j < unk2; j++)
                {
                    Unk2Struct data2 = new Unk2Struct();
                    data2.unk3 = reader.ReadInt16();
                    //var name2 = stringTable.ToStringUTF8Z(unk3);
                    data2.unk4 = reader.ReadInt16();
                    data2.unk5 = reader.ReadInt16();
                    data2.unk6 = reader.ReadInt16();
                    data2.unk7 = reader.ReadInt32();
                    data2.unk8 = reader.ReadInt32();
                    data1.unk2[j] = data2;
                }

                ushort unk9 = reader.ReadUInt16();
                data1.unk3 = new Unk3Struct[unk9];
                for (ushort k = 0; k < unk9; k++)
                {
                    short unk10 = reader.ReadInt16();
                    //var name3 = stringTable.ToStringUTF8Z(unk10);
                    int unk11 = reader.ReadInt32();
                    int unk12 = reader.ReadInt32();

                    ushort unk13 = reader.ReadUInt16();
                    for (ushort l = 0; l < unk13; l++)
                    {
                        short unk14 = reader.ReadInt16();
                        //var name4 = stringTable.ToStringUTF8Z(unk14);

                        int unk15 = reader.ReadInt32();
                        int unk16 = reader.ReadInt32();
                        byte unk17 = reader.ReadByte();

                        uint unk18 = reader.ReadUInt16();
                        for (ushort m = 0; m < unk18; m++)
                        {
                            short unk19 = reader.ReadInt16();
                            short unk20 = reader.ReadInt16();
                            short unk21 = reader.ReadInt16();
                            short unk22 = reader.ReadInt16();
                            ushort unk23 = reader.ReadUInt16();
                            short unk24 = reader.ReadInt16();
                            short unk25 = reader.ReadInt16();
                            short unk26 = reader.ReadInt16();
                        }
                    }
                }
                data[i] = data1;
            }
        }
    }
}