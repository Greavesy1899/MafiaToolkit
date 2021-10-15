using Toolkit;
using System.Collections.Generic;
using System.IO;

namespace Core.IO
{
    class FilePrefab : FileBase
    {
        public FilePrefab(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "PRF";
        }

        public override bool Open()
        {
            var files = parentDirectory.GetFilesFromDirectory<FileActor>();

            List<string> definitions = new List<string>();
            foreach(var file in files)
            {
                definitions.AddRange(file.GetDefinitionList());
            }

            PrefabEditor editor = new PrefabEditor(file);
            editor.InitEditor(definitions);

            return true;
        }
    }
}
