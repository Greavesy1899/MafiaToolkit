using System.Diagnostics;
using System.IO;
using ResourceTypes.Navigation;

namespace Core.IO
{
    public class FileNavigation_NAV : FileBase
    {
        public FileNavigation_NAV(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "NAV";
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
