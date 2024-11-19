using Toolkit.Forms;
using System.IO;

namespace Core.IO
{
    class FileEntityDataStorage : FileBase
    {
        public FileEntityDataStorage(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "EDS";
        }

        public override bool Open()
        {
            EntityDataStorageEditor editor = new EntityDataStorageEditor(file);
            return true;
        }
    }
}
