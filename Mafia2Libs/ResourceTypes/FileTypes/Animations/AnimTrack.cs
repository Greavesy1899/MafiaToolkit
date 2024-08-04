using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils.Extensions;
using Utils.VorticeUtils;
using ZLibNet;

namespace ResourceTypes.Animation2
{
    public class AnimTrack
    {
        public Utils.Models.SkeletonBoneIDs BoneID { get; set; }
        public byte Flags { get; set; }
        public bool IsDataPresent { get; set; }
        public byte DataFlags { get; set; }
        public short NumKeyFrames { get; set; }
        public byte ComponentSize { get; set; }
        public byte TimeSize { get; set; }
        public uint PackedReferenceQuat { get; set; }
        public float Scale { get; set; }
        public float Duration { get; set; }
        public byte[] KeyFrameData { get; set; }
        public (float time, Quaternion value)[] KeyFrames { get; set; } = new (float time, Quaternion value)[0];
        public float Unk00 { get; set; }
        public UnkDataBlock[] UnkData { get; set; } = new UnkDataBlock[0];
        public AnimTrack()
        {

        }

        public AnimTrack(Stream s)
        {
            Read(s);
        }

        public AnimTrack(BinaryReader br)
        {
            Read(br);
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            BoneID = (Utils.Models.SkeletonBoneIDs)br.ReadUInt64();
            Flags = br.ReadByte();
            IsDataPresent = br.ReadBoolean();
            DataFlags = br.ReadByte();
            NumKeyFrames = br.ReadInt16();
            ComponentSize = br.ReadByte();
            TimeSize = br.ReadByte();
            PackedReferenceQuat = br.ReadUInt32();
            Scale = br.ReadSingle();
            Duration = br.ReadSingle();
            KeyFrameData = br.ReadBytes(GetKeyframeDataSize(NumKeyFrames, ComponentSize, TimeSize));

            if ((Flags & 0x01) == 1)
            {
                short Count = br.ReadInt16();
                Unk00 = br.ReadSingle();
                UnkData = new UnkDataBlock[Count];

                for (int i = 0; i < UnkData.Length; i++)
                {
                    UnkData[i] = new(br);
                }
            }

            Dequantize();

            DumpTrackData();
        }

        public int GetKeyframeSize(int ComponentSize, int TimeSize)
        {
            // Perform the calculation and apply the bitwise AND operation
            return (3 * ComponentSize + TimeSize + 2) & 0x7F;
        }

        public int GetKeyframeDataSize(int NumRotationFrames, int ComponentSize, int TimeSize)
        {
            // Perform the calculation and apply the bitwise AND operation
            return 4 + 4 * (NumRotationFrames * GetKeyframeSize(ComponentSize, TimeSize) / 32);
        }

        private void Dequantize() //Code by RoadTrain
        {
            var data = new BigInteger(KeyFrameData);
            var quats = new List<(float time, Quaternion value)>();
            var chunkSize = 3 * ComponentSize + TimeSize + 2;

            if (ComponentSize != 10)
            {
                return;
            }

            var refQuat = UnpackQuaternion32(PackedReferenceQuat);

            for (var i = 0; i < NumKeyFrames; i++)
            {
                var dataCurrent = data >> (i * chunkSize);

                var time = (((int)((dataCurrent) & ((1 << TimeSize) - 1))) / (float)((1 << TimeSize) - 1)) * Duration;
                var rawData = (uint)((dataCurrent >> (TimeSize)) & 0xFFFFFFFF);

                quats.Add((time, UnpackQuaternion32(rawData) / refQuat));
            }

            KeyFrames = quats.ToArray();
        }

