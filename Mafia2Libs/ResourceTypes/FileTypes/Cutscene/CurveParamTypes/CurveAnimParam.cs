using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CurveAnimParam : ICurveParam
    {
        public CurveAnimParam()
        {

        }

        public CurveAnimParam(BinaryReader br)
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
    public class FloatLinear : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public float Value { get; set; } = 0.0f;
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
                Value = br.ReadSingle();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Value);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public FloatLinear()
        {

        }

        public FloatLinear(BinaryReader br)
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
            return 0;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FloatBezier : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int Unk01 { get; set; }
            [TypeConverter(typeof(Vector2Converter))]
            public Vector2 AnchorA { get; set; } = Vector2.Zero;
            [TypeConverter(typeof(Vector2Converter))]
            public Vector2 AnchorB { get; set; } = Vector2.Zero;
            public float Value { get; set; } = 0.0f;
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
                AnchorA = Vector2Extenders.ReadFromFile(br);
                AnchorB = Vector2Extenders.ReadFromFile(br);
                Value = br.ReadSingle();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Unk01);
                AnchorA.WriteToFile(bw);
                AnchorB.WriteToFile(bw);
                bw.Write(Value);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public FloatBezier()
        {

        }

        public FloatBezier(BinaryReader br)
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
            return 1;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FloatTCB : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int Unk01 { get; set; }
            [TypeConverter(typeof(Vector2Converter))]
            public Vector2 Position { get; set; } = Vector2.Zero;
            [TypeConverter(typeof(Vector2Converter))]
            public Vector2 PreviousTangent { get; set; } = Vector2.Zero;
            [TypeConverter(typeof(Vector2Converter))]
            public Vector2 NextTangent { get; set; } = Vector2.Zero;
            public float Value { get; set; } = 0.0f;
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
                Position = Vector2Extenders.ReadFromFile(br);
                PreviousTangent = Vector2Extenders.ReadFromFile(br);
                NextTangent = Vector2Extenders.ReadFromFile(br);
                Value = br.ReadSingle();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Unk01);
                Position.WriteToFile(bw);
                PreviousTangent.WriteToFile(bw);
                NextTangent.WriteToFile(bw);
                bw.Write(Value);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public FloatTCB()
        {

        }

        public FloatTCB(BinaryReader br)
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
            return 2;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector2Linear : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            [TypeConverter(typeof(Vector2Converter))]
            public Vector2 Value { get; set; } = Vector2.Zero;
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
                Value = Vector2Extenders.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public Vector2Linear()
        {

        }

        public Vector2Linear(BinaryReader br)
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
            return 3;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector2Bezier : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int Unk01 { get; set; }
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 AnchorA { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 AnchorB { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(Vector2Converter))]
            public Vector2 Value { get; set; } = Vector2.Zero;
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
                AnchorA = Vector3Utils.ReadFromFile(br);
                AnchorB = Vector3Utils.ReadFromFile(br);
                Value = Vector2Extenders.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Unk01);
                AnchorA.WriteToFile(bw);
                AnchorB.WriteToFile(bw);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public Vector2Bezier()
        {

        }

        public Vector2Bezier(BinaryReader br)
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
            return 4;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector2TCB : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int Unk01 { get; set; }
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Position { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 PreviousTangent { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 NextTangent { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(Vector2Converter))]
            public Vector2 Value { get; set; } = Vector2.Zero;
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
                Position = Vector3Utils.ReadFromFile(br);
                PreviousTangent = Vector3Utils.ReadFromFile(br);
                NextTangent = Vector3Utils.ReadFromFile(br);
                Value = Vector2Extenders.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Unk01);
                Position.WriteToFile(bw);
                PreviousTangent.WriteToFile(bw);
                NextTangent.WriteToFile(bw);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public Vector2TCB()
        {

        }

        public Vector2TCB(BinaryReader br)
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
            return 5;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector3Linear : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Value { get; set; } = Vector3.Zero;
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
                Value = Vector3Utils.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public Vector3Linear()
        {

        }

        public Vector3Linear(BinaryReader br)
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
            return 6;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector3Bezier : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int Unk01 { get; set; }
            [TypeConverter(typeof(Vector4Converter))]
            public Vector4 AnchorA { get; set; } = Vector4.Zero;
            [TypeConverter(typeof(Vector4Converter))]
            public Vector4 AnchorB { get; set; } = Vector4.Zero;
            [TypeConverter(typeof(Vector4Converter))]
            public Vector4 AnchorC { get; set; } = Vector4.Zero;
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Value { get; set; } = Vector3.Zero;
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
                AnchorA = Vector4Extenders.ReadFromFile(br);
                AnchorB = Vector4Extenders.ReadFromFile(br);
                AnchorC = Vector4Extenders.ReadFromFile(br);
                Value = Vector3Utils.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Unk01);
                AnchorA.WriteToFile(bw);
                AnchorB.WriteToFile(bw);
                AnchorC.WriteToFile(bw);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public Vector3Bezier()
        {

        }

        public Vector3Bezier(BinaryReader br)
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
            return 7;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector3TCB : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int Unk01 { get; set; }
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Position { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 PreviousTangent { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 NextTangent { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Value { get; set; } = Vector3.Zero;
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
                Position = Vector3Utils.ReadFromFile(br);
                PreviousTangent = Vector3Utils.ReadFromFile(br);
                NextTangent = Vector3Utils.ReadFromFile(br);
                Value = Vector3Utils.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Unk01);
                Position.WriteToFile(bw);
                PreviousTangent.WriteToFile(bw);
                NextTangent.WriteToFile(bw);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public Vector3TCB()
        {

        }

        public Vector3TCB(BinaryReader br)
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
            return 8;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class QuaternionLinear : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            [TypeConverter(typeof(QuaternionConverter))]
            public Quaternion Value { get; set; } = Quaternion.Identity;
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
                Value = QuaternionExtensions.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public QuaternionLinear()
        {

        }

        public QuaternionLinear(BinaryReader br)
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
            return 9;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class QuaternionBezier : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            [TypeConverter(typeof(QuaternionConverter))]
            public Quaternion Value { get; set; } = Quaternion.Identity;
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
                Value = QuaternionExtensions.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public QuaternionBezier()
        {

        }

        public QuaternionBezier(BinaryReader br)
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
            return 10;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class QuaternionTCB : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            [TypeConverter(typeof(QuaternionConverter))]
            public Quaternion Value { get; set; } = Quaternion.Identity;
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
                Value = QuaternionExtensions.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                Value.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public QuaternionTCB()
        {

        }

        public QuaternionTCB(BinaryReader br)
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
            return 11;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PositionXYZ : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameDataWrapper
        {
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class FrameData
            {
                public int StartFrame { get; set; }
                public int EndFrame { get; set; }
                public bool Unk00 { get; set; } = true;
                public FrameData()
                {

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
                }

                public virtual void Write(BinaryWriter bw)
                {
                    bw.Write(StartFrame);
                    bw.Write(EndFrame);
                    bw.Write(Unk00);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class FloatData : FrameData
            {
                public float Value { get; set; }
                public FloatData()
                {

                }

                public FloatData(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    base.Read(br);
                    Value = br.ReadSingle();
                }

                public override void Write(BinaryWriter bw)
                {
                    base.Write(bw);
                    bw.Write(Value);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class VectorData : FrameData
            {
                public int Unk01 { get; set; }
                [TypeConverter(typeof(Vector4Converter))]
                public Vector4 Unk02 { get; set; } = Vector4.Zero;
                public float Value { get; set; }
                public VectorData()
                {

                }

                public VectorData(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    base.Read(br);
                    Unk01 = br.ReadInt32();
                    Unk02 = Vector4Extenders.ReadFromFile(br);
                    Value = br.ReadSingle();
                }

                public override void Write(BinaryWriter bw)
                {
                    base.Write(bw);
                    bw.Write(Unk01);
                    Unk02.WriteToFile(bw);
                    bw.Write(Value);
                }
            }

            public int Type { get; set; }
            public FrameData[] Data { get; set; } = new FloatData[0];

            public FrameDataWrapper()
            {

            }

            public FrameDataWrapper(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                Type = br.ReadInt32();
                int Count = br.ReadInt32();

                switch (Type)
                {
                    case 0:
                        Data = new FloatData[Count];

                        for (int i = 0; i < Data.Length; i++)
                        {
                            Data[i] = new FloatData(br);
                        }
                        break;

                    case 1:
                        Data = new VectorData[Count];

                        for (int i = 0; i < Data.Length; i++)
                        {
                            Data[i] = new VectorData(br);
                        }
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(Type);
                bw.Write(Data.Length);

                foreach (var val in Data)
                {
                    val.Write(bw);
                }
            }
        }
        
        public int Unk00 { get; set; }
        public FrameDataWrapper X { get; set; } = new FrameDataWrapper();
        public FrameDataWrapper Y { get; set; } = new FrameDataWrapper();
        public FrameDataWrapper Z { get; set; } = new FrameDataWrapper();
        public short Unk01 { get; set; }

        public PositionXYZ()
        {

        }

        public PositionXYZ(BinaryReader br)
        {
            Read(br);
        }

        public override void Read(BinaryReader br)
        {
            base.Read(br);
            Unk00 = br.ReadInt32();
            X = new(br);
            Y = new(br);
            Z = new(br);
            Unk01 = br.ReadInt16();
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Unk00);
            X.Write(bw);
            Y.Write(bw);
            Z.Write(bw);
            bw.Write(Unk01);
        }

        public override int GetParamType()
        {
            return 27;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EulerXYZ : CurveAnimParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameDataWrapper
        {
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class FrameData
            {
                public int StartFrame { get; set; }
                public int EndFrame { get; set; }
                public bool Unk00 { get; set; } = true;
                public FrameData()
                {

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
                }

                public virtual void Write(BinaryWriter bw)
                {
                    bw.Write(StartFrame);
                    bw.Write(EndFrame);
                    bw.Write(Unk00);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class FloatData : FrameData
            {
                public float Value { get; set; }
                public FloatData()
                {

                }

                public FloatData(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    base.Read(br);
                    Value = br.ReadSingle();
                }

                public override void Write(BinaryWriter bw)
                {
                    base.Write(bw);
                    bw.Write(Value);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class QuaternionData : FrameData
            {
                public int Unk01 { get; set; }
                [TypeConverter(typeof(QuaternionConverter))]
                public Quaternion Unk02 { get; set; } = Quaternion.Identity;
                public float Value { get; set; }
                public QuaternionData()
                {

                }

                public QuaternionData(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    base.Read(br);
                    Unk01 = br.ReadInt32();
                    Unk02 = QuaternionExtensions.ReadFromFile(br);
                    Value = br.ReadSingle();
                }

                public override void Write(BinaryWriter bw)
                {
                    base.Write(bw);
                    bw.Write(Unk01);
                    Unk02.WriteToFile(bw);
                    bw.Write(Value);
                }
            }

            public int Type { get; set; }
            public FrameData[] Data { get; set; } = new FloatData[0];

            public FrameDataWrapper()
            {

            }

            public FrameDataWrapper(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                Type = br.ReadInt32();
                int Count = br.ReadInt32();

                switch (Type)
                {
                    case 0:
                        Data = new FloatData[Count];

                        for (int i = 0; i < Data.Length; i++)
                        {
                            Data[i] = new FloatData(br);
                        }
                        break;

                    case 1:
                        Data = new QuaternionData[Count];

                        for (int i = 0; i < Data.Length; i++)
                        {
                            Data[i] = new QuaternionData(br);
                        }
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(Type);
                bw.Write(Data.Length);

                foreach (var val in Data)
                {
                    val.Write(bw);
                }
            }
        }

        public int Unk00 { get; set; }
        public FrameDataWrapper X { get; set; } = new FrameDataWrapper();
        public FrameDataWrapper Y { get; set; } = new FrameDataWrapper();
        public FrameDataWrapper Z { get; set; } = new FrameDataWrapper();
        public short Unk01 { get; set; }

        public EulerXYZ()
        {

        }

        public EulerXYZ(BinaryReader br)
        {
            Read(br);
        }

        public override void Read(BinaryReader br)
        {
            base.Read(br);
            Unk00 = br.ReadInt32();
            X = new(br);
            Y = new(br);
            Z = new(br);
            Unk01 = br.ReadInt16();
        }

        public override void Write(BinaryWriter bw)
        {
            base.Write(bw);
            bw.Write(Unk00);
            X.Write(bw);
            Y.Write(bw);
            Z.Write(bw);
            bw.Write(Unk01);
        }

        public override int GetParamType()
        {
            return 28;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name;
        }
    }
}
