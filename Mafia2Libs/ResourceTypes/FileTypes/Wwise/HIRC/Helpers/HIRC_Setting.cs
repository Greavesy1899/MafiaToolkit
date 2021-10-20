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
    public class Setting
    {
        public int ID { get; set; }
        public float Value { get; set; }
        public Setting(int iID)
        {
            ID = iID;
        }

        public Setting()
        {
            ID = 0;
            Value = 0;
        }
    }
}
