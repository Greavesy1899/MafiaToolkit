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
using ResourceTypes.Translokator;
using ResourceTypes.Prefab;
using ResourceTypes.Misc;
using Utils.Types;
using System.Diagnostics;

namespace Mafia2Tool
{
    public static class SceneData
    {
        public static FrameNameTable FrameNameTable;
        public static FrameResource FrameResource;
        public static VertexBufferManager VertexBufferPool;
        public static IndexBufferManager IndexBufferPool;
        public static SoundSectorLoader SoundSector;
        public static Actor[] Actors;
        public static ItemDescLoader[] ItemDescs;
        public static Collision Collisions;
        public static CityAreas CityAreas;
        public static CityShops CityShops;
        public static Roadmap roadMap;
        public static AnimalTrafficLoader ATLoader;
        public static NAVData[] AIWorlds;
        public static NAVData[] OBJData;
        public static HPDData HPDData;
        public static TranslokatorLoader Translokator;
        public static PrefabLoader Prefabs;
        public static string ScenePath = "";

        private static SDSContentFile sdsContent;
        private static bool isBigEndian;

        private static FileInfo BuildFileInfo(string name)
        {
            var file = name;
            var info = new FileInfo(file);
            Debug.Assert(info.Exists);
            return info;
        }

        public static void BuildData(bool forceBigEndian)
        {
            List<FileInfo> vbps = new List<FileInfo>();
            List<FileInfo> ibps = new List<FileInfo>();
            List<ItemDescLoader> ids = new List<ItemDescLoader>();
            List<Actor> act = new List<Actor>();
            List<NAVData> aiw = new List<NAVData>();
            List<NAVData> obj = new List<NAVData>();

            isBigEndian = forceBigEndian;
            Utils.Models.VertexTranslator.IsBigEndian = forceBigEndian;

            if (isBigEndian)
            {
                MessageBox.Show("Detected 'Big Endian' formats. This will severely effect functionality!", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(ScenePath);
            sdsContent = new SDSContentFile();
            sdsContent.ReadFromFile(new FileInfo(Path.Combine(ScenePath + "/SDSContent.xml")));

            //IndexBuffers
            var paths = sdsContent.GetResourceFiles("IndexBufferPool", true);
            foreach(var item in paths)
            {
                ibps.Add(BuildFileInfo(item));
            }

            //Vertex Buffers
            paths = sdsContent.GetResourceFiles("VertexBufferPool", true);
            foreach (var item in paths)
            {
                vbps.Add(BuildFileInfo(item));
            }

            //Actors
            if (!isBigEndian)
            {
                paths = sdsContent.GetResourceFiles("Actors", true);
                foreach (var item in paths)
                {
                    try
                    {
                        act.Add(new Actor(item));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to read actor {0}", item);
                    }
                }
            }

            //FrameResource
            if (sdsContent.HasResource("FrameResource"))
            {
                var name = sdsContent.GetResourceFiles("FrameResource", true)[0];
                FrameResource = new FrameResource(name, isBigEndian);
            }

            //Item Desc
            if (!isBigEndian)
            {
                paths = sdsContent.GetResourceFiles("ItemDesc", true);
                foreach (var item in paths)
                {
                    ids.Add(new ItemDescLoader(item));
                }
            }

            //FrameNameTable
            if (sdsContent.HasResource("FrameNameTable"))
            {
                var name = sdsContent.GetResourceFiles("FrameNameTable", true)[0];
                FrameNameTable = new FrameNameTable(name, isBigEndian);
            }

            //Collisions
            if (!isBigEndian && sdsContent.HasResource("Collisions"))
            {
                var name = sdsContent.GetResourceFiles("Collisions", true)[0];
                Collisions = new Collision(name);
            }

            //AnimalTrafficPaths
            if (!isBigEndian && sdsContent.HasResource("AnimalTrafficPaths"))
            {
                var name = sdsContent.GetResourceFiles("AnimalTrafficPaths", true)[0];
                try
                {
                    ATLoader = new AnimalTrafficLoader(new FileInfo(name));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to read AnimalTrafficPaths {0}", ex.Message);
                }
            }

            if (!isBigEndian && sdsContent.HasResource("PREFAB"))
            {
                var name = sdsContent.GetResourceFiles("PREFAB", true)[0];
                PrefabLoader loader = new PrefabLoader(new FileInfo(name));
                Prefabs = loader;
            }

            //RoadMap
            if (!isBigEndian)
            {
                paths = sdsContent.GetResourceFiles("MemFile", true);
                foreach (var item in paths)
                {
                    if (item.Contains("RoadMap") || item.Contains("roadmap"))
                    {
                        roadMap = new Roadmap(new FileInfo(item));
                    }
                }
            }

            //Translokator
            //if (!isBigEndian && sdsContent.HasResource("Translokator"))
            //{
            //    var name = sdsContent.GetResourceFiles("Translokator", true)[0];
            //    Translokator = new TranslokatorLoader(new FileInfo(name));
            //}

            //Kynapse OBJ_DATA
            if (!isBigEndian)
            {
                //tis' broken for now
                paths = sdsContent.GetResourceFiles("NAV_OBJ_DATA", true);
                foreach (var item in paths)
                {
                    obj.Add(new NAVData(new FileInfo(item)));
                }

                //for (int i = 0; i < obj.Count; i++)
                //{
                //    obj[i].WriteToFile();
                //}
            }
            //if (!isBigEndian && sdsContent.HasResource("NAV_HPD_DATA"))
            //{
            //    var name = sdsContent.GetResourceFiles("NAV_HPD_DATA", true)[0];
            //    var data = new NAVData(new FileInfo(name));
            //    HPDData = (data.data as HPDData);
            //    data.WriteToFile();
            //}
            IndexBufferPool = new IndexBufferManager(ibps, dirInfo, isBigEndian);
            VertexBufferPool = new VertexBufferManager(vbps, dirInfo, isBigEndian);
            ItemDescs = ids.ToArray();
            Actors = act.ToArray();
            OBJData = obj.ToArray();
        }

        public static void UpdateResourceType()
        {
            sdsContent.CreateFileFromFolder();
            sdsContent.WriteToFile();
        }

        public static void CleanData()
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
        }
    }
    public static class MaterialData
    {
        public static bool HasLoaded = false;

        public static void Load()
        {
            MaterialsManager.ClearLoadedMTLs();

            try
            {
                MaterialsManager.ReadMatFiles(GameStorage.Instance.GetSelectedGame().Materials.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                HasLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.GetString("$ERROR_DIDNT_FIND_MTL") + ex.Message, Language.GetString("$ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
