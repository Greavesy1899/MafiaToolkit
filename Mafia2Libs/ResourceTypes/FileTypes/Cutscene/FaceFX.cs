using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTypes.Cutscene
{
    public class FaceFX
    {
        public FaceFXBlock[] FaceFXBlocks { get; set; } = new FaceFXBlock[0];

        public FaceFX(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader baseBr)
        {
            using (BinaryReader br = new(new MemoryStream(baseBr.ReadBytes(baseBr.ReadInt32() - 4))))
            {
                int Count = br.ReadInt32();

                FaceFXBlocks = new FaceFXBlock[Count];

                for (int i = 0; i < Count; i++)
                {
                    FaceFXBlocks[i] = new(br);
                }
            }
        }

        public void Write(BinaryWriter baseBw)
        {
            using (MemoryStream ms = new())
            {
                using (BinaryWriter bw = new(ms))
                {
                    bw.Write(FaceFXBlocks.Length);

                    foreach (var block in FaceFXBlocks)
                    {
                        block.Write(bw);
                    }
                }

                byte[] data = ms.ToArray();

                baseBw.Write(data.Length + 4);
                baseBw.Write(data);
            }
        }
    }
}
