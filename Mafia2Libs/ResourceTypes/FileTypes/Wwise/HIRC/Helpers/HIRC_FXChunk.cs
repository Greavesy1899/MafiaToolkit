using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FXChunk
    {
        public byte Index { get; set; }
        public uint ID { get; set; }
        public int IsShareSet { get; set; }
        public int IsRendered { get; set; }

        public FXChunk(byte bIndex,uint iID, byte bIsShareSet, byte bIsRendered)
        {
            Index = bIndex;
            ID = iID;
            IsShareSet = bIsShareSet;
            IsRendered = bIsRendered;
        }

        public FXChunk()
        {
            Index = 0;
            ID = 0;
            IsShareSet = 0;
            IsRendered = 0;
        }
    }
}
