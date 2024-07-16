using System.ComponentModel;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AnimBlockParam : ICurveParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int Unk00 { get; set; }
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk01 { get; set; } = true;
            public string AnimationName { get; set; }
            public int Unk02 { get; set; }
            public float Unk03 { get; set; }
            public int Unk04 { get; set; }
            public float Unk05 { get; set; }
            public FrameData()
            {

            }

            public FrameData(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                Unk00 = br.ReadInt32();
                StartFrame = br.ReadInt32();
                EndFrame = br.ReadInt32();
                Unk01 = br.ReadBoolean();
                AnimationName = br.ReadString16();
                Unk02 = br.ReadInt32();
                Unk03 = br.ReadSingle();
                Unk04 = br.ReadInt32();
                Unk05 = br.ReadSingle();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(Unk00);
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk01);
                bw.WriteString16(AnimationName);
                bw.Write(Unk02);
                bw.Write(Unk03);
                bw.Write(Unk04);
                bw.Write(Unk05);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public AnimBlockParam()
        {

        }

        public AnimBlockParam(BinaryReader br)
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
            return 13;
        }

        public override string ToString()
        {
            return GetType().Name + " Frames: " + Data.Length;
        }
    }
}
