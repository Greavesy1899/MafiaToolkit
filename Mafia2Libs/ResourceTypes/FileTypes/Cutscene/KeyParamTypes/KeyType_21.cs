using System.IO;
using Utils.Extensions;

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

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            uint NumInformations = stream.ReadUInt32(isBigEndian);
            KeyInformation = new KeyInfo_21[NumInformations];

            for(int i = 0; i < NumInformations; i++)
            {
                KeyInfo_21 Info = new KeyInfo_21();
                Info.KeyFrameStart = stream.ReadUInt32(isBigEndian);
                Info.KeyFrameEnd = stream.ReadUInt32(isBigEndian);
                Info.Unk0 = stream.ReadByte8();
                Info.Unk1 = stream.ReadUInt32(isBigEndian);
                Info.Unk2 = stream.ReadUInt16(isBigEndian);
                KeyInformation[i] = Info;
            }

            Unk0 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(KeyInformation.Length, isBigEndian);

            foreach(KeyInfo_21 Info in KeyInformation)
            {
                stream.Write(Info.KeyFrameStart, isBigEndian);
                stream.Write(Info.KeyFrameEnd, isBigEndian);
                stream.WriteByte(Info.Unk0);
                stream.Write(Info.Unk1, isBigEndian);
                stream.Write(Info.Unk2, isBigEndian);
            }

            stream.Write(Unk0, isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("Num Keys: {0}", KeyInformation.Length);
        }
    }
}
