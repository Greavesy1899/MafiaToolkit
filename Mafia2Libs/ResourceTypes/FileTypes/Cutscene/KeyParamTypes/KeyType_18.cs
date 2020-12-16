using System.IO;
using Utils.Extensions;
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
                frames.Unk04 = stream.ReadInt32(isBigEndian);
                frames.Unk05 = stream.ReadByte8();
                frames.Unk06 = stream.ReadSingle(isBigEndian);
                frames.Unk07 = stream.ReadSingle(isBigEndian);
                frames.Unk08 = stream.ReadSingle(isBigEndian);
                frames.Unk09 = stream.ReadSingle(isBigEndian);
                frames.Unk10 = stream.ReadInt32(isBigEndian);
                frames.Unk11 = stream.ReadSingle(isBigEndian);
                frames.Unk12 = stream.ReadSingle(isBigEndian);
                Sounds[i] = frames;
            }

            Unk05 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(NumSounds, isBigEndian);

            foreach(FrameData FrameInfo in Sounds)
            {
                stream.Write(FrameInfo.Unk01, isBigEndian);
                stream.Write(FrameInfo.Unk02, isBigEndian);
                stream.WriteByte(FrameInfo.Unk03);
                stream.WriteString16(FrameInfo.SoundFile, isBigEndian);
                stream.Write(FrameInfo.Unk04, isBigEndian);
                stream.WriteByte(FrameInfo.Unk05);
                stream.Write(FrameInfo.Unk06, isBigEndian);
                stream.Write(FrameInfo.Unk07, isBigEndian);
                stream.Write(FrameInfo.Unk08, isBigEndian);
                stream.Write(FrameInfo.Unk09, isBigEndian);
                stream.Write(FrameInfo.Unk10, isBigEndian);
                stream.Write(FrameInfo.Unk11, isBigEndian);
                stream.Write(FrameInfo.Unk12, isBigEndian);
            }

            stream.Write(Unk05, isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("NumSounds: {0}", Sounds.Length);
        }
    }
}
