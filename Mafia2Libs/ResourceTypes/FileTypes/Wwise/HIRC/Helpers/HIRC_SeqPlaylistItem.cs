using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SeqPlaylistItem
    {
        public uint SegmentID { get; set; }
        public uint ItemID { get; set; }
        public int RSType { get; set; }
        public int Loop { get; set; }
        public int LoopMin { get; set; }
        public int LoopMax { get; set; }
        public uint Weight { get; set; }
        public uint AvoidRepeatCount { get; set; }
        public int IsUsingWeight { get; set; }
        public int IsShuffle { get; set; }
        public List<SeqPlaylistItem> Children { get; set; }
        public SeqPlaylistItem(BinaryReader br)
        {
            SegmentID = br.ReadUInt32();
            ItemID = br.ReadUInt32();
            uint numChildren = br.ReadUInt32();
            RSType = br.ReadInt32();
            Loop = br.ReadInt16();
            LoopMin = br.ReadInt16();
            LoopMax = br.ReadInt16();
            Weight = br.ReadUInt32();
            AvoidRepeatCount = br.ReadUInt16();
            IsUsingWeight = br.ReadByte();
            IsShuffle = br.ReadByte();
            Children = new List<SeqPlaylistItem>();

            for (int i = 0; i < numChildren; i++)
            {
                Children.Add(new SeqPlaylistItem(br));
            }
        }

        public SeqPlaylistItem()
        {
            SegmentID = 0;
            ItemID = 0;
            RSType = 0;
            Loop = 0;
            LoopMin = 0;
            LoopMax = 0;
            Weight = 0;
            AvoidRepeatCount = 0;
            IsUsingWeight = 0;
            IsShuffle = 0;
            Children = new List<SeqPlaylistItem>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(SegmentID);
            bw.Write(ItemID);
            bw.Write(Children.Count);
            bw.Write(RSType);
            bw.Write((short)Loop);
            bw.Write((short)LoopMin);
            bw.Write((short)LoopMax);
            bw.Write(Weight);
            bw.Write((short)AvoidRepeatCount);
            bw.Write((byte)IsUsingWeight);
            bw.Write((byte)IsShuffle);

            foreach (SeqPlaylistItem child in Children)
            {
                child.WriteToFile(bw);
            }
        }

        public int GetCount()
        {
            int count = 1;

            foreach (SeqPlaylistItem child in Children)
            {
                count += child.GetCount();
            }

            return count;
        }

        public int GetLength()
        {
            int itemLength = 30;

            foreach (SeqPlaylistItem item in Children)
            {
                itemLength += item.GetLength();
            }

            return itemLength;
        }
    }
}
