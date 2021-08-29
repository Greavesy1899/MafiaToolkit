using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

using ResourceTypes.Wwise;
using Utils;

namespace ResourceTypes.Wwise.Helpers
{
    public class StateChunk
    {
        [System.ComponentModel.Browsable(false)]
        public HIRCObject parent { get; set; }
        public uint groupId { get; set; }
        public byte syncType { get; set; } //0x00 = Immediate
        public List<State> states { get; set; }
        public StateChunk(BinaryReader br, HIRCObject parentObject)
        {
            parent = parentObject;
            groupId = br.ReadUInt32();
            syncType = br.ReadByte();
            int stateCount = br.ReadUInt16();
            states = new List<State>();

            for (int i = 0; i < stateCount; i++)
            {
                states.Add(new State(br));
            }
        }

        public StateChunk()
        {
            groupId = 0;
            syncType = 0;
            states = new List<State>();
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(groupId);
            bw.Write(syncType);
            bw.Write((short)states.Count);

            foreach (State state in states)
            {
                state.WriteToFile(bw);
            }
        }

        public int GetLength()
        {
            int chunkLength = 7 + states.Count * 8;

            return chunkLength;
        }
    }
}
