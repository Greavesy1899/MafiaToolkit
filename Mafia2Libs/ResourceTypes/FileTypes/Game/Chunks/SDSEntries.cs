using System.Collections.Generic;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.CGame
{
    /// <summary>
    /// Chunk type 2 (0xE002) - Contains a list of SDS slot preload entries.
    /// Each entry specifies a slot type and a path to load.
    /// When entry type is 5, the game loads it as slot type 35.
    /// </summary>
    public class SDSEntries : IGameChunk
    {
        public SDSPreloadEntry[] Entries { get; set; } = new SDSPreloadEntry[0];

        public SDSEntries()
        {
        }

        public SDSEntries(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            List<SDSPreloadEntry> entries = new();

            using (MemoryStream ms = new(br.ReadBytes(br.ReadInt32())))
            {
                int entryCount = ms.ReadInt32(false);

                for (int i = 0; i < entryCount; i++)
                {
                    entries.Add(new SDSPreloadEntry(ms));
                }
            }

            Entries = entries.ToArray();
        }

        public void Write(BinaryWriter bw)
        {
            using (MemoryStream ms = new())
            {
                ms.Write(Entries.Length, false);

                foreach (var entry in Entries)
                {
                    entry.Write(ms);
                }

                bw.Write((int)ms.Length);
                bw.Write(ms.ToArray());
            }
        }

        public int GetChunkType()
        {
            return 2 | 0xE000;
        }
    }

    /// <summary>
    /// An SDS preload entry with a slot type and path.
    /// </summary>
    public class SDSPreloadEntry
    {
        /// <summary>
        /// Slot type identifier. When this is 5, the game loads with slot type 35.
        /// </summary>
        public int SlotType { get; set; }

        /// <summary>
        /// Path to the SDS file or resource.
        /// </summary>
        public string Path { get; set; } = "";

        public SDSPreloadEntry()
        {
        }

        public SDSPreloadEntry(MemoryStream ms)
        {
            Read(ms);
        }

        public void Read(MemoryStream ms)
        {
            SlotType = ms.ReadInt32(false);
            Path = ms.ReadString();
        }

        public void Write(MemoryStream ms)
        {
            ms.Write(SlotType, false);
            ms.WriteString(Path);
        }
    }
}
