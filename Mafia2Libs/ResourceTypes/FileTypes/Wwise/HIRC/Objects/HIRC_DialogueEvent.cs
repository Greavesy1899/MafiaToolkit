using System.IO;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Objects
{
    public class DialogueEvent //Probably unused
    {
        [System.ComponentModel.Browsable(false)]
        public int Type { get; set; }
        [System.ComponentModel.Browsable(false)]
        public uint Length { get; set; }
        public uint ID { get; set; }
        [System.ComponentModel.Browsable(false)]
        public byte[] Data { get; set; }
        public DialogueEvent(BinaryReader br, int iType)
        {
            Type = iType;
            Length = br.ReadUInt32();
            ID = br.ReadUInt32();
            Data = br.ReadBytes((int)Length - 4);
        }
    }
}
