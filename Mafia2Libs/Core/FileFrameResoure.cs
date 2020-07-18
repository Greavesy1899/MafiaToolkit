using Mafia2Tool;
using System;
using System.IO;

namespace Core.IO
{
    public class FileFrameResource : FileBase
    {
        private bool bForceBigEndian;

        public FileFrameResource(FileInfo info) : base(info)
        {
            bForceBigEndian = false;
        }

        public override bool Open()
        {
            //make sure to load materials.
            MaterialData.Load();

            //we now build scene data from GameExplorer rather than d3d viewer.
            SceneData.ScenePath = file.DirectoryName;
            SceneData.BuildData(bForceBigEndian);

            //d3d viewer expects data inside scenedata.
            MapEditor d3dForm = new MapEditor(file);
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
