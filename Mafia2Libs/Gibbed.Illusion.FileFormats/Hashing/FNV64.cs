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

using System;
using System.Text;

namespace Gibbed.Illusion.FileFormats.Hashing
{
    public static class FNV64
    {
        public const ulong Initial = 0xCBF29CE484222325;

        public static ulong Hash(string value)
        {
            return Hash(value, Encoding.GetEncoding(1252));
        }

        public static ulong Hash(string value, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (value == null)
            {
                return Hash(null, 0, 0);
            }

            var bytes = encoding.GetBytes(value);
            return Hash(bytes, 0, bytes.Length);
        }

        public static ulong Hash(byte[] buffer, int offset, int count)
        {
            return Hash(buffer, offset, count, Initial);
        }

        public static ulong Hash(byte[] buffer, int offset, int count, ulong hash)
        {
            if (buffer == null)
            {
                return hash;
            }

            for (int i = offset; i < offset + count; i++)
            {
                hash *= 0x00000100000001B3;
                hash ^= buffer[i];
            }

            return hash;
        }
    }
}
