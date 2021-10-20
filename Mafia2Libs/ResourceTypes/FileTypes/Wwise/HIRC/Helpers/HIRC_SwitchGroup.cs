using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SwitchGroup
    {
        public uint ID { get; set; }
        public List<uint> Items { get; set; } //ulStateid
        public SwitchGroup(BinaryReader br)
        {
            ID = br.ReadUInt32();
            uint itemCount = br.ReadUInt32();
            Items = new List<uint>();

            for (int i = 0; i < itemCount; i++)
            {
                uint ID = br.ReadUInt32();
                Items.Add(ID);
            }
        }

        public SwitchGroup()
        {
            ID = 0;
            Items = new List<uint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Items.Count);

            foreach (uint item in Items)
            {
                bw.Write(item);
            }
        }

        public int GetLength()
        {
            int Length = 8 + Items.Count * 4;

            return Length;
        }
    }
}
