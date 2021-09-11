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
    public class ExceptionItem
    {
        public uint ID { get; set; }
        public int IsBus { get; set; }

        public ExceptionItem(BinaryReader br)
        {
            ID = br.ReadUInt32();
            IsBus = br.ReadByte();
        }

        public ExceptionItem()
        {
            ID = 0;
            IsBus = 0;
        }
    }
}
