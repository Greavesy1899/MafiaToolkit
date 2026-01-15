using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.CGame
{
    /// <summary>
    /// Chunk type 1 (0xE001) - Preload Manager containing SDS search paths.
    /// These paths define directories where the game looks for SDS files to preload.
    /// The game uses up to 4 of these paths (stored at C_Game offsets +12, +16, +20, +24).
    /// Common values include "/sds/City/", "/sds/Shops/", "/sds/Traffic/", "/sds/Cars/".
    /// </summary>
    public class PreloadManager : IGameChunk
    {
        public PreloadSlot[] Slots { get; set; } = new PreloadSlot[0];

        public PreloadManager()
        {
        }

        public PreloadManager(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            List<PreloadSlot> preloadSlots = new();

            using (MemoryStream ms = new(br.ReadBytes(br.ReadInt32())))
            {
                while (ms.Position != ms.Length)
                {
                    preloadSlots.Add(new(ms));
                }
            }

            Slots = preloadSlots.ToArray();
        }

        public void Write(BinaryWriter bw)
        {
            using (MemoryStream ms = new())
            {
                foreach (var slot in Slots)
                {
                    slot.Write(ms);
                }

                bw.Write((int)ms.Length);
                bw.Write(ms.ToArray());
            }
        }

        public int GetChunkType()
        {
            return 1 | 0xE000;
        }
    }

    /// <summary>
    /// A preload slot entry containing an SDS search path.
    /// </summary>
    public class PreloadSlot
    {
        /// <summary>
        /// SDS search path directory (e.g., "/sds/City/").
        /// </summary>
        [Description("SDS search path directory (e.g., /sds/City/)")]
        public string Path { get; set; } = "";

        public PreloadSlot()
        {
        }

        public PreloadSlot(MemoryStream ms)
        {
            Read(ms);
        }

        public void Read(MemoryStream ms)
        {
            Path = ms.ReadString();
        }

        public void Write(MemoryStream ms)
        {
            ms.WriteString(Path);
        }
    }
}
