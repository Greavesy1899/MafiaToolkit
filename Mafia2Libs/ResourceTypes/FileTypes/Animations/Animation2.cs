using System.IO;
using Utils.Logging;

namespace ResourceTypes.Animation2
{
    public class Animation2
    {
        public Header Header { get; set; } = new();
        public bool IsDataPresent { get; set; } //Not confirmed?
        public Event[] Events { get; set; } = new Event[0];
        public ushort Unk00 { get; set; }
        public ushort Unk01 { get; set; }
        public AnimTrack[] Tracks { get; set; } = new AnimTrack[0];
        public short[] UnkShorts00 { get; set; } = new short[0];
        public short[] UnkShorts01 { get; set; } = new short[0];

        public Animation2()
        {

        }

        public Animation2(string fileName)
        {
            Read(fileName);
        }

        public Animation2(Stream s)
        {
            Read(s);
        }

        public Animation2(BinaryReader br)
        {
            Read(br);
        }

        public void Read(string fileName)
        {
            using (MemoryStream ms = new(File.ReadAllBytes(fileName)))
            {
                Read(ms);
            }
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
            Header = new(br);

            IsDataPresent = br.ReadBoolean();

            Events = new Event[Header.NumEvents];

            for (int i = 0; i < Events.Length; i++)
            {
                Events[i] = new(br);
            }

            Unk00 = br.ReadUInt16();
            Unk01 = br.ReadUInt16();
            short Count2 = br.ReadInt16();

            ToolkitAssert.Ensure(Header.Count == Count2, "Animation2: Count 1 != Count 2");

            Count2 = IsDataPresent ? Count2++ : Count2;

            Tracks = new AnimTrack[Count2];

            for (int i = 0;i < Tracks.Length; i++)
            {
                Tracks[i] = new(br);
            }

            UnkShorts00 = new short[Unk01];
            UnkShorts01 = new short[Count2];

            for (int i = 0; i < UnkShorts00.Length; i++)
            {
                UnkShorts00[i] = br.ReadInt16();
            }

            for (int i = 0; i < UnkShorts01.Length; i++)
            {
                UnkShorts01[i] = br.ReadInt16();
            }

            ToolkitAssert.Ensure(br.BaseStream.Position == br.BaseStream.Length, "Animation2: Failed to reach EOF.");
        }
    }
}
