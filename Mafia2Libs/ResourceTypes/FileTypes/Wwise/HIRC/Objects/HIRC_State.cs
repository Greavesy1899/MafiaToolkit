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
        public int type { get; set; }
        public uint id { get; set; }
        public List<Prop> props { get; set; }
        public State(BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            props = new List<Prop>();
            uint numProps = br.ReadUInt16();

            for (int i = 0; i < numProps; i++)
            {
                int id = br.ReadInt16();
                props.Add(new Prop(id));
            }

            foreach (Prop prop in props)
            {
                uint value = br.ReadUInt32();
                prop.value = value;
            }
        }

        public State()
        {
            type = 0;
            id = 0;
            props = new List<Prop>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write((short)props.Count);

            foreach (Prop prop in props)
            {
                bw.Write((short)prop.id);
            }

            foreach (Prop prop in props)
            {
                bw.Write(prop.value);
            }
        }

        public int GetLength()
        {
            int length = 6 + props.Count * 6;

            return length;
        }
    }
}
