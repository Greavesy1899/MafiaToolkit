using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MediaMap
    {
        public int ID { get; set; }
        public uint SourceID { get; set; }

        public MediaMap(BinaryReader br)
        {
            ID = br.ReadByte();
            SourceID = br.ReadUInt32();
        }

        public MediaMap()
        {
            ID = 0;
            SourceID = 0;
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write((byte)ID);
            bw.Write(SourceID);
        }
    }
}
