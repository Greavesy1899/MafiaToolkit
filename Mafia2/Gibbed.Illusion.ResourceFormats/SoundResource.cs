using System.IO;
using Gibbed.IO;

//THIS ISN'T GIBBED. BUT STILL USES GIBBED STUFF :)
namespace Gibbed.Illusion.ResourceFormats
{
    public class SoundResource
    {
        public string Name;
        public int FileSize;
        public byte[] Data;

        /// <summary>
        /// Deserialize resource using passed bytes.
        /// </summary>
        /// <param name="data"></param>
        public void Deserialize(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);

            byte length = input.ReadValueU8();
            this.Name = input.ReadString(length);
            this.FileSize = input.ReadValueS32();
            this.Data = input.ReadBytes((int)(this.FileSize));
        }
    }
}
