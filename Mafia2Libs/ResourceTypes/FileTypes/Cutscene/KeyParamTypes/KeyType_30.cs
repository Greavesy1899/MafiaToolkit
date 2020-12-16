using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_30 : IKeyType
    {
        public class UnkData
        {
            public int Unk01 { get; set; } // Key Frame Start?
            public int Unk02 { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public float Unk04 { get; set; } // Could be new position

            public override string ToString()
            {
                return string.Format("{0} Start: {1} End: {2}", Unk04, Unk02, Unk03);
            }
        }

        public int NumUnkData { get; set; }
        public UnkData[] Frames { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            NumUnkData = stream.ReadInt32(isBigEndian);
            Frames = new UnkData[NumUnkData];

            for (int i = 0; i < NumUnkData; i++)
            {
                UnkData frames = new UnkData();
                frames.Unk01 = stream.ReadInt32(isBigEndian);
                frames.Unk02 = stream.ReadInt32(isBigEndian);
                frames.Unk03 = stream.ReadByte8();
                frames.Unk04 = stream.ReadSingle(isBigEndian);
                Frames[i] = frames;
            }

            Unk05 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(NumUnkData, isBigEndian);
            
            foreach(UnkData Entry in Frames)
            {
                stream.Write(Entry.Unk01, isBigEndian);
                stream.Write(Entry.Unk02, isBigEndian);
                stream.WriteByte(Entry.Unk03);
                stream.Write(Entry.Unk01, isBigEndian);
            }

            stream.Write(Unk05, isBigEndian);
        }

        public override string ToString()
        {
            return string.Format("NumFrames: {0}", Frames.Length);
        }
    }
}
