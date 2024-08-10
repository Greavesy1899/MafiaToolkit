using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
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
