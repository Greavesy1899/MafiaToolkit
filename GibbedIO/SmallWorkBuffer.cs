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
using System.IO;
using System.Threading;

namespace Gibbed.IO
{
    internal static class SmallWorkBuffer
    {
        public const int BufferSize = 8;

        private static readonly ThreadLocal<byte[]> _SmallWorkBuffer =
            new ThreadLocal<byte[]>(() => new byte[BufferSize]);

        public static byte[] Get(int count)
        {
            if (count < 0 || count > BufferSize)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return _SmallWorkBuffer.Value;
        }

        public static byte[] ReadBytes(Stream stream, int count)
        {
            if (count < 0 || count > BufferSize)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            var buffer = _SmallWorkBuffer.Value;
            var read = stream.Read(buffer, 0, count);
            if (read != count)
            {
                throw new EndOfStreamException();
            }
            return buffer;
        }
    }
}
