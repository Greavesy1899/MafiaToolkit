using System.Numerics;
using Utils.Models;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_RotKey
    {
        public float Time { get; set; }
        public Quaternion Value { get; set; }

        public MT_RotKey()
        {
            Time = 0.0f;
            Value = Quaternion.Identity;
        }

        public MT_RotKey((float, Quaternion) InValue)
        {
            Time = InValue.Item1;
            Value = InValue.Item2;
        }
    }

    public class MT_PosKey
    {
        public float Time { get; set; }
        public Vector3 Value { get; set; }

        public MT_PosKey()
        {
            Time = 0.0f;
            Value = Vector3.Zero;
        }

        public MT_PosKey((float, Vector3) InValue)
        {
            Time = InValue.Item1;
            Value = InValue.Item2;
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
    }

    public class MT_Animation : IValidator
    {
        public string AnimName { get; set; }
        public MT_AnimTrack[] Tracks { get; set; }

        public MT_Animation()
        {
            Tracks = new MT_AnimTrack[0];
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            // TODO
            return true;
        }
    }
}
