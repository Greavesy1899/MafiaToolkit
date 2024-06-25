using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.StringHelpers;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_43 : IKeyType
    {
        public class FrameData
        {
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public string SoundFile { get; set; } // Sound to play.
            public byte Unk04 { get; set; }
            public float Unk05 { get; set; }
            public float Unk06 { get; set; }
            public float Unk07 { get; set; }
            public int Unk08 { get; set; } // This was 0.0f for scr_train_2?
            public float Unk09 { get; set; } // This was 1.0f for scr_train_2?
            public int Unk10 { get; set; }
            public float Unk11 { get; set; } // This was 1.0f for scr_train_2?
            public int Unk12 { get; set; }
            public float Unk13 { get; set; } // This was 80.0f for scr_train_2?
            public float Unk14 { get; set; } // This was 10.0f for scr_train_2?
            public byte Unk15 { get; set; } // Is position available?
            public Vector3 Position { get; set; } // Position of Sound? (Origin or Source)
            public Quaternion Rotation { get; set; } // This is possible; although I can't really understand it.
            public byte Unk16 { get; set; }
            public ushort Unk17 { get; set; }

            public override string ToString()
            {
                return string.Format("Sound: {0} Start: {1} End: {2}", SoundFile, KeyFrameStart, KeyFrameEnd);
            }
        }

        public FrameData[] Frames { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int NumFrames = br.ReadInt32();
            Frames = new FrameData[NumFrames];

            for (int i = 0; i < NumFrames; i++)
            {
                FrameData frames = new FrameData();
                frames.KeyFrameStart = br.ReadInt32();
                frames.KeyFrameEnd = br.ReadInt32();
                frames.Unk03 = br.ReadByte();
                frames.SoundFile = br.ReadString16();
                frames.Unk04 = br.ReadByte();
                frames.Unk05 = br.ReadSingle();
                frames.Unk06 = br.ReadSingle();
                frames.Unk07 = br.ReadSingle();
                frames.Unk08 = br.ReadInt32();
                frames.Unk09 = br.ReadSingle();
                frames.Unk10 = br.ReadInt32();
                frames.Unk11 = br.ReadSingle();
                frames.Unk12 = br.ReadInt32();
                frames.Unk13 = br.ReadSingle();
                frames.Unk14 = br.ReadSingle();
                frames.Unk15 = br.ReadByte();
                frames.Position = Vector3Utils.ReadFromFile(br);
                frames.Rotation = QuaternionExtensions.ReadFromFile(br);
                frames.Unk16 = br.ReadByte();
                Frames[i] = frames;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Frames.Length);

            foreach(FrameData Entry in Frames)
            {
                bw.Write(Entry.KeyFrameStart);
                bw.Write(Entry.KeyFrameEnd);
                bw.Write(Entry.Unk03);
                bw.WriteString16(Entry.SoundFile);
                bw.Write(Entry.Unk04);
                bw.Write(Entry.Unk05);
                bw.Write(Entry.Unk06);
                bw.Write(Entry.Unk07);
                bw.Write(Entry.Unk08);
                bw.Write(Entry.Unk09);
                bw.Write(Entry.Unk10);
                bw.Write(Entry.Unk11);
                bw.Write(Entry.Unk12);
                bw.Write(Entry.Unk13);
                bw.Write(Entry.Unk14);
                bw.Write(Entry.Unk15);
                Entry.Position.WriteToFile(bw);
                Entry.Rotation.WriteToFile(bw);
                bw.Write(Entry.Unk16);
            }

            bw.Write(Unk05);
        }

        public override string ToString()
        {
            return string.Format("Frames: {0}", Frames.Length);
        }
    }
}
