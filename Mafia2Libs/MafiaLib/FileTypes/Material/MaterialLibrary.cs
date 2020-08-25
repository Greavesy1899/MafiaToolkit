using System.IO;
using System.Collections.Generic;
using System.Linq;
using Utils.Logging;
using System;
using Utils.Settings;
using SharpDX.Direct3D11;

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

        public MaterialLibrary()
        {
            name = "";
            materials = new Dictionary<ulong, Material>();
            unk2 = 0;
            version = GameStorage.Instance.GetSelectedGame().GameType == GamesEnumerator.MafiaII_DE 
                ? VersionsEnumerator.V_58 
                : VersionsEnumerator.V_57;
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
                    if (sampler.UnkSet0.Length != size)
                    {
                        var array = sampler.UnkSet0;
                        Array.Resize(ref array, size);
                        sampler.UnkSet0 = array;
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

        private bool DoesHashContainValue(string left, ulong right)
        {
            string LeftValue = left;
            string RightValue = right.ToString();

            return RightValue.Contains(LeftValue);
        }

        private bool DoesMaterialContainTexture(string text, Material material)
        {
            foreach(var sampler in material.Samplers)
            {
                string FileNameLowerCase = sampler.File.ToLower();
                return FileNameLowerCase.Contains(text);
            }

            return false;
        }

        public Material[] SearchForMaterialsByName(string Name, SearchTypesString searchType)
        {
            string NameToFilter = Name.ToLower();

            List<Material> Filtered = new List<Material>();
            foreach (var Pair in materials)
            {
                if (searchType.Equals(SearchTypesString.MaterialName))
                {
                    string MaterialName = Pair.Value.MaterialName.ToLower();
                    if (MaterialName.Contains(NameToFilter))
                    {
                        Filtered.Add(Pair.Value);
                    }
                } 
                else if(searchType.Equals(SearchTypesString.TextureName))
                {
                    if(DoesMaterialContainTexture(NameToFilter, Pair.Value))
                    {
                        Filtered.Add(Pair.Value);
                    }
                }
            }

            return Filtered.ToArray();
        }

        public Material[] SearchForMaterialsByHash(string value, SearchTypesHashes searchType)
        {
            List<Material> Filtered = new List<Material>();

            foreach (var Pair in materials)
            {
                Material Value = Pair.Value;
                ulong ValueToSearch = 0;

                switch(searchType)
                {
                    case SearchTypesHashes.ShaderHash:
                        ValueToSearch = Value.ShaderHash;
                        break;
                    case SearchTypesHashes.ShaderID:
                        ValueToSearch = Value.ShaderID;
                        break;
                    case SearchTypesHashes.MaterialHash:
                        ValueToSearch = Value.MaterialHash;
                        break;
                    default:
                        ValueToSearch = 0;
                        break;
                }

                if(DoesHashContainValue(value, ValueToSearch))
                {
                    Filtered.Add(Value);
                }
            }

            return Filtered.ToArray();
        }

        public Material[] SelectSearchTypeAndProceedSearch(string text, int currentSearchType)
        {
            Material[] Filtered = null;

            if (currentSearchType >= 3)
            {
                Filtered = SearchForMaterialsByHash(text, (SearchTypesHashes)currentSearchType);
            }
            else
            {
                Filtered = SearchForMaterialsByName(text, (SearchTypesString)currentSearchType);
            }

            return Filtered;
        }
    }
}
