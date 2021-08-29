using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Objects
{
    public class Event
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        [System.ComponentModel.Browsable(false)]
        private HIRCObject parent { get; set; }
        public uint id { get; set; }
        public List<uint> actionIDs { get; set; }
        public Event(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            parent = parentObject;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            int actionsCount = br.ReadInt32();
            actionIDs = new List<uint>();

            for (uint i = 0; i < actionsCount; i++)
            {
                actionIDs.Add(br.ReadUInt32());
            }
        }

        public Event(HIRCObject parentObject)
        {
            type = 0;
            parent = parentObject;
            id = 0;
            actionIDs = new List<uint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write(actionIDs.Count);

            foreach (uint action in actionIDs)
            {
                bw.Write(action);
            }
        }

        public int GetLength()
        {
            int length = 8 + actionIDs.Count * 4;
            return length;
        }
    }
}
