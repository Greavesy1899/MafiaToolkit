using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class Envelope
    {
        [Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public List<Prop> Props { get; set; }
        public List<RangedModifier> RangedModifiers { get; set; }
        public List<RTPC> rtpc { get; set; }
        [Browsable(false)]
        public byte[] Data { get; set; }
        public Envelope(BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            Props = new List<Prop>();
            int PropsCount = br.ReadByte();

            for (int i = 0; i < PropsCount; i++)
            {
                byte Key = br.ReadByte();
                Props.Add(new Prop(Key));
            }

            foreach (Prop prop in Props)
            {
                prop.Value = br.ReadUInt32();
            }

            RangedModifiers = new List<RangedModifier>();
            int RangedModifiersCount = br.ReadByte();

            for (int i = 0; i < RangedModifiersCount; i++)
            {
                byte ID = br.ReadByte();
                uint min = br.ReadUInt32();
                uint max = br.ReadUInt32();
                RangedModifiers.Add(new RangedModifier(ID, min, max));
            }

            rtpc = new List<RTPC>();
            int stateRTPCCount = br.ReadUInt16();

            for (int i = 0; i < stateRTPCCount; i++)
            {
                rtpc.Add(new RTPC(br));
            }
        }

        public Envelope()
        {
            Type = 0;
            ID = 0;
            Props = new List<Prop>();
            RangedModifiers = new List<RangedModifier>();
            rtpc = new List<RTPC>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write((byte)Props.Count);

            foreach (Prop prop in Props)
            {
                bw.Write((byte)prop.ID);
            }

            foreach (Prop prop in Props)
            {
                bw.Write(prop.Value);
            }

            bw.Write((byte)RangedModifiers.Count);

            foreach (RangedModifier mod in RangedModifiers)
            {
                bw.Write(mod.ID);
                bw.Write(mod.Min);
                bw.Write(mod.Max);
            }

            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int Length = 8 + Props.Count * 5 + RangedModifiers.Count * 9;

            foreach (RTPC value in rtpc)
            {
                Length += value.GetLength();
            }

            return Length;
        }
    }
}
