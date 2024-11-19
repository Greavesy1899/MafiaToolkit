using System.IO;
using ResourceTypes.M3.XBin;

namespace FileTypes.XBin.StreamMap.Commands
{
    public interface ICommand
    {
        void ReadFromFile(BinaryReader reader);

        void WriteToFile(XBinWriter writer);
        int GetSize();
        uint GetMagic();
    }
}
