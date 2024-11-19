using System.IO;
using System.Numerics;
using Utils.Models;
using Utils.StringHelpers;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_RotKey
    {
        public float Time { get; set; }
        public Quaternion Value { get; set; }

        public bool ReadFromFile(BinaryReader Reader)
        {
            Time = Reader.ReadSingle();

            Quaternion TempQuat = Quaternion.Identity;
            TempQuat[0] = Reader.ReadSingle();
            TempQuat[1] = Reader.ReadSingle();
            TempQuat[2] = Reader.ReadSingle();
            TempQuat[3] = Reader.ReadSingle();
            Value = TempQuat;

            return true;
        }

        public bool WriteToFile(BinaryWriter Writer)
        {
            Writer.Write(Time);

            Writer.Write(Value[0]);
            Writer.Write(Value[1]);
            Writer.Write(Value[2]);
            Writer.Write(Value[3]);

            return true;
        }
    }

    public class MT_PosKey
    {
        public float Time { get; set; }
        public Vector3 Value { get; set; }

        public bool ReadFromFile(BinaryReader Reader)
        {
            Time = Reader.ReadSingle();
            
            Vector3 TempPos = Vector3.Zero;
            TempPos[0] = Reader.ReadSingle();
            TempPos[1] = Reader.ReadSingle();
            TempPos[2] = Reader.ReadSingle();

            return true;
        }

        public bool WriteToFile(BinaryWriter Writer)
        {
            Writer.Write(Time);

            Writer.Write(Value[0]);
            Writer.Write(Value[1]);
            Writer.Write(Value[2]);

            return true;
        }
    }

    public class MT_AnimTrack
    {
        public SkeletonBoneIDs BoneID { get; set; }
        public string BoneName { get; set; }
        public float Duration { get; set; }
        public MT_RotKey[] RotKeyFrames { get; set; }
        public MT_PosKey[] PosKeyFrames { get; set; }

        public MT_AnimTrack()
        {
            BoneName = string.Empty;
            BoneID = SkeletonBoneIDs.BaseRef;

            RotKeyFrames = new MT_RotKey[0];
            PosKeyFrames = new MT_PosKey[0];
        }

        public bool ReadFromFile(BinaryReader Reader)
        {
            bool bValid = true;

            BoneID = (SkeletonBoneIDs)Reader.ReadInt16();
            BoneName = Reader.ReadString8();
            Duration = Reader.ReadSingle();

            ushort NumRotKeys = Reader.ReadUInt16();
            ushort NumPosKeys = Reader.ReadUInt16();
            RotKeyFrames = new MT_RotKey[(ushort)NumRotKeys];
            PosKeyFrames = new MT_PosKey[(ushort)NumPosKeys];

            for(ushort i = 0; i < NumRotKeys; i++)
            {
                MT_RotKey KeyFrame = new MT_RotKey();
                bValid &= KeyFrame.ReadFromFile(Reader);
                RotKeyFrames[i] = KeyFrame;
            }

            for (ushort i = 0; i < NumPosKeys; i++)
            {
                MT_PosKey KeyFrame = new MT_PosKey();
                bValid &= KeyFrame.ReadFromFile(Reader);
                PosKeyFrames[i] = KeyFrame;
            }

            return bValid;
        }

        public bool WriteToFile(BinaryWriter Writer)
        {
            Writer.Write((ushort)BoneID);
            Writer.WriteString8(BoneName);
            Writer.Write(Duration);

            Writer.Write((ushort)PosKeyFrames.Length);
            Writer.Write((ushort)RotKeyFrames.Length);
            
            foreach(MT_RotKey KeyFrame in RotKeyFrames)
            {
                KeyFrame.WriteToFile(Writer);
            }

            foreach (MT_PosKey KeyFrame in PosKeyFrames)
            {
                KeyFrame.WriteToFile(Writer);
            }

            return true;
        }
    }

    public class MT_Animation : IValidator
    {
        public MT_AnimTrack[] Tracks { get; set; }

        public MT_Animation()
        {
            Tracks = new MT_AnimTrack[0];
        }

        public bool ReadFromFile(BinaryReader Reader)
        {
            bool bValid = true;

            ushort NumTracks = Reader.ReadUInt16();
            Tracks = new MT_AnimTrack[(ushort)NumTracks];

            for (ushort i = 0; i < NumTracks; i++)
            {
                MT_AnimTrack Track = new MT_AnimTrack();
                bValid &= Track.ReadFromFile(Reader);
                Tracks[i] = Track;
            }

            return bValid;
        }

        public bool WriteToFile(BinaryWriter Writer)
        {
            Writer.Write((ushort)Tracks.Length);
            foreach(MT_AnimTrack Track in Tracks)
            {
                Track.WriteToFile(Writer);
            }

            return true;
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            // TODO
            return true;
        }
    }
}
