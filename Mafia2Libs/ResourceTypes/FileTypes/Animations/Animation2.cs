using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.Animation2
{
    // Probably not the correct name, but saw it in mac
    public class AnimQuantized
    {
        private ulong BoneID;
        private byte Flags;

        private byte DataFlags;
        private ushort NumFrames;
        private byte ComponentSize; // in bits
        private byte TimeSize; // in bits

        // Could be compression? X Y Z?
        private float unk8;
        private float unk9;
        private float Duration;

        private byte[] AnimQuantizedRotations;
        private Quaternion[] AnimData;

        // Only if Flags & 2 is valid
        private float unk11;
        private byte[] AnimQuantizedPositions; // When writing, do = [Array.Length / 12]

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
                    NumFrames = reader.ReadUInt16(); // 0x14061c302
                    ComponentSize = reader.ReadByte(); // 0x14061c31d
                    TimeSize = reader.ReadByte(); // 0x14061c33f
                    unk8 = reader.ReadSingle(); // not a float
                    unk9 = reader.ReadSingle(); // relates to rotations
                    Duration = reader.ReadSingle(); // anim duration

                    // Somehow magically get the size from unk5, unk6 and unk7.
                    int Size = GetSize();
                    AnimQuantizedRotations = reader.ReadBytes(Size);
                    Dequantize();

                    // An extra bit of data which seems to include even more data.
                    // I'm going to assume that this could be Vector3?
                    if ((DataFlags & 2) == 2)
                    {
                        short NumEntries = reader.ReadInt16();
                        unk11 = reader.ReadSingle(); //compression? Seems quite large though..
                        AnimQuantizedPositions = reader.ReadBytes(12 * NumEntries);
                    }
                }
            }
        }

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

        private void Dequantize()
        {
            var data = new BigInteger(AnimQuantizedRotations);
            var quats = new List<Quaternion>();
            var chunkSize = 3 * ComponentSize + TimeSize + 2;

            for (var i = 0; i < NumFrames; i++)
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

            AnimData = quats.ToArray();
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
    }

    public class Animation2Loader
    {
        private class UnknownEntry0
        {
            public uint Unk0;
            public float Unk1;
        }

        private int animSetID; //usually in the name. Different types. If not using skeleton, it's 0xFFFF
        private ushort unk0;
        private ushort unk1;
        private Vector4 unk2;
        private Vector3 unk3;
        private Vector3 unk4;
        private ulong hash;
        private byte unk5;
        private float Duration;

        private ushort unk7;
        private byte unk8;

        private UnknownEntry0[] UnknownEntries;

        private ushort Unk13;
        private ushort Unk14;
        private ushort Unk15;

        private ulong unk9_hash;
        private AnimQuantized unk9_entry;

        private AnimQuantized[] Entries;

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

            unk0 = reader.ReadUInt16();
            unk1 = reader.ReadUInt16();
            unk2 = Vector4Extenders.ReadFromFile(reader); // Could be Quaternion
            unk3 = Vector3Utils.ReadFromFile(reader); // Position or Scale?
            unk4 = Vector3Utils.ReadFromFile(reader); // Position or Scale?
            hash = reader.ReadUInt64();
            unk5 = reader.ReadByte();
            Duration = reader.ReadSingle();

            unk7 = reader.ReadUInt16(); // Could be same as unk15
            unk8 = reader.ReadByte();

            unk9_hash = reader.ReadUInt64();

            // IDA mentions if this is 0, then return false
            byte bIsDataPresent = reader.ReadByte();
            if(bIsDataPresent == 0)
            {
                // insert fail message
                return false;
            }

            Debug.Assert(bIsDataPresent == 1, "Expected bIsDataPresent to be 1, got something else.");

            // Only read if unk0 is more than 0. (In theory we don't need this best to stick it here)
            if (unk0 > 0)
            {
                UnknownEntries = new UnknownEntry0[unk0];
                for (int i = 0; i < unk0; i++)
                {
                    UnknownEntry0 NewEntry = new UnknownEntry0();
                    NewEntry.Unk0 = reader.ReadUInt32();
                    NewEntry.Unk1 = reader.ReadSingle();
                    UnknownEntries[i] = NewEntry;
                }
            }

            // This could actually be a loop based on the number stored in unk8. Best to keep an eye on it.
            Unk13 = reader.ReadUInt16();
            Unk14 = reader.ReadUInt16();
            Unk15 = reader.ReadUInt16(); // Could be same as unk7
            Debug.Assert(unk8 == 3, "These three could actually be a loop or a set.. IDA pseudo code references unk8.");

            // This data only seems to be present if unk9_entry is set to something other than 0.
            unk9_entry = new AnimQuantized();
            if (unk9_hash != 0)
            {
                unk9_entry.ReadFromFile(reader);
            }

            // Iterate through all entries
            Entries = new AnimQuantized[Unk15];
            for (int i = 0; i < Unk15; i++)
            {
                AnimQuantized TestObject = new AnimQuantized();
                TestObject.ReadFromFile(reader);
                Entries[i] = TestObject;
            }

            // not sure.. But seems to work?
            int Total = Unk14 + Unk15;
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
