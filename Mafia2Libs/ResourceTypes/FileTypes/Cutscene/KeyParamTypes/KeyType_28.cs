using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_28 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
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
            public UnkData[] Frames { get; set; }

        }

        public uint Unk01 { get; set; }
        public UnkWrapper[] Wrappers { get; set; }
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            Wrappers = new UnkWrapper[3];
            Unk01 = br.ReadUInt32();
            for (int x = 0; x < 3; x++)
            {
                UnkWrapper data = new UnkWrapper();
                data.Unk0 = br.ReadInt32();
                int NumUnkData = br.ReadInt32();
                data.Frames = new UnkWrapper.UnkData[NumUnkData];

                for (int i = 0; i < NumUnkData; i++)
                {
                    UnkWrapper.UnkData frames = new UnkWrapper.UnkData();
                    frames.Unk01 = br.ReadInt32();
                    frames.Unk02 = br.ReadInt32();
                    frames.Unk03 = br.ReadUInt16();
                    frames.Unk04 = br.ReadUInt16();
                    frames.Unk05 = br.ReadByte();
                    frames.Unk06 = new float[5];

                    for (int z = 0; z < 5; z++)
                    {
                        frames.Unk06[z] = br.ReadSingle();
                    }

                    data.Frames[i] = frames;
                }

                Wrappers[x] = data;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Unk01);

            foreach(UnkWrapper Wrapper in Wrappers)
            {
                bw.Write(Wrapper.Unk0);
                bw.Write(Wrapper.Frames.Length);

                foreach(UnkWrapper.UnkData Info in Wrapper.Frames)
                {
                    bw.Write(Info.Unk01);
                    bw.Write(Info.Unk02);
                    bw.Write(Info.Unk03);
                    bw.Write(Info.Unk04);
                    bw.Write(Info.Unk05);

                    foreach(float Value in Info.Unk06)
                    {
                        bw.Write(Value);
                    }
                }
            }

            bw.Write(Unk05);
        }

        public override string ToString()
        {
            return string.Format("Num Frames: {0}", Wrappers[0].Frames.Length + Wrappers[1].Frames.Length + Wrappers[2].Frames.Length);
        }
    }
}
