using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Objects
{
    public class Event
    {
        [Browsable(false)]
        public int Type { get; set; }
        [Browsable(false)]
        private HIRCObject Parent { get; set; }
        public uint ID { get; set; }
        public List<uint> ActionIDs { get; set; }
        public Event(HIRCObject ParentObject, BinaryReader br, int iType)
        {
            Type = iType;
            Parent = ParentObject;
            uint Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            int actionsCount = br.ReadInt32();
            ActionIDs = new List<uint>();

            for (uint i = 0; i < actionsCount; i++)
            {
                ActionIDs.Add(br.ReadUInt32());
            }
        }

        public Event(HIRCObject ParentObject)
        {
            Type = 0;
            Parent = ParentObject;
            ID = 0;
            ActionIDs = new List<uint>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)Type);
            bw.Write(GetLength());
            bw.Write(ID);
            bw.Write(ActionIDs.Count);

            foreach (uint action in ActionIDs)
            {
                bw.Write(action);
            }
        }

        public int GetLength()
        {
            int Length = 8 + ActionIDs.Count * 4;
            return Length;
        }
    }
}
