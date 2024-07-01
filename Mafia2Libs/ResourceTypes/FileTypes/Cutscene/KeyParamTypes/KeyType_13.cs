using System.ComponentModel;
using System.IO;
using Utils.Extensions;
using Utils.StringHelpers;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_13 : IKeyType
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class AnimationData
        {
            public int Unk01 { get; set; }
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk04 { get; set; } // Is Name Available?
            public string AnimationName { get; set; } // Links too .an2 file
            public int Unk05 { get; set; }
            public float Unk06 { get; set; }
            public int Unk07 { get; set; }
            public float Unk08 { get; set; }

            public override string ToString()
            {
                return string.Format("{0} Start: {1} End: {2}", AnimationName, KeyFrameStart, KeyFrameEnd);
            }
        }

        public AnimationData[] Animations { get; set; }
        public ushort Unk01 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int animationCount = br.ReadInt32();
            Animations = new AnimationData[animationCount];

            for (int i = 0; i < Animations.Length; i++)
            {
                AnimationData animation = new AnimationData();
                animation.Unk01 = br.ReadInt32();
                animation.KeyFrameStart = br.ReadInt32();
                animation.KeyFrameEnd = br.ReadInt32();
                animation.Unk04 = br.ReadByte();
                animation.AnimationName = br.ReadString16();
                animation.Unk05 = br.ReadInt32();
                animation.Unk06 = br.ReadSingle();
                animation.Unk07 = br.ReadInt32();
                animation.Unk08 = br.ReadSingle();
                Animations[i] = animation;
            }

            Unk01 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Animations.Length);

            foreach(AnimationData Animation in Animations)
            {
                bw.Write(Animation.Unk01);
                bw.Write(Animation.KeyFrameStart);
                bw.Write(Animation.KeyFrameEnd);
                bw.Write(Animation.Unk04);
                bw.WriteString16(Animation.AnimationName);
                bw.Write(Animation.Unk05);
                bw.Write(Animation.Unk06);
                bw.Write(Animation.Unk07);
                bw.Write(Animation.Unk08);
            }

            bw.Write(Unk01);
        }

        public override string ToString()
        {
            return string.Format("Type: 13 Animations: {0}", Animations.Length);
        }
    }
}
