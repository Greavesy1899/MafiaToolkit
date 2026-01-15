using System.ComponentModel;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.CGame
{
    /// <summary>
    /// Chunk type 3 (0xE003) - Weather and fog settings for the game level.
    /// Contains a weather template name and fog distance parameters.
    /// </summary>
    public class WeatherSettings : IGameChunk
    {
        /// <summary>
        /// Whether custom weather settings are enabled.
        /// </summary>
        [Description("Whether custom weather settings are enabled")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Weather template name (e.g., "fog_sky_den").
        /// References a weather preset configuration.
        /// </summary>
        [Description("Weather template name (e.g., fog_sky_den)")]
        public string WeatherTemplate { get; set; } = "";

        /// <summary>
        /// Near fog distance - where fog starts to appear.
        /// </summary>
        [Description("Near fog distance - where fog starts to appear")]
        public float FogNear { get; set; }

        /// <summary>
        /// Far fog distance - where fog reaches maximum density.
        /// </summary>
        [Description("Far fog distance - where fog reaches maximum density")]
        public float FogFar { get; set; }

        public WeatherSettings()
        {
        }

        public WeatherSettings(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            using (MemoryStream ms = new(br.ReadBytes(br.ReadInt32())))
            {
                Enabled = ms.ReadBoolean();
                WeatherTemplate = ms.ReadString();
                FogNear = ms.ReadSingle(false);
                FogFar = ms.ReadSingle(false);
            }
        }

        public void Write(BinaryWriter bw)
        {
            using (MemoryStream ms = new())
            {
                ms.Write(Enabled);
                ms.WriteString(WeatherTemplate);
                ms.Write(FogNear, false);
                ms.Write(FogFar, false);

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
