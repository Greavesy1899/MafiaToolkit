using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class MusicTrack
    {
        [Browsable(false)]
        public int Type { get; set; }
        [Browsable(false)]
        private HIRCObject Parent { get; set; }
        public uint ID { get; set; }
        public byte MusicFlags { get; set; } //bit1 = "bOverrideParentMidiTempo", bit2 = "bOverrideParentMidiTarget", bit3 = "bMidiTargetTypeBus"
        public List<TrackSource> TrackSources { get; set; }
        public List<TrackItem> TrackPlaylist { get; set; }
        public uint SubtrackCount { get; set; }
        public uint SourceID { get; set; }
        public List<AutomationItem> AutomationItems { get; set; }
        public NodeBase NodeBase { get; set; }
        public byte TrackType { get; set; } //0x03 = Switch
        public byte SwitchGroupType { get; set; }
        public uint SwitchGroupID { get; set; }
        public uint DefaultSwitch { get; set; }
        public List<uint> SwitchAssoc { get; set; }
        public uint SourceFadeTransitionTime { get; set; }
        public uint SourceFadeCurve { get; set; } //0x04 = Linear
        public uint SourceFadeOffset { get; set; }
        public uint TransitionSyncType { get; set; } //0x02 = NextBar
        public uint TransitionCueFilterHash { get; set; }
        public uint DestinationFadeTransitionTime { get; set; }
        public uint DestinationFadeCurve { get; set; } //0x04 = Linear
        public uint DestinationFadeOffset { get; set; }
        public uint LookAheadTime { get; set; }
        public MusicTrack(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            Parent = ParentObject;
            uint Length = br.ReadUInt32();
            long initPos = br.BaseStream.Position;
            ID = br.ReadUInt32();
            MusicFlags = br.ReadByte();
            TrackSources = new List<TrackSource>();
            uint numSources = br.ReadUInt32();

            for (int i = 0; i < numSources; i++)
            {
                TrackSources.Add(new TrackSource(br));
            }

            TrackPlaylist = new List<TrackItem>();
            uint numTracks = br.ReadUInt32();

            for (int i = 0; i < numTracks; i++)
            {
                TrackPlaylist.Add(new TrackItem(br));
            }

            if (numTracks != 0)
            {
                SubtrackCount = br.ReadUInt32();
            }

            AutomationItems = new List<AutomationItem>();
            uint numAutomationItems = br.ReadUInt32();

            for (int i = 0; i < numAutomationItems; i++)
            {
                AutomationItems.Add(new AutomationItem(br));
            }

            NodeBase = new NodeBase(br, ParentObject);
            TrackType = br.ReadByte();

            if (TrackType == 0 || TrackType == 1 || TrackType == 2)
            { }
            else if (TrackType == 3)
            {
                SwitchGroupType = br.ReadByte();
                SwitchGroupID = br.ReadUInt32();
                DefaultSwitch = br.ReadUInt32();
                SwitchAssoc = new List<uint>();
                uint numSwitchAssoc = br.ReadUInt32();

                for (int i = 0; i < numSwitchAssoc; i++)
                {
                    SwitchAssoc.Add(br.ReadUInt32());
                }

                SourceFadeTransitionTime = br.ReadUInt32();
                SourceFadeCurve = br.ReadUInt32();
                SourceFadeOffset = br.ReadUInt32();
                TransitionSyncType = br.ReadUInt32();
                TransitionCueFilterHash = br.ReadUInt32();
                DestinationFadeTransitionTime = br.ReadUInt32();
                DestinationFadeCurve = br.ReadUInt32();
                DestinationFadeOffset = br.ReadUInt32();
            }
            else
            {
                MessageBox.Show("Detected unknown Type 11 track Type at: " + br.BaseStream.Position.ToString("X") + " Type: " + TrackType.ToString());
            }

            LookAheadTime = br.ReadUInt32();
        }

        public MusicTrack(HIRCObject ParentObject)
        {
            Type = 0;
            Parent = ParentObject;
            ID = 0;
            MusicFlags = 0;
            TrackSources = new List<TrackSource>();
            TrackPlaylist = new List<TrackItem>();
            SubtrackCount = 0;
            SourceID = 0;
            AutomationItems = new List<AutomationItem>();
            NodeBase = new NodeBase(ParentObject);
            TrackType = 0;
            SwitchGroupType = 0;
            SwitchGroupID = 0;
            DefaultSwitch = 0;
            SwitchAssoc = new List<uint>();
            SourceFadeTransitionTime = 0;
            SourceFadeCurve = 0;
            SourceFadeOffset = 0;
            TransitionSyncType = 0;
            TransitionCueFilterHash = 0;
            DestinationFadeTransitionTime = 0;
            DestinationFadeCurve = 0;
            DestinationFadeOffset = 0;
            LookAheadTime = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write(MusicFlags);
            bw.Write(TrackSources.Count);

            foreach (TrackSource source in TrackSources)
            {
                TrackSource.WriteTrackSource(bw, source);
            }

            bw.Write(TrackPlaylist.Count);

            foreach (TrackItem item in TrackPlaylist)
            {
                item.WriteToFile(bw);
            }

            if (TrackPlaylist.Count != 0)
            {
                bw.Write(SubtrackCount);
            }

            bw.Write(AutomationItems.Count);

            foreach (AutomationItem item in AutomationItems)
            {
                AutomationItem.WriteAutomationItem(bw, item);
            }

            NodeBase.WriteToFile(bw);

            bw.Write(TrackType);

            if (TrackType == 3)
            {
                bw.Write(SwitchGroupType);
                bw.Write(SwitchGroupID);
                bw.Write(DefaultSwitch);
                bw.Write(SwitchAssoc.Count);

                foreach (uint assoc in SwitchAssoc)
                {
                    bw.Write(assoc);
                }

                bw.Write(SourceFadeTransitionTime);
                bw.Write(SourceFadeCurve);
                bw.Write(SourceFadeOffset);
                bw.Write(TransitionSyncType);
                bw.Write(TransitionCueFilterHash);
                bw.Write(DestinationFadeTransitionTime);
                bw.Write(DestinationFadeCurve);
                bw.Write(DestinationFadeOffset);
            }

            bw.Write(LookAheadTime);
        }

        public int GetLength()
        {
            int Length = 22 + NodeBase.GetLength() + TrackSources.Count * 14 + TrackPlaylist.Count * 40;

            foreach (AutomationItem item in AutomationItems)
            {
                Length += item.GetLength();
            }

            if (TrackPlaylist.Count != 0)
            {
                Length += 4;
            }

            if (TrackType == 0 || TrackType == 1 || TrackType == 2)
            { }
            else if (TrackType == 3)
            {
                Length += 45 + SwitchAssoc.Count * 4;
            }

            return Length;
        }
    }
}
