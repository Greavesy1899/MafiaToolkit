using System.ComponentModel;
using System.IO;
using System.Windows.Controls;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_28 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class DataWrapper
        {
            public class Type0Data
            {
                public int KeyFrameStart { get; set; } // Key Frame Start?
                public int KeyFrameEnd { get; set; } // Key Frame End?
                public byte Unk00 { get; set; } // Always 1?
                public float Unk01 { get; set; }
                public Type0Data(BinaryReader br)
                {
                    Read(br);
                }

                public void Read(BinaryReader br)
                {
                    KeyFrameStart = br.ReadInt32();
                    KeyFrameEnd = br.ReadInt32();
                    Unk00 = br.ReadByte();
                    Unk01 = br.ReadSingle();
                }

                public void Write(BinaryWriter bw)
                {
                    bw.Write(KeyFrameStart);
                    bw.Write(KeyFrameEnd);
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                }
                public override string ToString()
                {
                    return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
                }
            }

            public class Type1Data
            {
                public int KeyFrameStart { get; set; } // Key Frame Start?
                public int KeyFrameEnd { get; set; } // Key Frame End?
                public byte Unk00 { get; set; } // Always 1?
                public ushort Unk01 { get; set; }
                public ushort Unk02 { get; set; }
                public float[] Unk03 { get; set; }

                public Type1Data(BinaryReader br)
                {
                    Read(br);
                }

                public void Read(BinaryReader br)
                {
                    KeyFrameStart = br.ReadInt32();
                    KeyFrameEnd = br.ReadInt32();
                    Unk00 = br.ReadByte();
                    Unk01 = br.ReadUInt16();
                    Unk02 = br.ReadUInt16();
                    Unk03 = new float[5];

                    for (int z = 0; z < Unk03.Length; z++)
                    {
                        Unk03[z] = br.ReadSingle();
                    }
                }

                public void Write(BinaryWriter bw)
                {
                    bw.Write(KeyFrameStart);
                    bw.Write(KeyFrameEnd);
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);

                    foreach (float Value in Unk03)
                    {
                        bw.Write(Value);
                    }
                }

                public override string ToString()
                {
                    return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
                }
            }

            public int Type { get; set; }
            public Type0Data[] FramesType0 { get; set; } = new Type0Data[0];
            public Type1Data[] FramesType1 { get; set; } = new Type1Data[0];
            public DataWrapper(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                Type = br.ReadInt32();
                int Count = br.ReadInt32();

                switch (Type)
                {
                    case 0:
                        FramesType0 = new Type0Data[Count];

                        for (int i = 0; i < FramesType0.Length; i++)
                        {
                            FramesType0[i] = new Type0Data(br);
                        }
                        break;

                    case 1:
                        FramesType1 = new Type1Data[Count];

                        for (int i = 0; i < FramesType1.Length; i++)
                        {
                            FramesType1[i] = new Type1Data(br);
                        }
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(Type);

                switch (Type)
                {
                    case 0:
                        bw.Write(FramesType0.Length);

                        foreach (var frame in FramesType0)
                        {
                            frame.Write(bw);
                        }
                        break;

                    case 1:
                        bw.Write(FramesType1.Length);

                        foreach (var frame in FramesType1)
                        {
                            frame.Write(bw);
                        }
                        break;
                }
            }
        }

        public uint Unk00 { get; set; }
        public DataWrapper[] Wrappers { get; set; }
        public ushort Unk01 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            Wrappers = new DataWrapper[3];
            Unk00 = br.ReadUInt32();

            for (int x = 0; x < 3; x++)
            {
                Wrappers[x] = new DataWrapper(br);
            }

            Unk01 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Unk00);

            foreach(DataWrapper Wrapper in Wrappers)
            {
                Wrapper.Write(bw);
            }

            bw.Write(Unk01);
        }

        public override string ToString()
        {
            return string.Format("Type: 28 Frames: {0}", Wrappers[0].FramesType0.Length + Wrappers[1].FramesType0.Length + Wrappers[2].FramesType0.Length + Wrappers[0].FramesType1.Length + Wrappers[1].FramesType1.Length + Wrappers[2].FramesType1.Length);
        }
    }
}
