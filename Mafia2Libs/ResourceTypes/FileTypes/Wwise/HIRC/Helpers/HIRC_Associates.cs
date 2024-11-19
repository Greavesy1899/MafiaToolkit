using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Assoc
    {
        public uint ID { get; set; }
        public List<GraphPoint> Curves { get; set; }
        public Assoc(BinaryReader br)
        {
            ID = br.ReadUInt32();
            Curves = new List<GraphPoint>();
            uint numCurves = br.ReadUInt32();

            for (int i = 0; i < numCurves; i++)
            {
                Curves.Add(new GraphPoint(br));
            }
        }

        public Assoc()
        {
            ID = 0;
            Curves = new List<GraphPoint>();
        }

        public static void WriteAssoc(BinaryWriter bw, Assoc assoc)
        {
            bw.Write(assoc.ID);
            bw.Write(assoc.Curves.Count);

            foreach (GraphPoint point in assoc.Curves)
            {
                point.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int Length = 8 + Curves.Count * 12;

            return Length;
        }
    }
}
