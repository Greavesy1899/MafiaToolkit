using System;
using System.IO;
using Gibbed.IO;

//THIS ISN'T GIBBED. BUT STILL USES GIBBED STUFF :)
namespace Gibbed.Illusion.ResourceFormats
{
    public class SoundResource
    {
        public byte ID;
        public string Name;
        public int FileSize;
        public byte[] Data;

        /// <summary>
        /// Deserialize resource using passed bytes.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="namelength">string length to read.</param>
        public void Deserialize(byte[] data, byte namelength)
        {
            MemoryStream input = new MemoryStream(data);

            this.ID = input.ReadValueU8();
            this.Name = input.ReadString(namelength);
            this.FileSize = input.ReadValueS32();
            this.Data = input.ReadBytes((int)(this.FileSize));
        }
    }
}
