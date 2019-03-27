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
using Gibbed.IO;
using Mafia2;
using Utils.Logging;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class TextureResource : IResourceType
    {
        public ulong NameHash;
        public byte Unknown8;
        public byte HasMIP;
        public byte[] Data;

        public TextureResource()
        {
        }

        public TextureResource(ulong hash, byte hasMIP, byte[] data)
        {
            NameHash = hash;
            Unknown8 = 0;
            HasMIP = hasMIP;
            Data = data;
        }

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            output.WriteValueU64(this.NameHash, endian);
            output.WriteValueU8(this.Unknown8);
            if (version == 2)
            {
                output.WriteValueU8(this.HasMIP);
            }
            output.WriteBytes(this.Data);
            Log.WriteLine("Packing: " + ToString());
        }

        public void SerializeMIP(ushort version, Stream output, Endian endian)
        {
            output.WriteValueU64(this.NameHash, endian);
            output.WriteValueU8(this.HasMIP);
            output.WriteBytes(this.Data);
            Log.WriteLine("Packing: " + ToString());
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            this.NameHash = input.ReadValueU64(endian);
            this.Unknown8 = input.ReadValueU8();
            this.HasMIP = input.ReadValueU8();

            //if (this.HasMIP != 0 && this.HasMIP != 1)
            //{
            //    throw new InvalidOperationException();
            //}

            this.Data = input.ReadBytes((int)(input.Length - input.Position));
            Log.WriteLine("Unpacking: " + ToString());
        }

        public void DeserializeMIP(ushort version, Stream input, Endian endian)
        {
            this.NameHash = input.ReadValueU64(endian);
            this.Unknown8 = input.ReadValueU8();

            if (this.HasMIP != 0 && this.HasMIP != 1)
            {
                throw new InvalidOperationException();
            }

            this.Data = input.ReadBytes((int)(input.Length - input.Position));
            Log.WriteLine("Unpacking: " + ToString());
        }

        public override string ToString()
        {
            return string.Format("Hash: {0}, Unk1: {1}, HasMIP: {2}", NameHash, Unknown8, HasMIP);
        }
    }
}
