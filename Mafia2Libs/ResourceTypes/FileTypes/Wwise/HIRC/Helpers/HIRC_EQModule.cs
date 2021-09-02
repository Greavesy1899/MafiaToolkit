using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EQModule
    {
        public uint FilterType { get; set; }
        public float Gain { get; set; }
        public float Frequency { get; set; }
        public float qFactor { get; set; }
        public int on { get; set; }
        public EQModule(BinaryReader br)
        {
            FilterType = br.ReadUInt32();
            Gain = br.ReadSingle();
            Frequency = br.ReadSingle();
            qFactor = br.ReadSingle();
            on = br.ReadByte();
        }

        public EQModule()
        {
            FilterType = 0;
            Gain = 0;
            Frequency = 0;
            qFactor = 0;
            on = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(FilterType);
            bw.Write(Gain);
            bw.Write(Frequency);
            bw.Write(qFactor);
            bw.Write((byte)on);
        }
    }
}
