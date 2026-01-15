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
                    chunk = new SDSEntries(br);
                    break;

                case 3:
                    chunk = new WeatherSettings(br);
                    break;

                case 4:
                    chunk = new GameFlags(br);
                    break;

                case 5:
                    chunk = new MissionSettings(br);
                    break;

                default:
                    chunk = new UnknownChunk(br, Type);

                    break;
            }

            return chunk;
        }
    }
}
