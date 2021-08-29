using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class StateProp
    {
        public int id { get; set; }
        public int accumulatorType { get; set; }
        public int inDb { get; set; }
        public StateProp(BinaryReader br)
        {
            id = br.ReadByte();
            accumulatorType = br.ReadByte();
            inDb = br.ReadByte();
        }

        public StateProp()
        {
            id = 0;
            accumulatorType = 0;
            inDb = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)id);
            bw.Write((byte)accumulatorType);
            bw.Write((byte)inDb);
        }
    }
}
