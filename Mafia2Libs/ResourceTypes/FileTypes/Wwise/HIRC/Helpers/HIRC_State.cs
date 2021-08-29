using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Helpers
{
    public class State
    {
        public uint id { get; set; }
        public uint instanceId { get; set; }
        public State(BinaryReader br)
        {
            id = br.ReadUInt32();
            instanceId = br.ReadUInt32();
        }

        public State()
        {
            id = 0;
            instanceId = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(instanceId);
        }
    }
}
