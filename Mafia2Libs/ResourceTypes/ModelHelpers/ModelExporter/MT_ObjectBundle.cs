using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Utils.Models;
using Utils.StringHelpers;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_ObjectBundle : IValidator
    {
        private const string FileHeader = "MTB";
        private const int FileVersion = 0;

        public MT_Object[] Objects { get; set; }

        public MT_ObjectBundle()
        {
            Objects = new MT_Object[0];
        }

        public ModelRoot BuildGLTF()
        {
            // TODO: Find a name
            SceneBuilder Scene = new SceneBuilder("MAFIA TOOLKIT BUNDLE");

            if (Objects != null)
            {
                foreach(MT_Object ModelObject in Objects)
                {
                    NodeBuilder ModelNode = ModelObject.BuildGLTF(Scene, null);
                    Scene.AddNode(ModelNode);
                }
            }

            return Scene.ToGltf2();
        }

        public void BuildFromGLTF(ModelRoot InRoot)
        {
            if(InRoot == null)
            {
                // no root, no point in continuing this
                return;
            }

            // Access the scene, as it has logical root
            List<MT_Object> ImportedObjects = new List<MT_Object>();
            Scene CurrentScene = InRoot.DefaultScene;
            foreach(Node CurNode in CurrentScene.VisualChildren)
            {
                MT_Object PotentialChildObject = MT_Object.TryBuildFromNode(CurNode);
                if (PotentialChildObject != null)
                {
                    ImportedObjects.Add(PotentialChildObject);
                }
            }

            // Problem with reimporting is the fact that we cannot associate an animation with a mesh
            // Which is fine, because we only did that so we can attach the animation when writing the mesh
            // Instead we can just dump them into a new MT_Object, which won't be imported as a Frame.
            if(InRoot.LogicalAnimations.Count > 0)
            {
                MT_Object AnimationObject = new MT_Object();
                AnimationObject.ObjectName = "ANIMATION_OBJECT";
                AnimationObject.ObjectType = MT_ObjectType.Null;
                AnimationObject.ObjectFlags |= MT_ObjectFlags.HasSkinning;

                // Skeleton is theoretically empty for joints, but this should be okay.
                MT_Skeleton NewSkeleton = new MT_Skeleton();
                AnimationObject.Skeleton = NewSkeleton;
                NewSkeleton.Animations = new MT_Animation[InRoot.LogicalAnimations.Count];

                for(int i = 0; i < NewSkeleton.Animations.Length; i++)
                {
                    // Create a new anim and cache in local obj
                    Animation CurrentAnim = InRoot.LogicalAnimations[i];
                    NewSkeleton.Animations[i] = new MT_Animation();
                    MT_Animation NewAnimation = NewSkeleton.Animations[i];

                    // start porting data
                    NewAnimation.AnimName = CurrentAnim.Name;

                    NewAnimation.Tracks = new MT_AnimTrack[CurrentAnim.Channels.Count];
                    for (int z = 0; z < CurrentAnim.Channels.Count; z++)
                    {
                        // New channel (or track for Mafia II) and cache in local obj
                        AnimationChannel CurrentChannel = CurrentAnim.Channels[z];
                        NewAnimation.Tracks[z] = new MT_AnimTrack();
                        MT_AnimTrack NewAnimTrack = NewAnimation.Tracks[z];

                        NewAnimTrack.Duration = CurrentAnim.Duration;
                        NewAnimTrack.BoneName = CurrentChannel.TargetNode.Name;

                        // TODO: Need to resolve issue with missing bones!
                        SkeletonBoneIDs BoneID = SkeletonBoneIDs.BaseRef;
                        Enum.TryParse<SkeletonBoneIDs>(NewAnimTrack.BoneName, out BoneID);

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

                ImportedObjects.Add(AnimationObject);
            }

            Objects = ImportedObjects.ToArray();
        }

        public void Accept(IVisitor InVisitor)
        {
            foreach(MT_Object Object in Objects)
            {
                Object.Accept(InVisitor);
            }
        }

        protected override bool InternalValidate(MT_ValidationTracker TrackerObject)
        {
            bool bIsValid = true;

            foreach(MT_Object ModelObject in Objects)
            {
                bool bIsObjectValid = ModelObject.ValidateObject(TrackerObject);
                bIsValid &= bIsObjectValid;
            }

            return bIsValid;
        }
    }
}
