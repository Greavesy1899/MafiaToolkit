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
using Gibbed.Illusion.FileFormats;
using Gibbed.IO;

namespace Gibbed.Mafia2.FileFormats.Archive
{
    public struct ResourceType : IEquatable<ResourceType>
    {
        public uint Id;
        public string Name;
        public uint Parent;

        public void Write(Stream output, Endian endian)
        {
            output.WriteValueU32(this.Id, endian);
            output.WriteStringU32(this.Name, endian);
            output.WriteValueU32(this.Parent, endian);
        }

        public static ResourceType Read(Stream input, Endian endian)
        {
            ResourceType instance;
            instance.Id = input.ReadValueU32(endian);
            instance.Name = input.ReadStringU32(endian);
            instance.Parent = input.ReadValueU32(endian);
            return instance;
        }

        public bool Equals(ResourceType other)
        {
            return this.Id == other.Id &&
                   string.Equals(this.Name, other.Name) == true &&
                   this.Parent == other.Parent;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj) == true)
            {
                return false;
            }
            return obj is ResourceType && Equals((ResourceType)obj) == true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)this.Id;
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)this.Parent;
                return hashCode;
            }
        }

        public static bool operator ==(ResourceType left, ResourceType right)
        {
            return left.Equals(right) == true;
        }

        public static bool operator !=(ResourceType left, ResourceType right)
        {
            return left.Equals(right) == false;
        }
    }
}
