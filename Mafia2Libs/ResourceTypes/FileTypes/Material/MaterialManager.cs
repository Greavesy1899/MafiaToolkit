// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Logging;
using Utils.Settings;

namespace ResourceTypes.Materials
{
    /**
     * A set of params to pass into the MaterialManager when the Toolkit needs to add a batch of
     * materials which may need to live in different libraries. Perfect use-case is Model Import system.
     */
    public struct MaterialAddRequestParams
    {
        private IMaterial Material;
        private string LibraryName;

        public MaterialAddRequestParams(IMaterial InMaterial, string InLibraryName)
        {
            Material = InMaterial;
            LibraryName = InLibraryName;
        }

        public IMaterial GetMaterial() { return Material; }
        public string GetLibraryName() { return LibraryName; }
    }

    /**
     * MaterialsManager. Stores all active libraries.
     * Supports v57, v58 and v63.
     */
    public class MaterialsManager
    {
        private static Dictionary<string, MaterialLibrary> matLibs = new Dictionary<string, MaterialLibrary>(StringComparer.OrdinalIgnoreCase);

        public static Dictionary<string, MaterialLibrary> MaterialLibraries {
            get { return matLibs; }
            set { matLibs = value; }
        }

        public static void ReadMatFiles(string[] names)
        {
            ReadMatFiles(names, null);
        }

        public static void ReadMatFiles(string[] names, IEnumerable<string> additionalSearchDirectories)
        {
            List<string> materialFiles = DiscoverMaterialLibraries(names, additionalSearchDirectories);
            foreach (string materialPath in materialFiles)
            {
                TryReadMaterialLibrary(materialPath);
            }

            Log.WriteLine(string.Format("Loaded {0} material libraries.", matLibs.Count));
        }

        private static List<string> DiscoverMaterialLibraries(
            string[] configuredNames,
            IEnumerable<string> additionalSearchDirectories)
        {
            List<string> materialFiles = new List<string>();
            HashSet<string> seenFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> searchDirectories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string configuredName in configuredNames ?? Array.Empty<string>())
            {
                if (string.IsNullOrWhiteSpace(configuredName))
                {
                    continue;
                }

                string fullPath;
                try
                {
                    fullPath = Path.GetFullPath(configuredName.Trim());
                }
                catch (Exception exception)
                {
                    Log.WriteLine(
                        string.Format("Invalid MTL path '{0}': {1}", configuredName, exception.Message),
                        LoggingTypes.WARNING);
                    continue;
                }

                string directory = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
                {
                    searchDirectories.Add(directory);
                }

                if (File.Exists(fullPath) && seenFiles.Add(fullPath))
                {
                    materialFiles.Add(fullPath);
                }
                else if (!File.Exists(fullPath))
                {
                    Log.WriteLine("Configured MTL does not exist: " + fullPath, LoggingTypes.WARNING);
                }
            }

            foreach (string directory in additionalSearchDirectories ?? Array.Empty<string>())
            {
                if (!string.IsNullOrWhiteSpace(directory) && Directory.Exists(directory))
                {
                    searchDirectories.Add(Path.GetFullPath(directory));
                }
            }

            foreach (string directory in searchDirectories)
            {
                IEnumerable<string> siblingLibraries;
                try
                {
                    siblingLibraries = Directory
                        .EnumerateFiles(directory, "*.mtl", SearchOption.TopDirectoryOnly)
                        .OrderBy(GetMaterialLibraryPriority)
                        .ThenBy(path => Path.GetFileName(path), StringComparer.OrdinalIgnoreCase)
                        .ToArray();
                }
                catch (Exception exception)
                {
                    Log.WriteLine(
                        string.Format("Could not scan MTL folder '{0}': {1}", directory, exception.Message),
                        LoggingTypes.WARNING);
                    continue;
                }

                foreach (string siblingPath in siblingLibraries)
                {
                    string fullSiblingPath = Path.GetFullPath(siblingPath);
                    if (seenFiles.Add(fullSiblingPath))
                    {
                        materialFiles.Add(fullSiblingPath);
                    }
                }
            }

            return materialFiles;
        }

