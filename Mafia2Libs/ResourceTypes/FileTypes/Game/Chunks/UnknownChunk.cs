using System.ComponentModel;
using System.IO;

namespace ResourceTypes.CGame
{
    /// <summary>
    /// Temporary/unknown chunk type used as a fallback for unrecognized chunk types.
    /// Also handles the terminator chunk (type 0xFEAB / 65195) which signals end of parsing.
    /// </summary>
    public class UnknownChunk : IGameChunk
    {
        /// <summary>
        /// Chunk type identifier.
        /// Common value: 65195 (0xFEAB) = Terminator chunk that ends parsing.
        /// </summary>
        [Description("Chunk type (65195 = terminator)")]
        public int Type { get; set; }

        /// <summary>
        /// Raw chunk data bytes (empty for terminator chunks).
        /// </summary>
        public byte[] Data { get; set; } = new byte[0];

        public UnknownChunk()
        {
        }

        public UnknownChunk(BinaryReader br, int _Type)
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
