using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;

using BitStreams;
using Mafia2Tool.Utils.Helpers;
using Utils.VorticeUtils;

namespace ResourceTypes.Animation2
{
    public struct RotationKeyframe
    {
        public float Time;
        public Quaternion Rotation;
    }

    public struct PositionKeyframe
    {
        public float Time;
        public Vector3 Position;
    }

    // Probably not the correct name, but saw it in mac
    public class AnimQuantized
    {
        private ulong BoneID;
        private byte Flags;

        private byte DataFlags;
        private ushort NumRotationFrames;
        private byte ComponentSize; // in bits
        private byte TimeSize; // in bits

        private uint PackedReferenceQuat;
        private float RotationScale;
        private float Duration;

        private byte[] QuantizedRotationKeyframes;
        private RotationKeyframe[] RotationKeyframes;

        // Only if Flags & 2 is valid
        private ushort NumPositionFrames;
        private float PositionScale;
        private byte[] QuantizedPositionKeyframes; // When writing, do = [Array.Length / 12]
        private PositionKeyframe[] PositionKeyframes;

        public void ReadFromFile(BinaryReader reader)
        {
            BoneID = reader.ReadUInt64();

            // Seems like flags - used later too.
            Flags = reader.ReadByte();
            if ((Flags & 32) == 32)
            {
                byte bIsDataPresent = reader.ReadByte(); // Not sure if correct.
                if (bIsDataPresent != 0)
                {
                    DataFlags = reader.ReadByte();
                    NumRotationFrames = reader.ReadUInt16(); // 0x14061c302
                    ComponentSize = reader.ReadByte(); // 0x14061c31d
                    TimeSize = reader.ReadByte(); // 0x14061c33f
                    PackedReferenceQuat = reader.ReadUInt32();
                    RotationScale = reader.ReadSingle(); // relates to rotations
                    Duration = reader.ReadSingle(); // anim duration

                    if (ComponentSize > 32)
                        throw new InvalidDataException("Rotation component size must be 32 bits or less!");

                    if (TimeSize > 32)
                        throw new InvalidDataException("Rotation time size must be 32 bits or less!");

                    // Somehow magically get the size from unk5, unk6 and unk7.
                    var size = GetRotationKeyframeDataSize();
                    QuantizedRotationKeyframes = reader.ReadBytes((int)size);
                    DecodeRotationKeyframes();

                    // An extra bit of data which seems to include even more data.
                    // I'm going to assume that this could be Vector3?
                    if ((DataFlags & 2) == 2)
                    {
                        NumPositionFrames = reader.ReadUInt16();
                        PositionScale = reader.ReadSingle(); //compression? Seems quite large though..
                        QuantizedPositionKeyframes = reader.ReadBytes(12 * NumPositionFrames);
                        DecodePositionKeyframes();
                    }
                }
            }
        }

        private uint GetRotationKeyframeSize()
            => (uint)((3 * ComponentSize + TimeSize + 2) & 0x7F);

        private uint GetRotationKeyframeDataSize()
            // aligned to uint
            => 4 + 4 * (NumRotationFrames * GetRotationKeyframeSize() / 32);

#if false
        // See code in engine at sub_14061C260 (M2DE EXE)
        // Might be 
        private int GetSize()
        {
            int Var0 = NumFrames; // (v7 + 8); // 0x14061c302
            int Var1 = ComponentSize; // v25 // 0x14061c31d
            int Var2 = TimeSize; // v26 // 0x14061c33f

            int v7_10 = (0 ^ 32 * Var1) & 2016; // 6 middle bits
            var v15 = v7_10 ^ (v7_10 ^ Var2) & 31; // 5 lower bits
            var v16 = v15 ^ (v15 ^ ((3 * Var1 + Var2 + 2) << 11)) & 260096; // 7 higher bits

            // Speculation is that 'unk6' represents bits per coord, and 'unk7' represents bits per something else (w component?)
            var sizeSimplified = 4 * (Var0 * ((3 * Var1 + Var2 + 2) & 0x7F) >> 5) + 4;
            var size = 4 * (Var0 * ((v16 >> 11) & 0x7F) >> 5) + 4;

            Debug.Assert(size == sizeSimplified);

            return size;

            /**(v7 + 10) ^= (*(v7 + 10) ^ 32 * v25) & 2016;
            v15 = *(v7 + 10) ^ (*(v7 + 10) ^ v26) & 31;
            *(v7 + 10) = v15;
            v16 = v15 ^ (v15 ^ ((3 * v25 + v26 + 2) << 11)) & 260096;
            *(v7 + 10) = v16;
            v17 = 4 * (*(v7 + 8) * ((v16 >> 11) & 0x7F) >> 5) + 4;
            v18 = (*(*v4 + 8i64))(v4, v17, 1i64);
            *v7 = v18;*/
        }
#endif

