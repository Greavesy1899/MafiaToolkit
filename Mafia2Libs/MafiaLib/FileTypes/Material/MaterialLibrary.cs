using System.IO;
using System.Collections.Generic;
using System.Linq;
using Utils.Logging;

namespace ResourceTypes.Materials
{
    public class MaterialLibrary
    {
        private int unk1;
        private int unk2;
        private Dictionary<ulong, Material> materials;
        private string name;

        public Dictionary<ulong, Material> Materials {
            get { return materials; }
            set { materials = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Read Material Library and add them to the dictionary.
        /// </summary>
        /// <param name="name">path of file.</param>
        public void ReadMatFile(string name)
        {
            materials = new Dictionary<ulong, Material>();
            this.name = name;

            using (BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                string header = new string(reader.ReadChars(4));
                unk1 = reader.ReadInt32();
                int numMats = reader.ReadInt32();
                unk2 = reader.ReadInt32();

                for (int i = 0; i != numMats; i++)
                {
                    Material mat = new Material(reader);
                    materials.Add(mat.MaterialHash, mat);
                }
            }
        }

        /// <summary>
        /// Write Material Library and add them to the dictionary.
        /// </summary>
        /// <param name="name">path to file.</param>
        public void WriteMatFile(string name)
        {
            this.name = name;

            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.Create)))
            {
                writer.Write("MTLB".ToCharArray());
                writer.Write(unk1);
                writer.Write(materials.Count);
                writer.Write(unk2);
                for (int i = 0; i != materials.Count; i++)
                    materials.ElementAt(i).Value.WriteToFile(writer);
            }
        }

        public void BuildMatLibraryFromList(string name, List<Material> materials)
        {
            this.name = name;
            this.materials = new Dictionary<ulong, Material>();
            foreach (var mat in materials)
            {
                if (!this.materials.ContainsKey(mat.MaterialHash))
                    this.materials.Add(mat.MaterialHash, mat);
            }
        }

        /// <summary>
        /// Get material via hash.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public Material LookupMaterialByHash(ulong hash)
        {
            Material mat;

            if (materials.TryGetValue(hash, out mat))
                return mat;
            else
            {
                Log.WriteLine("Unable to find material with key: " + hash, LoggingTypes.WARNING);
                return null;
            }
        }
    }
}
