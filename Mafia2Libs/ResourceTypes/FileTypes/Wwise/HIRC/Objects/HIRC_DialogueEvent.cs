using System.IO;
using System.Collections.Generic;

namespace ResourceTypes.Wwise.Objects
{
    public class DialogueEvent //Probably unused
    {
        [System.ComponentModel.Browsable(false)]
        public int type { get; set; }
        [System.ComponentModel.Browsable(false)]
        public uint length { get; set; }
        public uint id { get; set; }
        [System.ComponentModel.Browsable(false)]
        public byte[] data { get; set; }
        public DialogueEvent(BinaryReader br, int iType)
        {
            type = iType;
            length = br.ReadUInt32();
            id = br.ReadUInt32();
            data = br.ReadBytes((int)length - 4);
        }
    }
}
