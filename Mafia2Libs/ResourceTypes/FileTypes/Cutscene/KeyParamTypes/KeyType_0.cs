using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_0 : IKeyType
    {
        public class FrameData
        {
            public int Unk01 { get; set; } // Key Frame Start?
            public int Unk02 { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public float Unk04 { get; set; } // Could be new position

            public override string ToString()
            {
                return string.Format("{0} Start: {1} End: {2}", Unk04, Unk02, Unk03);
            }
        }

        public int NumFrames { get; set; }
        public FrameData[] Frames { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            NumFrames = br.ReadInt32();
            Frames = new FrameData[NumFrames];

            for (int i = 0; i < NumFrames; i++)
            {
                FrameData frames = new FrameData();
                frames.Unk01 = br.ReadInt32();
                frames.Unk02 = br.ReadInt32();
                frames.Unk03 = br.ReadByte();
                frames.Unk04 = br.ReadSingle();
                Frames[i] = frames;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(NumFrames);

            foreach (FrameData Entry in Frames)
            {
                bw.Write(Entry.Unk01);
                bw.Write(Entry.Unk02);
                bw.Write(Entry.Unk03);
                bw.Write(Entry.Unk04);
            }

            bw.Write(Unk05);
        }

        public override string ToString()
        {
            return string.Format("NumFrames: {0}", Frames.Length);
        }
    }
}
