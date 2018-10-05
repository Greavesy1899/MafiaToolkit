using System.IO;
using Gibbed.IO;

//THIS ISN'T GIBBED. BUT STILL USES GIBBED STUFF :)
namespace Gibbed.Mafia2.ResourceFormats
{
    public class SoundResource : IResourceType
    {
        private byte[] data;

        public string Name;
        public int FileSize;
        public byte[] Data {
            get { return data; }
            set {
                data = value;
                FileSize = value.Length;
            }
        }

        public void Deserialize(ushort version, Stream input, Endian endian)
        {
            byte length = input.ReadValueU8();
            this.Name = input.ReadString(length);
            this.FileSize = input.ReadValueS32();
            this.Data = input.ReadBytes(FileSize);
        }

        public void Serialize(ushort version, Stream input, Endian endian)
        {
            input.WriteValueU8((byte)Name.Length);
            input.WriteString(Name);
            input.WriteValueS32(FileSize);
            input.WriteBytes(Data);
        }
    }
}
