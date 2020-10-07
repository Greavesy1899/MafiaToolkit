using SharpDX;
using System.IO;
using Utils.Extensions;
using Utils.SharpDXExtensions;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_19 : IKeyType
    {
        public class FrameData
        {
            public int Unk01 { get; set; } // Key Frame Start?
            public int Unk02 { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public string SoundFile { get; set; } // Sound to play.
            public byte[] RestOfTheData { get; set; } // The rest of the data.
            //public byte Unk04 { get; set; }
            //public float Unk05 { get; set; }
            //public float Unk06 { get; set; }
            //public float Unk07 { get; set; }
            //public int Unk08 { get; set; } // This was 0.0f for scr_train_2?
            //public float Unk09 { get; set; } // This was 1.0f for scr_train_2?
            //public int Unk10 { get; set; }
            //public float Unk11 { get; set; } // This was 1.0f for scr_train_2?
            //public int Unk12 { get; set; }
            //public float Unk13 { get; set; }
            //public float Unk14 { get; set; } 
            //public float Unk15 { get; set; } 
            //public float Unk16 { get; set; } 
            //public float Unk17 { get; set; }
            //public float Unk18 { get; set; } 
            //public int Unk19 { get; set; }
            //public float Unk20 { get; set; }

            public override string ToString()
            {
                return string.Format("Sound: {0} Start: {1} End: {2}", SoundFile, Unk01, Unk02);
            }
        }

        public int NumSounds { get; set; }
        public FrameData[] Sounds { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            NumSounds = stream.ReadInt32(isBigEndian);
            Sounds = new FrameData[NumSounds];

            for (int i = 0; i < NumSounds; i++)
            {
                FrameData frames = new FrameData();
                frames.Unk01 = stream.ReadInt32(isBigEndian);
                frames.Unk02 = stream.ReadInt32(isBigEndian);
                frames.Unk03 = stream.ReadByte8();
                frames.SoundFile = stream.ReadString16(isBigEndian);
                frames.RestOfTheData = stream.ReadBytes(79);
                //frames.Unk04 = stream.ReadByte8();
                //frames.Unk05 = stream.ReadSingle(isBigEndian);
                //frames.Unk06 = stream.ReadSingle(isBigEndian);
                //frames.Unk07 = stream.ReadSingle(isBigEndian);
                //frames.Unk08 = stream.ReadInt32(isBigEndian);
                //frames.Unk09 = stream.ReadSingle(isBigEndian);
                //frames.Unk10 = stream.ReadInt32(isBigEndian);
                //frames.Unk11 = stream.ReadSingle(isBigEndian);
                //frames.Unk12 = stream.ReadInt32(isBigEndian);
                //frames.Unk13 = stream.ReadSingle(isBigEndian);
                //frames.Unk14 = stream.ReadSingle(isBigEndian);
                //frames.Unk15 = stream.ReadSingle(isBigEndian);
                //frames.Unk16 = stream.ReadInt32(isBigEndian);
                //frames.Unk17 = stream.ReadSingle(isBigEndian);
                //frames.Unk18 = stream.ReadSingle(isBigEndian);
                //frames.Unk19 = stream.ReadInt32(isBigEndian);
                //frames.Unk20 = stream.ReadSingle(isBigEndian);
                Sounds[i] = frames;
            }

            Unk05 = stream.ReadUInt16(isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("NumFrames: {0}", Sounds.Length);
        }
    }
}
