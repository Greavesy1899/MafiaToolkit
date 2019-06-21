using System;
using System.IO;

namespace ResourceTypes.SDS
{
    public class FrameManager : IManager
    {
        const string managerName = "FrameResource";
        const int version = 0x15;

        public string GetResourceDescriptor()
        {
            return managerName;
        }

        public bool IsVersionSupported(int ver)
        {
            return ver > version;
        }

        public void PackResource(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void UnpackResource(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
