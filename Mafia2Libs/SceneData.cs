using Mafia2;
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
        //public static string ScenePath = "E:/Games/Steam/steamapps/common/Mafia II/pc/sds/cars/extracted/ascot_baileys200_pha";
        public static string ScenePath = "E:/Games/Steam/steamapps/common/Mafia II/pc/sds/city/extracted/chinatown";
        //public static string ScenePath = "E:/Games/Steam/steamapps/common/Mafia II/pc/sds/city_univers/extracted/city_univers";

        public static void BuildData()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(ScenePath);
            FileInfo[] files = dirInfo.GetFiles();

            List<FileInfo> vbps = new List<FileInfo>();
            List<FileInfo> ibps = new List<FileInfo>();

            foreach (FileInfo file in files)
            {
                if (file.FullName.Contains("VertexBufferPool"))
                    vbps.Add(file);

                if (file.FullName.Contains("IndexBufferPool"))
                    ibps.Add(file);

                if (file.FullName.Contains("FrameResource_0.bin"))
                    FrameResource = new FrameResource(file.FullName);

                if (file.FullName.Contains("FrameNameTable_0.bin"))
                    FrameNameTable = new FrameNameTable(file.FullName);
            }

            IndexBufferPool = new IndexBufferPool(ibps);
            VertexBufferPool = new VertexBufferPool(vbps);

            using (BinaryWriter writer = new BinaryWriter(File.Create("newFrameNameTable_0.bin")))
            {
                FrameNameTable.WriteToFile(writer);
            }          
        }
    }

    public static class MaterialData
    {
        public static Material[] Default;
        public static Material[] Default50;
        public static Material[] Default60;
        public static string MaterialPath = "E:/Games/Steam/steamapps/common/Mafia II/edit/materials";
    }
}
