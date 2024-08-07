using System.IO;

namespace ResourceTypes.CGame
{
    public interface IGameChunk
    {
        public void Read(BinaryReader br)
        {
            
        }

        public void Write(BinaryWriter bw)
        {
            
        }

        public int GetChunkType()
        {
            return 0;
        }
    }
}
