using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RangedModifier
    {
        public byte ID { get; set; }
        public uint Min { get; set; }
        public uint Max { get; set; }

        public RangedModifier(byte bID, uint fMin, uint fMax)
        {
            ID = bID;
            Min = fMin;
            Max = fMax;
        }

        public RangedModifier()
        {
            ID = 0;
            Min = 0;
            Max = 0;
        }
    }
}
