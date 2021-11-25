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
    public class FeedbackNode
    {
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public List<FeedbackSource> FeedbackSources { get; set; }
        public NodeBase NodeBase { get; set; }
        public FeedbackNode(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            FeedbackSources = new List<FeedbackSource>();
            uint numSources = br.ReadUInt32();

            for (int i = 0; i < numSources; i++)
            {
                FeedbackSources.Add(new FeedbackSource(br));
            }

            NodeBase = new NodeBase(br, ParentObject);
        }

        public FeedbackNode(HIRCObject ParentObject)
        {
            Type = 0;
            ID = 0;
            FeedbackSources = new List<FeedbackSource>();
            NodeBase = new NodeBase(ParentObject);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write(FeedbackSources.Count);

            foreach (FeedbackSource source in FeedbackSources)
            {
                source.WriteToFile(bw);
            }

            NodeBase.WriteToFile(bw);
        }

        public int GetLength()
        {
            int Length = 8 + NodeBase.GetLength() + FeedbackSources.Count * 26;

            return Length;
        }
    }
}
