using System.IO;
using Utils.Extensions;

namespace ResourceTypes.CGame
{
    public class Type3Chunk : IGameChunk
    {
        public bool Unk00 { get; set; }
        public string Unk01 { get; set; } = ""; //Weather template?
        public float Unk02 { get; set; }
        public float Unk03 { get; set; }
        public Type3Chunk()
        {

        }

        public Type3Chunk(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            using (MemoryStream ms = new(br.ReadBytes(br.ReadInt32())))
            {
                Unk00 = ms.ReadBoolean();
                Unk01 = ms.ReadString();
                Unk02 = ms.ReadSingle(false);
                Unk03 = ms.ReadSingle(false);
            }
        }

        public void Write(BinaryWriter bw)
        {
            using (MemoryStream ms = new())
            {
                ms.Write(Unk00);
                ms.WriteString(Unk01);
                ms.Write(Unk02, false);
                ms.Write(Unk03, false);

                bw.Write((int)ms.Length);
                bw.Write(ms.ToArray());
            }
        }

        public int GetChunkType()
        {
            return 3 | 0xE000;
        }
    }
}
