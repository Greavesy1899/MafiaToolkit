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
    public class FxShareSet
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        public uint id { get; set; }
        public FXBase fxBase { get; set; }
        [System.ComponentModel.Browsable(false)]
        public byte[] data { get; set; }
        public FxShareSet(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            parent = parentObject;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            fxBase = new FXBase(parent, br);
        }

        public FxShareSet(HIRCObject parentObject)
        {
            parent = parentObject;
            type = 0;
            id = 0;
            fxBase = new FXBase(parent);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            fxBase.WriteToFile(bw);
        }

        public int GetLength()
        {
            int length = 4 + fxBase.GetLength();

            return length;
        }
    }
}
