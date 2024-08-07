using System.IO;
using Utils.Logging;

namespace ResourceTypes.CGame
{
    public class Type4Chunk : IGameChunk
    {
        public short Unk00 { get; set; }
        public Type4Chunk()
        {

        }

        public Type4Chunk(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            int Length = br.ReadInt32();

            ToolkitAssert.Ensure(Length == 2, "C_Game chunk type 4 length != 2.");

            Unk00 = br.ReadInt16();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(2);
            bw.Write(Unk00);
        }

        public int GetChunkType()
        {
            return 4 | 0xE000;
        }
    }
}
