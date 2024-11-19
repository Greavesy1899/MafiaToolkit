using System;
using System.Xml;
using System.Xml.Linq;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vertex
    {
        public float xValue { get; set; }
        public float yValue { get; set; }
        public float zValue { get; set; }
        public uint Duration { get; set; }

        public Vertex (float fxValue, float fyValue, float fzValue, uint iDuration)
        {
            xValue = fxValue;
            yValue = fyValue;
            zValue = fzValue;
            Duration = iDuration;
        }

        public Vertex()
        {
            xValue = 0;
            yValue = 0;
            zValue = 0;
            Duration = 0;
        }
    }
}
