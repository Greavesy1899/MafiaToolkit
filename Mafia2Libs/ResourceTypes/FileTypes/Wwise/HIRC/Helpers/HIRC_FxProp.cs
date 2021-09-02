using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FxProp
    {
        public int ID { get; set; }
        public int RTPCAccumulator { get; set; }
        public float Value { get; set; }
        public FxProp(int iID, int rtpcAccum, float fValue)
        {
            ID = iID;
            RTPCAccumulator = rtpcAccum;
            Value = fValue;
        }

        public FxProp()
        {
            ID = 0;
            RTPCAccumulator = 0;
            Value = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)ID);
            bw.Write((byte)RTPCAccumulator);
            bw.Write(Value);
        }
    }
}
