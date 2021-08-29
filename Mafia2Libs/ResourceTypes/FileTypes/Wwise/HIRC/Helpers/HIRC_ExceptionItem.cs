using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class ExceptionItem
    {
        public uint id { get; set; }
        public int isBus { get; set; }

        public ExceptionItem(BinaryReader br)
        {
            id = br.ReadUInt32();
            isBus = br.ReadByte();
        }

        public ExceptionItem()
        {
            id = 0;
            isBus = 0;
        }
    }
}
