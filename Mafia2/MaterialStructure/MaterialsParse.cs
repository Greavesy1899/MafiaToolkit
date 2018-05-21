using System;
using System.IO;
using Gibbed.IO;

namespace Mafia2
{
    public class MaterialsParse
    {

        static Material[] materials;

        public MaterialsParse()
        {
            ReadMatFile("default.mtl");
        }

        public static void ReadMatFile(string materialName)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(materialName, FileMode.Open)))
            {

                string header = new string(reader.ReadChars(4));
                reader.ReadInt32();
                int num2 = reader.ReadInt32();
                reader.ReadInt32();

                materials = new Material[num2];

                for (int i = 0; i != materials.Length; i++)
                {
                    materials[i] = new Material(reader);
                    materials[i].ArrayNum = i;
                }
            }
        }

        public static void WriteMatFile(string materialName)
        {
            using (FileStream writer = new FileStream(materialName, FileMode.Truncate))
            {
                writer.WriteString("MTLB");
                writer.WriteValueS32(57);
                writer.WriteValueS32(materials.Length);
                writer.WriteValueS32(0);

                for (int i = 0; i != materials.Length; i++)
                {
                    materials[i].WriteToFile(writer);
                }
            }
        }

        public static string GetMatName(string text)
        {
            for (int i = 0; i != materials.Length; i++)
            {
                if (materials[i].MaterialNumStr == text)
                {
                    return materials[i].MaterialName;
                }
            }
            return "null material";
        }

        public static Material[] GetMaterials()
        {
            return materials;
        }

        public static string LookupMaterialByHash(ulong hash)
        {
            if (materials == null)
                ReadMatFile("default.mtl");

            for (int i = 0; i != materials.Length; i++)
            {
                if (hash == materials[i].MaterialNumID)
                {
                    if (materials[i].SPS.Length == 0)
                        return materials[i].MaterialName;

                    return materials[i].SPS[0].File;
                }
            }
            return "UNKNOWN";
        }
    }
}
