using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_19 : IKeyType
    {
        public class SoundData_Type19
        {
            public int Unk01 { get; set; } // Key Frame Start?
            public int Unk02 { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public string SoundFile { get; set; } // Sound to play.
            public byte[] RestOfTheData { get; set; } // The rest of the data.
            public byte Unk04 { get; set; }
            public float Unk05 { get; set; }
            public float Unk06 { get; set; }
            public float Unk07 { get; set; }
            public int Unk08 { get; set; }
            public float Unk09 { get; set; }
            public float Unk10 { get; set; }
            public int Unk11 { get; set; }
            public float Unk12 { get; set; }
            public int Unk13 { get; set; }
            public float Unk14 { get; set; }
            public float Unk15 { get; set; }
            public float Unk16 { get; set; }
            public float Unk17 { get; set; }
            public byte Unk18 { get; set; }
            public Vector3 Position { get; set; }
            public float Unk19 { get; set; }
            public float Unk20 { get; set; }
            public float Unk21 { get; set; }
            public float Unk22 { get; set; }
            public byte IsMeshAvailableFlags { get; set; }
            public string FrameName { get; set; }
            public ulong MainJointHash { get; set; }
            public ulong FrameHash { get; set; }

            public override string ToString()
            {
                return string.Format("Sound: {0} Start: {1} End: {2} Frame: {3}", SoundFile, Unk01, Unk02, FrameName);
            }
        }

        public int NumSounds { get; set; }
        public SoundData_Type19[] Sounds { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            NumSounds = stream.ReadInt32(isBigEndian);
            Sounds = new SoundData_Type19[NumSounds];

            for (int i = 0; i < NumSounds; i++)
            {
                SoundData_Type19 SoundInfo = new SoundData_Type19();
                SoundInfo.Unk01 = stream.ReadInt32(isBigEndian);
                SoundInfo.Unk02 = stream.ReadInt32(isBigEndian);
                SoundInfo.Unk03 = stream.ReadByte8();
                SoundInfo.SoundFile = stream.ReadString16(isBigEndian);
                SoundInfo.Unk04 = stream.ReadByte8();
                SoundInfo.Unk05 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk06 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk07 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk08 = stream.ReadInt32(isBigEndian);
                SoundInfo.Unk10 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk11 = stream.ReadInt32(isBigEndian);
                SoundInfo.Unk12 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk13 = stream.ReadInt32(isBigEndian);
                SoundInfo.Unk14 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk15 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk16 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk17 = stream.ReadSingle(isBigEndian);

                SoundInfo.Unk18 = stream.ReadByte8();
                SoundInfo.Position = Vector3Utils.ReadFromFile(stream, isBigEndian);
                SoundInfo.Unk19 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk20 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk21 = stream.ReadSingle(isBigEndian);
                SoundInfo.Unk22 = stream.ReadSingle(isBigEndian);

                SoundInfo.IsMeshAvailableFlags = stream.ReadByte8();
                if(SoundInfo.IsMeshAvailableFlags == 3)
                {
                    SoundInfo.FrameName = stream.ReadString16(isBigEndian);
                    SoundInfo.MainJointHash = stream.ReadUInt64(isBigEndian);
                    SoundInfo.FrameHash = stream.ReadUInt64(isBigEndian);
                }
                else if(SoundInfo.IsMeshAvailableFlags == 1)
                {
                    SoundInfo.FrameName = stream.ReadString16(isBigEndian);
                    SoundInfo.MainJointHash = stream.ReadUInt64(isBigEndian);
                }

                Sounds[i] = SoundInfo;
            }

            Unk05 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);

            stream.Write(NumSounds, isBigEndian);

            foreach(SoundData_Type19 Info in Sounds)
            {
                stream.Write(Info.Unk01, isBigEndian);
                stream.Write(Info.Unk02, isBigEndian);
                stream.WriteByte(Info.Unk03);
                stream.WriteString16(Info.SoundFile, isBigEndian);
                stream.WriteByte(Info.Unk04);
                stream.Write(Info.Unk05, isBigEndian);
                stream.Write(Info.Unk06, isBigEndian);
                stream.Write(Info.Unk07, isBigEndian);
                stream.Write(Info.Unk08, isBigEndian);
                stream.Write(Info.Unk10, isBigEndian);
                stream.Write(Info.Unk11, isBigEndian);
                stream.Write(Info.Unk12, isBigEndian);
                stream.Write(Info.Unk13, isBigEndian);
                stream.Write(Info.Unk14, isBigEndian);
                stream.Write(Info.Unk15, isBigEndian);
                stream.Write(Info.Unk16, isBigEndian);
                stream.Write(Info.Unk17, isBigEndian);
                stream.WriteByte(Info.Unk18);
                Info.Position.WriteToFile(stream, isBigEndian);
                stream.Write(Info.Unk19, isBigEndian);
                stream.Write(Info.Unk20, isBigEndian);
                stream.Write(Info.Unk21, isBigEndian);
                stream.Write(Info.Unk22, isBigEndian);

                stream.WriteByte(Info.IsMeshAvailableFlags);
                if(Info.IsMeshAvailableFlags == 3)
                {
                    stream.WriteString16(Info.FrameName, isBigEndian);
                    stream.Write(Info.MainJointHash, isBigEndian);
                    stream.Write(Info.FrameHash, isBigEndian);
                }
                else if(Info.IsMeshAvailableFlags == 1)
                {
                    stream.WriteString16(Info.FrameName, isBigEndian);
                    stream.Write(Info.MainJointHash, isBigEndian);
                }
            }

            stream.Write(Unk05, isBigEndian);
        }
        public override string ToString()
        {
            return string.Format("NumSounds (19): {0}", Sounds.Length);
        }
    }
}
