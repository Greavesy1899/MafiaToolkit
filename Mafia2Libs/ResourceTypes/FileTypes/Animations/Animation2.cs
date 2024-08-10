using ResourceTypes.ModelHelpers.ModelExporter;
using System.IO;
using Utils.Logging;

namespace ResourceTypes.Animation2
{
    public class Animation2
    {
        public Header Header { get; set; } = new();
        public bool IsDataPresent { get; set; } //Not confirmed?
        public Event[] Events { get; set; } = new Event[0];
        public ushort Unk00 { get; set; }
        public ushort Unk01 { get; set; }
        public AnimTrack[] Tracks { get; set; } = new AnimTrack[0];
        public short[] UnkShorts00 { get; set; } = new short[0];
        public short[] UnkShorts01 { get; set; } = new short[0];

        public Animation2()
        {

        }

        public Animation2(string fileName)
        {
            Read(fileName);
        }

        public Animation2(Stream s)
        {
            Read(s);
        }

        public Animation2(BinaryReader br)
        {
            Read(br);
        }

        public void Read(string fileName)
        {
            using (MemoryStream ms = new(File.ReadAllBytes(fileName)))
            {
                Read(ms);
            }
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            Header = new(br);

            IsDataPresent = br.ReadBoolean();

            Events = new Event[Header.NumEvents];

            for (int i = 0; i < Events.Length; i++)
            {
                Events[i] = new(br);
            }

            Unk00 = br.ReadUInt16();
            Unk01 = br.ReadUInt16();
            short Count2 = br.ReadInt16();

            ToolkitAssert.Ensure(Header.Count == Count2, "Animation2: Count 1 != Count 2");

            Count2 = Header.RootBoneID != 0 ? (short)(Count2 + 1) : Count2;

            Tracks = new AnimTrack[Count2];

            for (int i = 0;i < Tracks.Length; i++)
            {
                Tracks[i] = new(br);
            }

            UnkShorts00 = new short[Unk01];
            UnkShorts01 = new short[Header.Count];

            for (int i = 0; i < UnkShorts00.Length; i++)
            {
                UnkShorts00[i] = br.ReadInt16();
            }

            for (int i = 0; i < UnkShorts01.Length; i++)
            {
                UnkShorts01[i] = br.ReadInt16();
            }

            ToolkitAssert.Ensure(br.BaseStream.Position == br.BaseStream.Length, "Animation2: Failed to reach EOF.");
        }

        public MT_Animation ConvertToAnimation()
        {
            MT_Animation NewAnimation = new MT_Animation();
            NewAnimation.Tracks = new MT_AnimTrack[Tracks.Length];

            for (int z = 0; z < Tracks.Length; z++)
            {
                AnimTrack Track = Tracks[z];

                MT_AnimTrack NewTrack = new MT_AnimTrack();
                NewAnimation.Tracks[z] = NewTrack;

                NewTrack.BoneID = Track.BoneID;
                NewTrack.BoneName = NewTrack.BoneID.ToString();
                NewTrack.Duration = Track.Duration;

                NewTrack.RotKeyFrames = new MT_RotKey[Track.KeyFrames.Length];
                for (int i = 0; i < Track.KeyFrames.Length; i++)
                {
                    MT_RotKey KeyFrame = new MT_RotKey();
                    KeyFrame.Time = Track.KeyFrames[i].time;
                    KeyFrame.Value = Track.KeyFrames[i].value;
                    NewTrack.RotKeyFrames[i] = KeyFrame;
                }

                NewTrack.PosKeyFrames = new MT_PosKey[Track.Positions.KeyFrames.Length];
                for (int i = 0; i < Track.Positions.KeyFrames.Length; i++)
                {
                    MT_PosKey KeyFrame = new MT_PosKey();
                    KeyFrame.Time = Track.Positions.KeyFrames[i].time;
                    KeyFrame.Value = Track.Positions.KeyFrames[i].value;
                    NewTrack.PosKeyFrames[i] = KeyFrame;
                }
            }

            return NewAnimation;
        }
    }
}
