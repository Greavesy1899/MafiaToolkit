using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class SeqPlaylistItem
    {
        public uint segmentId { get; set; }
        public uint itemId { get; set; }
        public int RSType { get; set; }
        public int Loop { get; set; }
        public int LoopMin { get; set; }
        public int LoopMax { get; set; }
        public uint weight { get; set; }
        public uint avoidRepeatCount { get; set; }
        public int isUsingWeight { get; set; }
        public int isShuffle { get; set; }
        public List<SeqPlaylistItem> children { get; set; }
        public SeqPlaylistItem(BinaryReader br)
        {
            segmentId = br.ReadUInt32();
            itemId = br.ReadUInt32();
            uint numChildren = br.ReadUInt32();
            RSType = br.ReadInt32();
            Loop = br.ReadInt16();
            LoopMin = br.ReadInt16();
            LoopMax = br.ReadInt16();
            weight = br.ReadUInt32();
            avoidRepeatCount = br.ReadUInt16();
            isUsingWeight = br.ReadByte();
            isShuffle = br.ReadByte();
            children = new List<SeqPlaylistItem>();

            for (int i = 0; i < numChildren; i++)
            {
                children.Add(new SeqPlaylistItem(br));
            }
        }

        public SeqPlaylistItem()
        {
            segmentId = 0;
            itemId = 0;
            RSType = 0;
            Loop = 0;
            LoopMin = 0;
            LoopMax = 0;
            weight = 0;
            avoidRepeatCount = 0;
            isUsingWeight = 0;
            isShuffle = 0;
            children = new List<SeqPlaylistItem>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(segmentId);
            bw.Write(itemId);
            bw.Write(children.Count);
            bw.Write(RSType);
            bw.Write((short)Loop);
            bw.Write((short)LoopMin);
            bw.Write((short)LoopMax);
            bw.Write(weight);
            bw.Write((short)avoidRepeatCount);
            bw.Write((byte)isUsingWeight);
            bw.Write((byte)isShuffle);

            foreach (SeqPlaylistItem child in children)
            {
                child.WriteToFile(bw);
            }
        }

        public int GetCount()
        {
            int count = 1;

            foreach (SeqPlaylistItem child in children)
            {
                count += child.GetCount();
            }

            return count;
        }

        public int GetLength()
        {
            int itemLength = 30;

            foreach (SeqPlaylistItem item in children)
            {
                itemLength += item.GetLength();
            }

            return itemLength;
        }
    }
}
