using System;
using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class Functions
    {
        public static Random RandomGenerator = new Random();

        public static int IndexOfValue(Dictionary<int, object> dic, int key)
        {
            int index = -1;
            foreach (KeyValuePair<int, object> entry in dic)
            {
                if (entry.Key == key)
                    return index;
                else
                    index++;                 
            }
            return index;
        }
        public static string ReadString8(BinaryReader reader)
        {
            byte size = reader.ReadByte();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString16(BinaryReader reader)
        {
            short size = reader.ReadInt16();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString32(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString64(BinaryReader reader)
        {
            long size = reader.ReadInt64();
            return new string(reader.ReadChars((int)size));
        }
        public static string ReadString(BinaryReader reader)
        {
            string newString = "";

            while (reader.PeekChar() != '\0')
            {
                newString += reader.ReadChar();
            }
            reader.ReadByte();
            return newString;
        }
        public static void WriteString(BinaryWriter writer, string text)
        {
            writer.Write(text.ToCharArray());
            writer.Write('\0');
        }
        public static void WriteString32(BinaryWriter writer, string text)
        {
            writer.Write(text.Length);
            writer.Write(text.ToCharArray());
        }
    }
    public static class Extensions
    {
        public static int IndexOfValue<Tkey, TValue>(this Dictionary<Tkey, TValue> dic, int key)
        {
            int index = 0;
            foreach (KeyValuePair<Tkey, TValue> entry in dic)
            {
                if (Convert.ToInt32(entry.Key) == key)
                    return index;
                else
                    index++;
            }
            return -1;
        }
    }

    public static partial class FunctionSwap
    {
        public static ulong Swap(this ulong value)
        {
            return ((0x00000000000000FFu) & (value >> 56) |
                    (0x000000000000FF00u) & (value >> 40) |
                    (0x0000000000FF0000u) & (value >> 24) |
                    (0x00000000FF000000u) & (value >> 8) |
                    (0x000000FF00000000u) & (value << 8) |
                    (0x0000FF0000000000u) & (value << 24) |
                    (0x00FF000000000000u) & (value << 40) |
                    (0xFF00000000000000u) & (value << 56));
        }
    }
}
