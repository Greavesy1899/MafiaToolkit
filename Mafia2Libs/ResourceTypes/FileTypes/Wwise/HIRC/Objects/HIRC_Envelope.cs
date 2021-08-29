using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class Envelope
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public List<Prop> props { get; set; }
        public List<RangedModifier> rangedModifiers { get; set; }
        public List<RTPC> rtpc { get; set; }
        [System.ComponentModel.Browsable(false)]
        public byte[] data { get; set; }
        public Envelope(BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            props = new List<Prop>();
            int propsCount = br.ReadByte();

            for (int i = 0; i < propsCount; i++)
            {
                byte key = br.ReadByte();
                props.Add(new Prop(key));
            }

            foreach (Prop prop in props)
            {
                prop.value = br.ReadUInt32();
            }

            rangedModifiers = new List<RangedModifier>();
            int rangedModifiersCount = br.ReadByte();

            for (int i = 0; i < rangedModifiersCount; i++)
            {
                byte id = br.ReadByte();
                uint min = br.ReadUInt32();
                uint max = br.ReadUInt32();
                rangedModifiers.Add(new RangedModifier(id, min, max));
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
            type = 0;
            id = 0;
            props = new List<Prop>();
            rangedModifiers = new List<RangedModifier>();
            rtpc = new List<RTPC>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write((byte)props.Count);

            foreach (Prop prop in props)
            {
                bw.Write((byte)prop.id);
            }

            foreach (Prop prop in props)
            {
                bw.Write(prop.value);
            }

            bw.Write((byte)rangedModifiers.Count);

            foreach (RangedModifier mod in rangedModifiers)
            {
                bw.Write(mod.id);
                bw.Write(mod.min);
                bw.Write(mod.max);
            }

            bw.Write((short)rtpc.Count);

            foreach (RTPC value in rtpc)
            {
                value.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int length = 8 + props.Count * 5 + rangedModifiers.Count * 9;

            foreach (RTPC value in rtpc)
            {
                length += value.GetLength();
            }

            return length;
        }
    }
}
