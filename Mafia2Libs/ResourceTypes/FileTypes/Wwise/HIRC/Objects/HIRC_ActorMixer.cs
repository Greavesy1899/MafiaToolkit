using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ResourceTypes.Wwise.Helpers;

namespace ResourceTypes.Wwise.Objects
{
    public class ActorMixer
    {
        [Browsable(false)]
        public int Type { get; set; }
        public uint ID { get; set; }
        public NodeBase NodeBase { get; set; }
        public List<uint> ChildIDs { get; set; } //IDs of child HIRC objects
        public ActorMixer(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            NodeBase = new NodeBase(br, ParentObject);
            ChildIDs = new List<uint>();
            uint numChilds = br.ReadUInt32();

            for (int i = 0; i < numChilds; i++)
            {
                uint Key = br.ReadUInt32();
                ChildIDs.Add(Key);
            }
        }

        public ActorMixer(HIRCObject ParentObject)
        {
            Type = 0;
            ID = 0;
            NodeBase = new NodeBase(ParentObject);
            ChildIDs = new List<uint>();
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
        }

        public int GetLength()
        {
            int Length = 8 + NodeBase.GetLength() + ChildIDs.Count * 4;

            return Length;
        }
    }
}
