using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise.Helpers;
using Utils;

namespace ResourceTypes.Wwise.Objects
{
    public class FeedbackBus
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public Bus feedbackBusData { get; set; }
        public FeedbackBus(HIRCObject parentObject, BinaryReader br, int iType)
        {
            parent = parentObject;
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            feedbackBusData = new Bus(parent,br, length);
        }

        public FeedbackBus(HIRCObject parentObject)
        {
            parent = parentObject;
            type = 0;
            id = 0;
            feedbackBusData = new Bus(parentObject);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            feedbackBusData.WriteToFile(bw);
        }

        public int GetLength()
        {
            int length = 4 + feedbackBusData.GetLength();

            return length;
        }
    }
}
