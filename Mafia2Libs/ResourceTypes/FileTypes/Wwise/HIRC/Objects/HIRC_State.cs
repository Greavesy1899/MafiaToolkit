using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise.Helpers;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Objects
{
    public class State
    {
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public List<Prop> Props { get; set; }
        public State(BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            Props = new List<Prop>();
            uint numProps = br.ReadUInt16();

            for (int i = 0; i < numProps; i++)
            {
                int ID = br.ReadInt16();
                Props.Add(new Prop(ID));
            }

            foreach (Prop prop in Props)
            {
                uint value = br.ReadUInt32();
                prop.Value = value;
            }
        }

        public State()
        {
            Type = 0;
            ID = 0;
            Props = new List<Prop>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write((short)Props.Count);

            foreach (Prop prop in Props)
            {
                bw.Write((short)prop.ID);
            }

            foreach (Prop prop in Props)
            {
                bw.Write(prop.Value);
            }
        }

        public int GetLength()
        {
            int Length = 6 + Props.Count * 6;

            return Length;
        }
    }
}
