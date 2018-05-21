using System;
using System.IO;

namespace Mafia2 {
    public class MaterialsParse {

        static Material[] materials;

        public MaterialsParse() {
            ReadMaterialsFile();
        }

        public static void ReadMaterialsFile() {
            using (BinaryReader reader = new BinaryReader(File.Open("default.mtl", FileMode.Open))) {

                string header = new string(reader.ReadChars(4));
                reader.ReadInt32();
                int num2 = reader.ReadInt32();
                reader.ReadInt32();

                materials = new Material[num2];

                for (int i = 0; i != materials.Length; i++) {
                    materials[i] = new Material(reader);
                    materials[i].ArrayNum = i;
                }
            }
        }

        public static string GetMatName(string text) {
            for(int i = 0; i != materials.Length; i++) {
                if(materials[i].MaterialNumStr == text) {
                    return materials[i].MaterialName;
                }
            }
            return "null material";
        }

        public static Material[] GetMaterials() {
            return materials;
        }

        public static string LookupMaterialByHash(ulong hash)
        {
            if (materials == null)
                ReadMaterialsFile();

            for(int i = 0; i != materials.Length; i++)
            {
                if(hash == materials[i].MaterialNumID)
                {
                    return materials[i].SPS[0].File;
                }
            }
            return "UNKNOWN";
        }
    }
}
