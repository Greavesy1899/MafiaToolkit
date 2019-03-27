using System.Collections.Generic;
using System.Linq;
using Utils.Logging;

namespace ResourceTypes.Materials
{
    public class MaterialsManager
    {
        public static Dictionary<string, MaterialLibrary> MTLs = new Dictionary<string, MaterialLibrary>();

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
                MTLs.Add(mtl.Name, mtl);
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

            for (int i = 0; i != MTLs.Count; i++)
            {
                if (mat != null)
                    return mat;

                mat = MTLs.ElementAt(i).Value.LookupMaterialByHash(hash);
            }

            return mat;
        }

        public static void ClearLoadedMTLs()
        {
            MTLs.Clear();
        }
    }
}
