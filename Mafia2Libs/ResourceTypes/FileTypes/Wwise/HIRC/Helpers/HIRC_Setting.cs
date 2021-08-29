using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class Setting
    {
        public int id { get; set; }
        public float value { get; set; }
        public Setting(int iId)
        {
            id = iId;
        }

        public Setting()
        {
            id = 0;
            value = 0;
        }
    }
}
