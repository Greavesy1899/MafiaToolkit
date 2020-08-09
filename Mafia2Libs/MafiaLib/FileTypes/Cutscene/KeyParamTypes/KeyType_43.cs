using SharpDX;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_43 : IKeyType
    {
        public class FrameData
        {
            public int Unk01 { get; set; } // Key Frame Start?
            public int Unk02 { get; set; } // Key Frame End?
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
                return string.Format("Sound: {0} Start: {1} End: {2}", SoundFile, Unk01, Unk02);
            }
        }

        public int NumFrames { get; set; }
        public FrameData[] Frames { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            NumFrames = stream.ReadInt32(isBigEndian);
            Frames = new FrameData[NumFrames];

            for (int i = 0; i < NumFrames; i++)
            {
                FrameData frames = new FrameData();
                frames.Unk01 = stream.ReadInt32(isBigEndian);
                frames.Unk02 = stream.ReadInt32(isBigEndian);
                frames.Unk03 = stream.ReadByte8();
                frames.SoundFile = stream.ReadString16(isBigEndian);
                frames.Unk04 = stream.ReadByte8();
                frames.Unk05 = stream.ReadSingle(isBigEndian);
                frames.Unk06 = stream.ReadSingle(isBigEndian);
                frames.Unk07 = stream.ReadSingle(isBigEndian);
                frames.Unk08 = stream.ReadInt32(isBigEndian);
                frames.Unk09 = stream.ReadSingle(isBigEndian);
                frames.Unk10 = stream.ReadInt32(isBigEndian);
                frames.Unk11 = stream.ReadSingle(isBigEndian);
                frames.Unk12 = stream.ReadInt32(isBigEndian);
                frames.Unk13 = stream.ReadSingle(isBigEndian);
                frames.Unk14 = stream.ReadSingle(isBigEndian);
                frames.Unk15 = stream.ReadByte8();
                frames.Position = Vector3Extenders.ReadFromFile(stream, isBigEndian);
                frames.Rotation = QuaternionExtensions.ReadFromFile(stream, isBigEndian);
                frames.Unk16 = stream.ReadByte8();
                Frames[i] = frames;
            }

            Unk05 = stream.ReadUInt16(isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("NumFrames: {0}", Frames.Length);
        }
    }
}
