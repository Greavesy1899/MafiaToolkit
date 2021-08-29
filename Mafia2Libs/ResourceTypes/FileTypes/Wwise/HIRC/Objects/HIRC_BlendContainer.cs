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
        public int type { get; set; }
        public uint id { get; set; }
        public NodeBase nodeBase { get; set; }
        public List<uint> childIDs { get; set; } //IDs of child HIRC objects
        public List<Layer> layers { get; set; }
        public byte[] skip { get; set; }
        public BlendContainer(HIRCObject parentObject, BinaryReader br, int iType)
        {
            type = iType;
            uint length = br.ReadUInt32();
            long initPos = br.BaseStream.Position;
            id = br.ReadUInt32();
            nodeBase = new NodeBase(br, parentObject);
            childIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint key = br.ReadUInt32();
                childIDs.Add(key);
            }

            layers = new List<Layer>();
            uint numLayers = br.ReadUInt32();

            for (int i = 0; i < numLayers; i++)
            {
                layers.Add(new Layer(br));
            }

            long endPos = br.BaseStream.Position;
            skip = br.ReadBytes((int)(length - (endPos - initPos)));
        }

        public BlendContainer(HIRCObject parentObject)
        {
            type = 0;
            id = 0;
            nodeBase = new NodeBase(parentObject);
            childIDs = new List<uint>();
            layers = new List<Layer>();
            skip = new byte[0];
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(GetLength());
            bw.Write(id);

            nodeBase.WriteToFile(bw);

            bw.Write(childIDs.Count);

            foreach (uint child in childIDs)
            {
                bw.Write(child);
            }

            bw.Write(layers.Count);

            foreach (Layer layer in layers)
            {
                Layer.WriteLayer(bw, layer);
            }

            bw.Write(skip);
        }

        public int GetLength()
        {
            int length = 12 + nodeBase.GetLength() + childIDs.Count * 4;

            if (skip != null)
            {
                length += skip.Length;
            }

            foreach (Layer layer in layers)
            {
                length += layer.GetLength();
            }

            return length;
        }
    }
}
