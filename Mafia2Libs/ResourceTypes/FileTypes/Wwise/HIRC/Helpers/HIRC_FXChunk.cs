using System;
using System.Xml;
using System.Xml.Linq;

namespace ResourceTypes.Wwise.Helpers
{
    public class FXChunk
    {
        public byte index { get; set; }
        public uint id { get; set; }
        public int isShareSet { get; set; }
        public int isRendered { get; set; }

        public FXChunk(byte bIndex,uint iId, byte bIsShareSet, byte bIsRendered)
        {
            index = bIndex;
            id = iId;
            isShareSet = bIsShareSet;
            isRendered = bIsRendered;
        }

        public FXChunk()
        {
            index = 0;
            id = 0;
            isShareSet = 0;
            isRendered = 0;
        }
    }
}
