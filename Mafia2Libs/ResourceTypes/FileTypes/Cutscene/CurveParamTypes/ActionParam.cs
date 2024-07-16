using System.ComponentModel;
using System.IO;
using System;
using Utils.StringHelpers;
using Utils.Types;
using System.Diagnostics;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ActionParam : ICurveParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int Type { get; set; }

            public FrameData()
            {

            }

            public FrameData(FrameData _base)
            {
                StartFrame = _base.StartFrame;
                EndFrame = _base.EndFrame;
                Unk00 = _base.Unk00;
                Type = _base.Type;
            }

            public FrameData(BinaryReader br)
            {
                Read(br);
            }

            public virtual void Read(BinaryReader br)
            {
                StartFrame = br.ReadInt32();
                EndFrame = br.ReadInt32();
                Unk00 = br.ReadBoolean();
                Type = br.ReadInt32();
            }

            public virtual void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Type);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class CameraFrameData : FrameData
        {
            public string Unk02 { get; set; } // Frame Name?
            public int Unk03 { get; set; }
            public int Unk04 { get; set; } // Possible ending KeyFrame?
            public int Unk05 { get; set; }
            public HashName NameHash { get; set; } // Another Name?
            public int Unk06 { get; set; } // 4?
            public float[] Unk07 { get; set; } = new float[0];
            public CameraFrameData()
            {

            }

            public CameraFrameData(FrameData _base) : base(_base)
            {

            }

            public CameraFrameData(BinaryReader br)
            {
                Read(br);
            }

            public override void Read(BinaryReader br)
            {
                Unk02 = br.ReadString16();
                Unk03 = br.ReadInt32();
                Unk04 = br.ReadInt32();
                Unk05 = br.ReadInt32();
                NameHash = new HashName();
                NameHash.ReadFromFile(br);
                Unk06 = br.ReadInt32();

                switch (Unk06)
                {
                    case 0:
                    case 1:
                    case 4:
                    case 5:
                    case 8:
                    case 9:

                        break;

                    default:
                        Unk07 = new float[7];

                        for (int i = 0; i < Unk07.Length; i++)
                        {
                            Unk07[i] = br.ReadSingle();
                        }
                        break;
                }
            }

            public override void Write(BinaryWriter bw)
            {
                base.Write(bw);
                bw.WriteString16(Unk02);
                bw.Write(Unk03);
                bw.Write(Unk04);
                bw.Write(Unk05);
                NameHash.WriteToFile(bw);
                bw.Write(Unk06);

                switch (Unk06)
                {
                    case 0:
                    case 1:
                    case 4:
                    case 5:
                    case 8:
                    case 9:

                        break;

                    default:
                        if (Unk07.Length < 7)
                        {
                            float[] floats = new float[7];
                            Array.Copy(Unk07, 0, floats, 0, Unk07.Length);
                            Unk07 = floats;
                        }

                        for (int i = 0; i < 7; i++)
                        {
                            bw.Write(Unk07[i]);
                        }
                        break;
                }
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class Unk30FrameData : FrameData
        {
            public float Unk02 { get; set; }
            public Unk30FrameData()
            {

            }

            public Unk30FrameData(FrameData _base) : base(_base)
            {

            }

            public Unk30FrameData(BinaryReader br)
            {
                Read(br);
            }

            public override void Read(BinaryReader br)
            {
                Unk02 = br.ReadSingle();
            }

            public override void Write(BinaryWriter bw)
            {
                base.Write(bw);
                bw.Write(Unk02);
            }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class ModelFrameData : FrameData
        {
            public string Unk02 { get; set; } // Frame Name?
            public int Unk03 { get; set; }
            public int Unk04 { get; set; }
            public int Unk05 { get; set; } // Possible ending KeyFrame?
            public int Unk06 { get; set; }
            public HashName NameHash { get; set; } // Another Name?
            public int Unk07 { get; set; }
            public float[] Unk08 { get; set; } = new float[0];

            public ModelFrameData(FrameData _base) : base(_base)
            {

            }

            public ModelFrameData(BinaryReader br)
            {
                Read(br);
            }

            public override void Read(BinaryReader br)
            {
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
                    case 4:
                    case 5:
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

            public override void Write(BinaryWriter bw)
            {
                base.Write(bw);
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
                    case 4:
                    case 5:
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
        }

        public ActionParam()
        {

        }

        public ActionParam(BinaryReader br)
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
    public class ModelAction : ActionParam
    {
        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public ModelAction()
        {

        }

        public ModelAction(BinaryReader br)
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
                FrameData baseData = new FrameData(br);
                ModelFrameData modelData = new(baseData);
                modelData.Read(br);
                Data[i] = modelData;
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
            return 22;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CameraAction : ActionParam
    {
        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public CameraAction()
        {

        }

        public CameraAction(BinaryReader br)
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
                FrameData baseData = new FrameData(br);
                CameraFrameData cameraFrameData = new(baseData);
                cameraFrameData.Read(br);
                Data[i] = cameraFrameData;
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
            return 39;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CameraTargetAction : ActionParam
    {
        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public CameraTargetAction()
        {

        }

        public CameraTargetAction(BinaryReader br)
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
                FrameData baseData = new FrameData(br);
                CameraFrameData modelData = new(baseData);
                modelData.Read(br);
                Data[i] = modelData;
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
            return 40;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Unk30Action : ActionParam
    {
        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public Unk30Action()
        {

        }

        public Unk30Action(BinaryReader br)
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
                FrameData baseData = new FrameData(br);
                CameraFrameData modelData = new(baseData);
                modelData.Read(br);
                Data[i] = modelData;
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
            return 30;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }
}
