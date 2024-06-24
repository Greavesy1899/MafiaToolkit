using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_27 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class QuaternionWrapper
        {
            public class QuaternionData
            {
                public uint KeyFrameStart { get; set; }
                public uint KeyFrameEnd { get; set; }
                public ushort Unk0 { get; set; }
                public ushort KeyType { get; set; }
                public byte Unk01 { get; set; }
                public Quaternion Rotation { get; set; }
                public float Unk03 { get; set; }

                public override string ToString()
                {
                    return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
                }
            }

            public uint Unk0 { get; set; }
            public uint NumRotations { get; set; }
            public QuaternionData[] RotationData { get; set; }

            public void ReadFromFile(BinaryReader br)
            {
                Unk0 = br.ReadUInt32();
                NumRotations = br.ReadUInt32();
                RotationData = new QuaternionData[NumRotations];

                for(int i = 0; i < NumRotations; i++)
                {
                    QuaternionData RotationInfo = new QuaternionData();
                    RotationInfo.KeyFrameStart = br.ReadUInt32();
                    RotationInfo.KeyFrameEnd = br.ReadUInt32();
                    RotationInfo.Unk0 = br.ReadUInt16();
                    RotationInfo.KeyType = br.ReadUInt16();
                    RotationInfo.Unk01 = br.ReadByte();
                    RotationInfo.Rotation = QuaternionExtensions.ReadFromFile(br);
                    RotationInfo.Unk03 = br.ReadSingle();
                    RotationData[i] = RotationInfo;
                }
            }

            public void WriteToFile(BinaryWriter bw)
            {
                bw.Write(Unk0);
                bw.Write(RotationData.Length);

                foreach(QuaternionData Info in RotationData)
                {
                    bw.Write(Info.KeyFrameStart);
                    bw.Write(Info.KeyFrameEnd);
                    bw.Write(Info.Unk0);
                    bw.Write(Info.KeyType);
                    bw.Write(Info.Unk01);
                    Info.Rotation.WriteToFile(bw);
                    bw.Write(Info.Unk03);
                }
            }

            public override string ToString()
            {
                return string.Format("Num Rotations: {0}", NumRotations);
            }

        }

        public uint Unk0 { get; set; }
        public QuaternionWrapper RotationWrapper0 { get; set; }
        public QuaternionWrapper RotationWrapper1 { get; set; }
        public QuaternionWrapper RotationWrapper2 { get; set; }
        public ushort Unk1 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);
            Unk0 = br.ReadUInt32();

            RotationWrapper0 = new QuaternionWrapper();
            RotationWrapper0.ReadFromFile(br);
            RotationWrapper1 = new QuaternionWrapper();
            RotationWrapper1.ReadFromFile(br);
            RotationWrapper2 = new QuaternionWrapper();
            RotationWrapper2.ReadFromFile(br);

            Unk1 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Unk0);

            RotationWrapper0.WriteToFile(bw);
            RotationWrapper1.WriteToFile(bw);
            RotationWrapper2.WriteToFile(bw);

            bw.Write(Unk1);
        }
    }
}
