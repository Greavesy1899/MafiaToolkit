using System;
using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class Functions
    {
        public static Random RandomGenerator = new Random();
        public static Vector3 ReadBound(BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
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
