using System.IO;
using Utils.StringHelpers;
using Utils.VorticeUtils;
using System.Numerics;
using SharpGLTF.Scenes;
using System.Collections.Generic;
using SharpGLTF.Animations;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_Joint
    {
        public string Name { get; set; }
        public uint UsageFlags { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public int ParentJointIndex { get; set; }

    }
    public class MT_Skeleton : IValidator
    {
        public MT_Joint[] Joints { get; set; }
        public MT_Animation[] Animations { get; set; }

        public MT_Skeleton()
        {
            Joints = new MT_Joint[0];
            Animations = new MT_Animation[0];
        }

        public NodeBuilder[] BuildGLTF(int LodIndex)
        {
            NodeBuilder[] JointNodes = new NodeBuilder[Joints.Length];
            for(uint Index = 0; Index < Joints.Length; Index++)
            {
                MT_Joint CurrentJoint = Joints[Index];

                // create node with transform
                NodeBuilder JointNode = new NodeBuilder(CurrentJoint.Name)
                    .WithLocalTranslation(CurrentJoint.Position)
                    .WithLocalRotation(CurrentJoint.Rotation)
                    .WithLocalScale(CurrentJoint.Scale);

                // Add to the parent joint node
                if (CurrentJoint.ParentJointIndex != 255)
                {
                    string ParentJointName = Joints[CurrentJoint.ParentJointIndex].Name;
                    JointNodes[CurrentJoint.ParentJointIndex].AddNode(JointNode);
                }

                // then to lookup
                JointNodes[Index] = JointNode;
            }

            // let the skeleton add any animations
            AddAnimations(JointNodes);

            return JointNodes;
        }

        private void AddAnimations(NodeBuilder[] JointNodes)
        {
            // Dictionary is faster lookups, better then array
            Dictionary<string, NodeBuilder> JointLookup = new Dictionary<string, NodeBuilder>();
            foreach (NodeBuilder JointNode in JointNodes)
            {
                JointLookup.Add(JointNode.Name, JointNode);
            }

            // we can build up the animations here, using the joint nodes provided
            foreach (MT_Animation Animation in Animations)
            {
                foreach (MT_AnimTrack Track in Animation.Tracks)
                {
                    // TODO: We can likely improve this with some voodoo C# magic
                    List<(float, Quaternion)> RotationKeyFrames = new List<(float, Quaternion)>();
                    foreach (MT_RotKey RotKeyFrame in Track.RotKeyFrames)
                    {
                        RotationKeyFrames.Add((RotKeyFrame.Time, RotKeyFrame.Value));
                    }

                    List<(float, Vector3)> PositionKeyFrames = new List<(float, Vector3)>();
                    foreach (MT_PosKey PosKeyFrame in Track.PosKeyFrames)
                    {
                        PositionKeyFrames.Add((PosKeyFrame.Time, PosKeyFrame.Value));
                    }

                    if (JointLookup.ContainsKey(Track.BoneName))
                    {
                        NodeBuilder Joint = JointLookup[Track.BoneName];
                        Joint.SetRotationTrack(Animation.AnimName, CurveSampler.CreateSampler(RotationKeyFrames.ToArray()));
                        Joint.SetTranslationTrack(Animation.AnimName, CurveSampler.CreateSampler(PositionKeyFrames.ToArray()));
                    }
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
