using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System.IO;
using Utils.StringHelpers;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public class MT_ObjectBundle : IValidator
    {
        private const string FileHeader = "MTB";
        private const int FileVersion = 0;

        public MT_Object[] Objects { get; set; }
        public MT_Animation Animation { get; set; }

        public bool ReadFromFile(BinaryReader reader)
        {
            string TempHeader = new string(reader.ReadChars(3));
            if(!TempHeader.Equals(FileHeader))
            {
                return false;
            }

            int TempFileVersion = reader.ReadByte();
            if(TempFileVersion != FileVersion)
            {
                return false;
            }

            uint NumObjects = reader.ReadUInt32();
            Objects = new MT_Object[NumObjects];

            for(int i = 0; i < NumObjects; i++)
            {
                MT_Object NewObject = new MT_Object();
                bool bIsValid = NewObject.ReadFromFile(reader);
                Objects[i] = NewObject;

                // Failed to read Object, return
                if(!bIsValid)
                {
                    return false;
                }
            }

            uint HasAnimation = reader.ReadUInt32();
            if(HasAnimation == 1)
            {
                Animation = new MT_Animation();
                Animation.ReadFromFile(reader);
            }

            return true;
        }

        public void WriteToFile(BinaryWriter writer)
        {
            StringHelpers.WriteString(writer, "MTB", false);
            writer.Write((byte)FileVersion);

            // Write Models to file
            int NumObjects = (Objects != null ?  Objects.Length : 0);
            writer.Write(NumObjects);
            if (Objects != null)
            {
                foreach (MT_Object ModelObject in Objects)
                {
                    ModelObject.WriteToFile(writer);
                }
            }

            int HasAnimation = (Animation != null ? 1 : 0);
            writer.Write(HasAnimation);
            if (Animation != null)
            {
                Animation.WriteToFile(writer);
            }
        }

        public void BuildGLTF()
        {
            // TODO: Find a name
            SceneBuilder Scene = new SceneBuilder("temp name");

            if (Objects != null)
            {
                foreach(MT_Object ModelObject in Objects)
                {
                    Scene.AddNode(ModelObject.BuildGLTF());
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

            if(Animation != null)
            {
                bIsValid &= Animation.ValidateObject(TrackerObject);
            }

            return bIsValid;
        }
    }
}
