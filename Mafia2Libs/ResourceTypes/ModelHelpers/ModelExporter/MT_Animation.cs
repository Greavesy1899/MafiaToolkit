using System.IO;
using Utils.StringHelpers;
using Utils.Models;
using System.Numerics;
using SharpGLTF.Animations;
using SharpGLTF.Scenes;
using System.Collections.Generic;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_RotKey
    {
        public float Time { get; set; }
        public Quaternion Value { get; set; }
    }

    public class MT_PosKey
    {
        public float Time { get; set; }
        public Vector3 Value { get; set; }
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

        public void BuildAnimation(SkinnedTransformer SkinnedMesh, string AllocatedAnimName)
        {
            (NodeBuilder, Matrix4x4)[] JointAndMatrices = SkinnedMesh.GetJointBindings();
            Dictionary<string, NodeBuilder> JointLookup = new Dictionary<string, NodeBuilder>();
            foreach((NodeBuilder, Matrix4x4) Pair in JointAndMatrices)
            {
                JointLookup.Add(Pair.Item1.Name, Pair.Item1);
            }

            foreach (MT_AnimTrack Track in Tracks)
            {
                List<(float, Quaternion)> RotationKeyFrames = new List<(float, Quaternion)>();
                foreach(MT_RotKey RotKeyFrame in Track.RotKeyFrames)
                {
                    RotationKeyFrames.Add((RotKeyFrame.Time, RotKeyFrame.Value));
                }

                List<(float, Vector3)> PositionKeyFrames = new List<(float, Vector3)>();
                foreach(MT_PosKey PosKeyFrame in Track.PosKeyFrames)
                {
                    PositionKeyFrames.Add((PosKeyFrame.Time, PosKeyFrame.Value));
                }

                if(JointLookup.ContainsKey(Track.BoneName))
                {
                    NodeBuilder Joint = JointLookup[Track.BoneName];
                    Joint.SetRotationTrack(AllocatedAnimName, CurveSampler.CreateSampler(RotationKeyFrames.ToArray()));
                    Joint.SetTranslationTrack(AllocatedAnimName, CurveSampler.CreateSampler(PositionKeyFrames.ToArray()));
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
