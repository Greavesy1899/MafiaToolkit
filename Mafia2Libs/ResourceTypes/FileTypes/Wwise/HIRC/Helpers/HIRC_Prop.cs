using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class Prop
    {
        public int id { get; set; }
        public uint value { get; set; }
        public Prop(int iId)
        {
            id = iId;
        }

        public Prop()
        {
            id = 0;
            value = 0;
        }
    }
}
