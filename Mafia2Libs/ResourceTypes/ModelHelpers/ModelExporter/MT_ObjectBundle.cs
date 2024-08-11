using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System.Collections.Generic;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_ObjectBundle : IValidator
    {
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
                    Animation CurrentAnim = InRoot.LogicalAnimations[i];

                    NewSkeleton.Animations[i] = new MT_Animation();
                    NewSkeleton.Animations[i].BuildAnimation(CurrentAnim);
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
