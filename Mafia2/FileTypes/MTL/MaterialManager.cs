using System.Collections.Generic;
using System.IO;

namespace Mafia2
{
    public class MaterialsManager
    {
        private static Material[] mats;

        public static Material[] ReadMatFile(string name)
        {
            Material[] materials;

            using (BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                string header = new string(reader.ReadChars(4));
                reader.ReadInt32();
                int num2 = reader.ReadInt32();
                reader.ReadInt32();

                materials = new Material[num2];

                for (int i = 0; i != materials.Length; i++)
                    materials[i] = new Material(reader);
            }

            mats = materials;

            return materials;
        }

        public static void WriteMatFile(string name)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.Create)))
            {
                writer.Write("MTLB".ToCharArray());
                writer.Write(57);
                writer.Write(mats.Length);
                writer.Write(0);
                for (int i = 0; i != mats.Length; i++)
                    mats[i].WriteToFile(writer);
            }
        }

        /// <summary>
        /// Used to save from external material source. 
        /// </summary>
        /// <param name="name">name of material library.</param>
        /// <param name="materials">set of materials to save.</param>
        public static void WriteMatFile(string name, Material[] materials)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.Create)))
            {
                writer.Write("MTLB".ToCharArray());
                writer.Write(57);
                writer.Write(materials.Length);
                writer.Write(0);
                for (int i = 0; i != materials.Length; i++)
                    materials[i].WriteToFile(writer);
            }
        }

        public static string GetMatName(string text)
        {
            if (mats == null)
                return "NULL";

            for (int i = 0; i != mats.Length; i++)
            {
                if (mats[i].MaterialNumStr == text)
                {
                    return mats[i].MaterialName;
                }
            }
            return "NULL";
        }

        public static Material[] GetMaterials()
        {
            return mats;
        }

        public static void SetMaterials(Material[] materials)
        {
            mats = materials;
        }

        public static string LookupMaterialByHash(ulong hash)
        {
            if (mats == null)
                ReadMatFile("default.mtl");

            for (int i = 0; i != mats.Length; i++)
            {
                if (hash == mats[i].MaterialNumID)
                {
                    if (mats[i].SPS.Length == 0)
                        return mats[i].MaterialName;

                    return mats[i].SPS[0].File;
                }
            }
            return "UNKNOWN";
        }
    }
}
