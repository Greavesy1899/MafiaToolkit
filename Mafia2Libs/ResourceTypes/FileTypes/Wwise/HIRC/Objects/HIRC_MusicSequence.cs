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
        public int type { get; set; }
        public uint id { get; set; }
        public byte musicFlags { get; set; } //bit1 = "bOverrideParentMidiTempo", bit2 = "bOverrideParentMidiTarget", bit3 = "bMidiTargetTypeBus" 
        public NodeBase nodeBase { get; set; }
        public List<uint> childIDs { get; set; } //IDs of child HIRC objects
        public double akMeterGridPeriod { get; set; }
        public double akMeterGridOffset { get; set; }
        public float akMeterTempo { get; set; }
        public byte akMeterTimeSigNumBeatsBar { get; set; }
        public byte akMeterTimeSigBeatValue { get; set; }
        public byte akMeterInfoFlag { get; set; }
        public List<byte[]> stingers { get; set; }
        public List<TransitionRule> rules { get; set; }
        public List<SeqPlaylistItem> seqPlaylistItems { get; set; }
        public MusicSequence(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            musicFlags = br.ReadByte();
            nodeBase = new NodeBase(br, parentObject);
            childIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint key = br.ReadUInt32();
                childIDs.Add(key);
            }

            akMeterGridPeriod = br.ReadDouble();
            akMeterGridOffset = br.ReadDouble();
            akMeterTempo = br.ReadSingle();
            akMeterTimeSigNumBeatsBar = br.ReadByte();
            akMeterTimeSigBeatValue = br.ReadByte();
            akMeterInfoFlag = br.ReadByte();
            stingers = new List<byte[]>();
            uint numStingers = br.ReadUInt32();

            for (int i = 0; i < numStingers; i++)
            {
                stingers.Add(br.ReadBytes(24));
            }

            rules = new List<TransitionRule>();
            uint numRules = br.ReadUInt32();

            for (int i = 0; i < numRules; i++)
            {
                rules.Add(new TransitionRule(br, parentObject));
            }

            seqPlaylistItems = new List<SeqPlaylistItem>();
            uint numSequenceItems = br.ReadUInt32();
            int readCount = 0;

            while (readCount < numSequenceItems)
            {
                seqPlaylistItems.Add(new SeqPlaylistItem(br));
                readCount += seqPlaylistItems[seqPlaylistItems.Count - 1].GetCount();
            }
        }

        public MusicSequence(HIRCObject parentObject)
        {
            type = 0;
            id = 0;
            musicFlags = 0;
            nodeBase = new NodeBase(parentObject);
            childIDs = new List<uint>();
            akMeterGridPeriod = 0;
            akMeterGridOffset = 0;
            akMeterTempo = 0;
            akMeterTimeSigNumBeatsBar = 0;
            akMeterTimeSigBeatValue = 0;
            akMeterInfoFlag = 0;
            stingers = new List<byte[]>();
            rules = new List<TransitionRule>();
            seqPlaylistItems = new List<SeqPlaylistItem>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write(musicFlags);

            nodeBase.WriteToFile(bw);

            bw.Write(childIDs.Count);

            foreach (uint value in childIDs)
            {
                bw.Write(value);
            }

            bw.Write(akMeterGridPeriod);
            bw.Write(akMeterGridOffset);
            bw.Write(akMeterTempo);
            bw.Write(akMeterTimeSigNumBeatsBar);
            bw.Write(akMeterTimeSigBeatValue);
            bw.Write(akMeterInfoFlag);
            bw.Write(stingers.Count);

            foreach (byte[] stinger in stingers)
            {
                bw.Write(stinger);
            }

            bw.Write(rules.Count);

            foreach (TransitionRule rule in rules)
            {
                rule.WriteToFile(bw);
            }

            long itemsPos = bw.BaseStream.Position;
            int totalItemCount = 0;
            bw.Write(seqPlaylistItems.Count);

            foreach (SeqPlaylistItem item in seqPlaylistItems)
            {
                totalItemCount += item.GetCount();
                item.WriteToFile(bw);
            }

            long endPos = bw.BaseStream.Position;
            bw.Seek((int)itemsPos, SeekOrigin.Begin);
            bw.Write(totalItemCount);
            bw.Seek((int)endPos, SeekOrigin.Begin); //Has to be done like this due to the playlist having subchildren and Idk any better way of solving it
        }

        public int GetLength()
        {
            int length = 44 + nodeBase.GetLength() + childIDs.Count * 4 + stingers.Count * 24;

            foreach (TransitionRule rule in rules)
            {
                length += rule.GetLength();
            }

            foreach (SeqPlaylistItem item in seqPlaylistItems)
            {
                length += item.GetLength();
            }

            return length;
        }
    }
}
