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
using System.Text;
using Gibbed.IO;

namespace Gibbed.Illusion.FileFormats
{
    public static class StreamHelpers
    {
        static StreamHelpers()
        {
            IO.StreamHelpers.DefaultEncoding = Encoding.GetEncoding(1252);
        }

        public static MemoryStream ReadToMemoryStreamSafe(this Stream stream, long size, Endian endian)
        {
            var output = new MemoryStream();

            uint computedHash = Hashing.FNV32.Initial;
            long remaining = size;
            byte[] buffer = new byte[4096];
            while (remaining > 0)
            {
                int block = (int)(Math.Min(remaining, 4096));
                var read = stream.Read(buffer, 0, block);
                if (read != block)
                {
                    throw new EndOfStreamException();
                }
                computedHash = Hashing.FNV32.Hash(buffer, 0, block);
                output.Write(buffer, 0, block);
                remaining -= block;
            }

            var hash = stream.ReadValueU32(endian);
            if (hash != computedHash)
            {
                //throw new InvalidDataException(string.Format("hash failure ({0:X} vs {1:X})",
                //                                             computedHash,
                //                                             hash));
            }

            output.Position = 0;
            return output;
        }

        public static void WriteFromMemoryStreamSafe(this Stream stream, MemoryStream input, Endian endian)
        {
            var position = input.Position;
            input.Position = 0;
            var buffer = input.GetBuffer();
            var length = (int)input.Length;
            var computedHash = Hashing.FNV32.Hash(buffer, 0, length);
            stream.Write(buffer, 0, length);
            stream.WriteValueU32(computedHash, endian);
            input.Position = position;
        }

        public static string ReadStringU16(this Stream stream, Endian endian)
        {
            var length = stream.ReadValueU16(endian);
            if (length > 0x3FF)
            {
                throw new InvalidOperationException();
            }
            return stream.ReadString(length);
        }

        public static void WriteStringU16(this Stream stream, string value, Endian endian)
        {
            ushort length = (ushort)value.Length;
            stream.WriteValueU16(length, endian);
            stream.WriteString(length == value.Length ? value : value.Substring(0, length));
        }

        public static string ReadStringU32(this Stream stream, Endian endian)
        {
            var length = stream.ReadValueU32(endian);
            if (length > 0x3FF)
            {
                return "unknown";
                //throw new InvalidOperationException();
            }
            return stream.ReadString(length);
        }

        public static void WriteStringU32(this Stream stream, string value, Endian endian)
        {
            stream.WriteValueS32(value.Length, endian);
            stream.WriteString(value);
        }
    }
}
