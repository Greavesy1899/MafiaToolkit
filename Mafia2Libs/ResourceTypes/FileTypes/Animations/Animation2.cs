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
        private ushort unk5;
        private byte unk6;
        private byte unk7;

        // Could be compression? X Y Z?
        private float unk8;
        private float unk9;
        private float unk10;

        private byte[] AnimQuantizedData;
        private Quaternion[] AnimData;

        // Only if Flags & 2 is valid
        private float unk11;
        private byte[] AnimQuantizedData_Flag2; // When writing, do = [Array.Length / 12]

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
                    unk5 = reader.ReadUInt16(); // 0x14061c302
                    unk6 = reader.ReadByte(); // 0x14061c31d
                    unk7 = reader.ReadByte(); // 0x14061c33f
                    unk8 = reader.ReadSingle(); // compression x?
                    unk9 = reader.ReadSingle(); // compression y?
                    unk10 = reader.ReadSingle(); // compression z?

                    // Somehow magically get the size from unk5, unk6 and unk7.
                    int Size = GetSize();
                    AnimQuantizedData = reader.ReadBytes(Size);
                    Dequantize();

                    // An extra bit of data which seems to include even more data.
                    // I'm going to assume that this could be Vector3?
                    if ((DataFlags & 2) == 2)
                    {
                        short NumEntries = reader.ReadInt16();
                        unk11 = reader.ReadSingle(); //compression? Seems quite large though..
                        AnimQuantizedData_Flag2 = reader.ReadBytes(12 * NumEntries);
                    }
                }
            }
        }

        // See code in engine at sub_14061C260 (M2DE EXE)
        // Might be 
        private int GetSize()
        {
            int Var0 = unk5; // (v7 + 8); // 0x14061c302
            int Var1 = unk6; // v25 // 0x14061c31d
            int Var2 = unk7; // v26 // 0x14061c33f

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
            var data = new BigInteger(AnimQuantizedData);
            var quats = new List<Quaternion>();
            var chunkSize = 3 * unk6 + unk7 + 2;

            for (var i = 0; i < unk5; i++)
            {
                var dataCurrent = data >> (i * chunkSize);

                var w = (dataCurrent >> (unk7)) & ((1 << unk7) - 1);
                var x = (dataCurrent >> (unk7 + unk6)) & ((1 << unk6) - 1);
                var y = (dataCurrent >> (unk7 + unk6 * 2)) & ((1 << unk6) - 1);
                var z = (dataCurrent >> (unk7 + unk6 * 3)) & ((1 << unk6) - 1);
                var bits = (dataCurrent >> (unk7 + unk6 * 3 + 2)) & ((1 << 2) - 1);

                var xNorm = (uint)x / (float)((1 << unk6) - 1);
                var yNorm = (uint)y / (float)((1 << unk6) - 1);
                var zNorm = (uint)z / (float)((1 << unk6) - 1);
                var wNorm = (uint)w / (float)((1 << unk7) - 1);

                quats.Add(new Quaternion(xNorm, yNorm, zNorm, wNorm));
            }

            AnimData = quats.ToArray();
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
        private float unk6;

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
            unk6 = reader.ReadSingle();

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
