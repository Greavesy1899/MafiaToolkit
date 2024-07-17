using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ParticleParam : ICurveParam
    {
        public ParticleParam()
        {

        }

        public ParticleParam(BinaryReader br)
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
    public class ParticleObject : ParticleParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public int EffectID { get; set; }
            public short Unk02 { get; set; }
            public int Unk03 { get; set; }
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Position { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(QuaternionConverter))]
            public Quaternion Rotation { get; set; } = Quaternion.Identity;
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
                EffectID = br.ReadInt32();
                Unk02 = br.ReadInt16();
                Unk03 = br.ReadInt32();
                Position = Vector3Utils.ReadFromFile(br);
                Rotation = QuaternionExtensions.ReadFromFile(br);
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(EffectID);
                bw.Write(Unk02);
                bw.Write(Unk03);
                Position.WriteToFile(bw);
                Rotation.WriteToFile(bw);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public ParticleObject()
        {

        }

        public ParticleObject(BinaryReader br)
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
            return 15;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }
}
