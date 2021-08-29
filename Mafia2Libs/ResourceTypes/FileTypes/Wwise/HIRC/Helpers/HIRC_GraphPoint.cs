using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace ResourceTypes.Wwise.Helpers
{
    public class GraphPoint
    {
        public float from { get; set; }
        public float to { get; set; }
        public uint interpolator { get; set; } //0x00 = Log3, 0x01 = Sine, 0x02 = Log1, 0x03 = InvSCurve, 0x04 = Linear, 0x05 = SCurve, 0x06 = Exp1, 0x07 = SineRecip, 0x08 = Exp3/LastFadeCurve, 0x09 = Constant
        public GraphPoint(BinaryReader br)
        {
            from = br.ReadSingle();
            to = br.ReadSingle();
            interpolator = br.ReadUInt32();
        }

        public GraphPoint()
        {
            from = 0;
            to = 0;
            interpolator = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(from);
            bw.Write(to);
            bw.Write(interpolator);
        }
    }
}
