using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class ArgumentGroup
    {
        public uint id { get; set; }
        public int value { get; set; }
        public ArgumentGroup(uint iId, int iValue)
        {
            id = iId;
            value = iValue;
        }

        public ArgumentGroup()
        {
            id = 0;
            value = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write((byte)value);
        }
    }
}
