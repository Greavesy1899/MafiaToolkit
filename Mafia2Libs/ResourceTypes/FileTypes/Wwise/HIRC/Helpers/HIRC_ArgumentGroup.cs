using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ArgumentGroup
    {
        public uint ID { get; set; }
        public int Value { get; set; }
        public ArgumentGroup(uint iID, int iValue)
        {
            ID = iID;
            Value = iValue;
        }

        public ArgumentGroup()
        {
            ID = 0;
            Value = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write((byte)Value);
        }
    }
}
