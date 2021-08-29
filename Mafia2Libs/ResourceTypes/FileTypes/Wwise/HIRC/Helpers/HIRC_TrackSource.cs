using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class TrackSource
    {
        public uint pluginId { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int pluginType { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int pluginCompany { get; set; }
        public byte streamType { get; set; } //0x00 = Data/Bnk, 0x02 = Streaming
        public uint sourceId { get; set; }
        public int inMemoryMediaSize { get; set; }
        public byte sourceBits { get; set; } //bit0 = "bIsLanguageSpecific", bit1 = "bPrefetch", bit3 = "bNonCachable", bit7 = "bHasSource"
        public TrackSource(BinaryReader br)
        {
            pluginType = br.ReadByte();
            pluginCompany = br.ReadByte();
            br.BaseStream.Seek((int)br.BaseStream.Position - 2, SeekOrigin.Begin);
            pluginId = br.ReadUInt32();
            streamType = br.ReadByte();
            sourceId = br.ReadUInt32();
            inMemoryMediaSize = br.ReadInt32();
            sourceBits = br.ReadByte();
        }

        public TrackSource()
        {
            pluginType = 0;
            pluginCompany = 0;
            pluginId = 0;
            streamType = 0;
            sourceId = 0;
            inMemoryMediaSize = 0;
            sourceBits = 0;
        }

        public static void WriteTrackSource(BinaryWriter bw, TrackSource source)
        {
            bw.Write(source.pluginId);
            bw.Write(source.streamType);
            bw.Write(source.sourceId);
            bw.Write(source.inMemoryMediaSize);
            bw.Write(source.sourceBits);
        }
    }
}
