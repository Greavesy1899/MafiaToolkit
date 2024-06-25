using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_21 : IKeyType
    {
        public class KeyInfo_21
        {
            public uint KeyFrameStart { get; set; }
            public uint KeyFrameEnd { get; set; }
            public byte Unk0 { get; set; } // 1?
            public uint Unk1 { get; set; }
            public ushort Unk2 { get; set; }

            public override string ToString()
            {
                return string.Format("Start: {0} End: {1} Value: {2}", KeyFrameStart, KeyFrameEnd, Unk1);
            }
        }

        public KeyInfo_21[] KeyInformation { get; set; }
        public ushort Unk0 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            uint NumInformations = br.ReadUInt32();
            KeyInformation = new KeyInfo_21[NumInformations];

            for(int i = 0; i < NumInformations; i++)
            {
                KeyInfo_21 Info = new KeyInfo_21();
                Info.KeyFrameStart = br.ReadUInt32();
                Info.KeyFrameEnd = br.ReadUInt32();
                Info.Unk0 = br.ReadByte();
                Info.Unk1 = br.ReadUInt32();
                Info.Unk2 = br.ReadUInt16();
                KeyInformation[i] = Info;
            }

            Unk0 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(KeyInformation.Length);

            foreach(KeyInfo_21 Info in KeyInformation)
            {
                bw.Write(Info.KeyFrameStart);
                bw.Write(Info.KeyFrameEnd);
                bw.Write(Info.Unk0);
                bw.Write(Info.Unk1);
                bw.Write(Info.Unk2);
            }

            bw.Write(Unk0);
        }

        public override string ToString()
        {
            return string.Format("Keys: {0}", KeyInformation.Length);
        }
    }
}
