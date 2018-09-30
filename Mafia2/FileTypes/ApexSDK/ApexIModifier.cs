using System.IO;

namespace ApexSDK
{
    public interface IModifier
    {
        ModifierType Type { get; set; }
        void ReadFromFile(BinaryReader reader);
        void WriteToFile(BinaryWriter writer);
    }
}
