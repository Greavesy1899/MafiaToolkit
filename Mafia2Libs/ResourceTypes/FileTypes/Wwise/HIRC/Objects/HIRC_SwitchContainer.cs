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
    public class SwitchContainer
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public NodeBase nodeBase { get; set; }
        public byte groupType { get; set; }
        public uint groupId { get; set; }
        public uint defaultSwitch { get; set; }
        public int continuousValidation { get; set; }
        public List<uint> childIDs { get; set; } //IDs of child HIRC objects
        public List<SwitchGroup> SwitchGroups { get; set; }
        public List<SwitchParam> SwitchParams { get; set; }
        public SwitchContainer(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            nodeBase = new NodeBase(br, parentObject);
            groupType = br.ReadByte();
            groupId = br.ReadUInt32();
            defaultSwitch = br.ReadUInt32();
            continuousValidation = br.ReadByte();
            childIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint key = br.ReadUInt32();
                childIDs.Add(key);
            }

            SwitchGroups = new List<SwitchGroup>();
            uint numSwitchGroups = br.ReadUInt32();

            for (int i = 0; i < numSwitchGroups; i++)
            {
                SwitchGroups.Add(new SwitchGroup(br));
            }

            SwitchParams = new List<SwitchParam>();
            uint numSwitchParams = br.ReadUInt32();

            for (int i = 0; i < numSwitchParams; i++)
            {
                SwitchParams.Add(new SwitchParam(br));
            }
        }

        public SwitchContainer(HIRCObject parentObject)
        {
            type = 0;
            id = 0;
            nodeBase = new NodeBase(parentObject);
            groupType = 0;
            groupId = 0;
            defaultSwitch = 0;
            continuousValidation = 0;
            childIDs = new List<uint>();
            SwitchGroups = new List<SwitchGroup>();
            SwitchParams = new List<SwitchParam>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);

            nodeBase.WriteToFile(bw);

            bw.Write(groupType);
            bw.Write(groupId);
            bw.Write(defaultSwitch);
            bw.Write((byte)continuousValidation);
            bw.Write(childIDs.Count);

            foreach (uint child in childIDs)
            {
                bw.Write(child);
            }

            bw.Write(SwitchGroups.Count);

            foreach (SwitchGroup group in SwitchGroups)
            {
                group.WriteToFile(bw);
            }

            bw.Write(SwitchParams.Count);

            foreach (SwitchParam param in SwitchParams)
            {
                param.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int length = 26 + nodeBase.GetLength() + childIDs.Count * 4 + SwitchParams.Count * 14;

            foreach (SwitchGroup group in SwitchGroups)
            {
                length += group.GetLength();
            }

            return length;
        }
    }
}
