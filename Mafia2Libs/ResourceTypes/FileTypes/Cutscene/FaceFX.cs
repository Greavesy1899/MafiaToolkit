using System.IO;

namespace ResourceTypes.Cutscene
{
    public class FaceFX
    {
        public FaceFXBlock[] FaceFXBlocks { get; set; } = new FaceFXBlock[0];
        public int Unk00 { get; set; }

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

                Unk00 = br.ReadInt32();
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

                    bw.Write(Unk00);
                }

                byte[] data = ms.ToArray();

                baseBw.Write(data.Length + 4);
                baseBw.Write(data);
            }
        }
    }
}
