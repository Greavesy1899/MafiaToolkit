using System;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.StringHelpers;
using Utils.Types;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_22 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk00 { get; set; } // Is Available?
            public int Unk01 { get; set; }
            public string Unk02 { get; set; } // Frame Name?
            public int Unk03 { get; set; }
            public int Unk04 { get; set; }
            public int Unk05 { get; set; } // Possible ending KeyFrame?
            public int Unk06 { get; set; }
            public HashName NameHash { get; set; } // Another Name?
            public int Unk07 { get; set; }
            public float[] Unk08 { get; set; } = new float[0];
            public FrameData(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                KeyFrameStart = br.ReadInt32();
                KeyFrameEnd = br.ReadInt32();
                Unk00 = br.ReadByte();
                Unk01 = br.ReadInt32();
                Unk02 = br.ReadString16();
                Unk03 = br.ReadInt32();
                Unk04 = br.ReadInt32();
                Unk05 = br.ReadInt32();
                Unk06 = br.ReadInt32();
                NameHash = new HashName();
                NameHash.ReadFromFile(br);
                Unk07 = br.ReadInt32();

                switch (Unk07)
                {
                    case 0:
                    case 1:
                    case 8:
                    case 9:

                        break;

                    default:
                        Unk08 = new float[7];

                        for (int i = 0; i < Unk08.Length; i++)
                        {
                            Unk08[i] = br.ReadSingle();
                        }
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(KeyFrameStart);
                bw.Write(KeyFrameEnd);
                bw.Write(Unk00);
                bw.Write(Unk01);
                bw.WriteString16(Unk02);
                bw.Write(Unk03);
                bw.Write(Unk04);
                bw.Write(Unk05);
                bw.Write(Unk06);
                NameHash.WriteToFile(bw);
                bw.Write(Unk07);

                switch (Unk07)
                {
                    case 0:
                    case 1:
                    case 8:
                    case 9:

                        break;

                    default:
                        if (Unk08.Length < 7)
                        {
                            float[] floats = new float[7];
                            Array.Copy(Unk08, 0, floats, 0, Unk08.Length);
                            Unk08 = floats;
                        }

                        for (int i = 0; i < 7; i++)
                        {
                            bw.Write(Unk08[i]);
                        }
                        break;
                }
            }

            public override string ToString()
            {
                return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
            }
        }

        public FrameData[] Data { get; set; }
        public ushort Unk00 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int Count = br.ReadInt32();
            Data = new FrameData[Count];

            for (int i = 0; i < Count; i++)
            {
                Data[i] = new FrameData(br);
            }

            Unk00 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Data.Length);

            foreach (FrameData Entry in Data)
            {
                Entry.Write(bw);
            }

            bw.Write(Unk00);
        }

        public override string ToString()
        {
            return string.Format("Type: 22 Frames: {0}", Data.Length);
        }
    }
}
