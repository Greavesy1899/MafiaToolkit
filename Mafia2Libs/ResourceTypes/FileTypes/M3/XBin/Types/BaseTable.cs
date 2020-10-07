using ResourceTypes.FileTypes.M3.XBin;
using System.IO;

namespace ResourceTypes.M3.XBin
{
    public interface BaseTable
    {
        // Serialization/Deserialization from binary.
        void ReadFromFile(BinaryReader reader);
        void WriteToFile(XBinWriter writer);

        // Serialization/Deserialization from XML.
        void ReadFromXML();
        void WriteToXML();
    }
}
