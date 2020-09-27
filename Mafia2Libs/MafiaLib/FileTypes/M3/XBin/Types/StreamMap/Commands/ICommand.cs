using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public interface ICommand
    {
        void ReadFromFile(BinaryReader reader);

        void WriteToFile(BinaryWriter writer);
    }
}
