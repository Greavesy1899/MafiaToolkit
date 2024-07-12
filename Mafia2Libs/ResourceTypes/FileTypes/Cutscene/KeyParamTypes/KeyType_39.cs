using System;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.StringHelpers;
using Utils.Types;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_39 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class DataWrapper
        {
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class DefaultData
            {
                public DefaultData()
                {

                }

                public DefaultData(BinaryReader br)
                {
                    Read(br);
                }

                public virtual void Read(BinaryReader br)
                {

                }

                public virtual void Write(BinaryWriter bw)
                {

                }
            }
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class Type1Data : DefaultData
            {
                public string Unk00 { get; set; } // Frame Name?
                public int Unk01 { get; set; }
                public int Unk02 { get; set; } // Possible ending KeyFrame?
                public int Unk03 { get; set; }
                public HashName NameHash { get; set; } // Another Name?
                public int Unk04 { get; set; } // 4?
                public float[] Unk05 { get; set; } = new float[0];
                public Type1Data()
                {

                }

                public Type1Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadString16();
                    Unk01 = br.ReadInt32();
                    Unk02 = br.ReadInt32();
                    Unk03 = br.ReadInt32();
                    NameHash = new HashName();
                    NameHash.ReadFromFile(br);
                    Unk04 = br.ReadInt32();

                    if (Unk04 == 6)
                    {
                        Unk05 = new float[7];

                        for (int i = 0; i < Unk05.Length; i++)
                        {
                            Unk05[i] = br.ReadSingle();
                        }
                    }
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.WriteString16(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);
                    bw.Write(Unk03);
                    NameHash.WriteToFile(bw);
                    bw.Write(Unk04);

                    if (Unk04 == 6)
                    {
                        if (Unk05.Length < 7)
                        {
                            float[] floats = new float[7];
                            Array.Copy(Unk05, 0, floats, 0, Unk05.Length);
                            Unk05 = floats;
                        }

                        for (int i = 0; i < 7; i++)
                        {
                            bw.Write(Unk05[i]);
                        }
                    }
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class Type3Data : DefaultData
            {
                public string Unk00 { get; set; } // Frame Name?
                public int Unk01 { get; set; }
                public ulong Unk02 { get; set; }
                public int Unk03 { get; set; }
                public int Unk04 { get; set; }
                public int Unk05 { get; set; }
                public short Unk06 { get; set; }
                public Type3Data()
                {

                }

                public Type3Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadString16();
                    Unk01 = br.ReadInt32();
                    Unk02 = br.ReadUInt64();
                    Unk03 = br.ReadInt32();
                    Unk04 = br.ReadInt32();
                    Unk05 = br.ReadInt32();
                    Unk06 = br.ReadInt16();
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.WriteString16(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);
                    bw.Write(Unk03);
                    bw.Write(Unk04);
                    bw.Write(Unk05);
                    bw.Write(Unk06);
                }
            }

            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk00 { get; set; } // Is Available?
            public int Type { get; set; }
            public DefaultData Data { get; set; } = new Type1Data();
            public DataWrapper(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                KeyFrameStart = br.ReadInt32();
                KeyFrameEnd = br.ReadInt32();
                Unk00 = br.ReadByte();
                Type = br.ReadInt32();

                switch (Type)
                {
                    case 3:
                        Data = new Type3Data(br);
                        break;

                    default:
                        Data = new Type1Data(br);
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(KeyFrameStart);
                bw.Write(KeyFrameEnd);
                bw.Write(Unk00);
                bw.Write(Type);
                Data.Write(bw);
            }

            public override string ToString()
            {
                return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
            }
        }

        public DataWrapper[] Data { get; set; }
        public ushort Unk00 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int Count = br.ReadInt32();
            Data = new DataWrapper[Count];

            for(int i = 0; i < Count; i++)
            {
                Data[i] = new DataWrapper(br);
            }

            Unk00 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Data.Length);

            foreach(DataWrapper Entry in Data)
            {
                Entry.Write(bw);
            }

            bw.Write(Unk00);
        }

        public override string ToString()
        {
            return string.Format("Type: 39 Frames: {0}", Data.Length);
        }
    }
}
