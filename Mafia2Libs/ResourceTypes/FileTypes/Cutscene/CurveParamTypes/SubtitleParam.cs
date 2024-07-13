using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SubtitleParam : ICurveParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int Unk01 { get; set; }
            public int TextID { get; set; }
            public float Unk02 { get; set; }
            public float Unk03 { get; set; }
            public int Unk04 { get; set; }
            public int Unk05 { get; set; }
            public float Unk06 { get; set; }
            public int Unk07 { get; set; }
            public byte Unk08 { get; set; }
            public byte Unk09 { get; set; }
            public byte Unk10 { get; set; }
            public byte Unk11 { get; set; }
            public FrameData()
            {

            }

            public FrameData(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                StartFrame = br.ReadInt32();
                EndFrame = br.ReadInt32();
                Unk00 = br.ReadBoolean();
                Unk01 = br.ReadInt32();
                TextID = br.ReadInt32();
                Unk02 = br.ReadSingle();
                Unk03 = br.ReadSingle();
                Unk04 = br.ReadInt32();
                Unk05 = br.ReadInt32();
                Unk06 = br.ReadSingle();
                Unk07 = br.ReadInt32();
                Unk08 = br.ReadByte();
                Unk09 = br.ReadByte();
                Unk10 = br.ReadByte();
                Unk11 = br.ReadByte();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Unk01);
                bw.Write(TextID);
                bw.Write(Unk02);
                bw.Write(Unk03);
                bw.Write(Unk04);
                bw.Write(Unk05);
                bw.Write(Unk06);
                bw.Write(Unk07);
                bw.Write(Unk08);
                bw.Write(Unk09);
                bw.Write(Unk10);
                bw.Write(Unk11);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public SubtitleParam()
        {

        }

        public SubtitleParam(BinaryReader br)
        {
            Read(br);
        }

        public override void Read(BinaryReader br)
        {
            base.Read(br);
            int Count = br.ReadInt32();
            Data = new FrameData[Count];

            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] = new(br);
            }

            Unk00 = br.ReadInt16();
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Data.Length);

            foreach (var data in Data)
            {
                data.Write(bw);
            }

            bw.Write(Unk00);
        }

        public override int GetParamType()
        {
            return 12;
        }

        public override string ToString()
        {
            return GetType().Name + " Frames: " + Data.Length;
        }
    }
}
