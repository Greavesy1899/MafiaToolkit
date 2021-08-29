using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    public class TransitionRule
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        public List<TransitionSrcRule> sources { get; set; }
        public int sourceId { get; set; }
        public List<TransitionDstRule> destinations { get; set; }
        public int destinationId { get; set; }
        public byte allocTransObjectFlag { get; set; }
        public TransitionObject transitionObject { get; set; }
        public TransitionRule(BinaryReader br, HIRCObject parentObject)
        {
            parent = parentObject;
            sources = new List<TransitionSrcRule>();
            destinations = new List<TransitionDstRule>();
            uint numSrcs = br.ReadUInt32();
            sourceId = br.ReadInt32();
            uint numDsts = br.ReadUInt32();
            destinationId = br.ReadInt32();

            for (int i = 0; i < numSrcs; i++)
            {
                sources.Add(new TransitionSrcRule(br));
            }

            for (int i = 0; i < numDsts; i++)
            {
                destinations.Add(new TransitionDstRule(br, parent));
            }

            allocTransObjectFlag = br.ReadByte();

            if (allocTransObjectFlag == 1)
            {
                transitionObject = new TransitionObject(br);
            }
            else if (allocTransObjectFlag != 1 && allocTransObjectFlag != 0)
            {
                long testOffset = br.BaseStream.Position;
                MessageBox.Show("allocTransObjectFlag != 0 nor 1 at: " + testOffset.ToString("X"));
            }
            else
            {
                transitionObject = new TransitionObject();
            }
        }

        public TransitionRule()
        {
            sources = new List<TransitionSrcRule>();
            destinations = new List<TransitionDstRule>();
            sourceId = 0;
            destinationId = 0;
            allocTransObjectFlag = 0;
            transitionObject = new TransitionObject();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(sources.Count);
            bw.Write(sourceId);
            bw.Write(destinations.Count);
            bw.Write(destinationId);

            foreach (TransitionSrcRule src in sources)
            {
                bw.Write(src.transitionTime);
                bw.Write(src.fadeCurve);
                bw.Write(src.fadeOffset);
                bw.Write(src.syncType);
                bw.Write(src.cueFilterHash);
                bw.Write((byte)src.playPostExit);
            }

            foreach (TransitionDstRule dst in destinations)
            {
                bw.Write(dst.transitionTime);
                bw.Write(dst.fadeCurve);
                bw.Write(dst.fadeOffset);
                bw.Write(dst.cueFilterHash);
                bw.Write(dst.jumpToID);
                bw.Write((short)dst.entryType);
                bw.Write((byte)dst.playPreEntry);
                bw.Write(dst.destMatchSourceCueName);
            }

            bw.Write(allocTransObjectFlag);

            if (allocTransObjectFlag == 1)
            {
                bw.Write(transitionObject.id);
                bw.Write(transitionObject.fadeInTransitionTime);
                bw.Write(transitionObject.fadeInFadeCurve);
                bw.Write(transitionObject.fadeInFadeOffset);
                bw.Write(transitionObject.fadeOutTransitionTime);
                bw.Write(transitionObject.fadeOutFadeCurve);
                bw.Write(transitionObject.fadeOutFadeOffset);
                bw.Write((byte)transitionObject.playPreEntry);
                bw.Write((byte)transitionObject.playPostExit);
            }
        }

        public int GetLength()
        {
            int ruleLength = 17 + sources.Count * 21 + destinations.Count * 24;

            if (allocTransObjectFlag == 1)
            {
                ruleLength += transitionObject.GetLength();
            }

            return ruleLength;
        }
    }

    public class TransitionSrcRule
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        public uint transitionTime { get; set; }
        public uint fadeCurve { get; set; }
        public uint fadeOffset { get; set; }
        public uint syncType { get; set; }
        public uint cueFilterHash { get; set; }
        public int playPostExit { get; set; }
        public TransitionSrcRule(BinaryReader br)
        {
            transitionTime = br.ReadUInt32();
            fadeCurve = br.ReadUInt32();
            fadeOffset = br.ReadUInt32();
            syncType = br.ReadUInt32();
            cueFilterHash = br.ReadUInt32();
            playPostExit = br.ReadByte();
        }

        public TransitionSrcRule()
        {
            transitionTime = 0;
            fadeCurve = 0;
            fadeOffset = 0;
            syncType = 0;
            cueFilterHash = 0;
            playPostExit = 0;
        }
    }

    public class TransitionDstRule
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        public uint transitionTime { get; set; }
        public uint fadeCurve { get; set; }
        public uint fadeOffset { get; set; }
        public uint cueFilterHash { get; set; }
        public uint jumpToID { get; set; }
        public uint entryType { get; set; } //0x01 = SameTime
        public int playPreEntry { get; set; }
        public byte destMatchSourceCueName { get; set; }
        public TransitionDstRule(BinaryReader br, HIRCObject parentObject)
        {
            parent = parentObject;
            transitionTime = br.ReadUInt32();
            fadeCurve = br.ReadUInt32();
            fadeOffset = br.ReadUInt32();
            cueFilterHash = br.ReadUInt32();
            jumpToID = br.ReadUInt32();
            entryType = br.ReadUInt16();
            playPreEntry = br.ReadByte();
            destMatchSourceCueName = br.ReadByte();
        }

        public TransitionDstRule()
        {
            transitionTime = 0;
            fadeCurve = 0;
            fadeOffset = 0;
            cueFilterHash = 0;
            jumpToID = 0;
            entryType = 0;
            playPreEntry = 0;
            destMatchSourceCueName = 0;
        }
    }

    public class TransitionObject
    {
        [System.ComponentModel.Browsable(false)]
        private HIRCObject parent { get; set; }
        public uint id { get; set; }
        public uint fadeInTransitionTime { get; set; }
        public uint fadeInFadeCurve { get; set; }
        public uint fadeInFadeOffset { get; set; }
        public uint fadeOutTransitionTime { get; set; }
        public uint fadeOutFadeCurve { get; set; }
        public uint fadeOutFadeOffset { get; set; }
        public int playPreEntry { get; set; }
        public int playPostExit { get; set; }
        public TransitionObject(BinaryReader br)
        {
            id = br.ReadUInt32();
            fadeInTransitionTime = br.ReadUInt32();
            fadeInFadeCurve = br.ReadUInt32();
            fadeInFadeOffset = br.ReadUInt32();
            fadeOutTransitionTime = br.ReadUInt32();
            fadeOutFadeCurve = br.ReadUInt32();
            fadeOutFadeOffset = br.ReadUInt32();
            playPreEntry = br.ReadByte();
            playPostExit = br.ReadByte();
        }

        public TransitionObject()
        {
            id = 0;
            fadeInTransitionTime = 0;
            fadeInFadeCurve = 0;
            fadeInFadeOffset = 0;
            fadeOutTransitionTime = 0;
            fadeOutFadeCurve = 0;
            fadeOutFadeOffset = 0;
            playPreEntry = 0;
            playPostExit = 0;
        }

        public int GetLength()
        {
            int objLength = 30;
            return objLength;
        }
    }
}
