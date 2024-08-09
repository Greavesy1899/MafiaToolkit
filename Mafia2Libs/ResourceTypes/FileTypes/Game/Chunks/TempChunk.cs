using System.IO;

namespace ResourceTypes.CGame
{
    public class TempChunk : IGameChunk
    {
        public int Type { get; set; }
        public byte[] Data { get; set; } = new byte[0];
        public TempChunk()
        {

        }

        public TempChunk(BinaryReader br, int _Type)
        {
            Type = _Type;
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            Data = br.ReadBytes(br.ReadInt32());
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(Data.Length);
            bw.Write(Data);
        }

        public int GetChunkType()
        {
            return Type;
        }
    }
}
