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

        public MT_ObjectBundle()
        {
            Objects = new MT_Object[0];
        }

        public ModelRoot BuildGLTF()
        {
            // TODO: Find a name
            SceneBuilder Scene = new SceneBuilder("temp name");

            if (Objects != null)
            {
                foreach(MT_Object ModelObject in Objects)
                {
                    SceneBuilder ChildScene = ModelObject.BuildGLTF();
                    Scene.AddScene(ChildScene, Matrix4x4.Identity);
                }
            }

            return Scene.ToGltf2();
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
