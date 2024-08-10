using System.IO;
using Utils;

namespace ResourceTypes.ModelHelpers.ModelExporter
{
    public static class MT_ObjectHandler
    {
        /** Begin Deserialize functions */
        public static MT_Object ReadObjectFromFile(string file)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                MT_Object ModelObject = new MT_Object();
                bool bIsValid = ModelObject.ReadFromFile(reader);

                if(bIsValid)
                {
                    return ModelObject;
                }

                return null;
            }
        }

        public static MT_ObjectBundle ReadBundleFromFile(string file)
        {
            string FileLowered = file.ToLower();
            if(FileLowered.Contains(".fbx"))
            {
                string ChangedPathName = Path.ChangeExtension(FileLowered, ".mtb");
                FBXHelper.ConvertFBX(file, ChangedPathName);
                file = ChangedPathName;
            }

            using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
            {
                MT_ObjectBundle Bundle = new MT_ObjectBundle();
                //bool bIsValid = Bundle.ReadFromFile(reader);

                //if (bIsValid)
                //{
                //    return Bundle;
                //}

                return null;
            }
        }

        /** End Deserialize functions */
    }
}
