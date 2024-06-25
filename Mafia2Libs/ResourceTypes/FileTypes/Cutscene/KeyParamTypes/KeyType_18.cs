using System.IO;
using Utils.Extensions;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_18 : IKeyType
    {
        public class FrameData
        {
            public int Unk01 { get; set; } // Key Frame Start?
            public int Unk02 { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public string SoundFile { get; set; } // Sound to play.
            public int Unk04 { get; set; }
            public byte Unk05 { get; set; }
            public float Unk06 { get; set; }
            public float Unk07 { get; set; }
            public float Unk08 { get; set; }
            public float Unk09 { get; set; }
            public int Unk10 { get; set; }
            public float Unk11 { get; set; }
            public float Unk12 { get; set; }
            public override string ToString()
            {
                return string.Format("Sound: {0} Start: {1} End: {2}", SoundFile, Unk01, Unk02);
            }
        }

        public FrameData[] Sounds { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int NumSounds = br.ReadInt32();
            Sounds = new FrameData[NumSounds];

            for (int i = 0; i < NumSounds; i++)
            {
                FrameData frames = new FrameData();
                frames.Unk01 = br.ReadInt32();
                frames.Unk02 = br.ReadInt32();
                frames.Unk03 = br.ReadByte();
                frames.SoundFile = br.ReadString16();
                frames.Unk04 = br.ReadInt32();
                frames.Unk05 = br.ReadByte();
                frames.Unk06 = br.ReadSingle();
                frames.Unk07 = br.ReadSingle();
                frames.Unk08 = br.ReadSingle();
                frames.Unk09 = br.ReadSingle();
                frames.Unk10 = br.ReadInt32();
                frames.Unk11 = br.ReadSingle();
                frames.Unk12 = br.ReadSingle();
                Sounds[i] = frames;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Sounds.Length);

            foreach(FrameData FrameInfo in Sounds)
            {
                bw.Write(FrameInfo.Unk01);
                bw.Write(FrameInfo.Unk02);
                bw.Write(FrameInfo.Unk03);
                bw.WriteString16(FrameInfo.SoundFile);
                bw.Write(FrameInfo.Unk04);
                bw.Write(FrameInfo.Unk05);
                bw.Write(FrameInfo.Unk06);
                bw.Write(FrameInfo.Unk07);
                bw.Write(FrameInfo.Unk08);
                bw.Write(FrameInfo.Unk09);
                bw.Write(FrameInfo.Unk10);
                bw.Write(FrameInfo.Unk11);
                bw.Write(FrameInfo.Unk12);
            }

            bw.Write(Unk05);
        }

        public override string ToString()
        {
            return string.Format("NumSounds: {0}", Sounds.Length);
        }
    }
}
