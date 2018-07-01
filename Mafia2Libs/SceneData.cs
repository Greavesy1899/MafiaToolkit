using Mafia2;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mafia2Tool
{
    public static class SceneData
    {
        public static FrameNameTable FrameNameTable;
        public static FrameResource FrameResource;
        public static VertexBufferPool VertexBufferPool;
        public static IndexBufferPool IndexBufferPool;
        public static SoundSector SoundSector;
        public static Actor Actors;
        public static ItemDesc[] ItemDescs;
        public static Collision Collisions;
        public static CityAreas CityAreas;
        public static string ScenePath = Properties.Settings.Default.SDSPath2;

        public static void BuildData()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(ScenePath);
            FileInfo[] files = dirInfo.GetFiles();

            List<FileInfo> vbps = new List<FileInfo>();
            List<FileInfo> ibps = new List<FileInfo>();
            List<ItemDesc> ids = new List<ItemDesc>();

            foreach (FileInfo file in files)
            {
                if (file.FullName.Contains("VertexBufferPool"))
                    vbps.Add(file);

                if (file.FullName.Contains("IndexBufferPool"))
                    ibps.Add(file);

               //at if (file.FullName.Contains("ItemDesc"))
                   // ids.Add(new ItemDesc(file.FullName));

                if (file.FullName.Contains("FrameResource_0.bin"))
                    FrameResource = new FrameResource(file.FullName);

                if (file.FullName.Contains("FrameNameTable_0.bin"))
                    FrameNameTable = new FrameNameTable(file.FullName);

                if (file.FullName.Contains("SoundSector"))
                    SoundSector = new SoundSector(file.FullName);

                //if (file.FullName.Contains("Actors"))
                //    Actors = new Actor(file.FullName);

                if (file.FullName.Contains("Collision"))
                    Collisions = new Collision(file.FullName);

                if (file.FullName.Contains("cityareas"))
                    CityAreas = new CityAreas(file.FullName);
            }

            IndexBufferPool = new IndexBufferPool(ibps);
            VertexBufferPool = new VertexBufferPool(vbps);
            ItemDescs = ids.ToArray();

            for (int i = 0; i != ItemDescs.Length; i++)
            {
                ItemDescs[i].WriteToEDC();
            }

            if (Actors == null)
                return;

            AttachActors();
            FrameResource.UpdateEntireFrame();
        }

        public static void AttachActors()
        {
            for (int i = 0; i != FrameResource.FrameObjects.Length; i++)
            {
                if (FrameResource.FrameObjects[i].GetType() == typeof(FrameObjectFrame))
                {
                    for (int x = 0; x != Actors.Items.Length; x++)
                    {
                        if (Actors.Items[x].Hash1 == (FrameResource.FrameObjects[i] as FrameObjectFrame).ActorHash.uHash)
                        {
                            (FrameResource.FrameObjects[i] as FrameObjectFrame).Item = Actors.Items[x];
                        }
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
    }
}
