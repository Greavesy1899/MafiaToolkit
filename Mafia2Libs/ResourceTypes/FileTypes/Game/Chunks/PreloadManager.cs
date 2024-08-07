using System.Collections.Generic;
using System.IO;
using Utils.Extensions;

namespace ResourceTypes.CGame
{
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

    public class PreloadSlot
    {
        public string Value { get; set; } = "";
        public PreloadSlot()
        {

        }

        public PreloadSlot(MemoryStream ms)
        {
            Read(ms);
        }

        public void Read(MemoryStream ms)
        {
            Value = ms.ReadString();
        }

        public void Write(MemoryStream ms)
        {
            ms.WriteString(Value);
        }
    }
}
