using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class EQModule
    {
        public uint filterType { get; set; }
        public float gain { get; set; }
        public float frequency { get; set; }
        public float qFactor { get; set; }
        public int on { get; set; }
        public EQModule(BinaryReader br)
        {
            filterType = br.ReadUInt32();
            gain = br.ReadSingle();
            frequency = br.ReadSingle();
            qFactor = br.ReadSingle();
            on = br.ReadByte();
        }

        public EQModule()
        {
            filterType = 0;
            gain = 0;
            frequency = 0;
            qFactor = 0;
            on = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(filterType);
            bw.Write(gain);
            bw.Write(frequency);
            bw.Write(qFactor);
            bw.Write((byte)on);
        }
    }
}
