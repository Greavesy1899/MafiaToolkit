using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ArgumentGroup
    {
        public uint ID { get; set; }
        public int Value { get; set; }
        public ArgumentGroup(uint iID, int iValue)
        {
            ID = iID;
            Value = iValue;
        }

        public ArgumentGroup()
        {
            ID = 0;
            Value = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write((byte)Value);
        }
    }
}
