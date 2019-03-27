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
    public struct FileHeader
    {
        public uint ResourceTypeTableOffset;
        public uint BlockTableOffset;
        public uint XmlOffset;
        public uint SlotRamRequired;
        public uint SlotVramRequired;
        public uint OtherRamRequired;
        public uint OtherVramRequired;
        public uint Flags;
        public byte[] Unknown20;
        public uint ResourceCount;

        public void Write(Stream output, Endian endian)
        {
            output.WriteValueU32(this.ResourceTypeTableOffset, endian);
            output.WriteValueU32(this.BlockTableOffset, endian);
            output.WriteValueU32(this.XmlOffset, endian);
            output.WriteValueU32(this.SlotRamRequired, endian);
            output.WriteValueU32(this.SlotVramRequired, endian);
            output.WriteValueU32(this.OtherRamRequired, endian);
            output.WriteValueU32(this.OtherVramRequired, endian);
            output.WriteValueU32(this.Flags, endian);
            output.Write(this.Unknown20, 0, this.Unknown20.Length);
            output.WriteValueU32(this.ResourceCount, endian);
        }

        public static FileHeader Read(Stream input, Endian endian)
        {
            FileHeader instance;
            instance.ResourceTypeTableOffset = input.ReadValueU32(endian);
            instance.BlockTableOffset = input.ReadValueU32(endian);
            instance.XmlOffset = input.ReadValueU32(endian);
            instance.SlotRamRequired = input.ReadValueU32(endian);
            instance.SlotVramRequired = input.ReadValueU32(endian);
            instance.OtherRamRequired = input.ReadValueU32(endian);
            instance.OtherVramRequired = input.ReadValueU32(endian);
            instance.Flags = input.ReadValueU32(endian);
            instance.Unknown20 = input.ReadBytes(16);
            instance.ResourceCount = input.ReadValueU32(endian);
            return instance;
        }
    }
}
