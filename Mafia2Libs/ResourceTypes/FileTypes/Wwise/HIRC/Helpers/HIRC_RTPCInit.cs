using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class RTPCInit
    {
        public int id { get; set; }
        public float value { get; set; }

        public RTPCInit(BinaryReader br)
        {
            id = br.ReadByte();
            value = br.ReadSingle();
        }

        public RTPCInit()
        {
            id = 0;
            value = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)id);
            bw.Write(value);
        }
    }
}
