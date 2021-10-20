using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Extensions;
using Utils.Helpers.Reflection;
using Utils.Settings;
using Utils.StringHelpers;

namespace ResourceTypes.M3.XBin.Vehicles
{
    public interface IVehicleTableItem
    {
        void ReadEntry(BinaryReader Reader);
        void WriteEntry(XBinWriter Writer);
    }
}
