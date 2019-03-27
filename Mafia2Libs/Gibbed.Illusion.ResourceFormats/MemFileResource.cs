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

//THIS VERSION IS MODIFIED.
//SEE ORIGINAL CODE HERE::
//https://github.com/gibbed/Gibbed.Illusion
using System;
using System.IO;
using Gibbed.Illusion.FileFormats;
using Gibbed.IO;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class MemFileResource : IResourceType
    {
        public string Name;
        public uint Unk1;
        public byte[] Data;

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            output.WriteStringU32(this.Name, endian);
            output.WriteValueU32(this.Unk1, endian);
            output.WriteValueS32(this.Data.Length, endian);
            output.WriteBytes(this.Data);
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            this.Name = input.ReadStringU32(endian);
            this.Unk1 = input.ReadValueU32(endian);
            if (this.Unk1 != 1)
            {
                throw new InvalidOperationException();
            }
            uint size = input.ReadValueU32(endian);
            this.Data = input.ReadBytes((int)size);
        }

        /// <summary>
        /// Deserialize resource using passed bytes.
        /// </summary>
        /// <param name="data"></param>
        public void Deserialize(byte[] data, Endian endian)
        {
            MemoryStream input = new MemoryStream(data);

            this.Name = input.ReadStringU32(endian);
            this.Unk1 = input.ReadValueU32(endian);
            if (this.Unk1 != 1)
            {
                throw new InvalidOperationException();
            }
            uint size = input.ReadValueU32(endian);
            this.Data = input.ReadBytes((int)size);
        }
    }
}
