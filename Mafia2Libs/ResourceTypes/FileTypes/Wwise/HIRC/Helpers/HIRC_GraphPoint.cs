using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GraphPoint
    {
        public float From { get; set; }
        public float To { get; set; }
        public uint Interpolator { get; set; } //0x00 = Log3, 0x01 = Sine, 0x02 = Log1, 0x03 = InvSCurve, 0x04 = Linear, 0x05 = SCurve, 0x06 = Exp1, 0x07 = SineRecip, 0x08 = Exp3/LastFadeCurve, 0x09 = Constant
        public GraphPoint(BinaryReader br)
        {
            From = br.ReadSingle();
            To = br.ReadSingle();
            Interpolator = br.ReadUInt32();
        }

        public GraphPoint()
        {
            From = 0;
            To = 0;
            Interpolator = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(From);
            bw.Write(To);
            bw.Write(Interpolator);
        }
    }
}
