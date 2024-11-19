using System.Collections.Generic;
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
        private static Dictionary<string, MaterialLibrary> matLibs = new Dictionary<string, MaterialLibrary>();

        public static Dictionary<string, MaterialLibrary> MaterialLibraries {
            get { return matLibs; }
            set { matLibs = value; }
        }

        public static void ReadMatFiles(string[] names)
        {
            for (int i = 0; i != names.Length; i++)
            {
                // Version will be replaced in ReadMatFile()
                MaterialLibrary mtl = new MaterialLibrary(VersionsEnumerator.V_57);
                mtl.ReadMatFile(names[i]);
                matLibs.Add(mtl.Name, mtl);
                Log.WriteLine("Succesfully read MTL: " + names[i]);
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