        private static Quaternion UnpackQuaternion(uint packedQuat)
        {
            const float scale = 0.001382418f;
            var comp1f = (float)(packedQuat >> 22);
            var comp2f = (float)((packedQuat >> 12) & 0x3ff);
            var comp3f = (float)((packedQuat >> 2) & 0x3ff);
            var comp1 = (comp1f - 511.5f) * scale;
            var comp2 = (comp2f - 511.5f) * scale;
            var comp3 = (comp3f - 511.5f) * scale;
            var comp4 = (float)Math.Sqrt(1f - comp1 * comp1 - comp2 * comp2 - comp3 * comp3);
            var result = (packedQuat & 3) switch
            {
                1 => new Quaternion(comp1, comp4, comp2, comp3),
                2 => new Quaternion(comp1, comp2, comp4, comp3),
                3 => new Quaternion(comp1, comp2, comp3, comp4),
                _ => new Quaternion(comp4, comp1, comp2, comp3),
            };
            return result;
        }

        private void DecodeRotationKeyframes()
        {
            var signMask = 1 << (ComponentSize - 1);
            var invSignMask = ~signMask;
            var signShift = 32 - ComponentSize;
            var maxComponentRecp = 1f / ((1u << (ComponentSize - 1)) - 1);
            var keyframeSize = (int)GetRotationKeyframeSize();
            var refQuat = UnpackQuaternion(PackedReferenceQuat);
            var stream = BitStream.Create(QuantizedRotationKeyframes);

            var keyframes = new List<RotationKeyframe>(NumRotationFrames);

            for (ushort i = 0; i < NumRotationFrames; ++i)
            {
                stream.Seek(0, i * keyframeSize);
                var time = stream.ReadSingleUnorm(TimeSize) * Duration;

                float comp1;
                {
                    var compBytes = stream.ReadBytes(ComponentSize);
                    var compi = BitConverter.ToInt32(compBytes);
                    var compAbs = BitConverter.Int32BitsToSingle(compi & invSignMask) * maxComponentRecp * RotationScale;
                    var compSign = (compi & signMask) << signShift;
                    compi = BitConverter.SingleToInt32Bits(compAbs) | compSign;
                    comp1 = BitConverter.Int32BitsToSingle(compi);
                }

                float comp2;
                {
                    var compBytes = stream.ReadBytes(ComponentSize);
                    var compi = BitConverter.ToInt32(compBytes);
                    var compAbs = BitConverter.Int32BitsToSingle(compi & invSignMask) * maxComponentRecp * RotationScale;
                    var compSign = (compi & signMask) << signShift;
                    compi = BitConverter.SingleToInt32Bits(compAbs) | compSign;
                    comp2 = BitConverter.Int32BitsToSingle(compi);
                }

                float comp3;
                {
                    var compBytes = stream.ReadBytes(ComponentSize);
                    var compi = BitConverter.ToInt32(compBytes);
                    var compAbs = BitConverter.Int32BitsToSingle(compi & invSignMask) * maxComponentRecp * RotationScale;
                    var compSign = (compi & signMask) << signShift;
                    compi = BitConverter.SingleToInt32Bits(compAbs) | compSign;
                    comp3 = BitConverter.Int32BitsToSingle(compi);
                }

                var comp4 = (float)Math.Sqrt(1f - comp1 * comp1 - comp2 * comp2 - comp3 * comp3);
                var selectorBytes = stream.ReadBytes(2);
                var selector = BitConverter.ToInt32(selectorBytes);

                var quat = selector switch
                {
                    1 => new Quaternion(comp1, comp4, comp2, comp3),
                    2 => new Quaternion(comp1, comp2, comp4, comp3),
                    3 => new Quaternion(comp1, comp2, comp3, comp4),
                    _ => new Quaternion(comp4, comp1, comp2, comp3),
                };

                keyframes.Add(
                    new RotationKeyframe
                    {
                        Time = time,
                        Rotation = quat * refQuat,
                    }
                );
            }

            RotationKeyframes = keyframes.ToArray();
        }

