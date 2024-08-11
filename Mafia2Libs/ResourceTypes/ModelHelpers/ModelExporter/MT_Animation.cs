using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public (float, Quaternion) AsPair()
        {
            return (Time, Value);
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

        public (float, Vector3) AsPair()
        {
            return (Time, Value);
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

        public void BuildAnimation(Animation InAnimation)
        {
            // start porting data
            AnimName = InAnimation.Name;

            Tracks = new MT_AnimTrack[InAnimation.Channels.Count];
            for (int z = 0; z < InAnimation.Channels.Count; z++)
            {
                // New channel (or track for Mafia II) and cache in local obj
                AnimationChannel CurrentChannel = InAnimation.Channels[z];
                Tracks[z] = new MT_AnimTrack();
                MT_AnimTrack NewAnimTrack = Tracks[z];

                NewAnimTrack.Duration = InAnimation.Duration;
                NewAnimTrack.BoneName = CurrentChannel.TargetNode.Name;

                // TODO: Need to resolve issue with missing bones!
                SkeletonBoneIDs BoneID = SkeletonBoneIDs.BaseRef;
                Enum.TryParse<SkeletonBoneIDs>(NewAnimTrack.BoneName, out BoneID);
                NewAnimTrack.BoneID = BoneID;

                // Convert Position
                IAnimationSampler<Vector3> PosSampler = CurrentChannel.GetTranslationSampler();
                if (PosSampler != null)
                {
                    List<MT_PosKey> PositionKeyList = new List<MT_PosKey>();

                    IEnumerable<(float, Vector3)> PosKeys = PosSampler.GetLinearKeys();
                    Array.ForEach<(float, Vector3)>(PosKeys.ToArray(), (delegate ((float, Vector3) Item) { PositionKeyList.Add(new MT_PosKey(Item)); }));

                    NewAnimTrack.PosKeyFrames = PositionKeyList.ToArray();
                }

                // Convert Rotation
                IAnimationSampler<Quaternion> RotSampler = CurrentChannel.GetRotationSampler();
                if (RotSampler != null)
                {
                    List<MT_RotKey> RotationKeyList = new List<MT_RotKey>();

                    IEnumerable<(float, Quaternion)> RotKeys = RotSampler.GetLinearKeys();
                    Array.ForEach<(float, Quaternion)>(RotKeys.ToArray(), (delegate ((float, Quaternion) Item) { RotationKeyList.Add(new MT_RotKey(Item)); }));

                    NewAnimTrack.RotKeyFrames = RotationKeyList.ToArray();
                }
            }
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            // TODO
            return true;
        }
    }
}
