using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class StateProp
    {
        public int ID { get; set; }
        public int AccumulatorType { get; set; }
        public int InDb { get; set; }
        public StateProp(BinaryReader br)
        {
            ID = br.ReadByte();
            AccumulatorType = br.ReadByte();
            InDb = br.ReadByte();
        }

        public StateProp()
        {
            ID = 0;
            AccumulatorType = 0;
            InDb = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)ID);
            bw.Write((byte)AccumulatorType);
            bw.Write((byte)InDb);
        }
    }
}
