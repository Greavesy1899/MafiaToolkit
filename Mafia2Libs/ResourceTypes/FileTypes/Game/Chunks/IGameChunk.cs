using System.IO;

namespace ResourceTypes.CGame
{
    /// <summary>
    /// Interface for C_Game file chunks.
    ///
    /// Chunk types (from IDA analysis):
    /// - 0xE001 (57345): PreloadManager - SDS search paths
    /// - 0xE002 (57346): SDSEntries - SDS preload entries with slot types
    /// - 0xE003 (57347): WeatherSettings - Weather/fog settings
    /// - 0xE004 (57348): GameFlags - Two byte flags
    /// - 0xE005 (57349): MissionSettings - Mission settings (trick name, actors file)
    /// - 0xFEAB (65195): UnknownChunk - Terminator, signals end of chunk parsing
    /// </summary>
    public interface IGameChunk
    {
        public void Read(BinaryReader br)
        {
        }

        public void Write(BinaryWriter bw)
        {
        }

        /// <summary>
        /// Gets the chunk type identifier (e.g., 0xE001 for PreloadManager).
        /// The low byte (& 0xFF) identifies the chunk type.
        /// </summary>
        public int GetChunkType()
        {
            return 0;
        }
    }
}
