using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.VorticeUtils;

namespace ResourceTypes.Animation2
{
    public class AnimTrack
    {
        public static byte TargetTimeSize = 22; //Max 31
        public static byte TargetComponentSize = 24; //Max 63
        public static float TargetScale = 1.0f;
        public bool TrackDataChanged = true;
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

            TrackDataChanged = false;

            //DumpTrackData();
        }

        public void Write(BinaryWriter bw)
        {
            if (TrackDataChanged)
            {
                KeyFrameData = Quantize(Duration);
            }

            bw.Write((long)BoneID);
            bw.Write(Flags);
            bw.Write(IsDataPresent);
            bw.Write(DataFlags);
            bw.Write(NumKeyFrames);
            bw.Write(ComponentSize);
            bw.Write(TimeSize);
            bw.Write(PackedReferenceQuat);
            bw.Write(Scale);
            bw.Write(Duration);
            bw.Write(KeyFrameData);

            if ((Flags & 0x01) == 1)
            {
                bw.Write(UnkData.Length);
                bw.Write(Unk00);

                foreach (var item in UnkData)
                {
                    item.Write(bw);
                }
            }
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

        private void Dequantize()
        {
            var data = new BigInteger(KeyFrameData);
            var quats = new List<(float time, Quaternion value)>();
            var chunkSize = 3 * ComponentSize + TimeSize + 2;

            var refQuat = UnpackReferenceQuaternion(PackedReferenceQuat);

            for (var i = 0; i < NumKeyFrames; i++)
            {
                var dataCurrent = data >> (i * chunkSize);

                var time = (((int)((dataCurrent) & ((1 << TimeSize) - 1))) / (float)((1 << TimeSize) - 1)) * Duration;
                var rawData = dataCurrent >> TimeSize;

                quats.Add((time, refQuat * UnpackQuaternion(ComponentSize, Scale, rawData)));
            }

            KeyFrames = quats.ToArray();
        }

        private byte[] Quantize(float duration)
        {
            TimeSize = TargetTimeSize;
            ComponentSize = TargetComponentSize;
            Scale = TargetScale;
            Duration = duration;
            PackedReferenceQuat = PackReferenceQuaternion(Quaternion.Identity);
            var refQuat = UnpackReferenceQuaternion(PackedReferenceQuat);
            var invRefQuat = Quaternion.Inverse(refQuat);

            var chunkSize = 3 * ComponentSize + TimeSize + 2;
            var data = new BigInteger();

            for (int i = 0; i < KeyFrames.Length; i++)
            {
                var frame = KeyFrames[i];
                int timeMask = ((1 << TimeSize) - 1);

                int offset = chunkSize * i;
                int time = ((int)Math.Round((frame.time / Duration) * timeMask)) & timeMask;
                var packedQuaternion = PackQuaternion(ComponentSize, Scale, frame.value, invRefQuat);
                var bigTime = new BigInteger(time);
                var bigVal0 = new BigInteger(packedQuaternion.iVal0);
                var bigVal1 = new BigInteger(packedQuaternion.iVal1);
                var bigVal2 = new BigInteger(packedQuaternion.iVal2);
                var bigOmitted = new BigInteger(packedQuaternion.omittedComponent);
                bigTime <<= offset;
                bigVal0 <<= (offset + TimeSize + ComponentSize * 0);
                bigVal1 <<= (offset + TimeSize + ComponentSize * 1);
                bigVal2 <<= (offset + TimeSize + ComponentSize * 2);
                bigOmitted <<= (offset + TimeSize + ComponentSize * 3);

                data |= bigTime;
                data |= bigVal0;
                data |= bigVal1;
                data |= bigVal2;
                data |= bigOmitted;
            }

            var size = GetKeyframeDataSize(KeyFrames.Length, ComponentSize, TimeSize);
            var bytes = data.ToByteArray();
            
            if (size < bytes.Length)
            {
                throw new NotSupportedException();
            }

            byte[] keyFrameData = new byte[size];
            Array.Copy(bytes, 0, keyFrameData, 0, bytes.Length);

            return keyFrameData;
        }

        private Quaternion UnpackReferenceQuaternion(uint rawData)
        {
            double fRatio = Math.Sqrt(2.0) / 1023.0;

            double fValue0 = (((rawData >> 22) & 0x3FF) - 511.5) * fRatio;
            double fValue1 = (((rawData >> 12) & 0x3FF) - 511.5) * fRatio;
            double fValue2 = (((rawData >> 2) & 0x3FF) - 511.5) * fRatio;

            double sum = 1.0 - (fValue0 * fValue0 + fValue1 * fValue1 + fValue2 * fValue2);
            double reciproq = 1.0 / Math.Sqrt(sum);
            double fValue3 = sum * reciproq;

            Quaternion q = new(0, 0, 0, 1);

            switch ((rawData >> 0) & 3)
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

        public uint PackReferenceQuaternion(Quaternion q)
        {
            uint rawData = 0;

            float[] values = new float[4];
            values[0] = q.X;
            values[1] = q.Y;
            values[2] = q.Z;
            values[3] = q.W;

            int omittedComponent = 0;
            float maxValue = Math.Abs(values[0]);
            for (int i = 1; i < 4; i++)
            {
                if (Math.Abs(values[i]) > maxValue)
                {
                    maxValue = Math.Abs(values[i]);
                    omittedComponent = i;
                }
            }

            double fRatio = 1.0 / (Math.Sqrt(2.0) / 1023.0);

            int iX = (int)Math.Round((q.X * fRatio) + 511.5);
            int iY = (int)Math.Round((q.Y * fRatio) + 511.5);
            int iZ = (int)Math.Round((q.Z * fRatio) + 511.5);
            int iW = (int)Math.Round((q.W * fRatio) + 511.5);
            rawData = (uint)omittedComponent;

            uint iValue0 = 0;
            uint iValue1 = 0;
            uint iValue2 = 0;

            switch (omittedComponent)
            {
                case 0:
                    iValue0 = (uint)iY;
                    iValue1 = (uint)iZ;
                    iValue2 = (uint)iW;
                    break;

                case 1:
                    iValue0 = (uint)iX;
                    iValue1 = (uint)iZ;
                    iValue2 = (uint)iW;
                    break;

                case 2:
                    iValue0 = (uint)iX;
                    iValue1 = (uint)iY;
                    iValue2 = (uint)iW;
                    break;

                case 3:
                    iValue0 = (uint)iX;
                    iValue1 = (uint)iY;
                    iValue2 = (uint)iZ;
                    break;

                default:
                    throw new NotSupportedException();
            }

            iValue0 = (iValue0 & 0x3FF) << 22;
            iValue1 = (iValue1 & 0x3FF) << 12;
            iValue2 = (iValue2 & 0x3FF) << 2;

            return rawData | iValue0 | iValue1 | iValue2;
        }

        public Quaternion UnpackQuaternion(int componentSize, float scale, BigInteger rawData)
        {
            int mask = (1 << componentSize) - 1;
            int iValue0 = (int)((rawData >> componentSize * 0) & mask);
            int iValue1 = (int)((rawData >> componentSize * 1) & mask);
            int iValue2 = (int)((rawData >> componentSize * 2) & mask);
            int omittedComponent = (int)((rawData >> componentSize * 3) & 3);

            byte componentShift = (byte)(32 - componentSize);
            int componentBitMask = 1 << (componentSize - 1);

            float invMask = 1.0f / (float)(componentBitMask - 1);

            float v14 = (float)((iValue0 & ~componentBitMask) * invMask * scale);
            float v16 = (float)((iValue1 & ~componentBitMask) * invMask * scale);
            float v15 = (float)((iValue2 & ~componentBitMask) * invMask * scale);

            var i0 = ((iValue0 & componentBitMask) << componentShift);
            var i1 = ((iValue1 & componentBitMask) << componentShift);
            var i2 = ((iValue2 & componentBitMask) << componentShift);

            var i3 = BitConverter.ToInt32(BitConverter.GetBytes(v14), 0);
            var i4 = BitConverter.ToInt32(BitConverter.GetBytes(v16), 0);
            var i5 = BitConverter.ToInt32(BitConverter.GetBytes(v15), 0);

            var fValue0 = BitConverter.ToSingle(BitConverter.GetBytes(i0 | i3), 0);
            var fValue1 = BitConverter.ToSingle(BitConverter.GetBytes(i1 | i4), 0);
            var fValue2 = BitConverter.ToSingle(BitConverter.GetBytes(i2 | i5), 0);
            var fValue3 = (float)Math.Sqrt(1.0 - (fValue0 * fValue0 + fValue1 * fValue1 + fValue2 * fValue2));

            Quaternion q = new(0, 0, 0, 1);

            switch (omittedComponent)
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

        public (int omittedComponent, int iVal0, int iVal1, int iVal2) PackQuaternion(int componentSize, float scale, Quaternion q, Quaternion invRefQuat)
        {
            int mask = (1 << componentSize) - 1;
            int componentBitMask = 1 << (componentSize - 1);

            q = invRefQuat * q;

            float[] values = new float[4];
            values[0] = q.X;
            values[1] = q.Y;
            values[2] = q.Z;
            values[3] = q.W;

            int omittedComponent = 0;
            float maxValue = Math.Abs(values[0]);
            for (int i = 1; i < 4; i++)
            {
                if (Math.Abs(values[i]) > maxValue)
                {
                    maxValue = Math.Abs(values[i]);
                    omittedComponent = i;
                }
            }

            int iValue0 = 0;
            int iValue1 = 0;
            int iValue2 = 0;

            var i0 = ((int)((Math.Abs(values[0]) / scale) * (componentBitMask - 1))) + (Math.Sign(values[0]) < 0 ? componentBitMask : 0);
            var i1 = ((int)((Math.Abs(values[1]) / scale) * (componentBitMask - 1))) + (Math.Sign(values[1]) < 0 ? componentBitMask : 0);
            var i2 = ((int)((Math.Abs(values[2]) / scale) * (componentBitMask - 1))) + (Math.Sign(values[2]) < 0 ? componentBitMask : 0);
            var i3 = ((int)((Math.Abs(values[3]) / scale) * (componentBitMask - 1))) + (Math.Sign(values[3]) < 0 ? componentBitMask : 0);

            switch (omittedComponent)
            {
                case 0:
                    iValue0 = i1 & mask;
                    iValue1 = i2 & mask;
                    iValue2 = i3 & mask;
                    break;

                case 1:
                    iValue0 = i0 & mask;
                    iValue1 = i2 & mask;
                    iValue2 = i3 & mask;
                    break;

                case 2:
                    iValue0 = i0 & mask;
                    iValue1 = i1 & mask;
                    iValue2 = i3 & mask;
                    break;

                case 3:
                    iValue0 = i0 & mask;
                    iValue1 = i1 & mask;
                    iValue2 = i2 & mask;
                    break;

                default:
                    throw new NotSupportedException();
            }

            //long rawData = 0;
            //rawData |= (long)iValue0 & mask;
            //rawData |= ((long)iValue1 & mask) << componentSize;
            //rawData |= ((long)iValue2 & mask) << (componentSize * 2);
            //rawData |= ((long)omittedComponent & 3) << (componentSize * 3);

            return (omittedComponent, iValue0, iValue1, iValue2);
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

            using (MemoryStream ms = new())
            {
                using (BinaryWriter bw = new(ms))
                {
                    Write(bw);
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_{FNV32.Hash(data, 0, data.Length)}.bin"), data);
            }

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

            KeyFrameData = Quantize(Duration);
            Dequantize();

            using (MemoryStream ms = new())
            {
                using (BinaryWriter bw = new(ms))
                {
                    Write(bw);
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_{FNV32.Hash(data, 0, data.Length)}.bin"), data);
            }

            using (MemoryStream ms = new())
            {
                ms.Write(KeyFrames.Length, false);

                foreach (var val in KeyFrames)
                {
                    ms.Write(val.time, false);
                    val.value.WriteToFile(ms, false);
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_Decompressed2.bin"), data);
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

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_Decompressed2.txt"), data);
            }
        }
    }

    public class UnkDataBlock
    {
        public short BoneIndex { get; set; } //Time instead?
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

        public void Write(BinaryWriter bw)
        {
            bw.Write(BoneIndex);
            bw.Write(Data);
        }
    }
}
