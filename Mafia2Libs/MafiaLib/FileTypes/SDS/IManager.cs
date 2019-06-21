using Gibbed.IO;
using System.IO;

namespace ResourceTypes.SDS
{
    public interface IManager
    {
        void UnpackResource(Stream stream);
        void PackResource(Stream stream);
        bool IsVersionSupported(int ver);
        string GetResourceDescriptor();
    }
}
