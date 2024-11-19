using System.ComponentModel;
using System.IO;
using System.Text;

namespace ResourceTypes.Wwise.Helpers
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class MusicMarker
    {
        public uint ID { get; set; }
        public double Position { get; set; }
        public string MarkerName { get; set; }
        public MusicMarker(BinaryReader br)
        {
            ID = br.ReadUInt32();
            Position = br.ReadDouble();
            uint stringSize = br.ReadUInt32();
            byte[] stringBytes = br.ReadBytes((int)stringSize);
            MarkerName = Encoding.UTF8.GetString(stringBytes);
        }

        public MusicMarker()
        {
            ID = 0;
            Position = 0;
            MarkerName = "";
        }

        public void WriteToFile(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(Position);
            bw.Write(MarkerName.Length);
            bw.Write(MarkerName.ToCharArray());
        }

        public int GetLength()
        {
            int Length = 16 + MarkerName.Length;

            return Length;
        }
    }
}
