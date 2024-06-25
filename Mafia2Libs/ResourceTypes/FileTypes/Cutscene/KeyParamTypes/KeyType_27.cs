using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_27 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class Type27DataWrapper
        {
            public class QuaternionData
            {
                public uint KeyFrameStart { get; set; }
                public uint KeyFrameEnd { get; set; }
                public byte Unk0 { get; set; }
                public int KeyType { get; set; }
                public Quaternion Rotation { get; set; }
                public float Unk03 { get; set; }

                public override string ToString()
                {
                    return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
                }
            }

            public class Float32Data
            {
                public uint KeyFrameStart { get; set; }
                public uint KeyFrameEnd { get; set; }
                public byte Unk0 { get; set; }
                public float Value { get; set; }

                public override string ToString()
                {
                    return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
                }
            }

            public uint Type { get; set; }
            public QuaternionData[] RotationData { get; set; } = new QuaternionData[0];
            public Float32Data[] FloatData { get; set; } = new Float32Data[0];

            public void ReadFromFile(BinaryReader br)
            {
                Type = br.ReadUInt32();
                int Count = br.ReadInt32();

                switch (Type)
                {
                    case 0:
                        FloatData = new Float32Data[Count];

                        for (int i = 0; i < Count; i++)
                        {
                            Float32Data FloatInfo = new();
                            FloatInfo.KeyFrameStart = br.ReadUInt32();
                            FloatInfo.KeyFrameEnd = br.ReadUInt32();
                            FloatInfo.Unk0 = br.ReadByte();
                            FloatInfo.Value = br.ReadSingle();
                            FloatData[i] = FloatInfo;
                        }
                        break;

                    case 1:
                        RotationData = new QuaternionData[Count];

                        for (int i = 0; i < Count; i++)
                        {
                            QuaternionData RotationInfo = new();
                            RotationInfo.KeyFrameStart = br.ReadUInt32();
                            RotationInfo.KeyFrameEnd = br.ReadUInt32();
                            RotationInfo.Unk0 = br.ReadByte();
                            RotationInfo.KeyType = br.ReadInt32();
                            RotationInfo.Rotation = QuaternionExtensions.ReadFromFile(br);
                            RotationInfo.Unk03 = br.ReadSingle();
                            RotationData[i] = RotationInfo;
                        }
                        break;
                }
            }

            public void WriteToFile(BinaryWriter bw)
            {
                bw.Write(Type);

                switch (Type)
                {
                    case 0:
                        bw.Write(FloatData.Length);

                        foreach (var Info in FloatData)
                        {
                            bw.Write(Info.KeyFrameStart);
                            bw.Write(Info.KeyFrameEnd);
                            bw.Write(Info.Unk0);
                            bw.Write(Info.Value);
                        }
                        break;

                    case 1:
                        bw.Write(RotationData.Length);

                        foreach (var Info in RotationData)
                        {
                            bw.Write(Info.KeyFrameStart);
                            bw.Write(Info.KeyFrameEnd);
                            bw.Write(Info.Unk0);
                            bw.Write(Info.KeyType);
                            Info.Rotation.WriteToFile(bw);
                            bw.Write(Info.Unk03);
                        }
                        break;
                }
            }

            public override string ToString()
            {
                return string.Format("Num Rotations: {0} Num Floats: {1}", RotationData.Length, FloatData.Length);
            }

        }

        public uint Unk0 { get; set; }
        public Type27DataWrapper Type27DataWrapper0 { get; set; }
        public Type27DataWrapper Type27DataWrapper1 { get; set; }
        public Type27DataWrapper Type27DataWrapper2 { get; set; }
        public ushort Unk1 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);
            Unk0 = br.ReadUInt32();

            Type27DataWrapper0 = new Type27DataWrapper();
            Type27DataWrapper0.ReadFromFile(br);
            Type27DataWrapper1 = new Type27DataWrapper();
            Type27DataWrapper1.ReadFromFile(br);
            Type27DataWrapper2 = new Type27DataWrapper();
            Type27DataWrapper2.ReadFromFile(br);

            Unk1 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Unk0);

            Type27DataWrapper0.WriteToFile(bw);
            Type27DataWrapper1.WriteToFile(bw);
            Type27DataWrapper2.WriteToFile(bw);

            bw.Write(Unk1);
        }

        public override string ToString()
        {
            return string.Format("Num Rotations: {0} Num Floats: {1}", Type27DataWrapper0.RotationData.Length + Type27DataWrapper1.RotationData.Length + Type27DataWrapper2.RotationData.Length,
                                                                       Type27DataWrapper0.FloatData.Length + Type27DataWrapper1.FloatData.Length + Type27DataWrapper2.FloatData.Length);
        }
    }
}
