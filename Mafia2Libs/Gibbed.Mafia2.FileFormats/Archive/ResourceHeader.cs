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
using Gibbed.IO;

namespace Gibbed.Mafia2.FileFormats.Archive
{
    internal struct ResourceHeader
    {
        public uint TypeId;
        public uint Size; // includes this header
        public ushort Version;
        public uint SlotRamRequired;
        public uint SlotVramRequired;
        public uint OtherRamRequired;
        public uint OtherVramRequired;
        public ushort Unk01;
        public uint Unk02;
        public ushort Unk03;

        public static ResourceHeader Read(Stream input, Endian endian, uint version)
        {
            ResourceHeader instance;
            instance.TypeId = input.ReadValueU32(endian);
            instance.Size = input.ReadValueU32(endian);
            instance.Version = input.ReadValueU16(endian);
            instance.SlotRamRequired = input.ReadValueU32(endian);
            instance.SlotVramRequired = input.ReadValueU32(endian);
            instance.OtherRamRequired = input.ReadValueU32(endian);
            instance.OtherVramRequired = input.ReadValueU32(endian);

            if (version == 20)
            {
                instance.Unk01 = input.ReadValueU16(endian);
                instance.Unk02 = input.ReadValueU32(endian);
                instance.Unk03 = input.ReadValueU16(endian);
            } else
            {
                instance.Unk01 = 0;
                instance.Unk02 = 0;
                instance.Unk03 = 0;
            }
            return instance;
        }

        public void Write(Stream output, Endian endian, uint version)
        {
            output.WriteValueU32(this.TypeId, endian);
            output.WriteValueU32(this.Size, endian);
            output.WriteValueU16(this.Version, endian);
            output.WriteValueU32(this.SlotRamRequired, endian);
            output.WriteValueU32(this.SlotVramRequired, endian);
            output.WriteValueU32(this.OtherRamRequired, endian);
            output.WriteValueU32(this.OtherVramRequired, endian);

            if (version == 20)
            {
                output.WriteValueU16(this.Unk01, endian);
                output.WriteValueU32(this.Unk02, endian);
                output.WriteValueU32(this.Unk03, endian);
            }
        }
    }
}
