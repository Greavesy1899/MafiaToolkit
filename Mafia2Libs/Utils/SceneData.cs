// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using ResourceTypes.FrameResource;
using ResourceTypes.FrameNameTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Utils.Settings;
using Utils.Language;
using ResourceTypes.BufferPools;
using ResourceTypes.City;
using ResourceTypes.ItemDesc;
using ResourceTypes.Materials;
using ResourceTypes.Sound;
using ResourceTypes.Actors;
using ResourceTypes.Collisions;
using ResourceTypes.Navigation;
using ResourceTypes.Navigation.Traffic;
using ResourceTypes.Translokator;
using ResourceTypes.Prefab;
using ResourceTypes.Misc;
using Utils.Types;
using System.Diagnostics;
using Utils.Models;
using System.Linq;
using Utils.Logging;

namespace Mafia2Tool
{
    public class SceneData
    {
        public FrameNameTable FrameNameTable;
        public FrameResource FrameResource;
        public VertexBufferManager VertexBufferPool;
        public IndexBufferManager IndexBufferPool;
        public SoundSectorResource SoundSector;
        public Actor[] Actors;
        public ItemDescLoader[] ItemDescs;
        public Collision Collisions;
        public CityAreas CityAreas;
        public CityShops CityShops;
        public IRoadmap roadMap;
        public AnimalTrafficLoader ATLoader;
        public NAVData[] AIWorlds;
        public NAVData[] OBJData;
        public HPDData HPDData;
        public TranslokatorLoader Translokator;
        public PrefabLoader Prefabs;
        public string ScenePath = "";
        public string SelectedFrameResourcePath = "";

        public SDSContentFile sdsContent;
        private bool isBigEndian;

        private FileInfo BuildFileInfo(string name)
        {
            var file = name;
            var info = new FileInfo(file);
            ToolkitAssert.Ensure(info.Exists, "File [{0}] does not exist!", name);
            return info;
        }

