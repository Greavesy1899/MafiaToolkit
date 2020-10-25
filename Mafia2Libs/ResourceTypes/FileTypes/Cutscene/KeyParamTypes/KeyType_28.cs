using System.IO;
using Utils.Extensions;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_28 : IKeyType
    {
        public class UnkWrapper
        {
            public class UnkData
            {
                public int Unk01 { get; set; } // Key Frame Start?
                public int Unk02 { get; set; } // Key Frame End?
                public ushort Unk03 { get; set; } // Always 1?
                public ushort Unk04 { get; set; } //0x1B?
                public byte Unk05 { get; set; } // ??
                public float[] Unk06 { get; set; }

                public override string ToString()
                {
                    return string.Format("Start: {0} End: {1}", Unk01, Unk02);
                }
            }

            public int Unk0 { get; set; }
            public int NumUnkData { get; set; }
            public UnkData[] Frames { get; set; }

        }

        public uint Unk01 { get; set; }
        public UnkWrapper[] Wrappers { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            base.ReadFromFile(stream, isBigEndian);

            Wrappers = new UnkWrapper[3];
            Unk01 = stream.ReadUInt32(isBigEndian);
            for (int x = 0; x < 3; x++)
            {
                UnkWrapper data = new UnkWrapper();
                data.Unk0 = stream.ReadInt32(isBigEndian);
                data.NumUnkData = stream.ReadInt32(isBigEndian);
                data.Frames = new UnkWrapper.UnkData[data.NumUnkData];

                for (int i = 0; i < data.NumUnkData; i++)
                {
                    UnkWrapper.UnkData frames = new UnkWrapper.UnkData();
                    frames.Unk01 = stream.ReadInt32(isBigEndian);
                    frames.Unk02 = stream.ReadInt32(isBigEndian);
                    frames.Unk03 = stream.ReadUInt16(isBigEndian);
                    frames.Unk04 = stream.ReadUInt16(isBigEndian);
                    frames.Unk05 = stream.ReadByte8();
                    frames.Unk06 = new float[5];

                    for (int z = 0; z < 5; z++)
                    {
                        frames.Unk06[z] = stream.ReadSingle(isBigEndian);
                    }

                    data.Frames[i] = frames;
                }

                Wrappers[x] = data;
            }

            Unk05 = stream.ReadUInt16(isBigEndian);
        }

        public override void WriteToFile(MemoryStream stream, bool isBigEndian)
        {
            base.WriteToFile(stream, isBigEndian);
            stream.Write(Unk01, isBigEndian);

            foreach(UnkWrapper Wrapper in Wrappers)
            {
                stream.Write(Wrapper.Unk0, isBigEndian);
                stream.Write(Wrapper.NumUnkData, isBigEndian);

                foreach(UnkWrapper.UnkData Info in Wrapper.Frames)
                {
                    stream.Write(Info.Unk01, isBigEndian);
                    stream.Write(Info.Unk02, isBigEndian);
                    stream.Write(Info.Unk03, isBigEndian);
                    stream.Write(Info.Unk04, isBigEndian);
                    stream.WriteByte(Info.Unk05);

                    foreach(float Value in Info.Unk06)
                    {
                        stream.Write(Value, isBigEndian);
                    }
                }
            }

            stream.Write(Unk05, isBigEndian);
        }
    }
}
