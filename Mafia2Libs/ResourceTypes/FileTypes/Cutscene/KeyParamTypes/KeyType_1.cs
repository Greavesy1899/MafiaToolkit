using System.IO;
using System.Numerics;
using Vortice.Mathematics;
using Utils.VorticeUtils;

namespace ResourceTypes.Cutscene.KeyParams
{
    public class KeyType_1 : IKeyType
    {
        public class FrameData
        {
            public int KeyFrameStart { get; set; } // Key Frame Start?
            public int KeyFrameEnd { get; set; } // Key Frame End?
            public byte Unk03 { get; set; } // Is Available?
            public uint Unk04 { get; set; }
            public uint Unk05 { get; set; }
            public Vector4 UnkVec { get; set; } = Vector4.One;

            public override string ToString()
            {
                return string.Format("Start: {0} End: {1}", KeyFrameStart, KeyFrameEnd);
            }
        }

        public FrameData[] Frames { get; set; } = new FrameData[0];
        public ushort Unk05 { get; set; }

        public override void ReadFromFile(BinaryReader br)
        {
            base.ReadFromFile(br);

            int NumFrames = br.ReadInt32();
            Frames = new FrameData[NumFrames];

            for (int i = 0; i < NumFrames; i++)
            {
                FrameData frames = new FrameData();
                frames.KeyFrameStart = br.ReadInt32();
                frames.KeyFrameEnd = br.ReadInt32();
                frames.Unk03 = br.ReadByte();
                frames.Unk04 = br.ReadUInt32();
                frames.Unk05 = br.ReadUInt32();
                frames.UnkVec = Vector4Extenders.ReadFromFile(br);
                Frames[i] = frames;
            }

            Unk05 = br.ReadUInt16();
        }

        public override void WriteToFile(BinaryWriter bw)
        {
            base.WriteToFile(bw);
            bw.Write(Frames.Length);

            foreach (FrameData Entry in Frames)
            {
                bw.Write(Entry.KeyFrameStart);
                bw.Write(Entry.KeyFrameEnd);
                bw.Write(Entry.Unk03);
                bw.Write(Entry.Unk04);
                bw.Write(Entry.Unk05);
                Entry.UnkVec.WriteToFile(bw);
            }

            bw.Write(Unk05);
        }

        public override string ToString()
        {
            return string.Format("Type: 1 Frames: {0}", Frames.Length);
        }
    }
}