        private static int GetMaterialLibraryPriority(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);

            if (name.Equals("default", StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            if (name.Equals("default50", StringComparison.OrdinalIgnoreCase))
            {
                return 10;
            }

            if (name.Equals("default60", StringComparison.OrdinalIgnoreCase))
            {
                return 20;
            }

            if (name.StartsWith("default", StringComparison.OrdinalIgnoreCase))
            {
                return 30;
            }

            return 100;
        }

        private static void TryReadMaterialLibrary(string path)
        {
            try
            {
                MaterialLibrary mtl = new MaterialLibrary(VersionsEnumerator.V_57);
                mtl.ReadMatFile(path);
                matLibs[path] = mtl;
                Log.WriteLine("Successfully read MTL: " + path);
            }
            catch (Exception exception)
            {
                Log.WriteLine(
                    string.Format("Failed to read MTL '{0}': {1}", path, exception.Message),
                    LoggingTypes.WARNING);
            }
        }

        public static void AddMaterialsToLibrary(List<MaterialAddRequestParams> Materials)
        {
            List<string> LibrariesToSave = new List<string>();

            // Iterate through all AddRequests.
            // We make sure the library exists before addding.
            // AddMaterial() already checks versioning.
            foreach(MaterialAddRequestParams AddParams in Materials)
            {
                string LibraryName = AddParams.GetLibraryName();
                if(MaterialLibraries.ContainsKey(LibraryName))
                {
                    // Request that the library adds this material
                    bool bAddMaterial = MaterialLibraries[LibraryName].AddMaterial(AddParams.GetMaterial());
                    
                    // Cache the fact we have modified this MTL.
                    if(bAddMaterial && !LibrariesToSave.Contains(LibraryName))
                    {
                        LibrariesToSave.Add(LibraryName);
                    }
                }
            }

            // Now iterate through all MTLs and try to save them
            foreach(string Library in LibrariesToSave)
            {
                // No check here as this list was only updated if it was added to.
                MaterialLibraries[Library].Save();
            }
        }


        public static IMaterial LookupMaterialByHash(ulong Hash)
        {
            foreach(MaterialLibrary Library in matLibs.Values)
            {
                // Library might be invalid
                if(Library != null)
                {
                    // Check Library for material
                    IMaterial FoundMaterial = Library.LookupMaterialByHash(Hash);
                    if(FoundMaterial != null)
                    {
                        return FoundMaterial;
                    }
                }
            }

            return null;
        }

        public static IMaterial LookupMaterialByName(string Name)
        {
            foreach (MaterialLibrary Library in matLibs.Values)
            {
                // Library might be invalid
                if (Library != null)
                {
                    // Check Library for material
                    IMaterial FoundMaterial = Library.LookupMaterialByName(Name);
                    if (FoundMaterial != null)
                    {
                        return FoundMaterial;
                    }
                }
            }

            return null;
        }

        public static IMaterial CreateMaterialNoStore(ulong Hash, string Name, MaterialPreset Preset)
        {
            VersionsEnumerator CurrentVersion = GetMTLVersionFromActiveGameType();

            IMaterial NewMaterial = MaterialFactory.ConstructMaterialWithPreset(CurrentVersion, Preset);
            NewMaterial.MaterialName.Set(Name);

            return NewMaterial;
        }

        public static VersionsEnumerator GetMTLVersionFromActiveGameType()
        {
            Game CurrentGame = GameStorage.Instance.GetSelectedGame();
            switch (CurrentGame.GameType)
            {
                case GamesEnumerator.MafiaII:
                    {
                        return VersionsEnumerator.V_57;
                    }
                case GamesEnumerator.MafiaII_DE:
                    {
                        return VersionsEnumerator.V_58;
                    }
                case GamesEnumerator.MafiaIII:
                case GamesEnumerator.MafiaI_DE:
                    {
                        return VersionsEnumerator.V_63;
                    }
                default:
                    {
                        // Unknown type
                        return VersionsEnumerator.Nill;
                    }
            }
        }


        public static void ClearLoadedMTLs()
        {
            matLibs.Clear();
        }
    }
}
