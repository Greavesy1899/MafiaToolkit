using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene
{
    public class FaceFXBlock
    {
        public string Name { get; set; }
        public FaceFXBlock(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            Name = br.ReadString16();
        }

        public void Write(BinaryWriter bw)
        {
            bw.WriteString16(Name);
        }
    }
}
