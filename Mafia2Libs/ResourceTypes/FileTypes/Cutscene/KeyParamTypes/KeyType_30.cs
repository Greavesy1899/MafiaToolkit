using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_30 : IKeyType
    {
        public class UnkData
        {
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public float Unk04 { get; set; } // Could be new position

            public override string ToString()
            {
                return string.Format("{0} Start: {1} End: {2}", Unk04, KeyFrameEnd, Unk03);
            }
        }

        public UnkData[] Frames { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int NumUnkData = br.ReadInt32();
            Frames = new UnkData[NumUnkData];

            for (int i = 0; i < NumUnkData; i++)
            {
                UnkData frames = new UnkData();
                frames.KeyFrameStart = br.ReadInt32();
                frames.KeyFrameEnd = br.ReadInt32();
                frames.Unk03 = br.ReadByte();
                frames.Unk04 = br.ReadSingle();
                Frames[i] = frames;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Frames.Length);
            
            foreach(UnkData Entry in Frames)
            {
                bw.Write(Entry.KeyFrameStart);
                bw.Write(Entry.KeyFrameEnd);
                bw.Write(Entry.Unk03);
                bw.Write(Entry.KeyFrameStart);
            }

            bw.Write(Unk05);
        }

        public override string ToString()
        {
            return string.Format("Frames: {0}", Frames.Length);
        }
    }
}
