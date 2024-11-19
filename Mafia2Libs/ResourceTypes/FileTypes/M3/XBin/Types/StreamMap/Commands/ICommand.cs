using ResourceTypes.M3.XBin;
using System.IO;

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
