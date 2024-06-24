using System.IO;
using Utils.Extensions;
using Utils.StringHelpers;
using Utils.Types;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_39 : IKeyType
    {
        public class KeyTypeData_39
        {
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public int Unk04 { get; set; }
            public string Unk05 { get; set; } // Frame Name?
            public int Unk06 { get; set; }
            public int Unk07 { get; set; } // Possible ending KeyFrame?
            public int Unk08 { get; set; }
            public HashName NameHash { get; set; } // Another Name?
            public int Unk09 { get; set; } // 4?
            public int[] UnkInts { get; set; } = new int[0];
            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3}", KeyFrameStart, KeyFrameEnd, Unk05, NameHash.ToString());
            }
        }

        public int Unk01 { get; set; }
        public KeyTypeData_39[] Unk01Data { get; set; }
        public ushort Unk02 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            Unk01 = br.ReadInt32();
            Unk01Data = new KeyTypeData_39[Unk01];

            for(int i = 0; i < Unk01; i++)
            {
                KeyTypeData_39 KeyData = new KeyTypeData_39();
                KeyData.KeyFrameStart = br.ReadInt32();
                KeyData.KeyFrameEnd = br.ReadInt32();
                KeyData.Unk03 = br.ReadByte();
                KeyData.Unk04 = br.ReadInt32();
                KeyData.Unk05 = br.ReadString16();
                KeyData.Unk06 = br.ReadInt32();
                KeyData.Unk07 = br.ReadInt32();
                KeyData.Unk08 = br.ReadInt32();
                KeyData.NameHash = new HashName();
                KeyData.NameHash.ReadFromFile(br);
                KeyData.Unk09 = br.ReadInt32();



                Unk01Data[i] = KeyData;
            }

            Unk02 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Unk01);

            foreach(KeyTypeData_39 Entry in Unk01Data)
            {
                bw.Write(Entry.KeyFrameStart);
                bw.Write(Entry.KeyFrameEnd);
                bw.Write(Entry.Unk03);
                bw.Write(Entry.Unk04);
                bw.WriteString16(Entry.Unk05);
                bw.Write(Entry.Unk06);
                bw.Write(Entry.Unk07);
                bw.Write(Entry.Unk08);
                Entry.NameHash.WriteToFile(bw);
                bw.Write(Entry.Unk09);
            }

            bw.Write(Unk02);
        }
    }
}
