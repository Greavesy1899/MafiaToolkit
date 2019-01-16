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

namespace Gibbed.Mafia2.FileFormats
{
    internal static class TEA
    {
        public static void Decrypt(uint[] v, uint[] keys, uint sum, uint delta, uint rounds)
        {
            uint v0 = v[0];
            uint v1 = v[1];

            for (uint i = 0; i < rounds; i++)
            {
                v1 -= ((v0 << 4) + keys[2]) ^ (v0 + sum) ^ ((v0 >> 5) + keys[3]);
                v0 -= ((v1 << 4) + keys[0]) ^ (v1 + sum) ^ ((v1 >> 5) + keys[1]);
                sum -= delta;
            }

            v[0] = v0;
            v[1] = v1;
        }

        public static void Decrypt(byte[] data, int offset, int count, uint[] keys, uint sum, uint delta, uint rounds)
        {
            for (int i = offset; i + 8 <= offset + count; i += 8)
            {
                UInt32[] v = new UInt32[2];
                v[0] = BitConverter.ToUInt32(data, i + 0);
                v[1] = BitConverter.ToUInt32(data, i + 4);

                Decrypt(v, keys, sum, delta, rounds);

                Array.Copy(BitConverter.GetBytes(v[0]), 0, data, i + 0, 4);
                Array.Copy(BitConverter.GetBytes(v[1]), 0, data, i + 4, 4);
            }
        }
    }
}
