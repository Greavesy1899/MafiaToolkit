using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class FeedbackSource
    {
        public uint companyId { get; set; }
        public uint deviceId { get; set; }
        public float volumeOffset { get; set; }
        public uint pluginId { get; set; }
        public byte streamType { get; set; }
        public uint sourceId { get; set; }
        public int inMemoryMediaSize { get; set; }
        public byte sourceBits { get; set; }
        public int size { get; set; }
        public FeedbackSource(BinaryReader br)
        {
            companyId = br.ReadUInt16();
            deviceId = br.ReadUInt16();
            volumeOffset = br.ReadSingle();
            pluginId = br.ReadUInt32();
            streamType = br.ReadByte();
            sourceId = br.ReadUInt32();
            inMemoryMediaSize = br.ReadInt32();
            sourceBits = br.ReadByte();
            size = br.ReadInt32();
        }

        public FeedbackSource()
        {
            companyId = 0;
            deviceId = 0;
            volumeOffset = 0;
            pluginId = 0;
            streamType = 0;
            sourceId = 0;
            inMemoryMediaSize = 0;
            sourceBits = 0;
            size = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((short)companyId);
            bw.Write((short)deviceId);
            bw.Write(volumeOffset);
            bw.Write(pluginId);
            bw.Write(streamType);
            bw.Write(sourceId);
            bw.Write(inMemoryMediaSize);
            bw.Write(sourceBits);
            bw.Write(size);
        }
    }
}
