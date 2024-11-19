using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AutomationItem
    {
        public uint ClipID { get; set; }
        public uint AutoType { get; set; }
        public List<GraphPoint> GraphPoints { get; set; }

        public AutomationItem(BinaryReader br)
        {
            ClipID = br.ReadUInt32();
            AutoType = br.ReadUInt32();
            GraphPoints = new List<GraphPoint>();
            uint numPoints = br.ReadUInt32();

            for (int i = 0; i < numPoints; i++)
            {
                GraphPoints.Add(new GraphPoint(br));
            }
        }

        public AutomationItem()
        {
            ClipID = 0;
            AutoType = 0;
            GraphPoints = new List<GraphPoint>();
        }

        public static void WriteAutomationItem(BinaryWriter bw, AutomationItem item)
        {
            bw.Write(item.ClipID);
            bw.Write(item.AutoType);
            bw.Write(item.GraphPoints.Count);

            foreach (GraphPoint point in item.GraphPoints)
            {
                point.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int Length = 12 + GraphPoints.Count * 12;

            return Length;
        }
    }
}
