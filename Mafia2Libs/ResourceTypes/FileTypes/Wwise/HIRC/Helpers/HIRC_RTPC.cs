using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RTPC
    {
        public uint ID { get; set; }
        public byte Type { get; set; } //0x00 = GameParameter, 0x02 = Modulaltor
        public byte Accumulator { get; set; } //0x00 = Exclusive, 0x01 = Additive
        public byte ParameterID { get; set; } //0x00 = Volume, 0x01 = LFE, 0x02 = Pitch, 0x03 = LPF, 0x04 = HPF, 0x05 = BusVolume, 0x0A = PositioningType, 0x1B = BypassFX3, 0x1E = FeedbackLowpass, 0x1F = FeedbackPitch, 0x29 = PlaybackSpeed, 0x2A = ModulatorLfoDepth, 0x3C = OutputBusVolume
        public uint CurveID { get; set; }
        public byte Scaling { get; set; } //0x00 = None, 0x02 = dB
        public List<GraphPoint> GraphPoints { get; set; }
        public RTPC(BinaryReader br)
        {
            ID = br.ReadUInt32();
            Type = br.ReadByte();
            Accumulator = br.ReadByte();
            ParameterID = br.ReadByte();
            CurveID = br.ReadUInt32();
            Scaling = br.ReadByte();
            int graphCount = br.ReadUInt16();
            GraphPoints = new List<GraphPoint>();

            for (int i = 0; i < graphCount; i++)
            {
                GraphPoints.Add(new GraphPoint(br));
            }
        }

        public RTPC()
        {
            ID = 0;
            Type = 0;
            Accumulator = 0;
            ParameterID = 0;
            CurveID = 0;
            Scaling = 0;
            GraphPoints = new List<GraphPoint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Type);
            bw.Write(Accumulator);
            bw.Write(ParameterID);
            bw.Write(CurveID);
            bw.Write(Scaling);
            bw.Write((short)GraphPoints.Count);

            foreach (GraphPoint point in GraphPoints)
            {
                point.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int rtpcLength = 14 + GraphPoints.Count * 12;

            return rtpcLength;
        }
    }
}
