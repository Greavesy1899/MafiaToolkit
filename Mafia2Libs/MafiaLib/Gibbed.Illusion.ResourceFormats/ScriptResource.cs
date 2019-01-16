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

using System.Collections.Generic;
using System.IO;
using Gibbed.Illusion.FileFormats;
using Gibbed.IO;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class ScriptResource : IResourceType
    {
        public string Path;
        public List<ScriptData> Scripts = new List<ScriptData>();

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            output.WriteStringU16(this.Path, endian);
            output.WriteValueS32(this.Scripts.Count, endian);
            foreach (var script in this.Scripts)
            {
                script.Serialize(version, output, endian);
            }
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            this.Path = input.ReadStringU16(endian);
            var count = input.ReadValueU32(endian);
            this.Scripts.Clear();
            for (uint i = 0; i < count; i++)
            {
                var script = new ScriptData();
                script.Deserialize(version, input, endian);
                this.Scripts.Add(script);
            }
        }
    }
}