        private Quaternion UnpackQuaternion32(uint rawData)
        {
            double fRatio = 0.001382418;

            uint iValue0 = (uint)((rawData >> 20) & 0x3FF);
            uint iValue1 = (uint)((rawData >> 10) & 0x3FF);
            uint iValue2 = (uint)((rawData >> 0) & 0x3FF);

            double fValue0 = (iValue0 - 511.5) * fRatio;
            double fValue1 = (iValue1 - 511.5) * fRatio;
            double fValue2 = (iValue2 - 511.5) * fRatio;

            double sum = 1.0 - (fValue0 * fValue0 - fValue1 * fValue1 - fValue2 * fValue2);
            double reciproq = 1.0 / Math.Sqrt(sum);
            double fValue3 = sum * reciproq;

            Quaternion q = new(0, 0, 0, 1);

            switch ((rawData >> 30) & 3)
            {
                case 0:
                    q = new((float)fValue3, (float)fValue0, (float)fValue1, (float)fValue2);
                    break;

                case 1:
                    q = new((float)fValue0, (float)fValue3, (float)fValue1, (float)fValue2);
                    break;

                case 2:
                    q = new((float)fValue0, (float)fValue1, (float)fValue3, (float)fValue2);
                    break;

                case 3:
                    q = new((float)fValue0, (float)fValue1, (float)fValue2, (float)fValue3);
                    break;

                default:
                    throw new NotSupportedException();
            }

            return q;
        }

        private void Quantize()
        {
            List<byte> data = new();
            ComponentSize = 16;
            TimeSize = 14;

            foreach (var keyFrame in KeyFrames)
            {
                float maxValue = 65535.0f;
                float maxTimeValue = 16383.0f;
                ulong quantizedData = ((ulong)((keyFrame.time * maxTimeValue) + (maxTimeValue / 2))) & 0x3FFF;
                List<float> values = new() { keyFrame.value.X, keyFrame.value.Y, keyFrame.value.Z, keyFrame.value.W };
                var omittedComponent = ((ulong)(values.IndexOf(values.Max()) & 0x03)) << 62;

                values.Remove(values.Max());

                var component1 = ((ulong)((values[0] * maxValue) + (maxValue / 2))) << 14;
                var component2 = ((ulong)((values[1] * maxValue) + (maxValue / 2))) << 30;
                var component3 = ((ulong)((values[2] * maxValue) + (maxValue / 2))) << 46;

                quantizedData = quantizedData | component1 | component2 | component3 | omittedComponent;
                data.AddRange(BitConverter.GetBytes(quantizedData));
            }

            KeyFrameData = data.ToArray();
        }

        private static float Normalize(int value, int size)
        {
            var maxValue = (float)((1 << size) - 1);
            return (value - maxValue / 2) / (maxValue);
        }

        private void DumpTrackData()
        {
            string folderPath = "%userprofile%\\Desktop\\AnimTracks";
            string path = Environment.ExpandEnvironmentVariables(folderPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_{FNV32.Hash(KeyFrameData, 0, KeyFrameData.Length)}.bin"), KeyFrameData);

            using (MemoryStream ms = new())
            {
                ms.Write(KeyFrames.Length, false);

                foreach (var val in KeyFrames)
                {
                    ms.Write(val.time, false);
                    val.value.WriteToFile(ms, false);
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_Decompressed.bin"), data);
            }

            using (MemoryStream ms = new())
            {
                using (StreamWriter sw = new(ms))
                {
                    foreach (var val in KeyFrames)
                    {
                        sw.Write(val.time.ToString("0.000000"));
                        sw.Write("|");
                        sw.Write(val.value.X.ToString("0.000000"));
                        sw.Write(",");
                        sw.Write(val.value.Y.ToString("0.000000"));
                        sw.Write(",");
                        sw.Write(val.value.Z.ToString("0.000000"));
                        sw.Write(",");
                        sw.Write(val.value.W.ToString("0.000000"));
                        sw.Write("\n");
                    }
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_Decompressed.txt"), data);
            }
        }
    }

    public class UnkDataBlock
    {
        public short BoneIndex { get; set; }
        public byte[] Data { get; set; } = new byte[10];

        public UnkDataBlock()
        {

        }

        public UnkDataBlock(Stream s)
        {
            Read(s);
        }

        public UnkDataBlock(BinaryReader br)
        {
            Read(br);
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }
        
        public void Read(BinaryReader br)
        {
            BoneIndex = br.ReadInt16();
            Data = br.ReadBytes(10);
        }
    }
}
