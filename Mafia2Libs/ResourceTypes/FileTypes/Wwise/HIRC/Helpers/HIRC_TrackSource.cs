using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TrackSource
    {
        public uint PluginID { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int PluginType { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int PluginCompany { get; set; }
        public byte StreamType { get; set; } //0x00 = Data/Bnk, 0x02 = Streaming
        public uint SourceID { get; set; }
        public int InMemoryMediaSize { get; set; }
        public byte SourceBits { get; set; } //bit0 = "bIsLanguageSpecific", bit1 = "bPrefetch", bit3 = "bNonCachable", bit7 = "bHasSource"
        public TrackSource(BinaryReader br)
        {
            PluginType = br.ReadByte();
            PluginCompany = br.ReadByte();
            br.BaseStream.Seek((int)br.BaseStream.Position - 2, SeekOrigin.Begin);
            PluginID = br.ReadUInt32();
            StreamType = br.ReadByte();
            SourceID = br.ReadUInt32();
            InMemoryMediaSize = br.ReadInt32();
            SourceBits = br.ReadByte();
        }

        public TrackSource()
        {
            PluginType = 0;
            PluginCompany = 0;
            PluginID = 0;
            StreamType = 0;
            SourceID = 0;
            InMemoryMediaSize = 0;
            SourceBits = 0;
        }

        public static void WriteTrackSource(BinaryWriter bw, TrackSource source)
        {
            bw.Write(source.PluginID);
            bw.Write(source.StreamType);
            bw.Write(source.SourceID);
            bw.Write(source.InMemoryMediaSize);
            bw.Write(source.SourceBits);
        }
    }
}
