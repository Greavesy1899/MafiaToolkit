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

            public override string ToString()
            {
                return string.Format("{0} {1} {2} {3}", KeyFrameStart, KeyFrameEnd, Unk05, NameHash.ToString());
            }
        }

        public int Unk01 { get; set; }
        public KeyTypeData_39[] Unk01Data { get; set; }
        public ushort Unk02 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            Unk01 = stream.ReadInt32(isBigEndian);
            Unk01Data = new KeyTypeData_39[Unk01];

            for(int i = 0; i < Unk01; i++)
            {
                KeyTypeData_39 KeyData = new KeyTypeData_39();
                KeyData.KeyFrameStart = stream.ReadInt32(isBigEndian);
                KeyData.KeyFrameEnd = stream.ReadInt32(isBigEndian);
                KeyData.Unk03 = stream.ReadByte8();
                KeyData.Unk04 = stream.ReadInt32(isBigEndian);
                KeyData.Unk05 = stream.ReadString16(isBigEndian);
                KeyData.Unk06 = stream.ReadInt32(isBigEndian);
                KeyData.Unk07 = stream.ReadInt32(isBigEndian);
                KeyData.Unk08 = stream.ReadInt32(isBigEndian);
                KeyData.NameHash = new HashName();
                KeyData.NameHash.ReadFromFile(stream, isBigEndian);
                KeyData.Unk09 = stream.ReadInt32(isBigEndian);
                Unk01Data[i] = KeyData;
            }

            Unk02 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk01, isBigEndian);

            foreach(KeyTypeData_39 Entry in Unk01Data)
            {
                stream.Write(Entry.KeyFrameStart, isBigEndian);
                stream.Write(Entry.KeyFrameEnd, isBigEndian);
                stream.WriteByte(Entry.Unk03);
                stream.Write(Entry.Unk04, isBigEndian);
                stream.WriteString16(Entry.Unk05, isBigEndian);
                stream.Write(Entry.Unk06, isBigEndian);
                stream.Write(Entry.Unk07, isBigEndian);
                stream.Write(Entry.Unk08, isBigEndian);
                Entry.NameHash.WriteToFile(stream, isBigEndian);
                stream.Write(Entry.Unk09, isBigEndian);
            }

            stream.Write(Unk02, isBigEndian);
        }
    }
}
