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
        #region F32
        public static float ReadValueF32(this Stream stream, Endian endian)
        {
            var value = stream.ReadValueU32(endian);
            return new OverlapSingle(value).AsF;
        }

        public static float ReadValueF32(this Stream stream)
        {
            return stream.ReadValueF32(Endian.Little);
        }

        public static void WriteValueF32(this Stream stream, float value, Endian endian)
        {
            var rawValue = new OverlapSingle(value).AsU;
            stream.WriteValueU32(rawValue, endian);
        }

        public static void WriteValueF32(this Stream stream, float value)
        {
            stream.WriteValueF32(value, Endian.Little);
        }
        #endregion
        #region F64
        public static double ReadValueF64(this Stream stream, Endian endian)
        {
            var value = stream.ReadValueU64(endian);
            return new OverlapDouble(value).AsD;
        }

        public static double ReadValueF64(this Stream stream)
        {
            return stream.ReadValueF64(Endian.Little);
        }

        public static void WriteValueF64(this Stream stream, double value, Endian endian)
        {
            var rawValue = new OverlapDouble(value).AsU;
            stream.WriteValueU64(rawValue, endian);
        }

        public static void WriteValueF64(this Stream stream, double value)
        {
            stream.WriteValueF64(value, Endian.Little);
        }
        #endregion
    }
}
