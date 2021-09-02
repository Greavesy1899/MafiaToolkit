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
    public class RandomContainer
    {
        //TODO - Read BitVectors on bit level
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public NodeBase NodeBase { get; set; }
        public uint LoopCount { get; set; }
        public uint LoopModMin { get; set; }
        public uint LoopModMax { get; set; }
        public float TransitionTimeBase { get; set; }
        public float TransitionTimeModMin { get; set; }
        public float TransitionTimeModMax { get; set; }
        public uint AvoidRepeatCount { get; set; }
        public byte TransitionMode { get; set; } //0x00 = Disabled
        public byte RandomMode { get; set; } //0x00 = Normal
        public byte Mode { get; set; } //0x00 = Random
        public byte BitVector { get; set; } //bit0 = "_bIsUsingWeight", bit1 = "bResetPlayListAtEachPlay", bit2 = "bIsRestartBackward", bit3 = "bIsContinuous", bit4 = "bIsGlobal"
        public List<uint> ChildIDs { get; set; } //IDs of child HIRC objects
        public List<PlaylistItem> Playlist { get; set; } //PlayID, Weight
        public RandomContainer(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            NodeBase = new NodeBase(br, ParentObject);
            LoopCount = br.ReadUInt16();
            LoopModMin = br.ReadUInt16();
            LoopModMax = br.ReadUInt16();
            TransitionTimeBase = br.ReadSingle();
            TransitionTimeModMin = br.ReadSingle();
            TransitionTimeModMax = br.ReadSingle();
            AvoidRepeatCount = br.ReadUInt16();
            TransitionMode = br.ReadByte();
            RandomMode = br.ReadByte();
            Mode = br.ReadByte();
            BitVector = br.ReadByte();
            ChildIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint Key = br.ReadUInt32();
                ChildIDs.Add(Key);
            }

            int numPlayList = br.ReadUInt16();
            Playlist= new List<PlaylistItem>();

            for (int i = 0; i < numPlayList; i++)
            {
                uint VertexOffset = br.ReadUInt32();
                uint numPlaylistVertices = br.ReadUInt32();
                Playlist.Add(new PlaylistItem(VertexOffset, numPlaylistVertices));
            }
        }

        public RandomContainer(HIRCObject ParentObject)
        {
            Type = 0;
            ID = 0;
            NodeBase = new NodeBase(ParentObject);
            LoopCount = 0;
            LoopModMin = 0;
            LoopModMax = 0;
            TransitionTimeBase = 0;
            TransitionTimeModMin = 0;
            TransitionTimeModMax = 0;
            AvoidRepeatCount = 0;
            TransitionMode = 0;
            RandomMode = 0;
            Mode = 0;
            BitVector = 0;
            ChildIDs = new List<uint>();
            Playlist = new List<PlaylistItem>();
    }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);

            NodeBase.WriteToFile(bw);

            bw.Write((short)LoopCount);
            bw.Write((short)LoopModMin);
            bw.Write((short)LoopModMax);
            bw.Write(TransitionTimeBase);
            bw.Write(TransitionTimeModMin);
            bw.Write(TransitionTimeModMax);
            bw.Write((short)AvoidRepeatCount);
            bw.Write(TransitionMode);
            bw.Write(RandomMode);
            bw.Write(Mode);
            bw.Write(BitVector);
            bw.Write(ChildIDs.Count);

            foreach (uint child in ChildIDs)
            {
                bw.Write(child);
            }

            bw.Write((short)Playlist.Count);

            foreach (PlaylistItem item in Playlist)
            {
                bw.Write(item.VertexOffset);
                bw.Write(item.VertexCount);
            }
        }

        public int GetLength()
        {
            int Length = 34 + ChildIDs.Count * 4 + NodeBase.GetLength() + Playlist.Count * 8;

            return Length;
        }
    }
}
