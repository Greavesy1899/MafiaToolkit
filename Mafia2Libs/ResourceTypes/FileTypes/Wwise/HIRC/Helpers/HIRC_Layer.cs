using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Layer
    {
        public uint ID { get; set; }
        public List<RTPC> rtpc { get; set; } //0x00 = Exclusive, 0x01 = Additive
        public uint rtpcID { get; set; }
        public byte rtpcType { get; set; }
        public List<Assoc> Associates { get; set; }
        public Layer(BinaryReader br)
        {
            ID = br.ReadUInt32();
            int rtpcCount = br.ReadUInt16();
            rtpc = new List<RTPC>();

            for (int i = 0; i < rtpcCount; i++)
            {
                rtpc.Add(new RTPC(br));
            }

            rtpcID = br.ReadUInt32();
            rtpcType = br.ReadByte();
            Associates = new List<Assoc>();
            uint numAssoc = br.ReadUInt32();

            for (int i = 0; i < numAssoc; i++)
            {
                Associates.Add(new Assoc(br));
            }
        }

        public Layer()
        {
            ID = 0;
            rtpc = new List<RTPC>();
            rtpcID = 0;
            rtpcType = 0;
            Associates = new List<Assoc>();
        }

        public static void WriteLayer(BinaryWriter bw, Layer layer)
        {
            bw.Write(layer.ID);
            bw.Write((short)layer.rtpc.Count);

            foreach (RTPC value in layer.rtpc)
            {
                value.WriteToFile(bw);
            }

            bw.Write(layer.rtpcID);
            bw.Write(layer.rtpcType);
            bw.Write(layer.Associates.Count);
            
            foreach (Assoc assoc in layer.Associates)
            {
                Assoc.WriteAssoc(bw, assoc);
            }
        }

        public int GetLength()
        {
            int Length = 15;

            foreach (RTPC value in rtpc)
            {
                Length += value.GetLength();
            }

            foreach (Assoc value in Associates)
            {
                Length += value.GetLength();
            }

            return Length;
        }
    }
}
