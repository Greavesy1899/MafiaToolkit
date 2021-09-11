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
    public class BlendContainer
    {
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public NodeBase NodeBase { get; set; }
        public List<uint> ChildIDs { get; set; } //IDs of child HIRC objects
        public List<Layer> Layers { get; set; }
        public byte[] Skip { get; set; }
        public BlendContainer(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            long initPos = br.BaseStream.Position;
            ID = br.ReadUInt32();
            NodeBase = new NodeBase(br, ParentObject);
            ChildIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint Key = br.ReadUInt32();
                ChildIDs.Add(Key);
            }

            Layers = new List<Layer>();
            uint numLayers = br.ReadUInt32();

            for (int i = 0; i < numLayers; i++)
            {
                Layers.Add(new Layer(br));
            }

            long endPos = br.BaseStream.Position;
            Skip = br.ReadBytes((int)(Length - (endPos - initPos)));
        }

        public BlendContainer(HIRCObject ParentObject)
        {
            Type = 0;
            ID = 0;
            NodeBase = new NodeBase(ParentObject);
            ChildIDs = new List<uint>();
            Layers = new List<Layer>();
            Skip = new byte[0];
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);

            NodeBase.WriteToFile(bw);

            bw.Write(ChildIDs.Count);

            foreach (uint child in ChildIDs)
            {
                bw.Write(child);
            }

            bw.Write(Layers.Count);

            foreach (Layer layer in Layers)
            {
                Layer.WriteLayer(bw, layer);
            }

            bw.Write(Skip);
        }

        public int GetLength()
        {
            int Length = 12 + NodeBase.GetLength() + ChildIDs.Count * 4;

            if (Skip != null)
            {
                Length += Skip.Length;
            }

            foreach (Layer layer in Layers)
            {
                Length += layer.GetLength();
            }

            return Length;
        }
    }
}
