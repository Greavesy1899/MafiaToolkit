using System.IO;

namespace ResourceTypes.Animation2
{
    public class Event
    {
        public uint EventID { get; set; }
        public float Time { get; set; }
        public Event()
        {

        }

        public Event(Stream s)
        {
            Read(s);
        }

        public Event(BinaryReader br)
        {
            Read(br);
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            EventID = br.ReadUInt32();
            Time = br.ReadSingle();
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(EventID);
            bw.Write(Time);
        }
    }
}
