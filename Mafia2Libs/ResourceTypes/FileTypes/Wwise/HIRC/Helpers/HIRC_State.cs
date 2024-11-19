using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class State
    {
        public uint ID { get; set; }
        public uint InstanceID { get; set; }
        public State(BinaryReader br)
        {
            ID = br.ReadUInt32();
            InstanceID = br.ReadUInt32();
        }

        public State()
        {
            ID = 0;
            InstanceID = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(InstanceID);
        }
    }
}
