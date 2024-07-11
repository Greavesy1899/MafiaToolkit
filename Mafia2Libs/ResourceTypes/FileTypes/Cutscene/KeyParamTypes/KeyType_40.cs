using System;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.StringHelpers;
using Utils.Types;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_40 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk00 { get; set; } // Is Available?
            public int Unk01 { get; set; }
            public int Unk02 { get; set; }
            public int Unk03 { get; set; }
            public int Unk04 { get; set; }
            public int Unk05 { get; set; }
            public int Unk06 { get; set; }
            public int Unk07 { get; set; }
            public int Unk08 { get; set; }
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
                Unk02 = br.ReadInt32();
                Unk03 = br.ReadInt32();
                Unk04 = br.ReadInt32();
                Unk05 = br.ReadInt32();
                Unk06 = br.ReadInt32();
                Unk07 = br.ReadInt32();
                Unk08 = br.ReadInt32();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(KeyFrameStart);
                bw.Write(KeyFrameEnd);
                bw.Write(Unk00);
                bw.Write(Unk01);
                bw.Write(Unk02);
                bw.Write(Unk03);
                bw.Write(Unk04);
                bw.Write(Unk05);
                bw.Write(Unk06);
                bw.Write(Unk07);
                bw.Write(Unk08);
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
            return string.Format("Type: 40 Frames: {0}", Data.Length);
        }
    }
}
