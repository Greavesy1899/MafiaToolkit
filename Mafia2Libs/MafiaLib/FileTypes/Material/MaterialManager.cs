using System.Collections.Generic;
using System.Linq;
using Utils.Logging;

namespace ResourceTypes.Materials
{
    public class MaterialsManager
    {
        private static Dictionary<string, MaterialLibrary> matLibs = new Dictionary<string, MaterialLibrary>();

        public static Dictionary<string, MaterialLibrary> MaterialLibraries {
            get { return matLibs; }
            set { matLibs = value; }
        }

        /// <summary>
        /// Read all mat files in an array and add to dictionary.
        /// </summary>
        /// <param name="names"></param>
        public static void ReadMatFiles(string[] names)
        {
            for (int i = 0; i != names.Length; i++)
            {
                MaterialLibrary mtl = new MaterialLibrary();
                mtl.ReadMatFile(names[i]);
                matLibs.Add(mtl.Name, mtl);
                Log.WriteLine("Succesfully read MTL: " + names[i]);
            }
        }

        /// <summary>
        /// Searches through all currently loaded material libraries.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static Material LookupMaterialByHash(ulong hash)
        {
            Material mat = null;

            for (int i = 0; i != matLibs.Count; i++)
            {
                if (mat != null)
                {
                    return mat;
                }

                mat = matLibs.ElementAt(i).Value.LookupMaterialByHash(hash);
            }

            return mat;
        }

        public static Material LookupMaterialByName(string name)
        {
            Material mat = null;

            for(int i = 0; i < matLibs.Count; i++)
            {
                if (mat != null)
                {
                    return mat;
                }

                mat = matLibs.ElementAt(i).Value.LookupMaterialByName(name);
            }

            return mat;
        }

        public static void ClearLoadedMTLs()
        {
            matLibs.Clear();
        }
    }
}
