using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class Layer
    {
        public uint id { get; set; }
        public List<RTPC> rtpc { get; set; } //0x00 = Exclusive, 0x01 = Additive
        public uint rtpcId { get; set; }
        public byte rtpcType { get; set; }
        public List<Assoc> associates { get; set; }
        public Layer(BinaryReader br)
        {
            id = br.ReadUInt32();
            int rtpcCount = br.ReadUInt16();
            rtpc = new List<RTPC>();

            for (int i = 0; i < rtpcCount; i++)
            {
                rtpc.Add(new RTPC(br));
            }

            rtpcId = br.ReadUInt32();
            rtpcType = br.ReadByte();
            associates = new List<Assoc>();
            uint numAssoc = br.ReadUInt32();

            for (int i = 0; i < numAssoc; i++)
            {
                associates.Add(new Assoc(br));
            }
        }

        public Layer()
        {
            id = 0;
            rtpc = new List<RTPC>();
            rtpcId = 0;
            rtpcType = 0;
            associates = new List<Assoc>();
        }

        public static void WriteLayer(BinaryWriter bw, Layer layer)
        {
            bw.Write(layer.id);
            bw.Write((short)layer.rtpc.Count);

            foreach (RTPC value in layer.rtpc)
            {
                value.WriteToFile(bw);
            }

            bw.Write(layer.rtpcId);
            bw.Write(layer.rtpcType);
            bw.Write(layer.associates.Count);
            
            foreach (Assoc assoc in layer.associates)
            {
                Assoc.WriteAssoc(bw, assoc);
            }
        }

        public int GetLength()
        {
            int length = 15;

            foreach (RTPC value in rtpc)
            {
                length += value.GetLength();
            }

            foreach (Assoc value in associates)
            {
                length += value.GetLength();
            }

            return length;
        }
    }
}
