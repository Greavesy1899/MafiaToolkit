using Mafia2Tool;
using System;
using System.IO;

namespace Core.IO
{
    public class MapEditorSession : FileBase
    {
        private bool bForceBigEndian;

        public MapEditorSession(FileInfo info) : base(info)
        {
        }

        public override bool Open()
        {
            //make sure to load materials.
            MaterialData.Load();

            //we now build scene data from GameExplorer rather than d3d viewer.
            string[] scenepaths = ReadMESScenePaths();
            
            MapEditor d3dForm = new MapEditor(file,scenepaths);
            d3dForm.Dispose();
            return true;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return "MES";
        }

        private string[] ReadMESScenePaths()
        {
            // Otevření streamu pro čtení souboru
            byte[] fileData = File.ReadAllBytes(file.FullName);
            using (var ms = new MemoryStream(fileData))
            using (var reader = new BinaryReader(ms))
            {
                int version = reader.ReadInt32();
                int numberOfScenes = reader.ReadInt32();
                
                string[] scenePaths = new string[numberOfScenes];
                
                for (int i = 0; i < numberOfScenes; i++)
                {
                    int stringLength = reader.ReadInt32();
                    
                    scenePaths[i] = new string(reader.ReadChars(stringLength));
                }

                return scenePaths;
            }
        }

    }
}
