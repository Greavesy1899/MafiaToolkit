using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils.Models;

namespace ResourceTypes.Animation2
{
    public class AnimTrack
    {
        public static byte TargetTimeSize = 22; //Max 31
        public static byte TargetComponentSize = 24; //Max 63
        public bool TrackDataChanged = true;
        public SkeletonBoneIDs BoneID { get; set; }
        public byte Flags { get; set; } = 0x23;
        public bool IsDataPresent { get; set; } = true;
        public byte DataFlags { get; set; } = 0x0B;
        public short NumKeyFrames { get; set; }
        public byte ComponentSize { get; set; } = TargetComponentSize;
        public byte TimeSize { get; set; } = TargetTimeSize;
        public uint PackedReferenceQuat { get; set; }
        public float Scale { get; set; } = 1.0f;
        public float Duration { get; set; }
        public byte[] KeyFrameData { get; set; } = new byte[0];
        public (float time, Quaternion value)[] KeyFrames { get; set; } = new (float time, Quaternion value)[0];
        public PositionData Positions { get; set; } = new();
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
            BoneID = (SkeletonBoneIDs)br.ReadUInt64();
            Flags = br.ReadByte();
            IsDataPresent = br.ReadBoolean();
            DataFlags = br.ReadByte();
            NumKeyFrames = br.ReadInt16();
            ComponentSize = br.ReadByte();
            TimeSize = br.ReadByte();
            PackedReferenceQuat = br.ReadUInt32();
            Scale = br.ReadSingle();
            Duration = br.ReadSingle();

            switch (Flags & 0x03)
            {
                case 1:
                    //Sample position
                    Positions = new(br);
                    break;

                case 2:
                    //Sample rotation
                    KeyFrameData = br.ReadBytes(GetKeyframeDataSize(NumKeyFrames, ComponentSize, TimeSize));
                    break;

                case 3:
                    //Sample rotation
                    KeyFrameData = br.ReadBytes(GetKeyframeDataSize(NumKeyFrames, ComponentSize, TimeSize));
                    //Sample position
                    Positions = new(br);
                    break;
            }

            Dequantize();

            TrackDataChanged = false;

            //DumpTrackData();
        }

