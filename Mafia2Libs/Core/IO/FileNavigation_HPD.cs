using ResourceTypes.Navigation;
using System.Diagnostics;
using System.IO;

namespace Core.IO
{
    public class FileNavigation_HPD : FileBase
    {
        public FileNavigation_HPD(FileInfo info) : base(info)
        {

        }

        public override string GetExtensionUpper()
        {
            return "HPD";
        }

        public override bool Open()
        {
            if(!Debugger.IsAttached)
            {
                return false; // debug only for now
            }

            NAVData Data = new NAVData(file);
            return true;
        }
    }
}
