using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ResourceTypes.Wwise.Helpers;
using ResourceTypes.Wwise;

namespace ResourceTypes.Wwise.Objects
{
    public class MusicTrack
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        [System.ComponentModel.Browsable(false)]
        private HIRCObject parent { get; set; }
        public uint id { get; set; }
        public byte musicFlags { get; set; } //bit1 = "bOverrideParentMidiTempo", bit2 = "bOverrideParentMidiTarget", bit3 = "bMidiTargetTypeBus"
        public List<TrackSource> trackSources { get; set; }
        public List<TrackItem> trackPlaylist { get; set; }
        public uint numSubtrack { get; set; }
        public uint sourceId { get; set; }
        public List<AutomationItem> automationItems { get; set; }
        public NodeBase nodeBase { get; set; }
        public byte trackType { get; set; } //0x03 = Switch
        public byte switchGroupType { get; set; }
        public uint switchGroupID { get; set; }
        public uint defaultSwitch { get; set; }
        public List<uint> switchAssoc { get; set; }
        public uint srcFadeTransitionTime { get; set; }
        public uint srcFadeCurve { get; set; } //0x04 = Linear
        public uint srcFadeOffset { get; set; }
        public uint transitionSyncType { get; set; } //0x02 = NextBar
        public uint transitionCueFilterHash { get; set; }
        public uint destFadeTransitionTime { get; set; }
        public uint destFadeCurve { get; set; } //0x04 = Linear
        public uint destFadeOffset { get; set; }
        public uint lookAheadTime { get; set; }
        public MusicTrack(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            parent = parentObject;
            uint length = br.ReadUInt32();
            long initPos = br.BaseStream.Position;
            id = br.ReadUInt32();
            musicFlags = br.ReadByte();
            trackSources = new List<TrackSource>();
            uint numSources = br.ReadUInt32();

            for (int i = 0; i < numSources; i++)
            {
                trackSources.Add(new TrackSource(br));
            }

            trackPlaylist = new List<TrackItem>();
            uint numTracks = br.ReadUInt32();

            for (int i = 0; i < numTracks; i++)
            {
                trackPlaylist.Add(new TrackItem(br));
            }

            if (numTracks != 0)
            {
                numSubtrack = br.ReadUInt32();
            }

            automationItems = new List<AutomationItem>();
            uint numAutomationItems = br.ReadUInt32();

            for (int i = 0; i < numAutomationItems; i++)
            {
                automationItems.Add(new AutomationItem(br));
            }

            nodeBase = new NodeBase(br, parentObject);
            trackType = br.ReadByte();

            if (trackType == 0 || trackType == 1 || trackType == 2)
            { }
            else if (trackType == 3)
            {
                switchGroupType = br.ReadByte();
                switchGroupID = br.ReadUInt32();
                defaultSwitch = br.ReadUInt32();
                switchAssoc = new List<uint>();
                uint numSwitchAssoc = br.ReadUInt32();

                for (int i = 0; i < numSwitchAssoc; i++)
                {
                    switchAssoc.Add(br.ReadUInt32());
                }

                srcFadeTransitionTime = br.ReadUInt32();
                srcFadeCurve = br.ReadUInt32();
                srcFadeOffset = br.ReadUInt32();
                transitionSyncType = br.ReadUInt32();
                transitionCueFilterHash = br.ReadUInt32();
                destFadeTransitionTime = br.ReadUInt32();
                destFadeCurve = br.ReadUInt32();
                destFadeOffset = br.ReadUInt32();
            }
            else
            {
                MessageBox.Show("Detected unknown type 11 track type at: " + br.BaseStream.Position.ToString("X") + " Type: " + trackType.ToString());
            }

            lookAheadTime = br.ReadUInt32();
        }

        public MusicTrack(HIRCObject parentObject)
        {
            type = 0;
            parent = parentObject;
            id = 0;
            musicFlags = 0;
            trackSources = new List<TrackSource>();
            trackPlaylist = new List<TrackItem>();
            numSubtrack = 0;
            sourceId = 0;
            automationItems = new List<AutomationItem>();
            nodeBase = new NodeBase(parentObject);
            trackType = 0;
            switchGroupType = 0;
            switchGroupID = 0;
            defaultSwitch = 0;
            switchAssoc = new List<uint>();
            srcFadeTransitionTime = 0;
            srcFadeCurve = 0;
            srcFadeOffset = 0;
            transitionSyncType = 0;
            transitionCueFilterHash = 0;
            destFadeTransitionTime = 0;
            destFadeCurve = 0;
            destFadeOffset = 0;
            lookAheadTime = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);
            bw.Write(musicFlags);
            bw.Write(trackSources.Count);

            foreach (TrackSource source in trackSources)
            {
                TrackSource.WriteTrackSource(bw, source);
            }

            bw.Write(trackPlaylist.Count);

            foreach (TrackItem item in trackPlaylist)
            {
                item.WriteToFile(bw);
            }

            if (trackPlaylist.Count != 0)
            {
                bw.Write(numSubtrack);
            }

            bw.Write(automationItems.Count);

            foreach (AutomationItem item in automationItems)
            {
                AutomationItem.WriteAutomationItem(bw, item);
            }

            nodeBase.WriteToFile(bw);

            bw.Write(trackType);

            if (trackType == 3)
            {
                bw.Write(switchGroupType);
                bw.Write(switchGroupID);
                bw.Write(defaultSwitch);
                bw.Write(switchAssoc.Count);

                foreach (uint assoc in switchAssoc)
                {
                    bw.Write(assoc);
                }

                bw.Write(srcFadeTransitionTime);
                bw.Write(srcFadeCurve);
                bw.Write(srcFadeOffset);
                bw.Write(transitionSyncType);
                bw.Write(transitionCueFilterHash);
                bw.Write(destFadeTransitionTime);
                bw.Write(destFadeCurve);
                bw.Write(destFadeOffset);
            }

            bw.Write(lookAheadTime);
        }

        public int GetLength()
        {
            int length = 22 + nodeBase.GetLength() + trackSources.Count * 14 + trackPlaylist.Count * 40;

            foreach (AutomationItem item in automationItems)
            {
                length += item.GetLength();
            }

            if (trackPlaylist.Count != 0)
            {
                length += 4;
            }

            if (trackType == 0 || trackType == 1 || trackType == 2)
            { }
            else if (trackType == 3)
            {
                length += 45 + switchAssoc.Count * 4;
            }

            return length;
        }
    }
}
