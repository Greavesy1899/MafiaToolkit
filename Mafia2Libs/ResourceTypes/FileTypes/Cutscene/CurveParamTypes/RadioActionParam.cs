using System.ComponentModel;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RadioActionParam : ICurveParam
    {
        public RadioActionParam()
        {

        }

        public RadioActionParam(BinaryReader br)
        {
            Read(br);
        }

        public override void Read(BinaryReader br)
        {
            base.Read(br);
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
        }

        public override int GetParamType()
        {
            return base.GetParamType();
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RadioAction : RadioActionParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int Unk00 { get; set; }
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk01 { get; set; } = true;
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
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(Unk00);
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk01);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public RadioAction()
        {

        }

        public RadioAction(BinaryReader br)
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
            return 48;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }
}
