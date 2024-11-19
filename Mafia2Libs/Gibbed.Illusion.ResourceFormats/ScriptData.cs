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
using Gibbed.Illusion.FileFormats;
using Gibbed.IO;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class ScriptData : IResourceType
    {
        public ulong NameHash;
        public ulong DataHash;
        public string Name;
        public byte[] Data;

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            if (version >= 2)
            {
                this.NameHash = Illusion.FileFormats.Hashing.FNV64.Hash(this.Name);
                this.DataHash = Illusion.FileFormats.Hashing.FNV64.Hash(this.Data, 0, this.Data.Length);
                output.WriteValueU64(this.NameHash, endian);
                output.WriteValueU64(this.DataHash, endian);
            }

            output.WriteStringU16(this.Name, endian);
            output.WriteValueS32(this.Data.Length, endian);
            output.WriteBytes(this.Data);
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            if (version >= 2)
            {
                this.NameHash = input.ReadValueU64(endian);
                this.DataHash = input.ReadValueU64(endian);
            }

            this.Name = input.ReadStringU16(endian);
            var size = input.ReadValueU32(endian);
            this.Data = input.ReadBytes((int)size);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
