using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class SwitchGroup
    {
        public uint id { get; set; }
        public List<uint> items { get; set; } //ulStateID
        public SwitchGroup(BinaryReader br)
        {
            id = br.ReadUInt32();
            uint itemCount = br.ReadUInt32();
            items = new List<uint>();

            for (int i = 0; i < itemCount; i++)
            {
                uint id = br.ReadUInt32();
                items.Add(id);
            }
        }

        public SwitchGroup()
        {
            id = 0;
            items = new List<uint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(items.Count);

            foreach (uint item in items)
            {
                bw.Write(item);
            }
        }

        public int GetLength()
        {
            int length = 8 + items.Count * 4;

            return length;
        }
    }
}