        private void DecodePositionKeyframes()
        {
            const int positionKeyframeSize = 12;
            const float maxTime = 28800f;
            const uint maxTimeQuantized = 0xffffffu;
            const float baseScale = 0.00000011920929f;

            var scale = baseScale * PositionScale;
            var keyframes = new List<PositionKeyframe>(NumPositionFrames);

            for (ushort i = 0; i < NumPositionFrames; ++i)
            {
                var keyframeOffset = i * positionKeyframeSize;
                var timeBytes = QuantizedPositionKeyframes[keyframeOffset..(keyframeOffset + 2)];
                var timeQuantized = BitConverter.ToUInt32(timeBytes);
                var time = (timeQuantized / (float)maxTimeQuantized) * maxTime;

                var posxBytes = QuantizedPositionKeyframes[(keyframeOffset + 3)..(keyframeOffset + 5)];
                var posxSign = posxBytes[2] & 0x80;
                posxBytes[2] &= 0x7f;

                var posyBytes = QuantizedPositionKeyframes[(keyframeOffset + 6)..(keyframeOffset + 8)];
                var posySign = posyBytes[2] & 0x80;
                posyBytes[2] &= 0x7f;

                var poszBytes = QuantizedPositionKeyframes[(keyframeOffset + 9)..(keyframeOffset + 11)];
                var poszSign = poszBytes[2] & 0x80;
                poszBytes[2] &= 0x7f;

                var posxabs = BitConverter.ToSingle(posxBytes) * scale;
                var posxi = BitConverter.SingleToInt32Bits(posxabs) | (posxSign << 24);
                var posx = BitConverter.Int32BitsToSingle(posxi);

                var posyabs = BitConverter.ToSingle(posyBytes) * scale;
                var posyi = BitConverter.SingleToInt32Bits(posyabs) | (posySign << 24);
                var posy = BitConverter.Int32BitsToSingle(posyi);

                var poszabs = BitConverter.ToSingle(poszBytes) * scale;
                var poszi = BitConverter.SingleToInt32Bits(poszabs) | (poszSign << 24);
                var posz = BitConverter.Int32BitsToSingle(poszi);

                keyframes.Add(
                    new PositionKeyframe
                    {
                        Time = time,
                        Position = new Vector3(posx, posy, posz),
                    }
                );
            }

            PositionKeyframes = keyframes.ToArray();
        }

#if false
        private void DequantizeRotationKeyframes()
        {
            var data = new BigInteger(QuantizedRotationKeyframes);
            var quats = new List<Quaternion>();
            var chunkSize = 3 * ComponentSize + TimeSize + 2;

            for (var i = 0; i < NumRotationFrames; i++)
            {
                var dataCurrent = data >> (i * chunkSize);

                var time = (dataCurrent) & ((1 << TimeSize) - 1);
                var component1 = (dataCurrent >> (TimeSize)) & ((1 << ComponentSize) - 1);
                var component2 = (dataCurrent >> (TimeSize + ComponentSize)) & ((1 << ComponentSize) - 1);
                var component3 = (dataCurrent >> (TimeSize + ComponentSize * 2)) & ((1 << ComponentSize) - 1);
                var omittedComponent = (dataCurrent >> (TimeSize + ComponentSize * 2 + 2)) & ((1 << 2) - 1);

                float x;
                float y;
                float z;
                float w;
                switch ((int)omittedComponent)
                {
                    case 0: // x omitted
                        y = Normalize((int)component1, ComponentSize);
                        z = Normalize((int)component2, ComponentSize);
                        w = Normalize((int)component3, ComponentSize);
                        x = (float)Math.Sqrt(1 - y * y - z * z - w * w);
                        break;
                    case 1: // y omitted
                        x = Normalize((int)component1, ComponentSize);
                        z = Normalize((int)component2, ComponentSize);
                        w = Normalize((int)component3, ComponentSize);
                        y = (float)Math.Sqrt(1 - x * x - z * z - w * w);
                        break;
                    case 2: // z omitted
                        x = Normalize((int)component1, ComponentSize);
                        y = Normalize((int)component2, ComponentSize);
                        w = Normalize((int)component3, ComponentSize);
                        z = (float)Math.Sqrt(1 - x * x - y * y - w * w);
                        break;
                    case 3: // w omitted
                        x = Normalize((int)component1, ComponentSize);
                        y = Normalize((int)component2, ComponentSize);
                        z = Normalize((int)component3, ComponentSize);
                        w = (float)Math.Sqrt(1 - x * x - y * y - z * z);
                        break;
                    default:
                        throw new Exception();
                }

                var quat = new Quaternion(x, y, z, w);
                var euler = quat.ToEuler();

                quats.Add(quat);
            }

            RotationKeyframes = quats.ToArray();
        }

