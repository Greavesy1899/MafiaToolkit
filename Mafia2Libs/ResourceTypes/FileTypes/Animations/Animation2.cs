using ResourceTypes.ModelHelpers.ModelExporter;
using System;
using System.IO;
using Utils.Logging;

namespace ResourceTypes.Animation2
{
    public class Animation2
    {
        public Header Header { get; set; } = new();
        public bool IsDataPresent { get; set; } //Not confirmed?
        public Event[] PrimaryEvents { get; set; } = new Event[0];
        public Event[] SecondaryEvents { get; set; } = new Event[0];
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

            PrimaryEvents = new Event[Header.NumPrimaryEvents];

            for (int i = 0; i < PrimaryEvents.Length; i++)
            {
                PrimaryEvents[i] = new(br);
            }

            SecondaryEvents = new Event[Header.NumSecondaryEvents];

            for (int i = 0; i < SecondaryEvents.Length; i++)
            {
                SecondaryEvents[i] = new(br);
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

        public void WriteToFile(string fileName)
        {
            using (MemoryStream ms = new())
            {
                using (BinaryWriter bw = new(ms))
                {
                    Write(bw);
                }

                File.WriteAllBytes(fileName, ms.ToArray());
            }
        }

        public void Write(BinaryWriter bw)
        {
            int Count = Header.RootBoneID != 0 ? (Tracks.Length - 1) : Tracks.Length;
            Header.Count = (short)Count;
            Header.NumPrimaryEvents = (short)PrimaryEvents.Length;
            Header.NumSecondaryEvents = (short)SecondaryEvents.Length;

            Header.Write(bw);
            bw.Write(IsDataPresent);

            foreach (var val in PrimaryEvents)
            {
                val.Write(bw);
            }

            foreach (var val in SecondaryEvents)
            {
                val.Write(bw);
            }

            bw.Write(Unk00);
            bw.Write(Unk01);
            bw.Write((short)Count);

            foreach (var track in Tracks)
            {
                track.Write(bw);
            }

            if (UnkShorts00.Length < Unk01)
            {
                short[] newUnk00Shorts = new short[Unk01];
                Array.Copy(UnkShorts00, 0, newUnk00Shorts, 0, UnkShorts00.Length);
                UnkShorts00 = newUnk00Shorts;
            }

            if (UnkShorts01.Length < Count)
            {
                short[] newUnk01Shorts = new short[Count];
                Array.Copy(UnkShorts01, 0, newUnk01Shorts, 0, UnkShorts01.Length);
                UnkShorts01 = newUnk01Shorts;
            }

            foreach (var val in UnkShorts00)
            {
                bw.Write(val);
            }

            foreach (var val in UnkShorts01)
            {
                bw.Write(val);
            }
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
                    NewTrack.RotKeyFrames[i] = new MT_RotKey(Track.KeyFrames[i]);
                }

                NewTrack.PosKeyFrames = new MT_PosKey[Track.Positions.KeyFrames.Length];
                for (int i = 0; i < Track.Positions.KeyFrames.Length; i++)
                {
                    NewTrack.PosKeyFrames[i] = new MT_PosKey(Track.Positions.KeyFrames[i]);
                }
            }

            return NewAnimation;
        }
    }
}
