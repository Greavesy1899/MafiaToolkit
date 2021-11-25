using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
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

            public void ReadFromFile(MemoryStream stream, bool isBigEndian)
            {
                Unk0 = stream.ReadUInt32(isBigEndian);
                NumRotations = stream.ReadUInt32(isBigEndian);
                RotationData = new QuaternionData[NumRotations];

                for(int i = 0; i < NumRotations; i++)
                {
                    QuaternionData RotationInfo = new QuaternionData();
                    RotationInfo.KeyFrameStart = stream.ReadUInt32(isBigEndian);
                    RotationInfo.KeyFrameEnd = stream.ReadUInt32(isBigEndian);
                    RotationInfo.Unk0 = stream.ReadUInt16(isBigEndian);
                    RotationInfo.KeyType = stream.ReadUInt16(isBigEndian);
                    RotationInfo.Unk01 = stream.ReadByte8();
                    RotationInfo.Rotation = QuaternionExtensions.ReadFromFile(stream, isBigEndian);
                    RotationInfo.Unk03 = stream.ReadSingle(isBigEndian);
                    RotationData[i] = RotationInfo;
                }
            }

            public void WriteToFile(MemoryStream stream, bool isBigEndian)
            {
                stream.Write(Unk0, isBigEndian);
                stream.Write(RotationData.Length, isBigEndian);

                foreach(QuaternionData Info in RotationData)
                {
                    stream.Write(Info.KeyFrameStart, isBigEndian);
                    stream.Write(Info.KeyFrameEnd, isBigEndian);
                    stream.Write(Info.Unk0, isBigEndian);
                    stream.Write(Info.KeyType, isBigEndian);
                    stream.WriteByte(Info.Unk01);
                    Info.Rotation.WriteToFile(stream, isBigEndian);
                    stream.Write(Info.Unk03, isBigEndian);
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

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            Unk0 = stream.ReadUInt32(isBigEndian);

            RotationWrapper0 = new QuaternionWrapper();
            RotationWrapper0.ReadFromFile(stream, isBigEndian);
            RotationWrapper1 = new QuaternionWrapper();
            RotationWrapper1.ReadFromFile(stream, isBigEndian);
            RotationWrapper2 = new QuaternionWrapper();
            RotationWrapper2.ReadFromFile(stream, isBigEndian);

            Unk1 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);
            stream.Write(Unk0, isBigEndian);

            RotationWrapper0.WriteToFile(stream, isBigEndian);
            RotationWrapper1.WriteToFile(stream, isBigEndian);
            RotationWrapper2.WriteToFile(stream, isBigEndian);

            stream.Write(Unk1, isBigEndian);
        }
    }
}
