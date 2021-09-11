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
    public class MusicSequence
    {
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public byte MusicFlags { get; set; } //bit1 = "bOverrideParentMidiTempo", bit2 = "bOverrideParentMidiTarget", bit3 = "bMidiTargetTypeBus" 
        public NodeBase NodeBase { get; set; }
        public List<uint> ChildIDs { get; set; } //IDs of child HIRC objects
        public double AKMeterGridPeriod { get; set; }
        public double AKMeterGridOffset { get; set; }
        public float AKMeterTempo { get; set; }
        public byte AKMeterTimeSigNumBeatsBar { get; set; }
        public byte AKMeterTimeSigBeatValue { get; set; }
        public byte AKMeterInfoFlag { get; set; }
        public List<byte[]> Stingers { get; set; }
        public List<TransitionRule> Rules { get; set; }
        public List<SeqPlaylistItem> SequencePlaylistItems { get; set; }
        public MusicSequence(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            MusicFlags = br.ReadByte();
            NodeBase = new NodeBase(br, ParentObject);
            ChildIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint Key = br.ReadUInt32();
                ChildIDs.Add(Key);
            }

            AKMeterGridPeriod = br.ReadDouble();
            AKMeterGridOffset = br.ReadDouble();
            AKMeterTempo = br.ReadSingle();
            AKMeterTimeSigNumBeatsBar = br.ReadByte();
            AKMeterTimeSigBeatValue = br.ReadByte();
            AKMeterInfoFlag = br.ReadByte();
            Stingers = new List<byte[]>();
            uint numStingers = br.ReadUInt32();

            for (int i = 0; i < numStingers; i++)
            {
                Stingers.Add(br.ReadBytes(24));
            }

            Rules = new List<TransitionRule>();
            uint numRules = br.ReadUInt32();

            for (int i = 0; i < numRules; i++)
            {
                Rules.Add(new TransitionRule(br, ParentObject));
            }

            SequencePlaylistItems = new List<SeqPlaylistItem>();
            uint numSequenceItems = br.ReadUInt32();
            int readCount = 0;

            while (readCount < numSequenceItems)
            {
                SequencePlaylistItems.Add(new SeqPlaylistItem(br));
                readCount += SequencePlaylistItems[SequencePlaylistItems.Count - 1].GetCount();
            }
        }

        public MusicSequence(HIRCObject ParentObject)
        {
            Type = 0;
            ID = 0;
            MusicFlags = 0;
            NodeBase = new NodeBase(ParentObject);
            ChildIDs = new List<uint>();
            AKMeterGridPeriod = 0;
            AKMeterGridOffset = 0;
            AKMeterTempo = 0;
            AKMeterTimeSigNumBeatsBar = 0;
            AKMeterTimeSigBeatValue = 0;
            AKMeterInfoFlag = 0;
            Stingers = new List<byte[]>();
            Rules = new List<TransitionRule>();
            SequencePlaylistItems = new List<SeqPlaylistItem>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write(MusicFlags);

            NodeBase.WriteToFile(bw);

            bw.Write(ChildIDs.Count);

            foreach (uint value in ChildIDs)
            {
                bw.Write(value);
            }

            bw.Write(AKMeterGridPeriod);
            bw.Write(AKMeterGridOffset);
            bw.Write(AKMeterTempo);
            bw.Write(AKMeterTimeSigNumBeatsBar);
            bw.Write(AKMeterTimeSigBeatValue);
            bw.Write(AKMeterInfoFlag);
            bw.Write(Stingers.Count);

            foreach (byte[] stinger in Stingers)
            {
                bw.Write(stinger);
            }

            bw.Write(Rules.Count);

            foreach (TransitionRule rule in Rules)
            {
                rule.WriteToFile(bw);
            }

            long itemsPos = bw.BaseStream.Position;
            int totalItemCount = 0;
            bw.Write(SequencePlaylistItems.Count);

            foreach (SeqPlaylistItem item in SequencePlaylistItems)
            {
                totalItemCount += item.GetCount();
                item.WriteToFile(bw);
            }

            long endPos = bw.BaseStream.Position;
            bw.Seek((int)itemsPos, SeekOrigin.Begin);
            bw.Write(totalItemCount);
            bw.Seek((int)endPos, SeekOrigin.Begin); //Has to be done like this due to the playlist having subChildren and Idk any better way of solving it
        }

        public int GetLength()
        {
            int Length = 44 + NodeBase.GetLength() + ChildIDs.Count * 4 + Stingers.Count * 24;

            foreach (TransitionRule rule in Rules)
            {
                Length += rule.GetLength();
            }

            foreach (SeqPlaylistItem item in SequencePlaylistItems)
            {
                Length += item.GetLength();
            }

            return Length;
        }
    }
}
