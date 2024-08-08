using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.StringHelpers;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.CurveParams
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SoundParam : ICurveParam
    {
        public SoundParam()
        {

        }

        public SoundParam(BinaryReader br)
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
    public class SoundObjectAmbient : SoundParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public string SoundFile { get; set; } = "";
            public int Unk01 { get; set; }
            public byte Unk02 { get; set; }
            public float Unk03 { get; set; }
            public float Unk04 { get; set; }
            public int Unk05 { get; set; }
            public float Unk06 { get; set; }
            public int Unk07 { get; set; }
            public float Unk08 { get; set; }
            public float Unk09 { get; set; }
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
                SoundFile = br.ReadString16();
                Unk01 = br.ReadInt32();
                Unk02 = br.ReadByte();
                Unk03 = br.ReadSingle();
                Unk04 = br.ReadSingle();
                Unk05 = br.ReadInt32();
                Unk06 = br.ReadSingle();
                Unk07 = br.ReadInt32();
                Unk08 = br.ReadSingle();
                Unk09 = br.ReadSingle();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.WriteString16(SoundFile);
                bw.Write(Unk01);
                bw.Write(Unk02);
                bw.Write(Unk03);
                bw.Write(Unk04);
                bw.Write(Unk05);
                bw.Write(Unk06);
                bw.Write(Unk07);
                bw.Write(Unk08);
                bw.Write(Unk09);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public SoundObjectAmbient()
        {

        }

        public SoundObjectAmbient(BinaryReader br)
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
            return 18;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SoundObjectPoint : SoundParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public string SoundFile { get; set; } = "";
            public bool Loop { get; set; }
            public int Unk02 { get; set; }
            public float Unk03 { get; set; }
            public float Unk04 { get; set; }
            public int Unk05 { get; set; }
            public float Unk06 { get; set; }
            public int Unk07 { get; set; }
            public float Unk08 { get; set; }
            public int Unk09 { get; set; }
            public float OuterRadius { get; set; }
            public float Unk11 { get; set; }
            public float Unk12 { get; set; }
            public float InnerRadius { get; set; }
            public byte Unk14 { get; set; }
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Position { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(QuaternionConverter))]
            public Quaternion Rotation { get; set; } = Quaternion.Identity;
            public byte IsMeshAvailableFlags { get; set; }
            public string FrameName { get; set; } = "";
            public ulong FrameHash { get; set; }
            public ulong MainJointHash { get; set; }
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
                SoundFile = br.ReadString16();
                Loop = br.ReadBoolean();
                Unk02 = br.ReadInt32();
                Unk03 = br.ReadSingle();
                Unk04 = br.ReadSingle();
                Unk05 = br.ReadInt32();
                Unk06 = br.ReadSingle();
                Unk07 = br.ReadInt32();
                Unk08 = br.ReadSingle();
                Unk09 = br.ReadInt32();
                OuterRadius = br.ReadSingle();
                Unk11 = br.ReadSingle();
                Unk12 = br.ReadSingle();
                InnerRadius = br.ReadSingle();
                Unk14 = br.ReadByte();
                Position = Vector3Utils.ReadFromFile(br);
                Rotation = QuaternionExtensions.ReadFromFile(br);
                IsMeshAvailableFlags = br.ReadByte();

                switch (IsMeshAvailableFlags)
                {
                    case 1:
                        FrameName = br.ReadString16();
                        MainJointHash = br.ReadUInt64();
                        break;

                    case 3:
                        FrameName = br.ReadString16();
                        MainJointHash = br.ReadUInt64();
                        FrameHash = br.ReadUInt64();
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.WriteString16(SoundFile);
                bw.Write(Loop);
                bw.Write(Unk02);
                bw.Write(Unk03);
                bw.Write(Unk04);
                bw.Write(Unk05);
                bw.Write(Unk06);
                bw.Write(Unk07);
                bw.Write(Unk08);
                bw.Write(Unk09);
                bw.Write(OuterRadius);
                bw.Write(Unk11);
                bw.Write(Unk12);
                bw.Write(InnerRadius);
                bw.Write(Unk14);
                Position.WriteToFile(bw);
                Rotation.WriteToFile(bw);
                bw.Write(IsMeshAvailableFlags);

                switch (IsMeshAvailableFlags)
                {
                    case 1:
                        bw.WriteString16(FrameName);
                        bw.Write(MainJointHash);
                        break;

                    case 3:
                        bw.WriteString16(FrameName);
                        bw.Write(MainJointHash);
                        bw.Write(FrameHash);
                        break;
                }
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public SoundObjectPoint()
        {

        }

        public SoundObjectPoint(BinaryReader br)
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
            return 19;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SoundObjectSphereAmbient : SoundParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public string SoundFile { get; set; } = "";
            public int Unk01 { get; set; }
            public byte Unk02 { get; set; }
            public float Unk03 { get; set; }
            public float Unk04 { get; set; }
            public int Unk05 { get; set; }
            public float Unk06 { get; set; }
            public int Unk07 { get; set; }
            public float Unk08 { get; set; }
            public int Unk09 { get; set; }
            public float Unk10 { get; set; }
            public float Unk11 { get; set; }
            public byte Unk12 { get; set; }
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 Position { get; set; } = Vector3.Zero;
            [TypeConverter(typeof(QuaternionConverter))]
            public Quaternion Rotation { get; set; } = Quaternion.Identity;
            public byte IsMeshAvailableFlags { get; set; }
            public string FrameName { get; set; } = "";
            public ulong FrameHash { get; set; }
            public ulong MainJointHash { get; set; }
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
                SoundFile = br.ReadString16();
                Unk01 = br.ReadInt32();
                Unk02 = br.ReadByte();
                Unk03 = br.ReadSingle();
                Unk04 = br.ReadSingle();
                Unk05 = br.ReadInt32();
                Unk06 = br.ReadSingle();
                Unk07 = br.ReadInt32();
                Unk08 = br.ReadSingle();
                Unk09 = br.ReadInt32();
                Unk10 = br.ReadSingle();
                Unk11 = br.ReadSingle();
                Unk12 = br.ReadByte();
                Position = Vector3Utils.ReadFromFile(br);
                Rotation = QuaternionExtensions.ReadFromFile(br);
                IsMeshAvailableFlags = br.ReadByte();

                switch (IsMeshAvailableFlags)
                {
                    case 1:
                        FrameName = br.ReadString16();
                        MainJointHash = br.ReadUInt64();
                        break;

                    case 3:
                        FrameName = br.ReadString16();
                        MainJointHash = br.ReadUInt64();
                        FrameHash = br.ReadUInt64();
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.WriteString16(SoundFile);
                bw.Write(Unk01);
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
                bw.Write(Unk12);
                Position.WriteToFile(bw);
                Rotation.WriteToFile(bw);
                bw.Write(IsMeshAvailableFlags);

                switch (IsMeshAvailableFlags)
                {
                    case 1:
                        bw.WriteString16(FrameName);
                        bw.Write(MainJointHash);
                        break;

                    case 3:
                        bw.WriteString16(FrameName);
                        bw.Write(MainJointHash);
                        bw.Write(FrameHash);
                        break;
                }
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public SoundObjectSphereAmbient()
        {

        }

        public SoundObjectSphereAmbient(BinaryReader br)
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
            return 43;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SoundEntityAction : SoundParam
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            public int StartFrame { get; set; }
            public int EndFrame { get; set; }
            public bool Unk00 { get; set; } = true;
            public bool Active { get; set; }
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
                Active = br.ReadBoolean();
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(StartFrame);
                bw.Write(EndFrame);
                bw.Write(Unk00);
                bw.Write(Active);
            }
        }

        public FrameData[] Data { get; set; } = new FrameData[0];
        public short Unk00 { get; set; }

        public SoundEntityAction()
        {

        }

        public SoundEntityAction(BinaryReader br)
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
            return 46;
        }

        public override string ToString()
        {
            return GetType().BaseType.Name + "::" + GetType().Name + " Frames: " + Data.Length;
        }
    }
}
