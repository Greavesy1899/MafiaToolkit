using System;
using System.Xml;
using System.Xml.Linq;

namespace ResourceTypes.Wwise.Helpers
{
    public class RangedModifier
    {
        public byte id { get; set; }
        public uint min { get; set; }
        public uint max { get; set; }

        public RangedModifier(byte bId, uint fMin, uint fMax)
        {
            id = bId;
            min = fMin;
            max = fMax;
        }

        public RangedModifier()
        {
            id = 0;
            min = 0;
            max = 0;
        }
    }
}
