using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class SwitchParam
    {
        public uint id { get; set; }
        public byte paramBitVector { get; set; }
        public byte paramBitVector2 { get; set; }
        public float fadeOutTime { get; set; }
        public float fadeInTime { get; set; }
        public SwitchParam(BinaryReader br)
        {
            id = br.ReadUInt32();
            paramBitVector = br.ReadByte();
            paramBitVector2 = br.ReadByte();
            fadeOutTime = br.ReadSingle();
            fadeInTime = br.ReadSingle();
        }

        public SwitchParam()
        {
            id = 0;
            paramBitVector = 0;
            paramBitVector2 = 0;
            fadeOutTime = 0;
            fadeInTime = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(paramBitVector);
            bw.Write(paramBitVector2);
            bw.Write(fadeOutTime);
            bw.Write(fadeInTime);
        }
    }
}
