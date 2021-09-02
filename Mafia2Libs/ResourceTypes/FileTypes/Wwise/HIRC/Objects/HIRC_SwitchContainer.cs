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
        public int Type { get; set; }
        public uint ID { get; set; }
        public NodeBase NodeBase { get; set; }
        public byte GroupType { get; set; }
        public uint GroupID { get; set; }
        public uint DefaultSwitch { get; set; }
        public int ContinuousValidation { get; set; }
        public List<uint> ChildIDs { get; set; } //IDs of child HIRC objects
        public List<SwitchGroup> SwitchGroups { get; set; }
        public List<SwitchParam> SwitchParams { get; set; }
        public SwitchContainer(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            NodeBase = new NodeBase(br, ParentObject);
            GroupType = br.ReadByte();
            GroupID = br.ReadUInt32();
            DefaultSwitch = br.ReadUInt32();
            ContinuousValidation = br.ReadByte();
            ChildIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint Key = br.ReadUInt32();
                ChildIDs.Add(Key);
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

        public SwitchContainer(HIRCObject ParentObject)
        {
            Type = 0;
            ID = 0;
            NodeBase = new NodeBase(ParentObject);
            GroupType = 0;
            GroupID = 0;
            DefaultSwitch = 0;
            ContinuousValidation = 0;
            ChildIDs = new List<uint>();
            SwitchGroups = new List<SwitchGroup>();
            SwitchParams = new List<SwitchParam>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);

            NodeBase.WriteToFile(bw);

            bw.Write(GroupType);
            bw.Write(GroupID);
            bw.Write(DefaultSwitch);
            bw.Write((byte)ContinuousValidation);
            bw.Write(ChildIDs.Count);

            foreach (uint child in ChildIDs)
            {
                bw.Write(child);
            }

            bw.Write(SwitchGroups.Count);

            foreach (SwitchGroup Group in SwitchGroups)
            {
                Group.WriteToFile(bw);
            }

            bw.Write(SwitchParams.Count);

            foreach (SwitchParam param in SwitchParams)
            {
                param.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int Length = 26 + NodeBase.GetLength() + ChildIDs.Count * 4 + SwitchParams.Count * 14;

            foreach (SwitchGroup Group in SwitchGroups)
            {
                Length += Group.GetLength();
            }

            return Length;
        }
    }
}
