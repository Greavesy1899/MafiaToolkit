using System.IO;

namespace ResourceTypes.CGame
{
    public static class GameChunkFactory
    {
        public static IGameChunk ReadFromFile(BinaryReader br)
        {
            IGameChunk chunk = null;

            int Type = br.ReadInt32();

            switch (Type & 0xFF)
            {
                case 1:
                    chunk = new PreloadManager(br);
                    break;

                case 2:
                    chunk = new Type2Chunk(br);
                    break;

                case 3:
                    chunk = new Type3Chunk(br);
                    break;

                case 4:
                    chunk = new Type4Chunk(br);
                    break;

                case 5:
                    chunk = new Type5Chunk(br);
                    break;

                default:
                    chunk = new TempChunk(br, Type);

                    break;
            }

            return chunk;
        }
    }
}
