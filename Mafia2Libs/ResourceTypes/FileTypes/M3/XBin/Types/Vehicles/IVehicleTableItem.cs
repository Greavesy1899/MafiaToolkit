using System.IO;

namespace ResourceTypes.M3.XBin.Vehicles
{
    public interface IVehicleTableItem
    {
        void ReadEntry(BinaryReader Reader);
        void WriteEntry(XBinWriter Writer);
    }
}
