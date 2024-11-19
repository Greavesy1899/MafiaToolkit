using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Curve
    {
        public byte Scaling { get; set; }
        public List<GraphPoint> GraphPoints { get; set; }
        public Curve(BinaryReader br)
        {
            Scaling = br.ReadByte();
            GraphPoints = new List<GraphPoint>();
            uint numPoints = br.ReadUInt16();

            for (int i = 0; i < numPoints; i++)
            {
                GraphPoints.Add(new GraphPoint(br));
            }
        }

        public Curve()
        {
            Scaling = 0;
            GraphPoints = new List<GraphPoint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(Scaling);
            bw.Write((short)GraphPoints.Count);

            foreach (GraphPoint point in GraphPoints)
            {
                point.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int Length = 3 + GraphPoints.Count * 12;

            return Length;
        }
    }
}
