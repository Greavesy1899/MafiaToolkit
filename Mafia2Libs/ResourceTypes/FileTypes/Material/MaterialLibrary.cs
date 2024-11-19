﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Utils.Logging;

namespace ResourceTypes.Materials
{
    public class MaterialLibrary
    {
        private VersionsEnumerator version;
        private int unk2;
        private Dictionary<ulong, IMaterial> materials;
        private string name;

        public VersionsEnumerator Version {
            get { return version; }
        }
        public Dictionary<ulong, IMaterial> Materials {
            get { return materials; }
            set { materials = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }

        public MaterialLibrary(VersionsEnumerator InVersion)
        {
            name = "";
            materials = new Dictionary<ulong, IMaterial>();
            unk2 = 0;
            version = InVersion;
        }

        public void ReadMatFile(string name)
        {
            materials = new Dictionary<ulong, IMaterial>();
            this.name = name;

            using (BinaryReader reader = new BinaryReader(File.Open(name, FileMode.Open)))
            {
                string header = new string(reader.ReadChars(4));
                version = (VersionsEnumerator)reader.ReadInt32();
                int numMats = reader.ReadInt32();
                unk2 = reader.ReadInt32();

                for (int i = 0; i != numMats; i++)
                {
                    IMaterial mat = MaterialFactory.ReadMaterialFromFile(reader, version);
                    materials.Add(mat.MaterialName.Hash, mat);
                }
            }
        }

        public void Save()
        {
            WriteMatFile(Name);
        }

        public void WriteMatFile(string name)
        {
            this.name = name;

            // Validate Materials (TODO: Maybe move to validate function?)
            for (int i = 0; i != materials.Count; i++)
            {
                IMaterial Material = materials.ElementAt(i).Value;
                if (Version == VersionsEnumerator.V_57 || Version == VersionsEnumerator.V_58)
                {
                    if(Material.GetMTLVersion() == VersionsEnumerator.V_63)
                    {
                        MessageBox.Show("Detected v63 (M1DE/M3 Material). Cannot save this file. Support will be added soon.", "Toolkit");
                        return;
                    }
                }
            }
            
            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.Create)))
            {
                writer.Write("MTLB".ToCharArray());
                writer.Write((int)version);
                writer.Write(materials.Count);
                writer.Write(unk2);

                // Write Materials
                for (int i = 0; i != materials.Count; i++)
                {
                    // Convert material if required.
                    IMaterial Material = materials.ElementAt(i).Value;
                    if(Material.GetMTLVersion() != Version)
                    {
                        Material = MaterialFactory.ConvertMaterial(Version, Material);
                    }

                    Material.WriteToFile(writer, version);
                }
            }
        }

        public bool AddMaterial(IMaterial InMaterial)
        {
            // Make sure same version
            if(InMaterial.IsVersion(Version))
            {
                // Add new material
                return Materials.TryAdd(InMaterial.GetMaterialHash(), InMaterial);
            }

            return false;
        }

        public void BuildMaterialLibrary(List<IMaterial> Materials, VersionsEnumerator InVersion)
        {
            version = InVersion;
            materials = new Dictionary<ulong, IMaterial>();

            foreach(var Material in Materials)
            {
                ulong Hash = Material.MaterialName.Hash;
                materials.Add(Hash, Material);
            }
        }

        public IMaterial LookupMaterialByHash(ulong hash)
        {
            IMaterial mat;

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

        public IMaterial LookupMaterialByName(string name)
        {
            IMaterial mat = null;

            foreach(var pair in materials)
            {
                var MatName = pair.Value.MaterialName.String;
                if(MatName.Equals(name))
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

        private bool DoesMaterialContainTexture(string text, IMaterial material)
        {
            ToolkitAssert.Ensure(material != null, "Attempted to look for a texture on a non-valid Material");
            return material.HasTexture(text);
        }

        public IMaterial[] SearchForMaterialsByName(string Name, SearchTypesString searchType)
        {
            string NameToFilter = Name.ToLower();

            List<IMaterial> Filtered = new List<IMaterial>();
            foreach (var Pair in materials)
            {
                if (searchType.Equals(SearchTypesString.MaterialName))
                {
                    string MaterialName = Pair.Value.MaterialName.String.ToLower();
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

        public IMaterial[] SearchForMaterialsByHash(string value, SearchTypesHashes searchType)
        {
            List<IMaterial> Filtered = new List<IMaterial>();

            foreach (var Pair in materials)
            {
                IMaterial Value = Pair.Value;
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
                        ValueToSearch = Value.GetMaterialHash();
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

        public IMaterial[] SelectSearchTypeAndProceedSearch(string text, int currentSearchType)
        {
            IMaterial[] Filtered = null;

            if (currentSearchType >= 2)
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
