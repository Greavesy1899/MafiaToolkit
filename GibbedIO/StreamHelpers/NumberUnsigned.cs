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

namespace Gibbed.IO
{
    public static partial class StreamHelpers
    {
        #region U8
        public static byte ReadValueU8(this Stream stream)
        {
            return SmallWorkBuffer.ReadBytes(stream, 1)[0];
        }

        public static void WriteValueU8(this Stream stream, byte value)
        {
            var data = SmallWorkBuffer.Get(1);
            data[0] = value;
            stream.Write(data, 0, 1);
        }
        #endregion
        #region U16
        public static ushort ReadValueU16(this Stream stream, Endian endian)
        {
            var data = SmallWorkBuffer.ReadBytes(stream, 2);
            var value = BitConverter.ToUInt16(data, 0);
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }
            return value;
        }

        public static ushort ReadValueU16(this Stream stream)
        {
            return stream.ReadValueU16(Endian.Little);
        }

        public static void WriteValueU16(this Stream stream, ushort value, Endian endian)
        {
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }
            var data = SmallWorkBuffer.Get(2);
            data[0] = (byte)((value & 0x00FFu) >> 0);
            data[1] = (byte)((value & 0xFF00u) >> 8);
            stream.Write(data, 0, 2);
        }

        public static void WriteValueU16(this Stream stream, ushort value)
        {
            stream.WriteValueU16(value, Endian.Little);
        }
        #endregion
        #region U32
        public static uint ReadValueU32(this Stream stream, Endian endian)
        {
            var data = SmallWorkBuffer.ReadBytes(stream, 4);
            var value = BitConverter.ToUInt32(data, 0);
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }
            return value;
        }

        public static uint ReadValueU32(this Stream stream)
        {
            return stream.ReadValueU32(Endian.Little);
        }

        public static void WriteValueU32(this Stream stream, uint value, Endian endian)
        {
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }
            var data = SmallWorkBuffer.Get(4);
            data[0] = (byte)((value & 0x000000FFu) >> 0);
            data[1] = (byte)((value & 0x0000FF00u) >> 8);
            data[2] = (byte)((value & 0x00FF0000u) >> 16);
            data[3] = (byte)((value & 0xFF000000u) >> 24);
            stream.Write(data, 0, 4);
        }

        public static void WriteValueU32(this Stream stream, uint value)
        {
            stream.WriteValueU32(value, Endian.Little);
        }
        #endregion
        #region U64
        public static ulong ReadValueU64(this Stream stream, Endian endian)
        {
            var data = SmallWorkBuffer.ReadBytes(stream, 8);
            var value = BitConverter.ToUInt64(data, 0);
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }
            return value;
        }

        public static ulong ReadValueU64(this Stream stream)
        {
            return stream.ReadValueU64(Endian.Little);
        }

        public static void WriteValueU64(this Stream stream, ulong value, Endian endian)
        {
            if (ShouldSwap(endian) == true)
            {
                value = value.Swap();
            }
            var data = SmallWorkBuffer.Get(8);
            var valuea = (uint)value;
            var valueb = (uint)(value >> 32);
            data[0] = (byte)((valuea & 0x000000FFu) >> 0);
            data[1] = (byte)((valuea & 0x0000FF00u) >> 8);
            data[2] = (byte)((valuea & 0x00FF0000u) >> 16);
            data[3] = (byte)((valuea & 0xFF000000u) >> 24);
            data[4] = (byte)((valueb & 0x000000FFu) >> 0);
            data[5] = (byte)((valueb & 0x0000FF00u) >> 8);
            data[6] = (byte)((valueb & 0x00FF0000u) >> 16);
            data[7] = (byte)((valueb & 0xFF000000u) >> 24);
            stream.Write(data, 0, 8);
        }

        public static void WriteValueU64(this Stream stream, ulong value)
        {
            stream.WriteValueU64(value, Endian.Little);
        }
        #endregion
    }
}
