using BitStreams;
using System;

namespace ResourceTypes.Prefab
{
    public static class PrefabUtils
    {
        public static ulong[] ReadHashArray(BitStream MemStream)
        {
            uint ArrayLength = MemStream.ReadUInt32();
            ulong[] HashArray = new ulong[ArrayLength];
            for(uint i = 0; i < ArrayLength; i++)
            {
                HashArray[i] = MemStream.ReadUInt64();
            }

            return HashArray;
        }

        public static void WriteHashArray(BitStream MemStream, ulong[] Hashes)
        {
            MemStream.WriteUInt32((uint)Hashes.Length);
            foreach(ulong Value in Hashes)
            {
                MemStream.WriteUInt64(Value);
            }
        }
    }

    public static class BitStreamUtils
    {
        public static float ReadSingle(this BitStream MemStream)
        {
            // TODO: Could be read 4 bytes??

            // Read int32, convert to float
            int Value = MemStream.ReadInt32();
            byte[] AsBytes = BitConverter.GetBytes(Value);
            float ConvertedValue = BitConverter.ToSingle(AsBytes, 0);

            return ConvertedValue;
        }

        public static void WriteSingle(this BitStream MemStream, float Value)
        {
            byte[] AsBytes = BitConverter.GetBytes(Value);
            int ConvertedValue = BitConverter.ToInt32(AsBytes, 0);

            MemStream.WriteInt32(ConvertedValue);
        }

        public static string ReadString32(this BitStream MemStream)
        {
            int StringSize = MemStream.ReadInt32();
            return MemStream.ReadString(StringSize);
        }

        public static void WriteString32(this BitStream MemStream, string Text)
        {
            MemStream.WriteInt32(Text.Length);
            MemStream.WriteString(Text);
        }

        public static void EndByte(this BitStream MemStream)
        {
            if (MemStream.BitPosition != 0)
            {
                int RemainingBits = (int)(8 - MemStream.BitPosition);
                MemStream.WriteBits(PopulateBitArray(RemainingBits), RemainingBits);
            }
        }

        public static Bit[] GetRemainingBytesFromBit(this BitStream MemStream)
        {
            if (MemStream.BitPosition != 0)
            {
                int RemainingBits = (int)(8 - MemStream.BitPosition);
                return MemStream.ReadBits(RemainingBits);
            }

            return null;
        }

        private static Bit[] PopulateBitArray(int Length)
        {
            Bit[] NewArray = new Bit[Length];
            for(int i = 0; i < Length; i++)
            {
                NewArray[i] = 0xFF;
            }

            return NewArray;
        }
    }
}
