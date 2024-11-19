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
using Gibbed.IO;

namespace Gibbed.Mafia2.FileFormats
{
    public static class ArchiveEncryption
    {
        static ArchiveEncryption()
        {
            _Keys = new uint[]
            {
                0x73766E46,
                0x6D454D5A,
                0x336A6D68,
                0x38425072,
            };

            _Settings = new[]
            {
                new Settings(0x79FB0B01, 0x4B989BCD, 5),
                new Settings(0xA62336C0, 0x9D3119B6, 32),
            };
        }

        private static readonly uint[] _Keys;
        private static Settings[] _Settings;

        public static MemoryStream Unwrap(Stream input)
        {
            var output = new MemoryStream();
            if (Unwrap(input, output) == true)
            {
                output.Position = 0;
                return output;
            }
            output.Dispose();
            return null;
        }

        public static bool Unwrap(Stream input, Stream output)
        {
            var basePosition = input.Position;
            if (basePosition + (0x90 + 15) > input.Length)
            {
                return false;
            }

            input.Position = basePosition + 0x90;
            var fsfh = input.ReadBytes(15);

            // "tables/fsfh.bin"
            if (Illusion.FileFormats.Hashing.FNV64.Hash(fsfh, 0, fsfh.Length) != 0x39DD22E69C74EC6F)
            {
                input.Position = basePosition;
                return false;
            }

            input.Seek(0x10000, SeekOrigin.Begin);
            var encryptedHeaderBytes = input.ReadBytes(16);
            var headerBytes = new byte[16];

            // figure out which TEA settings is correct
            foreach (var candidate in _Settings)
            {
                Array.Copy(encryptedHeaderBytes, headerBytes, 16);
                TEA.Decrypt(headerBytes, 0, 16, _Keys, candidate.Sum, candidate.Delta, candidate.Rounds);

                var magic = BitConverter.ToUInt32(headerBytes, 0).BigEndian();
                if (magic != ArchiveFile.Signature)
                {
                    continue;
                }

                var hash = BitConverter.ToUInt32(headerBytes, 12).LittleEndian();
                var computedHash = Illusion.FileFormats.Hashing.FNV32.Hash(headerBytes, 0, 12);
                if (hash != computedHash)
                {
                    continue;
                }

                input.Seek(0x10000, SeekOrigin.Begin);
                long remaining = input.Length - input.Position;
                byte[] data = new byte[0x4000];
                while (remaining > 0)
                {
                    int block = (int)(Math.Min(remaining, data.Length));
                    var read = input.Read(data, 0, block);
                    if (read != block)
                    {
                        throw new EndOfStreamException();
                    }
                    TEA.Decrypt(data, 0, block, _Keys, candidate.Sum, candidate.Delta, candidate.Rounds);
                    output.Write(data, 0, block);
                    remaining -= block;
                }
                return true;
            }

            return false;
        }

        private struct Settings
        {
            public uint Sum;
            public uint Delta;
            public uint Rounds;

            public Settings(uint sum, uint delta, uint rounds)
            {
                this.Sum = sum;
                this.Delta = delta;
                this.Rounds = rounds;
            }
        }
    }
}
