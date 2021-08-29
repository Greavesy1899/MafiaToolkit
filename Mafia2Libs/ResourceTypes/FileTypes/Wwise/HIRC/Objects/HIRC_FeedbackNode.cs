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
        public int type { get; set; }
        public uint id { get; set; }
        public List<FeedbackSource> feedbackSources { get; set; }
        public NodeBase nodeBase { get; set; }
        public FeedbackNode(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            feedbackSources = new List<FeedbackSource>();
            uint numSources = br.ReadUInt32();

            for (int i = 0; i < numSources; i++)
            {
                feedbackSources.Add(new FeedbackSource(br));
            }

            nodeBase = new NodeBase(br, parentObject);
        }

        public FeedbackNode(HIRCObject parentObject)
        {
            type = 0;
            id = 0;
            feedbackSources = new List<FeedbackSource>();
            nodeBase = new NodeBase(parentObject);
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write(feedbackSources.Count);

            foreach (FeedbackSource source in feedbackSources)
            {
                source.WriteToFile(bw);
            }

            nodeBase.WriteToFile(bw);
        }

        public int GetLength()
        {
            int length = 8 + nodeBase.GetLength() + feedbackSources.Count * 26;

            return length;
        }
    }
}