        private static float Normalize(int value, int size)
        {
            var maxValue = (float)((1 << size) - 1);
            return (value - maxValue / 2) / (maxValue);
        }

        private static float Normalize2(int value, int size)
        {
            var maxValue = (float)((1 << size - 1) - 1);
            return ((((1 << size - 1) & value) > 0) ? -1 : 1) * (value & ((1 << size - 1) - 1)) / maxValue;
        }
#endif
    }

    public class Animation2Loader
    {
        private class AnimationNotification
        {
            public uint Event;
            public float Time;
        }

        private int animSetID; //usually in the name. Different types. If not using skeleton, it's 0xFFFF
        private ushort NumNotifications;
        private ushort unk1;
        private Vector4 unk2;
        private Vector3 unk3;
        private Vector3 unk4;
        private ulong hash;
        private byte unk5;
        private float Duration;

        private ushort unk7;
        private byte unk8;

        private AnimationNotification[] Notifications;

        private ushort Unk13;
        private ushort Unk14;
        private ushort NumTracks;

        private ulong RootBoneId;
        private AnimQuantized RootBoneTrack;

        private AnimQuantized[] Tracks;

        private ushort[] TailData;

        public bool ReadFromFile(BinaryReader reader)
        {
            animSetID = reader.ReadInt32();
            
            // Make sure the header is valid (magic and version)
            if(!ValidateHeader(reader))
            {
                // insert fail message
                return false;
            }

            NumNotifications = reader.ReadUInt16();
            unk1 = reader.ReadUInt16();
            unk2 = Vector4Extenders.ReadFromFile(reader); // Could be Quaternion
            unk3 = Vector3Utils.ReadFromFile(reader); // Position or Scale?
            unk4 = Vector3Utils.ReadFromFile(reader); // Position or Scale?
            hash = reader.ReadUInt64();
            unk5 = reader.ReadByte();
            Duration = reader.ReadSingle();

            unk7 = reader.ReadUInt16(); // Could be same as unk15
            unk8 = reader.ReadByte();

            RootBoneId = reader.ReadUInt64();

            // IDA mentions if this is 0, then return false
            byte bIsDataPresent = reader.ReadByte();
            if(bIsDataPresent == 0)
            {
                // insert fail message
                return false;
            }

            Debug.Assert(bIsDataPresent == 1, "Expected bIsDataPresent to be 1, got something else.");

            // Only read if unk0 is more than 0. (In theory we don't need this best to stick it here)
            if (NumNotifications > 0)
            {
                Notifications = new AnimationNotification[NumNotifications];
                for (int i = 0; i < NumNotifications; i++)
                {
                    AnimationNotification notification = new AnimationNotification();
                    notification.Event = reader.ReadUInt32();
                    notification.Time = reader.ReadSingle();
                    Notifications[i] = notification;
                }
            }

            // This could actually be a loop based on the number stored in unk8. Best to keep an eye on it.
            Unk13 = reader.ReadUInt16();
            Unk14 = reader.ReadUInt16();
            NumTracks = reader.ReadUInt16(); // Could be same as unk7
            Debug.Assert(unk8 == 3, "These three could actually be a loop or a set.. IDA pseudo code references unk8.");

            // This data only seems to be present if unk9_entry is set to something other than 0.
            RootBoneTrack = new AnimQuantized();
            if (RootBoneId != 0)
            {
                RootBoneTrack.ReadFromFile(reader);
            }

            // Iterate through all entries
            Tracks = new AnimQuantized[NumTracks];
            for (int i = 0; i < NumTracks; i++)
            {
                AnimQuantized track = new AnimQuantized();
                track.ReadFromFile(reader);
                Tracks[i] = track;
            }

            // not sure.. But seems to work?
            int Total = Unk14 + NumTracks;
            TailData = new ushort[Total];
            for (int i = 0; i < Total; i++)
            {
                TailData[i] = reader.ReadUInt16();
            }

            Debug.Assert(reader.BaseStream.Position == reader.BaseStream.Length, "boo! didn't hit the end");

            return true;
        }

        private bool ValidateHeader(BinaryReader reader)
        {
            byte version = reader.ReadByte();
            uint magic = reader.ReadUInt32();

            return version == 2 && magic == 0xFA5612BC;
        }
    }
}
