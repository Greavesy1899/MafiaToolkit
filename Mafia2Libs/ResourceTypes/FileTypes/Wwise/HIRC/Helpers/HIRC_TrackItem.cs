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
    public class TrackItem
    {
        public uint ID { get; set; }
        public uint SourceID { get; set; }
        public double PlayAt { get; set; }
        public double BeginTrimOffset { get; set; }
        public double EndTrimOffset { get; set; }
        public double SourceDuration { get; set; }
        public TrackItem(BinaryReader br)
        {
            ID = br.ReadUInt32();
            SourceID = br.ReadUInt32();
            PlayAt = br.ReadDouble();
            BeginTrimOffset = br.ReadDouble();
            EndTrimOffset = br.ReadDouble();
            SourceDuration = br.ReadDouble();
        }

        public TrackItem()
        {
            ID = 0;
            SourceID = 0;
            PlayAt = 0;
            BeginTrimOffset = 0;
            EndTrimOffset = 0;
            SourceDuration = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(SourceID);
            bw.Write(PlayAt);
            bw.Write(BeginTrimOffset);
            bw.Write(EndTrimOffset);
            bw.Write(SourceDuration);
        }
    }
}
