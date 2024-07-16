using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CutEditParam : ICurveParam
    {
        public CutEditParam()
        {

        }

        public CutEditParam(BinaryReader br)
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
    public class CameraCut : CutEditParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int CameraAsset { get; set; }
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
                CameraAsset = br.ReadInt32();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(CameraAsset);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public CameraCut()
        {

        }

        public CameraCut(BinaryReader br)
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
            return 16;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CutChange : CutEditParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
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
                public int Unk00 { get; set; }
                public float Unk01 { get; set; }
                public float Unk02 { get; set; }
                public int Unk03 { get; set; }
                public Type1Data()
                {

                }

                public Type1Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadInt32();
                    Unk01 = br.ReadSingle();
                    Unk02 = br.ReadSingle();
                    Unk03 = br.ReadInt32();
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);
                    bw.Write(Unk03);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class Type2Data : DefaultData
            {
                public int Unk00 { get; set; }
                public float Unk01 { get; set; }
                public float Unk02 { get; set; }
                public float Unk03 { get; set; }
                public Type2Data()
                {

                }

                public Type2Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadInt32();
                    Unk01 = br.ReadSingle();
                    Unk02 = br.ReadSingle();
                    Unk03 = br.ReadSingle();
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);
                    bw.Write(Unk03);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class Type3Data : DefaultData
            {
                public float Unk00 { get; set; }
                public float Unk01 { get; set; }
                public float Unk02 { get; set; }
                public float Unk03 { get; set; }
                public float Unk04 { get; set; }
                public float Unk05 { get; set; }
                public Type3Data()
                {

                }

                public Type3Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadSingle();
                    Unk01 = br.ReadSingle();
                    Unk02 = br.ReadSingle();
                    Unk03 = br.ReadSingle();
                    Unk04 = br.ReadSingle();
                    Unk05 = br.ReadSingle();
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);
                    bw.Write(Unk03);
                    bw.Write(Unk04);
                    bw.Write(Unk05);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class Type32Data : DefaultData
            {
                public int Unk00 { get; set; }
                public int Unk01 { get; set; }
                public Type32Data()
                {

                }

                public Type32Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadInt32();
                    Unk01 = br.ReadInt32();
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                }
            }

            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } // Is Available?
            public int Type { get; set; }
            public DefaultData Data { get; set; } = new Type1Data();

            public FrameData(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                StartFrame = br.ReadInt32();
                EndFrame = br.ReadInt32();
                Unk00 = br.ReadBoolean();
                Type = br.ReadInt32();

                switch (Type)
                {
                    case 2:
                        Data = new Type2Data(br);
                        break;

                    case 3:
                        Data = new Type3Data(br);
                        break;

                    case 32:
                        Data = new Type32Data(br);
                        break;

                    default:
                        Data = new Type1Data(br);
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Type);
                Data.Write(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public CutChange()
        {

        }

        public CutChange(BinaryReader br)
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
            return 17;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }
}
