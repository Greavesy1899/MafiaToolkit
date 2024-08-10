using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
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
        public MT_Animation[] Animations { get; set; }

        public void BuildGLTF()
        {
            // TODO: Find a name
            SceneBuilder Scene = new SceneBuilder("temp name");

            if (Objects != null)
            {
                foreach(MT_Object ModelObject in Objects)
                {
                    SceneBuilder ChildScene = ModelObject.BuildGLTF(Animations);
                    Scene.AddScene(ChildScene, Matrix4x4.Identity);
                }
            }

            // TODO: This could either be the return value or we pass in file path
            // Or we pass the ModelRoot and Path to GLTFExporter, which deals with the rest
            ModelRoot TheModel = Scene.ToGltf2();
            TheModel.SaveGLTF("output.gltf");
            TheModel.SaveGLB("output.glb");
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

            foreach(MT_Animation Animation in Animations)
            {
                bIsValid &= Animation.ValidateObject(TrackerObject);
            }

            return bIsValid;
        }
    }
}
