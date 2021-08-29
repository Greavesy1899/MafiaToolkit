using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise.Helpers;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Objects
{
    public class ActorMixer
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public NodeBase nodeBase { get; set; }
        public List<uint> childIDs { get; set; } //IDs of child HIRC objects
        public ActorMixer(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            nodeBase = new NodeBase(br, parentObject);
            childIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint key = br.ReadUInt32();
                childIDs.Add(key);
            }
        }

        public ActorMixer(HIRCObject parentObject)
        {
            type = 0;
            id = 0;
            nodeBase = new NodeBase(parentObject);
            childIDs = new List<uint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);

            nodeBase.WriteToFile(bw);

            bw.Write(childIDs.Count);

            foreach (uint child in childIDs)
            {
                bw.Write(child);
            }
        }

        public int GetLength()
        {
            int length = 8 + nodeBase.GetLength() + childIDs.Count * 4;

            return length;
        }
    }
}
