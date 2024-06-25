using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.StringHelpers;
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

        public SoundData_Type19[] Sounds { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int NumSounds = br.ReadInt32();
            Sounds = new SoundData_Type19[NumSounds];

            for (int i = 0; i < NumSounds; i++)
            {
                SoundData_Type19 SoundInfo = new SoundData_Type19();
                SoundInfo.Unk01 = br.ReadInt32();
                SoundInfo.Unk02 = br.ReadInt32();
                SoundInfo.Unk03 = br.ReadByte();
                SoundInfo.SoundFile = br.ReadString16();
                SoundInfo.Unk04 = br.ReadByte();
                SoundInfo.Unk05 = br.ReadSingle();
                SoundInfo.Unk06 = br.ReadSingle();
                SoundInfo.Unk07 = br.ReadSingle();
                SoundInfo.Unk08 = br.ReadInt32();
                SoundInfo.Unk10 = br.ReadSingle();
                SoundInfo.Unk11 = br.ReadInt32();
                SoundInfo.Unk12 = br.ReadSingle();
                SoundInfo.Unk13 = br.ReadInt32();
                SoundInfo.Unk14 = br.ReadSingle();
                SoundInfo.Unk15 = br.ReadSingle();
                SoundInfo.Unk16 = br.ReadSingle();
                SoundInfo.Unk17 = br.ReadSingle();

                SoundInfo.Unk18 = br.ReadByte();
                SoundInfo.Position = Vector3Utils.ReadFromFile(br);
                SoundInfo.Unk19 = br.ReadSingle();
                SoundInfo.Unk20 = br.ReadSingle();
                SoundInfo.Unk21 = br.ReadSingle();
                SoundInfo.Unk22 = br.ReadSingle();

                SoundInfo.IsMeshAvailableFlags = br.ReadByte();
                if(SoundInfo.IsMeshAvailableFlags == 3)
                {
                    SoundInfo.FrameName = br.ReadString16();
                    SoundInfo.MainJointHash = br.ReadUInt64();
                    SoundInfo.FrameHash = br.ReadUInt64();
                }
                else if(SoundInfo.IsMeshAvailableFlags == 1)
                {
                    SoundInfo.FrameName = br.ReadString16();
                    SoundInfo.MainJointHash = br.ReadUInt64();
                }

                Sounds[i] = SoundInfo;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);

            bw.Write(Sounds.Length);

            foreach(SoundData_Type19 Info in Sounds)
            {
                bw.Write(Info.Unk01);
                bw.Write(Info.Unk02);
                bw.Write(Info.Unk03);
                bw.WriteString16(Info.SoundFile);
                bw.Write(Info.Unk04);
                bw.Write(Info.Unk05);
                bw.Write(Info.Unk06);
                bw.Write(Info.Unk07);
                bw.Write(Info.Unk08);
                bw.Write(Info.Unk10);
                bw.Write(Info.Unk11);
                bw.Write(Info.Unk12);
                bw.Write(Info.Unk13);
                bw.Write(Info.Unk14);
                bw.Write(Info.Unk15);
                bw.Write(Info.Unk16);
                bw.Write(Info.Unk17);
                bw.Write(Info.Unk18);
                Info.Position.WriteToFile(bw);
                bw.Write(Info.Unk19);
                bw.Write(Info.Unk20);
                bw.Write(Info.Unk21);
                bw.Write(Info.Unk22);

                bw.Write(Info.IsMeshAvailableFlags);
                if(Info.IsMeshAvailableFlags == 3)
                {
                    bw.WriteString16(Info.FrameName);
                    bw.Write(Info.MainJointHash);
                    bw.Write(Info.FrameHash);
                }
                else if(Info.IsMeshAvailableFlags == 1)
                {
                    bw.WriteString16(Info.FrameName);
                    bw.Write(Info.MainJointHash);
                }
            }

            bw.Write(Unk05);
        }
        public override string ToString()
        {
            return string.Format("NumSounds (19): {0}", Sounds.Length);
        }
    }
}
