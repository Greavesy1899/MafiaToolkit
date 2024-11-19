using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RTPCInit
    {
        public int ID { get; set; }
        public float Value { get; set; }

        public RTPCInit(BinaryReader br)
        {
            ID = br.ReadByte();
            Value = br.ReadSingle();
        }

        public RTPCInit()
        {
            ID = 0;
            Value = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)ID);
            bw.Write(Value);
        }
    }
}
