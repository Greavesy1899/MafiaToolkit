using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class RTPC
    {
        public uint id { get; set; }
        public byte type { get; set; } //0x00 = GameParameter, 0x02 = Modulaltor
        public byte accumulator { get; set; } //0x00 = Exclusive, 0x01 = Additive
        public byte paramId { get; set; } //0x00 = Volume, 0x01 = LFE, 0x02 = Pitch, 0x03 = LPF, 0x04 = HPF, 0x05 = BusVolume, 0x0A = PositioningType, 0x1B = BypassFX3, 0x1E = FeedbackLowpass, 0x1F = FeedbackPitch, 0x29 = PlaybackSpeed, 0x2A = ModulatorLfoDepth, 0x3C = OutputBusVolume
        public uint curveId { get; set; }
        public byte scaling { get; set; } //0x00 = None, 0x02 = dB
        public List<GraphPoint> graphPoints { get; set; }
        public RTPC(BinaryReader br)
        {
            id = br.ReadUInt32();
            type = br.ReadByte();
            accumulator = br.ReadByte();
            paramId = br.ReadByte();
            curveId = br.ReadUInt32();
            scaling = br.ReadByte();
            int graphCount = br.ReadUInt16();
            graphPoints = new List<GraphPoint>();

            for (int i = 0; i < graphCount; i++)
            {
                graphPoints.Add(new GraphPoint(br));
            }
        }

        public RTPC()
        {
            id = 0;
            type = 0;
            accumulator = 0;
            paramId = 0;
            curveId = 0;
            scaling = 0;
            graphPoints = new List<GraphPoint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(type);
            bw.Write(accumulator);
            bw.Write(paramId);
            bw.Write(curveId);
            bw.Write(scaling);
            bw.Write((short)graphPoints.Count);

            foreach (GraphPoint point in graphPoints)
            {
                point.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int rtpcLength = 14 + graphPoints.Count * 12;

            return rtpcLength;
        }
    }
}
