using System.ComponentModel;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.CGame
{
    /// <summary>
    /// Chunk type 5 (0xE005) - Mission settings chunk.
    /// Contains the mission trick name and path to actors configuration file.
    /// The game uses these to initialize mission data and spawn player actors.
    /// </summary>
    public class MissionSettings : IGameChunk
    {
        /// <summary>
        /// Mission trick/identifier name (e.g., "CITY_trick").
        /// Used to identify and configure the mission.
        /// </summary>
        [Description("Mission trick/identifier name (e.g., CITY_trick)")]
        public string MissionTrick { get; set; } = "";

        /// <summary>
        /// Path to the actors configuration file (e.g., "/missions/CITY/actors_player.bin").
        /// Defines player spawn and actor setup for the mission.
        /// </summary>
        [Description("Path to the actors configuration file")]
        public string ActorsFilePath { get; set; } = "";

        public MissionSettings()
        {
        }

        public MissionSettings(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            using (MemoryStream ms = new(br.ReadBytes(br.ReadInt32())))
            {
                if (ms.Position < ms.Length)
                {
                    MissionTrick = ms.ReadString();
                }
                if (ms.Position < ms.Length)
                {
                    ActorsFilePath = ms.ReadString();
                }
            }
        }

        public void Write(BinaryWriter bw)
        {
            using (MemoryStream ms = new())
            {
                ms.WriteString(MissionTrick);
                ms.WriteString(ActorsFilePath);

                bw.Write((int)ms.Length);
                bw.Write(ms.ToArray());
            }
        }

        public int GetChunkType()
        {
            return 5 | 0xE000;
        }
    }
}