        public void Write(BinaryWriter bw)
        {
            if (TrackDataChanged)
            {
                Quantize(Duration);
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
                Positions.Write(bw);
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
            if (KeyFrameData.Length == 0)
            {
                return;
            }

            var data = new BigInteger(KeyFrameData);
            var quats = new List<(float time, Quaternion value)>();
            var chunkSize = 3 * ComponentSize + TimeSize + 2;

            var refQuat = UnpackReferenceQuaternion(PackedReferenceQuat);

            for (var i = 0; i < NumKeyFrames; i++)
            {
                var dataCurrent = data >> (i * chunkSize);

                var time = (((int)((dataCurrent) & ((1 << TimeSize) - 1))) / (float)((1 << TimeSize) - 1)) * Duration;
                var rawData = dataCurrent >> TimeSize;

                quats.Add((time, Quaternion.Inverse(refQuat * UnpackQuaternion(ComponentSize, Scale, rawData))));
            }

            KeyFrames = quats.ToArray();
        }

        private void Quantize(float duration)
        {
            TimeSize = TargetTimeSize;
            ComponentSize = TargetComponentSize;
            Duration = duration;
            NumKeyFrames = (short)KeyFrames.Length;

            var refQuat = KeyFrames.Length > 0 ? Quaternion.Inverse(KeyFrames[0].value) : Quaternion.Identity;

            PackedReferenceQuat = PackReferenceQuaternion(refQuat);
            refQuat = UnpackReferenceQuaternion(PackedReferenceQuat);
            var invRefQuat = Quaternion.Inverse(refQuat);

            Scale = GetOptimalScale(invRefQuat);

            var chunkSize = 3 * ComponentSize + TimeSize + 2;
            var data = new BigInteger();

            for (int i = 0; i < KeyFrames.Length; i++)
            {
                var frame = KeyFrames[i];
                int timeMask = ((1 << TimeSize) - 1);

                int offset = chunkSize * i;
                int time = ((int)Math.Round((frame.time / Duration) * timeMask)) & timeMask;
                var packedQuaternion = PackQuaternion(ComponentSize, Scale, Quaternion.Inverse(frame.value), invRefQuat);
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

            KeyFrameData = keyFrameData;
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

        public (int omittedComponent, long iVal0, long iVal1, long iVal2) PackQuaternion(int componentSize, float scale, Quaternion q, Quaternion invRefQuat)
        {
            long mask = (1 << componentSize) - 1;
            long componentBitMask = 1 << (componentSize - 1);

            // We reunpack the Reference Quaternion and multiply the inverse with the keyframe quaternion to compensate for the precision loss
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

            long iValue0 = 0;
            long iValue1 = 0;
            long iValue2 = 0;
            long iSign0 = Math.Sign(values[0]) < 0 ? 1 << (componentSize - 1) : 0;
            long iSign1 = Math.Sign(values[1]) < 0 ? 1 << (componentSize - 1) : 0;
            long iSign2 = Math.Sign(values[2]) < 0 ? 1 << (componentSize - 1) : 0;
            long iSign3 = Math.Sign(values[3]) < 0 ? 1 << (componentSize - 1) : 0;

            var i0 = (((long)((Math.Abs(values[0]) / scale) * (componentBitMask - 1))) & mask) | iSign0;
            var i1 = (((long)((Math.Abs(values[1]) / scale) * (componentBitMask - 1))) & mask) | iSign1;
            var i2 = (((long)((Math.Abs(values[2]) / scale) * (componentBitMask - 1))) & mask) | iSign2;
            var i3 = (((long)((Math.Abs(values[3]) / scale) * (componentBitMask - 1))) & mask) | iSign3;

            switch (omittedComponent)
            {
                case 0:
                    iValue0 = i1;
                    iValue1 = i2;
                    iValue2 = i3;
                    break;

                case 1:
                    iValue0 = i0;
                    iValue1 = i2;
                    iValue2 = i3;
                    break;

                case 2:
                    iValue0 = i0;
                    iValue1 = i1;
                    iValue2 = i3;
                    break;

                case 3:
                    iValue0 = i0;
                    iValue1 = i1;
                    iValue2 = i2;
                    break;

                default:
                    throw new NotSupportedException();
            }

            return (omittedComponent, iValue0, iValue1, iValue2);
        }

        private static float Normalize(int value, int size)
        {
            var maxValue = (float)((1 << size) - 1);
            return (value - maxValue / 2) / (maxValue);
        }

        private float GetOptimalScale(Quaternion invRefQuat)
        {
            float scale = 0.0f;

            for (int i = 0; i < KeyFrames.Length; i++)
            {
                var frame = KeyFrames[i];
                var q = invRefQuat * Quaternion.Inverse(frame.value);
                List<float> values = new() { Math.Abs(q.X), Math.Abs(q.Y), Math.Abs(q.Z), Math.Abs(q.W) };
                values.Remove(values.Max());
                var temp = values.Max();
                scale = temp > scale ? temp : scale;
            }

            return scale;
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

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}.bin"), data);
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

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_Rotation_Decompressed.txt"), data);
            }

            using (MemoryStream ms = new())
            {
                using (StreamWriter sw = new(ms))
                {
                    foreach (var val in Positions.KeyFrames)
                    {
                        sw.Write(val.time.ToString("0.000000"));
                        sw.Write("|");
                        sw.Write(val.value.X.ToString("0.000000"));
                        sw.Write(",");
                        sw.Write(val.value.Y.ToString("0.000000"));
                        sw.Write(",");
                        sw.Write(val.value.Z.ToString("0.000000"));
                        sw.Write("\n");
                    }
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_Position_Decompressed.txt"), data);
            }

            Quantize(Duration);
            Dequantize();
            Positions.Quantize();
            Positions.Dequantize();

            using (MemoryStream ms = new())
            {
                using (BinaryWriter bw = new(ms))
                {
                    Write(bw);
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_2.bin"), data);
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

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_Rotation_Decompressed_2.txt"), data);
            }

            using (MemoryStream ms = new())
            {
                using (StreamWriter sw = new(ms))
                {
                    foreach (var val in Positions.KeyFrames)
                    {
                        sw.Write(val.time.ToString("0.000000"));
                        sw.Write("|");
                        sw.Write(val.value.X.ToString("0.000000"));
                        sw.Write(",");
                        sw.Write(val.value.Y.ToString("0.000000"));
                        sw.Write(",");
                        sw.Write(val.value.Z.ToString("0.000000"));
                        sw.Write("\n");
                    }
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_Position_Decompressed_2.txt"), data);
            }
        }
    }

    public class PositionData
    {
        public bool TrackDataChanged = true;
        public float Scale { get; set; }
        public byte[] Data { get; set; } = new byte[0];
        public (float time, Vector3 value)[] KeyFrames { get; set; } = new (float time, Vector3 value)[0];
        public PositionData()
        {

        }

        public PositionData(Stream s)
        {
            Read(s);
        }

        public PositionData(BinaryReader br)
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
            short Count = br.ReadInt16();
            Scale = br.ReadSingle();
            Data = br.ReadBytes(Count * 12);

            Dequantize();

            TrackDataChanged = false;
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((short)(Data.Length / 12));
            bw.Write(Scale);
            bw.Write(Data);
        }

        public void Dequantize()
        {
            if (Data.Length == 0)
            {
                return;
            }

            List<(float time, Vector3 value)> frames = new();
            var data = new BigInteger(Data);
            var chunkSize = 96;
            var QuantizationFactor = (2.0f / 16777215.0f) * Scale;

            for (int i = 0; i < Data.Length / 12; i++)
            {
                var dataCurrent = data >> (i * chunkSize);
                int time = (int)(dataCurrent & 0xFFFFFF);
                int iValue0 = (int)((dataCurrent >> 24) & 0xFFFFFF);
                int iValue1 = (int)((dataCurrent >> 48) & 0xFFFFFF);
                int iValue2 = (int)((dataCurrent >> 72) & 0xFFFFFF);
                int iSign0 = (int)((iValue0 & 0xFF800000) << 8);
                int iSign1 = (int)((iValue1 & 0xFF800000) << 8);
                int iSign2 = (int)((iValue2 & 0xFF800000) << 8);
                var fValue0 = ((int)(iValue0 & 0xFF7FFFFF) * QuantizationFactor);
                var fValue1 = ((int)(iValue1 & 0xFF7FFFFF) * QuantizationFactor);
                var fValue2 = ((int)(iValue2 & 0xFF7FFFFF) * QuantizationFactor);

                var X = BitConverter.ToSingle(BitConverter.GetBytes(iSign0 | BitConverter.ToInt32(BitConverter.GetBytes(fValue0), 0)), 0);
                var Y = BitConverter.ToSingle(BitConverter.GetBytes(iSign1 | BitConverter.ToInt32(BitConverter.GetBytes(fValue1), 0)), 0);
                var Z = BitConverter.ToSingle(BitConverter.GetBytes(iSign2 | BitConverter.ToInt32(BitConverter.GetBytes(fValue2), 0)), 0);

                frames.Add(((float)(time / (582.0 + 4.0 / 7.0)), new(X, Y, Z)));
            }

            KeyFrames = frames.ToArray();
        }

        public void Quantize()
        {
            Scale = GetOptimalScale();
            Scale = Scale > 0.0f ? Scale : 1.0f;

            var data = new BigInteger();
            var chunkSize = 96;
            var QuantizationFactor = 16777215.0f / (Scale * 2.0f);

            for (int i = 0; i < KeyFrames.Length; i++)
            {
                var KeyFrame = KeyFrames[i];

                var offset = i * chunkSize;

                long iSign0 = Math.Sign(KeyFrame.value.X) < 0 ? 1 << 23 : 0;
                long iSign1 = Math.Sign(KeyFrame.value.Y) < 0 ? 1 << 23 : 0;
                long iSign2 = Math.Sign(KeyFrame.value.Z) < 0 ? 1 << 23 : 0;

                var iTime = new BigInteger((uint)Math.Round(KeyFrame.time * (582.0 + 4.0 / 7.0)));
                var iValue0 = new BigInteger((((uint)Math.Round(Math.Abs(KeyFrame.value.X) * QuantizationFactor)) & 0x7FFFFF) | iSign0);
                var iValue1 = new BigInteger((((uint)Math.Round(Math.Abs(KeyFrame.value.Y) * QuantizationFactor)) & 0x7FFFFF) | iSign1);
                var iValue2 = new BigInteger((((uint)Math.Round(Math.Abs(KeyFrame.value.Z) * QuantizationFactor)) & 0x7FFFFF) | iSign2);

                data |= iTime << offset;
                data |= iValue0 << (offset + 24);
                data |= iValue1 << (offset + 48);
                data |= iValue2 << (offset + 72);
            }

            Data = data.ToByteArray();
        }

        public float GetOptimalScale()
        {
            float scale = 0.0f;

            for (int i = 0; i < KeyFrames.Length; i++)
            {
                var frame = KeyFrames[i];
                var v = frame.value;
                List<float> values = new() { Math.Abs(v.X), Math.Abs(v.Y), Math.Abs(v.Z) };
                var temp = values.Max();
                scale = temp > scale ? temp : scale;
            }

            return scale;
        }
    }
}
