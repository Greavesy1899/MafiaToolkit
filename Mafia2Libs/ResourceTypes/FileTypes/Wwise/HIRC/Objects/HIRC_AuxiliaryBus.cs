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
        public HIRCObject Parent { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public Bus BusData { get; set; }
        public AuxiliaryBus(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Parent = ParentObject;
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            BusData = new Bus(Parent, br, Length);
        }

        public AuxiliaryBus(HIRCObject ParentObject)
        {
            Parent = ParentObject;
            Type = 0;
            ID = 0;
            BusData = new Bus(Parent);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            BusData.WriteToFile(bw);
        }

        public int GetLength()
        {
            int Length = 4 + BusData.GetLength();

            return Length;
        }
    }
}
