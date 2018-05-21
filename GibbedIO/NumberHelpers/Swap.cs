/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

namespace Gibbed.IO
{
    public static partial class NumberHelpers
    {
        public static short Swap(this short value)
        {
            return (short)((ushort)value).Swap();
        }

        public static ushort Swap(this ushort value)
        {
            return (ushort)((0x00FFu) & (value >> 8) |
                            (0xFF00u) & (value << 8));
        }

        public static int Swap(this int value)
        {
            return (int)((uint)value).Swap();
        }

        public static uint Swap(this uint value)
        {
            return ((0x000000FFu) & (value >> 24) |
                    (0x0000FF00u) & (value >> 8) |
                    (0x00FF0000u) & (value << 8) |
                    (0xFF000000u) & (value << 24));
        }

        public static long Swap(this long value)
        {
            return (long)((ulong)value).Swap();
        }

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

        public static float Swap(this float value)
        {
            var overlap = new OverlapSingle(value);
            overlap.AsU = overlap.AsU.Swap();
            return overlap.AsF;
        }

        public static double Swap(this double value)
        {
            var overlap = new OverlapDouble(value);
            overlap.AsU = overlap.AsU.Swap();
            return overlap.AsD;
        }
    }
}
