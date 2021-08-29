using System;
using System.Xml;
using System.Xml.Linq;

namespace ResourceTypes.Wwise.Helpers
{
    public class Vertex
    {
        public float xValue { get; set; }
        public float yValue { get; set; }
        public float zValue { get; set; }
        public uint duration { get; set; }

        public Vertex (float fxValue, float fyValue, float fzValue, uint iDuration)
        {
            xValue = fxValue;
            yValue = fyValue;
            zValue = fzValue;
            duration = iDuration;
        }

        public Vertex()
        {
            xValue = 0;
            yValue = 0;
            zValue = 0;
            duration = 0;
        }
    }
}
