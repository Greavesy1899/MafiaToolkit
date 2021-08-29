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
        //TODO - Read bitVectors on bit level
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        public uint id { get; set; }
        public NodeBase nodeBase { get; set; }
        public uint loopCount { get; set; }
        public uint loopModMin { get; set; }
        public uint loopModMax { get; set; }
        public float transitionTimeBase { get; set; }
        public float transitionTimeModMin { get; set; }
        public float transitionTimeModMax { get; set; }
        public uint avoidRepeatCount { get; set; }
        public byte transitionMode { get; set; } //0x00 = Disabled
        public byte randomMode { get; set; } //0x00 = Normal
        public byte mode { get; set; } //0x00 = Random
        public byte bitVector { get; set; } //bit0 = "_bIsUsingWeight", bit1 = "bResetPlayListAtEachPlay", bit2 = "bIsRestartBackward", bit3 = "bIsContinuous", bit4 = "bIsGlobal"
        public List<uint> childIDs { get; set; } //IDs of child HIRC objects
        public List<PlaylistItem> mainPlaylist { get; set; } //PlayID, Weight
        public RandomContainer(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            id = br.ReadUInt32();
            nodeBase = new NodeBase(br, parentObject);
            loopCount = br.ReadUInt16();
            loopModMin = br.ReadUInt16();
            loopModMax = br.ReadUInt16();
            transitionTimeBase = br.ReadSingle();
            transitionTimeModMin = br.ReadSingle();
            transitionTimeModMax = br.ReadSingle();
            avoidRepeatCount = br.ReadUInt16();
            transitionMode = br.ReadByte();
            randomMode = br.ReadByte();
            mode = br.ReadByte();
            bitVector = br.ReadByte();
            childIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint key = br.ReadUInt32();
                childIDs.Add(key);
            }

            int numPlayList = br.ReadUInt16();
            mainPlaylist= new List<PlaylistItem>();

            for (int i = 0; i < numPlayList; i++)
            {
                uint vertexOffset = br.ReadUInt32();
                uint numPlaylistVertices = br.ReadUInt32();
                mainPlaylist.Add(new PlaylistItem(vertexOffset, numPlaylistVertices));
            }
        }

        public RandomContainer(HIRCObject parentObject)
        {
            type = 0;
            id = 0;
            nodeBase = new NodeBase(parentObject);
            loopCount = 0;
            loopModMin = 0;
            loopModMax = 0;
            transitionTimeBase = 0;
            transitionTimeModMin = 0;
            transitionTimeModMax = 0;
            avoidRepeatCount = 0;
            transitionMode = 0;
            randomMode = 0;
            mode = 0;
            bitVector = 0;
            childIDs = new List<uint>();
            mainPlaylist = new List<PlaylistItem>();
    }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);

            nodeBase.WriteToFile(bw);

            bw.Write((short)loopCount);
            bw.Write((short)loopModMin);
            bw.Write((short)loopModMax);
            bw.Write(transitionTimeBase);
            bw.Write(transitionTimeModMin);
            bw.Write(transitionTimeModMax);
            bw.Write((short)avoidRepeatCount);
            bw.Write(transitionMode);
            bw.Write(randomMode);
            bw.Write(mode);
            bw.Write(bitVector);
            bw.Write(childIDs.Count);

            foreach (uint child in childIDs)
            {
                bw.Write(child);
            }

            bw.Write((short)mainPlaylist.Count);

            foreach (PlaylistItem item in mainPlaylist)
            {
                bw.Write(item.vertexOffset);
                bw.Write(item.numVertices);
            }
        }

        public int GetLength()
        {
            int length = 34 + childIDs.Count * 4 + nodeBase.GetLength() + mainPlaylist.Count * 8;

            return length;
        }
    }
}
