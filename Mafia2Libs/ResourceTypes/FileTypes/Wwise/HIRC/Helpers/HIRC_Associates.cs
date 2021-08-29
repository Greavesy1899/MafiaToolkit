using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class Assoc
    {
        public uint id { get; set; }
        public List<GraphPoint> curves { get; set; }
        public Assoc(BinaryReader br)
        {
            id = br.ReadUInt32();
            curves = new List<GraphPoint>();
            uint numCurves = br.ReadUInt32();

            for (int i = 0; i < numCurves; i++)
            {
                curves.Add(new GraphPoint(br));
            }
        }

        public Assoc()
        {
            id = 0;
            curves = new List<GraphPoint>();
        }

        public static void WriteAssoc(BinaryWriter bw, Assoc assoc)
        {
            bw.Write(assoc.id);
            bw.Write(assoc.curves.Count);

            foreach (GraphPoint point in assoc.curves)
            {
                point.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int length = 8 + curves.Count * 12;

            return length;
        }
    }
}
