using System;
using System.Text;
using System.IO;
using System.Globalization;

namespace Mafia2
{
    public class Functions
    {
        public static ulong Hash64(string value)
        {
            byte[] bytes = Encoding.GetEncoding(1252).GetBytes(value);
            return Hash64(bytes, 0, bytes.Length);
        }

        public static ulong Hash64(byte[] buffer, int offset, int count)
        {
            return Hash64(buffer, offset, count, 14695981039346656037UL);
        }

        public static ulong Hash64(byte[] buffer, int offset, int count, ulong hash)
        {
            if (buffer == null)
                throw new ArgumentNullException("input");
            for (int index = offset; index < offset + count; ++index)
            {
                hash *= 1099511628211UL;
                hash ^= buffer[index];
            }
            return hash;
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            byte[] numArray = new byte[hexString.Length / 2];
            for (int index = 0; index < numArray.Length; ++index)
            {
                string s = hexString.Substring(index * 2, 2);
                numArray[index] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return numArray;
        }

        public static Vector3 ReadBound(BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
