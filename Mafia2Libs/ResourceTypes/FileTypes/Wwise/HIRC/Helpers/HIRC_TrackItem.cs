using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class TrackItem
    {
        public uint id { get; set; }
        public uint sourceId { get; set; }
        public double playAt { get; set; }
        public double beginTrimOffset { get; set; }
        public double endTrimOffset { get; set; }
        public double srcDuration { get; set; }
        public TrackItem(BinaryReader br)
        {
            id = br.ReadUInt32();
            sourceId = br.ReadUInt32();
            playAt = br.ReadDouble();
            beginTrimOffset = br.ReadDouble();
            endTrimOffset = br.ReadDouble();
            srcDuration = br.ReadDouble();
        }

        public TrackItem()
        {
            id = 0;
            sourceId = 0;
            playAt = 0;
            beginTrimOffset = 0;
            endTrimOffset = 0;
            srcDuration = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(sourceId);
            bw.Write(playAt);
            bw.Write(beginTrimOffset);
            bw.Write(endTrimOffset);
            bw.Write(srcDuration);
        }
    }
}
