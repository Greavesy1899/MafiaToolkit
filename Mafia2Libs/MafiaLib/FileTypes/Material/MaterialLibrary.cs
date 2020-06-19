using System.IO;
using System.Collections.Generic;
using System.Linq;
using Utils.Logging;
using System;

namespace ResourceTypes.Materials
{
    public class MaterialLibrary
    {
        private VersionsEnumerator version;
        private int unk2;
        private Dictionary<ulong, Material> materials;
        private string name;

        public VersionsEnumerator Version {
            get { return version; }
        }
        public Dictionary<ulong, Material> Materials {
            get { return materials; }
            set { materials = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }

        public void ReadMatFile(string name)
        {
            materials = new Dictionary<ulong, Material>();
            this.name = name;

            using (BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                string header = new string(reader.ReadChars(4));
                version = (VersionsEnumerator)reader.ReadInt32();
                int numMats = reader.ReadInt32();
                unk2 = reader.ReadInt32();

                for (int i = 0; i != numMats; i++)
                {
                    Material mat = new Material(reader, version);
                    materials.Add(mat.MaterialHash, mat);
                }
            }
        }

        public void WriteMatFile(string name)
        {
            this.name = name;

            var size = (version == VersionsEnumerator.V_58 ? 4 : 2);
            for (int i = 0; i < materials.Count; i++)
            {
                var mat = materials.ElementAt(i).Value;
                foreach (var sampler in mat.Samplers)
                {
                    if (sampler.Value.UnkSet0.Length != size)
                    {
                        var array = sampler.Value.UnkSet0;
                        Array.Resize(ref array, size);
                        sampler.Value.UnkSet0 = array;
                    }
                }
            }

            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.Create)))
            {
                writer.Write("MTLB".ToCharArray());
                writer.Write((int)version);
                writer.Write(materials.Count);
                writer.Write(unk2);
                for (int i = 0; i != materials.Count; i++)
                {
                    materials.ElementAt(i).Value.WriteToFile(writer, version);
                }
            }
        }

        public Material LookupMaterialByHash(ulong hash)
        {
            Material mat;

            if (materials.TryGetValue(hash, out mat))
            {
                return mat;
            }
            else
            {
                Log.WriteLine("Unable to find material with key: " + hash, LoggingTypes.WARNING);
                return null;
            }
        }

        public Material LookupMaterialByName(string name)
        {
            Material mat = null;

            foreach(var pair in materials)
            {
                if(pair.Value.MaterialName.Equals(name))
                {
                    mat = pair.Value;
                }
            }

            if(mat == null)
            {
                Log.WriteLine("Unable to find material with name: " + name, LoggingTypes.WARNING);
            }
            return mat;
        }
    }
}
