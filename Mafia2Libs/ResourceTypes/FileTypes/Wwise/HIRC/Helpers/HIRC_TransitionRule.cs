using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TransitionRule
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject Parent { get; set; }
        public List<TransitionSrcRule> Sources { get; set; }
        public int SourceID { get; set; }
        public List<TransitionDstRule> Destinations { get; set; }
        public int DestinationID { get; set; }
        public byte AllocTransObjectFlag { get; set; }
        public TransitionObject TransitionObject { get; set; }
        public TransitionRule(BinaryReader br, HIRCObject ParentObject)
        {
            Parent = ParentObject;
            Sources = new List<TransitionSrcRule>();
            Destinations = new List<TransitionDstRule>();
            uint numSrcs = br.ReadUInt32();
            SourceID = br.ReadInt32();
            uint numDsts = br.ReadUInt32();
            DestinationID = br.ReadInt32();

            for (int i = 0; i < numSrcs; i++)
            {
                Sources.Add(new TransitionSrcRule(br));
            }

            for (int i = 0; i < numDsts; i++)
            {
                Destinations.Add(new TransitionDstRule(br, Parent));
            }

            AllocTransObjectFlag = br.ReadByte();

            if (AllocTransObjectFlag == 1)
            {
                TransitionObject = new TransitionObject(br);
            }
            else if (AllocTransObjectFlag != 1 && AllocTransObjectFlag != 0)
            {
                long testOffset = br.BaseStream.Position;
                MessageBox.Show("AllocTransObjectFlag != 0 nor 1 at: " + testOffset.ToString("X"));
            }
            else
            {
                TransitionObject = new TransitionObject();
            }
        }

        public TransitionRule()
        {
            Sources = new List<TransitionSrcRule>();
            Destinations = new List<TransitionDstRule>();
            SourceID = 0;
            DestinationID = 0;
            AllocTransObjectFlag = 0;
            TransitionObject = new TransitionObject();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(Sources.Count);
            bw.Write(SourceID);
            bw.Write(Destinations.Count);
            bw.Write(DestinationID);

            foreach (TransitionSrcRule src in Sources)
            {
                bw.Write(src.TransitionTime);
                bw.Write(src.FadeCurve);
                bw.Write(src.FadeOffset);
                bw.Write(src.SyncType);
                bw.Write(src.CueFilterHash);
                bw.Write((byte)src.PlayPostExit);
            }

            foreach (TransitionDstRule dst in Destinations)
            {
                bw.Write(dst.TransitionTime);
                bw.Write(dst.FadeCurve);
                bw.Write(dst.FadeOffset);
                bw.Write(dst.CueFilterHash);
                bw.Write(dst.JumpToID);
                bw.Write((short)dst.EntryType);
                bw.Write((byte)dst.PlayPreEntry);
                bw.Write(dst.DestMatchSourceCueName);
            }

            bw.Write(AllocTransObjectFlag);

            if (AllocTransObjectFlag == 1)
            {
                bw.Write(TransitionObject.ID);
                bw.Write(TransitionObject.FadeInTransitionTime);
                bw.Write(TransitionObject.FadeInFadeCurve);
                bw.Write(TransitionObject.FadeInFadeOffset);
                bw.Write(TransitionObject.FadeOutTransitionTime);
                bw.Write(TransitionObject.FadeOutFadeCurve);
                bw.Write(TransitionObject.FadeOutFadeOffset);
                bw.Write((byte)TransitionObject.PlayPreEntry);
                bw.Write((byte)TransitionObject.PlayPostExit);
            }
        }

        public int GetLength()
        {
            int ruleLength = 17 + Sources.Count * 21 + Destinations.Count * 24;

            if (AllocTransObjectFlag == 1)
            {
                ruleLength += TransitionObject.GetLength();
            }

            return ruleLength;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TransitionSrcRule
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject Parent { get; set; }
        public uint TransitionTime { get; set; }
        public uint FadeCurve { get; set; }
        public uint FadeOffset { get; set; }
        public uint SyncType { get; set; }
        public uint CueFilterHash { get; set; }
        public int PlayPostExit { get; set; }
        public TransitionSrcRule(BinaryReader br)
        {
            TransitionTime = br.ReadUInt32();
            FadeCurve = br.ReadUInt32();
            FadeOffset = br.ReadUInt32();
            SyncType = br.ReadUInt32();
            CueFilterHash = br.ReadUInt32();
            PlayPostExit = br.ReadByte();
        }

        public TransitionSrcRule()
        {
            TransitionTime = 0;
            FadeCurve = 0;
            FadeOffset = 0;
            SyncType = 0;
            CueFilterHash = 0;
            PlayPostExit = 0;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TransitionDstRule
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject Parent { get; set; }
        public uint TransitionTime { get; set; }
        public uint FadeCurve { get; set; }
        public uint FadeOffset { get; set; }
        public uint CueFilterHash { get; set; }
        public uint JumpToID { get; set; }
        public uint EntryType { get; set; } //0x01 = SameTime
        public int PlayPreEntry { get; set; }
        public byte DestMatchSourceCueName { get; set; }
        public TransitionDstRule(BinaryReader br, HIRCObject ParentObject)
        {
            Parent = ParentObject;
            TransitionTime = br.ReadUInt32();
            FadeCurve = br.ReadUInt32();
            FadeOffset = br.ReadUInt32();
            CueFilterHash = br.ReadUInt32();
            JumpToID = br.ReadUInt32();
            EntryType = br.ReadUInt16();
            PlayPreEntry = br.ReadByte();
            DestMatchSourceCueName = br.ReadByte();
        }

        public TransitionDstRule()
        {
            TransitionTime = 0;
            FadeCurve = 0;
            FadeOffset = 0;
            CueFilterHash = 0;
            JumpToID = 0;
            EntryType = 0;
            PlayPreEntry = 0;
            DestMatchSourceCueName = 0;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TransitionObject
    {
        [System.ComponentModel.Browsable(false)]
        private HIRCObject Parent { get; set; }
        public uint ID { get; set; }
        public uint FadeInTransitionTime { get; set; }
        public uint FadeInFadeCurve { get; set; }
        public uint FadeInFadeOffset { get; set; }
        public uint FadeOutTransitionTime { get; set; }
        public uint FadeOutFadeCurve { get; set; }
        public uint FadeOutFadeOffset { get; set; }
        public int PlayPreEntry { get; set; }
        public int PlayPostExit { get; set; }
        public TransitionObject(BinaryReader br)
        {
            ID = br.ReadUInt32();
            FadeInTransitionTime = br.ReadUInt32();
            FadeInFadeCurve = br.ReadUInt32();
            FadeInFadeOffset = br.ReadUInt32();
            FadeOutTransitionTime = br.ReadUInt32();
            FadeOutFadeCurve = br.ReadUInt32();
            FadeOutFadeOffset = br.ReadUInt32();
            PlayPreEntry = br.ReadByte();
            PlayPostExit = br.ReadByte();
        }

        public TransitionObject()
        {
            ID = 0;
            FadeInTransitionTime = 0;
            FadeInFadeCurve = 0;
            FadeInFadeOffset = 0;
            FadeOutTransitionTime = 0;
            FadeOutFadeCurve = 0;
            FadeOutFadeOffset = 0;
            PlayPreEntry = 0;
            PlayPostExit = 0;
        }

        public int GetLength()
        {
            int objLength = 30;
            return objLength;
        }
    }
}
