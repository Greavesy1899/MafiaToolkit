using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene
{
    public class FaceFXBlock
    {
        public string Name { get; set; }
        public int Unk00 { get; set; }
        public FaceFXBlock(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            Name = br.ReadString16();
            Unk00 = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.WriteString16(Name);
            bw.Write(Unk00);
        }
    }
}
