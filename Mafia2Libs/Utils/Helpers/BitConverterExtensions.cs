using System;

namespace Mafia2Tool.Utils.Helpers
{
    public static class BitConverterExtensions
    {
        public static int ToInt32(byte[] value)
            => value.Length switch
            {
                1 => BitConverter.ToChar(value),
                2 => BitConverter.ToInt16(value),
                3 => (int)BitConverter.ToInt16(value) | (BitConverter.ToChar(value, 2) << 16),
                _ => BitConverter.ToInt32(value),
            };

        public static float ToSingle(byte[] value)
            => BitConverter.Int32BitsToSingle(ToInt32(value));
    }
}
