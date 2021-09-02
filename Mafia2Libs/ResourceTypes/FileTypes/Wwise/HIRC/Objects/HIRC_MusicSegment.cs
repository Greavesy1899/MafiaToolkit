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
    public class MusicSegment
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
        public double Duration { get; set; }
        public List<MusicMarker> MusicMarkers { get; set; }
        public MusicSegment(HIRCObject ParentObject, BinaryReader br, int iType)
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

            Duration = br.ReadDouble();
            MusicMarkers = new List<MusicMarker>();
            uint numMarkers = br.ReadUInt32();

            for (int i = 0; i < numMarkers; i++)
            {
                MusicMarkers.Add(new MusicMarker(br));
            }
        }

        public MusicSegment(HIRCObject ParentObject)
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
            Duration = 0;
            MusicMarkers = new List<MusicMarker>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write(MusicFlags);

            NodeBase.WriteToFile(bw);

            bw.Write(ChildIDs.Count);

            foreach (uint child in ChildIDs)
            {
                bw.Write(child);
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

            bw.Write(Duration);
            bw.Write(MusicMarkers.Count);

            foreach (MusicMarker marker in MusicMarkers)
            {
                marker.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int Length = 48 + NodeBase.GetLength() + ChildIDs.Count * 4 + Stingers.Count * 24;

            foreach (MusicMarker marker in MusicMarkers)
            {
                Length += marker.GetLength();
            }

            return Length;
        }
    }
}
