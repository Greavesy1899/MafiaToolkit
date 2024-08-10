using System.IO;
using Utils.Logging;

namespace ResourceTypes.CGame
{
    public class Type2Chunk : IGameChunk
    {
        public int Unk00 { get; set; }
        public Type2Chunk()
        {

        }

        public Type2Chunk(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            int Length = br.ReadInt32();

            ToolkitAssert.Ensure(Length == 4, "C_Game chunk type 2 length != 4.");

            Unk00 = br.ReadInt32();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(4);
            bw.Write(Unk00);
        }

        public int GetChunkType()
        {
            return 2 | 0xE000;
        }
    }
}
