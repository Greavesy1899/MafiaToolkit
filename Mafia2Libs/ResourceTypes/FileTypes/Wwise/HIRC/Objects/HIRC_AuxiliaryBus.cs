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
    public class AuxiliaryBus
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public Bus audioBusData { get; set; }
        public AuxiliaryBus(HIRCObject parentObject, BinaryReader br, int iType)
        {
            parent = parentObject;
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            audioBusData = new Bus(parent, br, length);
        }

        public AuxiliaryBus(HIRCObject parentObject)
        {
            parent = parentObject;
            type = 0;
            id = 0;
            audioBusData = new Bus(parent);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            audioBusData.WriteToFile(bw);
        }

        public int GetLength()
        {
            int length = 4 + audioBusData.GetLength();

            return length;
        }
    }
}
