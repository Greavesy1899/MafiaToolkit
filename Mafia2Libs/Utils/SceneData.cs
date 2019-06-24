using ResourceTypes.FrameResource;
using ResourceTypes.FrameNameTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Utils.Settings;
using Utils.Lang;
using ResourceTypes.BufferPools;
using ResourceTypes.City;
using ResourceTypes.ItemDesc;
using ResourceTypes.Materials;
using ResourceTypes.Sound;
using ResourceTypes.Actors;
using ResourceTypes.Collisions;
using ResourceTypes.Navigation;

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
        public static string ScenePath;

        public static void BuildData()
        {
            List<FileInfo> vbps = new List<FileInfo>();
            List<FileInfo> ibps = new List<FileInfo>();
            List<ItemDescLoader> ids = new List<ItemDescLoader>();
            List<Actor> act = new List<Actor>();
            List<NAVData> aiw = new List<NAVData>();
            List<NAVData> obj = new List<NAVData>();

            DirectoryInfo dirInfo = new DirectoryInfo(ScenePath);

            FileInfo[] files = dirInfo.GetFiles("*", SearchOption.AllDirectories);

            XmlDocument document = new XmlDocument();
            document.Load(ScenePath + "/SDSContent.xml");
            XPathNavigator nav = document.CreateNavigator();
            var nodes = nav.Select("/SDSResource/ResourceEntry");
            while (nodes.MoveNext() == true)
            {
                string type;
                string name;

                nodes.Current.MoveToFirstChild();
                type = nodes.Current.Value;
                nodes.Current.MoveToNext();
                name = ScenePath + "/" + nodes.Current.Value;

                if (type == "IndexBufferPool")
                    ibps.Add(new FileInfo(name));
                else if (type == "VertexBufferPool")
                    vbps.Add(new FileInfo(name));
                else if (type == "Actors")
                    act.Add(new Actor(name));
                else if (type == "FrameResource")
                    FrameResource = new FrameResource(name);
                else if (type == "ItemDesc")
                    ids.Add(new ItemDescLoader(name));
                else if (type == "FrameNameTable")
                    FrameNameTable = new FrameNameTable(name);
                else if (type == "Collisions")
                    Collisions = new Collision(name);
                else if (type == "AnimalTrafficPaths")
                    ATLoader = new AnimalTrafficLoader(new FileInfo(name));
                else if (nodes.Current.Value == "roadmap.gsd")
                    roadMap = new Roadmap(new FileInfo(name));
                //else if (type == "NAV_HPD_DATA")
                //    HPDData = new NAVData(new FileInfo(name));
            }

            IndexBufferPool = new IndexBufferManager(ibps);
            VertexBufferPool = new VertexBufferManager(vbps);
            ItemDescs = ids.ToArray();
            Actors = act.ToArray();
            OBJData = obj.ToArray();
        }

        public static void Reload()
        {
            CleanData();
            BuildData();
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
                MaterialsManager.ReadMatFiles(ToolkitSettings.MaterialLibs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                HasLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.GetString("$ERROR_DIDNT_FIND_MTL") + ex.Message, Language.GetString("$ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
