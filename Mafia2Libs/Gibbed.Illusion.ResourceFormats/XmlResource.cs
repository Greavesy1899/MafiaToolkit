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
using Utils.Settings;

namespace Gibbed.Mafia2.ResourceFormats
{
    public class XmlResource : IResourceType
    {
        public string Tag;
        public bool Unk1;
        public string Name;
        public bool Unk3;

        public string Content;

        public bool bFailedToDecompile = false;

        public void Serialize(ushort version, Stream output, Endian endian)
        {
            output.WriteStringU32(this.Tag, endian);

            if (version >= 3)
            {
                output.WriteValueU8((byte)(this.Unk1 ? 1 : 0));
            }

            output.WriteStringU32(this.Name, endian);

            if (version >= 2)
            {
                output.WriteValueU8((byte)(this.Unk3 ? 1 : 0));
            }
            if (this.Unk3 == false)
            {
                if (!bFailedToDecompile)
                {
                    XmlResource0.Serialize(output, this.Content, endian);
                }
                else
                {
                    byte[] data = File.ReadAllBytes(this.Content);
                    output.WriteBytes(data);
                }
            }
            else
            {
                XmlResource1.Serialize(output, this.Content, endian);
            }
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            this.Tag = input.ReadStringU32(endian);
            this.Unk1 = version >= 3 ? input.ReadValueU8() != 0 : true;
            this.Name = input.ReadStringU32(endian);
            this.Unk3 = version >= 2 ? input.ReadValueU8() != 0 : false;

            if(Name == "/config/ai/battle/eval_shoottarget/shootchain_precombat")
            {
                Console.Write("st");
            }

            // Super hacky solution to unpack XMLs with xml:xsi etc.
            if (this.Unk3 == false)
            {
                long currentPositon = input.Position;

                try
                {
                    this.Content = XmlResource0.Deserialize(input, endian);
                }
                catch(Exception ex)
                {
                    input.Position = currentPositon;
                    Console.WriteLine(ex.Message);
                    bFailedToDecompile = true;
                }
            }
            else
            {
                this.Content = XmlResource1.Deserialize(input, endian);
            }
        }
    }
}