        private string[] GetExistingResourceFiles(string resourceType)
        {
            return sdsContent
                .GetResourceFiles(resourceType, true)
                .Where(File.Exists)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(path =>
                {
                    int number = ExtractTrailingNumber(path);
                    return number < 0 ? int.MaxValue : number;
                })
                .ThenBy(path => path, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        private void AddExistingResourceFiles(string resourceType, List<FileInfo> output)
        {
            HashSet<string> existing = new HashSet<string>(
                GetExistingResourceFiles(resourceType),
                StringComparer.OrdinalIgnoreCase);

            foreach (string path in sdsContent.GetResourceFiles(resourceType, true))
            {
                if (!existing.Contains(path))
                {
                    Log.WriteLine(
                        string.Format("Skipping missing {0}: {1}", resourceType, path),
                        LoggingTypes.WARNING);
                }
            }

            foreach (string path in existing.OrderBy(path =>
                     {
                         int number = ExtractTrailingNumber(path);
                         return number < 0 ? int.MaxValue : number;
                     }).ThenBy(path => path, StringComparer.OrdinalIgnoreCase))
            {
                output.Add(new FileInfo(path));
            }
        }

        private string GetFirstExistingResourceFile(string resourceType)
        {
            return GetExistingResourceFiles(resourceType).FirstOrDefault();
        }

        private static int ExtractTrailingNumber(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return -1;
            }

            string name = Path.GetFileNameWithoutExtension(path);
            int end = name.Length - 1;

            while (end >= 0 && !char.IsDigit(name[end]))
            {
                end--;
            }

            if (end < 0)
            {
                return -1;
            }

            int numberEnd = end;
            while (end >= 0 && char.IsDigit(name[end]))
            {
                end--;
            }

            string number = name.Substring(end + 1, numberEnd - end);
            return int.TryParse(number, out int value) ? value : -1;
        }

        private string ResolveFrameResourcePath()
        {
            if (!string.IsNullOrWhiteSpace(SelectedFrameResourcePath) && File.Exists(SelectedFrameResourcePath))
            {
                return Path.GetFullPath(SelectedFrameResourcePath);
            }

            return GetFirstExistingResourceFile("FrameResource");
        }

        private string ResolveRelatedResourcePath(string resourceType, string frameResourcePath)
        {
            string[] candidates = GetExistingResourceFiles(resourceType);
            if (candidates.Length == 0)
            {
                return null;
            }

            if (candidates.Length == 1)
            {
                return candidates[0];
            }

            int frameNumber = ExtractTrailingNumber(frameResourcePath);
            if (frameNumber < 0)
            {
                return candidates[0];
            }

            return candidates
                .OrderBy(path =>
                {
                    int candidateNumber = ExtractTrailingNumber(path);
                    return candidateNumber < 0
                        ? int.MaxValue
                        : Math.Abs(candidateNumber - frameNumber);
                })
                .ThenBy(path => path, StringComparer.OrdinalIgnoreCase)
                .First();
        }

        public void BuildData(bool forceBigEndian)
        {
            List<FileInfo> vbps = new List<FileInfo>();
            List<FileInfo> ibps = new List<FileInfo>();
            List<ItemDescLoader> ids = new List<ItemDescLoader>();
            List<Actor> act = new List<Actor>();
            List<NAVData> aiw = new List<NAVData>();
            List<NAVData> obj = new List<NAVData>();

            isBigEndian = forceBigEndian;
            VertexTranslator.IsBigEndian = forceBigEndian;

            if (isBigEndian)
            {
                MessageBox.Show("Detected 'Big Endian' formats. This will severely effect functionality!", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(ScenePath);
            if (!dirInfo.Exists)
            {
                throw new DirectoryNotFoundException(
                    string.Format("The extracted scene folder does not exist: {0}", ScenePath));
            }

            sdsContent = new SDSContentFile();
            sdsContent.ReadFromFile(new FileInfo(Path.Combine(ScenePath, "SDSContent.xml")));

            // Index/vertex pools in extracted SDS folders are occasionally missing from
            // stale manifests. Only pass files which really exist to the pool managers.
            AddExistingResourceFiles("IndexBufferPool", ibps);
            AddExistingResourceFiles("VertexBufferPool", vbps);

            var paths = Array.Empty<string>();

            //Actors
            if (!isBigEndian)
            {
                paths = sdsContent.GetResourceFiles("Actors", true);
                foreach (var item in paths)
                {
                    try
                    {
                        if (File.Exists(item))
                        {
                            FileInfo NewFileInfo = new FileInfo(item);
                            act.Add(new Actor(NewFileInfo));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to read actor {0}: {1}", item, ex.Message);
                    }
                }
            }

            //FrameResource
            string frameResourcePath = ResolveFrameResourcePath();
            if (string.IsNullOrWhiteSpace(frameResourcePath))
            {
                throw new FileNotFoundException(
                    "No FrameResource file could be found in the extracted scene folder.");
            }

            FrameResource = new FrameResource(frameResourcePath, this, isBigEndian);

            //Item Desc
            if (!isBigEndian)
            {
                paths = sdsContent.GetResourceFiles("ItemDesc", true);
                foreach (var item in paths)
                {
                    if (File.Exists(item))
                    {
                        ids.Add(new ItemDescLoader(item));
                    }
                }
            }

            //FrameNameTable
            string frameNameTablePath = ResolveRelatedResourcePath("FrameNameTable", frameResourcePath);
            if (!string.IsNullOrWhiteSpace(frameNameTablePath))
            {
                FrameNameTable = new FrameNameTable(frameNameTablePath, isBigEndian);
            }
            else
            {
                Log.WriteLine(
                    "No matching FrameNameTable was found. Frame names may be unavailable.",
                    LoggingTypes.WARNING);
            }

            //Collisions
            if (!isBigEndian && sdsContent.HasResource("Collisions"))
            {
                string name = GetFirstExistingResourceFile("Collisions");
                if (!string.IsNullOrWhiteSpace(name))
                {
                    Collisions = new Collision(name);
                }
            }

            //~ENABLE THIS SECTION AT YOUR OWN RISK
            //AnimalTrafficPaths
            if (!isBigEndian && sdsContent.HasResource("AnimalTrafficPaths"))
            {
                string name = GetFirstExistingResourceFile("AnimalTrafficPaths");
                try
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        ATLoader = new AnimalTrafficLoader(new FileInfo(name));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to read AnimalTrafficPaths {0}", ex.Message);
                }
            }
            //~ENABLE THIS SECTION AT YOUR OWN RISK

#if DEBUG
            if (!isBigEndian && sdsContent.HasResource("PREFAB"))
            {
                var name = sdsContent.GetResourceFiles("PREFAB", true)[0];
                PrefabLoader loader = new PrefabLoader(new FileInfo(name));
                Prefabs = loader;
            }
#endif // DEBUG

            //RoadMap
#if DEBUG
            //if (!isBigEndian)
            //{
            //    paths = sdsContent.GetResourceFiles("MemFile", true);
            //    foreach (var item in paths)
            //    {
            //        if (item.Contains("RoadMap") || item.Contains("roadmap"))
            //        {
            //            using (FileStream RoadmapStream = File.Open(item, FileMode.Open))
            //            {
            //                roadMap = new RoadmapCe();
            //                roadMap.Read(RoadmapStream);
            //            }
            //        }
            //    }
            //}
#endif // DEBUG
            
            //Translokator
            if (!isBigEndian && sdsContent.HasResource("Translokator"))
            {
                string translokatorPath = ResolveRelatedResourcePath("Translokator", frameResourcePath);
                if (!string.IsNullOrWhiteSpace(translokatorPath))
                {
                    try
                    {
                        Translokator = new TranslokatorLoader(new FileInfo(translokatorPath));
                    }
                    catch (Exception exception)
                    {
                        Log.WriteLine(
                            string.Format("Failed to read Translokator '{0}': {1}", translokatorPath, exception.Message),
                            LoggingTypes.WARNING);
                        Translokator = null;
                    }
                }
            }

            // Kynapse Navigation
            if (ToolkitSettings.bNavigation)
            {
                // OBJ_DATA
                if (!isBigEndian)
                {
                    paths = sdsContent.GetResourceFiles("NAV_OBJ_DATA", true);
                    foreach (var item in paths)
                    {
                        if (File.Exists(item))
                        {
                            obj.Add(new NAVData(new FileInfo(item)));
                        }
                    }

                    OBJData = obj.ToArray();
                }

                // AI WORLD
                if (!isBigEndian)
                {
                    paths = sdsContent.GetResourceFiles("NAV_AIWORLD_DATA", true);
                    foreach (var Item in paths)
                    {
                        if (File.Exists(Item))
                        {
                            aiw.Add(new NAVData(new FileInfo(Item)));
                        }
                    }

                    AIWorlds = aiw.ToArray();
                }

                // HPD DATA
                if (!isBigEndian && sdsContent.HasResource("NAV_HPD_DATA"))
                {
                    string name = GetFirstExistingResourceFile("NAV_HPD_DATA");
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        var data = new NAVData(new FileInfo(name));
                        HPDData = (data.Data as HPDData);
                    }
                }
            }

            IndexBufferPool = new IndexBufferManager(ibps, dirInfo, isBigEndian);
            VertexBufferPool = new VertexBufferManager(vbps, dirInfo, isBigEndian);
            ItemDescs = ids.ToArray();
            Actors = act.ToArray();
        }

        public void UpdateResourceType()
        {
            sdsContent.CreateFileFromFolder();
            sdsContent.WriteToFile();
        }

        public Actor CreateNewActor()
        {
            string DirectoryAndName = string.Format("{0}/Actors_{1}.act", ScenePath, Actors.Length);
            Actor NewActorFile = new Actor(DirectoryAndName);

            // TODO: Terrible code, but this whole class is going to be re-written anyway.
            List<Actor> ActorFiles = Actors.ToList();
            ActorFiles.Add(NewActorFile);
            Actors = ActorFiles.ToArray();

            return NewActorFile;
        }

        public void CleanData()
        {
            FrameNameTable = null;
            FrameResource = null;
            VertexBufferPool = null;
            IndexBufferPool = null;
            SoundSector = null;
            Actors = null;
            ItemDescs = null;
            Collisions = null;
            CityAreas = null;
            CityShops = null;
            roadMap = null;
            ATLoader = null;
            AIWorlds = null;
            OBJData = null;
            HPDData = null;
            Translokator = null;
            Prefabs = null;
            sdsContent = null;
        }
        
        //for foolfroofing, maybe the imported textures could be cached until save, then reset it, and if user doesn't save and exit, all cached textures would be deleted
        public void ImportTextures(List<string> textures, string ImportScenePath)
        {
            foreach (var texture in textures)
            {
                if (TextureCheck(texture, ImportScenePath))
                {
                    CopyTexture(texture, ImportScenePath);
                }

                string mipTexture = "MIP_" + texture;
                if (TextureCheck(mipTexture, ImportScenePath))
                {
                    CopyTexture(mipTexture, ImportScenePath);
                }
            }

        }
        
        private bool TextureCheck(string importTextureName, string ImportScenePath)//done like this in case sdscontent wasn't updated, accurate option
        {
            //checking if importing texture exists
            string texPath = Path.Combine(ImportScenePath, importTextureName);
            if (!File.Exists(texPath))
            {
                return false;
            }
            //checking if the texture is already present
            texPath = Path.Combine(ScenePath, importTextureName);
            if (File.Exists(texPath))
            {
                return false;
            }

            return true;
        }

        private void CopyTexture(string texture, string ImportScenePath)
        {
            string importPath = Path.Combine(ImportScenePath, texture);
            string destinationPath = Path.Combine(ScenePath, texture);

            try
            {
                File.Copy(importPath, destinationPath);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error copying texture: {ex.Message}");
            }
        }
    }

    public static class MaterialData
    {
        public static bool HasLoaded = false;

        public static void Load(string scenePath = null)
        {
            MaterialsManager.ClearLoadedMTLs();
            HasLoaded = false;

            try
            {
                List<string> searchDirectories = new List<string>();
                if (!string.IsNullOrWhiteSpace(scenePath))
                {
                    DirectoryInfo sceneDirectory = new DirectoryInfo(scenePath);
                    if (sceneDirectory.Exists)
                    {
                        searchDirectories.Add(sceneDirectory.FullName);

                        if (sceneDirectory.Parent != null)
                        {
                            searchDirectories.Add(sceneDirectory.Parent.FullName);
                        }
                    }
                }

                string configuredMaterialList = GameStorage.Instance
                    .GetSelectedGame()
                    .Materials ?? string.Empty;

                string[] configuredLibraries = configuredMaterialList
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                MaterialsManager.ReadMatFiles(configuredLibraries, searchDirectories);
                HasLoaded = MaterialsManager.MaterialLibraries.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.GetString("$ERROR_DIDNT_FIND_MTL") + ex.Message, Language.GetString("$ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
