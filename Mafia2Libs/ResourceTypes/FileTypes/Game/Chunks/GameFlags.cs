using System.ComponentModel;
using System.IO;
using Utils.Logging;

namespace ResourceTypes.CGame
{
    /// <summary>
    /// Chunk type 4 (0xE004) - Contains two byte flags.
    /// The game stores these at separate byte offsets in the C_Game object.
    /// Purpose is currently unknown, possibly game state flags.
    /// </summary>
    public class GameFlags : IGameChunk
    {
        /// <summary>
        /// First byte flag (stored at C_Game offset +40).
        /// </summary>
        [Description("First byte flag")]
        public byte Flag0 { get; set; }

        /// <summary>
        /// Second byte flag (stored at C_Game offset +41).
        /// </summary>
        [Description("Second byte flag")]
        public byte Flag1 { get; set; }

        public GameFlags()
        {
        }

        public GameFlags(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            int Length = br.ReadInt32();

            ToolkitAssert.Ensure(Length == 2, "C_Game chunk type 4 length != 2.");

            Flag0 = br.ReadByte();
            Flag1 = br.ReadByte();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(2);
            bw.Write(Flag0);
            bw.Write(Flag1);
        }

        public int GetChunkType()
        {
            return 4 | 0xE000;
        }
    }
}
