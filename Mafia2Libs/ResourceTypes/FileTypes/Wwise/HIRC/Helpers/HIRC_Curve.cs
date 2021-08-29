using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class Curve
    {
        public byte scaling { get; set; }
        public List<GraphPoint> graphPoints { get; set; }
        public Curve(BinaryReader br)
        {
            scaling = br.ReadByte();
            graphPoints = new List<GraphPoint>();
            uint numPoints = br.ReadUInt16();

            for (int i = 0; i < numPoints; i++)
            {
                graphPoints.Add(new GraphPoint(br));
            }
        }

        public Curve()
        {
            scaling = 0;
            graphPoints = new List<GraphPoint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(scaling);
            bw.Write((short)graphPoints.Count);

            foreach (GraphPoint point in graphPoints)
            {
                point.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int length = 3 + graphPoints.Count * 12;

            return length;
        }
    }
}
