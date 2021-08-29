using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class FxProp
    {
        public int id { get; set; }
        public int RTPCAccumulator { get; set; }
        public float value { get; set; }
        public FxProp(int iId, int rtpcAccum, float fValue)
        {
            id = iId;
            RTPCAccumulator = rtpcAccum;
            value = fValue;
        }

        public FxProp()
        {
            id = 0;
            RTPCAccumulator = 0;
            value = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)id);
            bw.Write((byte)RTPCAccumulator);
            bw.Write(value);
        }
    }
}
