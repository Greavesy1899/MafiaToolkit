using System;
using System.IO;
using Mafia2Tool;

namespace Core.IO
{
    public class FileFrameResource : FileBase
    {
        public SceneData SceneData = new SceneData();
        private bool bForceBigEndian;

        public FileFrameResource(FileInfo info) : base(info)
        {
            SceneData.ScenePath = info.DirectoryName;
            bForceBigEndian = false;
        }

        public override bool Open()
        {
            //make sure to load materials.
            MaterialData.Load();

            //we now build scene data from GameExplorer rather than d3d viewer.
            SceneData.BuildData(bForceBigEndian);

            //d3d viewer expects data inside scenedata.
            MapEditor d3dForm = new MapEditor(file,SceneData);
            d3dForm.Dispose();
            return true;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return "FR";
        }

        public void SetBigEndian(bool bResult)
        {
            bForceBigEndian = bResult;
        }
    }
}
