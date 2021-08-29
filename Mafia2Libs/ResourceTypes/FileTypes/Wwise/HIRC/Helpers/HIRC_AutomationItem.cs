using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class AutomationItem
    {
        public uint clipId { get; set; }
        public uint autoType { get; set; }
        public List<GraphPoint> graphPoints { get; set; }

        public AutomationItem(BinaryReader br)
        {
            clipId = br.ReadUInt32();
            autoType = br.ReadUInt32();
            graphPoints = new List<GraphPoint>();
            uint numPoints = br.ReadUInt32();

            for (int i = 0; i < numPoints; i++)
            {
                graphPoints.Add(new GraphPoint(br));
            }
        }

        public AutomationItem()
        {
            clipId = 0;
            autoType = 0;
            graphPoints = new List<GraphPoint>();
        }

        public static void WriteAutomationItem(BinaryWriter bw, AutomationItem item)
        {
            bw.Write(item.clipId);
            bw.Write(item.autoType);
            bw.Write(item.graphPoints.Count);

            foreach (GraphPoint point in item.graphPoints)
            {
                point.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int length = 12 + graphPoints.Count * 12;

            return length;
        }
    }
}
