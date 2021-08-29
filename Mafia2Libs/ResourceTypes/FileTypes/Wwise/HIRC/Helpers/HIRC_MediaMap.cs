using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class MediaMap
    {
        public int id { get; set; }
        public uint sourceId { get; set; }

        public MediaMap(BinaryReader br)
        {
            id = br.ReadByte();
            sourceId = br.ReadUInt32();
        }

        public MediaMap()
        {
            id = 0;
            sourceId = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)id);
            bw.Write(sourceId);
        }
    }
}
