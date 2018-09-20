using Mafia2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace Mafia2Tool
{
    public static class SceneData
    {
        public static FrameNameTable FrameNameTable;
        public static FrameResource FrameResource;
        public static VertexBufferManager VertexBufferPool;
        public static IndexBufferManager IndexBufferPool;
        public static SoundSector SoundSector;
        public static Actor[] Actors;
        public static ItemDesc[] ItemDescs;
        public static Collision Collisions;
        public static CityAreas CityAreas;
        public static CityShops CityShops;
        public static string ScenePath;

        public static void BuildData()
        {
            List<FileInfo> vbps = new List<FileInfo>();
            List<FileInfo> ibps = new List<FileInfo>();
            List<ItemDesc> ids = new List<ItemDesc>();
            List<Actor> act = new List<Actor>();

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
                else if (type == "FrameResource")
                    FrameResource = new FrameResource(name);
                else if (type == "FrameNameTable")
                    FrameNameTable = new FrameNameTable(name);
                else if (type == "ItemDesc")
                    ids.Add(new ItemDesc(name));
                else if (type == "FrameNameTable")
                    FrameNameTable = new FrameNameTable(name);
                //else if ((type == "MemFile") && (name.Contains("cityshops")))
                //    CityShops = new CityShops(name);
                //else if (type == "Actors")
                //    act.Add(new Actor(name));
                //else if (type == "AudioSectors")
                //    SoundSector = new SoundSector(name);
                //else if ((type == "MemFile") && (name.Contains("cityareas")))
                //    CityAreas = new CityAreas(name);
            }

            IndexBufferPool = new IndexBufferManager(ibps);
            VertexBufferPool = new VertexBufferManager(vbps);
            ItemDescs = ids.ToArray();
            Actors = act.ToArray();

            for (int i = 0; i != ItemDescs.Length; i++)
                ItemDescs[i].WriteToEDC();

            if (Actors == null)
                return;

            AttachActors();
        }

        public static void AttachActors()
        {
            for (int y = 0; y != Actors.Length; y++)
            {
                Actor act = Actors[y];
                for (int i = 0; i != act.Definitions.Length; i++)
                {
                    for (int c = 0; c != act.Items.Length; c++)
                    {
                        if (act.Items[c].Hash1 == act.Definitions[i].Hash)
                        {
                            //FrameObjectFrame frame = FrameResource.FrameObjects[act.Definitions[i].FrameIndex] as FrameObjectFrame;
                            //frame.Item = act.Items[c];
                            //FrameResource.FrameObjects[act.Definitions[i].FrameIndex] = frame;
                        }
                    }
                }
            }
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
        }
    }

    public static class MaterialData
    {
        public static bool HasLoaded = false;

        /// <summary>
        /// Loads all material data from user-specified path.
        /// </summary>
        public static void Load()
        {
            try
            {
                MaterialsManager.ReadMatFiles(ToolkitSettings.MaterialLibs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                HasLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load materials. Error occured: \n\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
