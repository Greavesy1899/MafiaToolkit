using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class StateChunk
    {
        [Browsable(false)]
        public HIRCObject Parent { get; set; }
        public uint GroupID { get; set; }
        public byte SyncType { get; set; } //0x00 = Immediate
        public List<State> States { get; set; }
        public StateChunk(BinaryReader br, HIRCObject ParentObject)
        {
            Parent = ParentObject;
            GroupID = br.ReadUInt32();
            SyncType = br.ReadByte();
            int stateCount = br.ReadUInt16();
            States = new List<State>();

            for (int i = 0; i < stateCount; i++)
            {
                States.Add(new State(br));
            }
        }

        public StateChunk()
        {
            GroupID = 0;
            SyncType = 0;
            States = new List<State>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(GroupID);
            bw.Write(SyncType);
            bw.Write((short)States.Count);

            foreach (State state in States)
            {
                state.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int ChunkLength = 7 + States.Count * 8;

            return ChunkLength;
        }
    }
}
