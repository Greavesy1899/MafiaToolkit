using System.ComponentModel;
using System.IO;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_17 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class FrameData
        {
            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class DefaultData
            {
                public DefaultData()
                {

                }

                public DefaultData(BinaryReader br)
                {
                    Read(br);
                }

                public virtual void Read(BinaryReader br)
                {
                    
                }

                public virtual void Write(BinaryWriter bw)
                {
                    
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class Type1Data : DefaultData
            {
                public int Unk00 { get; set; }
                public float Unk01 { get; set; }
                public float Unk02 { get; set; }
                public int Unk03 { get; set; }
                public Type1Data()
                {

                }

                public Type1Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadInt32();
                    Unk01 = br.ReadSingle();
                    Unk02 = br.ReadSingle();
                    Unk03 = br.ReadInt32();
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);
                    bw.Write(Unk03);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class Type2Data : DefaultData
            {
                public int Unk00 { get; set; }
                public float Unk01 { get; set; }
                public float Unk02 { get; set; }
                public float Unk03 { get; set; }
                public Type2Data()
                {

                }

                public Type2Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadInt32();
                    Unk01 = br.ReadSingle();
                    Unk02 = br.ReadSingle();
                    Unk03 = br.ReadSingle();
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);
                    bw.Write(Unk03);
                }
            }

            [TypeConverter(typeof(ExpandableObjectConverter))]
            public class Type3Data : DefaultData
            {
                public float Unk00 { get; set; }
                public float Unk01 { get; set; }
                public float Unk02 { get; set; }
                public float Unk03 { get; set; }
                public float Unk04 { get; set; }
                public float Unk05 { get; set; }
                public Type3Data()
                {

                }

                public Type3Data(BinaryReader br)
                {
                    Read(br);
                }

                public override void Read(BinaryReader br)
                {
                    Unk00 = br.ReadSingle();
                    Unk01 = br.ReadSingle();
                    Unk02 = br.ReadSingle();
                    Unk03 = br.ReadSingle();
                    Unk04 = br.ReadSingle();
                    Unk05 = br.ReadSingle();
                }

                public override void Write(BinaryWriter bw)
                {
                    bw.Write(Unk00);
                    bw.Write(Unk01);
                    bw.Write(Unk02);
                    bw.Write(Unk03);
                    bw.Write(Unk04);
                    bw.Write(Unk05);
                }
            }

            public int KeyFrameStart { get; set; }
            public int KeyFrameEnd { get; set; }
            public byte Unk03 { get; set; } // Is Available?
            public int Type { get; set; }
            public DefaultData Data { get; set; } = new Type1Data();

            public FrameData(BinaryReader br)
            {
                Read(br);
            }

            public void Read(BinaryReader br)
            {
                KeyFrameStart = br.ReadInt32();
                KeyFrameEnd = br.ReadInt32();
                Unk03 = br.ReadByte();
                Type = br.ReadInt32();

                switch (Type)
                {
                    case 2:
                        Data = new Type2Data(br);
                        break;

                    case 3:
                        Data = new Type3Data(br);
                        break;

                    default:
                        Data = new Type1Data(br);
                        break;
                }
            }

            public void Write(BinaryWriter bw)
            {
                bw.Write(KeyFrameStart);
                bw.Write(KeyFrameEnd);
                bw.Write(Unk03);
                bw.Write(Type);
                Data.Write(bw);
            }

            public override string ToString()
            {
                return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
            }
        }
        public FrameData[] Data { get; set; }
        public ushort Unk01 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int Count = br.ReadInt32();
            Data = new FrameData[Count];

            for (int i = 0; i < Count; i++)
            {
                Data[i] = new(br);
            }

            Unk01 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Data.Length);

            for (int i = 0; i < Data.Length; i++)
            {
                Data[i].Write(bw);
            }

            bw.Write(Unk01);
        }

        public override string ToString()
        {
            return string.Format("Type: 17 Frames: {0}", Data.Length);
        }
    }
}
