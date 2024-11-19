using System.Diagnostics;
using System.IO;
using ResourceTypes.Navigation;

namespace Core.IO
{
    public class FileNavigation_OBJ : FileBase
    {
        public FileNavigation_OBJ(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "NOV";
        }

        public override bool Open()
        {
            if (!Debugger.IsAttached)
            {
                return false; // debug only for now
            }

            NAVData Data = new NAVData(file);
            return true;
        }
    }
}
