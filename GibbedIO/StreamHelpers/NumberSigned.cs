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

using System.IO;

namespace Gibbed.IO
{
    public static partial class StreamHelpers
    {
        #region S8
        public static sbyte ReadValueS8(this Stream stream)
        {
            return (sbyte)stream.ReadValueU8();
        }

        public static void WriteValueS8(this Stream stream, sbyte value)
        {
            stream.WriteValueU8((byte)value);
        }
        #endregion
        #region S16
        public static short ReadValueS16(this Stream stream, Endian endian)
        {
            return (short)stream.ReadValueU16(endian);
        }

        public static short ReadValueS16(this Stream stream)
        {
            return (short)stream.ReadValueU16();
        }

        public static void WriteValueS16(this Stream stream, short value, Endian endian)
        {
            stream.WriteValueU16((ushort)value, endian);
        }

        public static void WriteValueS16(this Stream stream, short value)
        {
            stream.WriteValueU16((ushort)value);
        }
        #endregion
        #region S32
        public static int ReadValueS32(this Stream stream, Endian endian)
        {
            return (int)stream.ReadValueU32(endian);
        }

        public static int ReadValueS32(this Stream stream)
        {
            return (int)stream.ReadValueU32();
        }

        public static void WriteValueS32(this Stream stream, int value, Endian endian)
        {
            stream.WriteValueU32((uint)value, endian);
        }

        public static void WriteValueS32(this Stream stream, int value)
        {
            stream.WriteValueU32((uint)value);
        }
        #endregion
        #region S64
        public static long ReadValueS64(this Stream stream, Endian endian)
        {
            return (long)stream.ReadValueU64(endian);
        }

        public static long ReadValueS64(this Stream stream)
        {
            return (long)stream.ReadValueU64(Endian.Little);
        }

        public static void WriteValueS64(this Stream stream, long value, Endian endian)
        {
            stream.WriteValueU64((ulong)value, endian);
        }

        public static void WriteValueS64(this Stream stream, long value)
        {
            stream.WriteValueU64((ulong)value);
        }
        #endregion
    }
}
