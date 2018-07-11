using Mafia2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Mafia2Tool
{
    public static class SceneData
    {
        public static FrameNameTable FrameNameTable;
        public static FrameResource FrameResource;
        public static VertexBufferManager VertexBufferPool;
        public static IndexBufferManager IndexBufferPool;
        public static SoundSector SoundSector;
        public static Actor Actors;
        public static ItemDesc[] ItemDescs;
        public static Collision Collisions;
        public static CityAreas CityAreas;
        public static string ScenePath = Properties.Settings.Default.SDSPath;

        public static void BuildData()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(ScenePath);
            FileInfo[] files = dirInfo.GetFiles();

            List<FileInfo> vbps = new List<FileInfo>();
            List<FileInfo> ibps = new List<FileInfo>();
            List<ItemDesc> ids = new List<ItemDesc>();

            foreach (FileInfo file in files)
            {
                //if (file.FullName.Contains("VertexBufferPool"))
                //    vbps.Add(file);

                //if (file.FullName.Contains("IndexBufferPool"))
                //    ibps.Add(file);

               //at if (file.FullName.Contains("ItemDesc"))
                   // ids.Add(new ItemDesc(file.FullName));

                if (file.FullName.Contains("FrameResource_0.bin"))
                    FrameResource = new FrameResource(file.FullName);

                if (file.FullName.Contains("FrameNameTable_0.bin"))
                    FrameNameTable = new FrameNameTable(file.FullName);

                //if (file.FullName.Contains("SoundSector"))
                //    SoundSector = new SoundSector(file.FullName);

                //if (file.FullName.Contains("Actors_0"))
                //    Actors = new Actor(file.FullName);

                //if (file.FullName.Contains("Collision"))
                //    Collisions = new Collision(file.FullName);

                //if (file.FullName.Contains("cityareas"))
                //    CityAreas = new CityAreas(file.FullName);
            }

            //IndexBufferPool = new IndexBufferManager(ibps);
            //VertexBufferPool = new VertexBufferManager(vbps);
            //ItemDescs = ids.ToArray();

            //for (int i = 0; i != ItemDescs.Length; i++)
            //    ItemDescs[i].WriteToEDC();

            if (Actors == null)
                return;

            //AttachActors();
            FrameResource.UpdateEntireFrame();
        }

        public static void AttachActors()
        {
            for (int i = 0; i != Actors.Definitions.Length; i++)
            {
                for (int c = 0; c != Actors.Items.Length; c++)
                {
                    if (Actors.Items[c].Hash1 == Actors.Definitions[i].Hash)
                    {
                        FrameObjectFrame frame = FrameResource.FrameObjects[Actors.Definitions[i].FrameIndex] as FrameObjectFrame;
                        frame.Item = Actors.Items[c];
                        FrameResource.FrameObjects[Actors.Definitions[i].FrameIndex] = frame;
                    }
                }
            }
        }
    }

    public static class MaterialData
    {
        public static Material[] Default;
        public static Material[] Default50;
        public static Material[] Default60;
        public static string MaterialPath = Properties.Settings.Default.MaterialPath;
        public static bool HasLoaded = false;
    }
}
