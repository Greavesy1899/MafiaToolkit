using ResourceTypes.M3.XBin;
using System;
using System.Diagnostics;
using System.IO;

namespace ResourceTypes.FileTypes.M3.XBin
{
    public static class XBinFactory
    {
        public static BaseTable ReadXBin(BinaryReader reader, ulong hash)
        {
            BaseTable XBinData = null;

            switch(hash)
            {
                case 0x5E42EF29E8A3E1D3: // StringTable
                    XBinData = new StringTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0xA869F8A3ED0CDAFC:
                    XBinData = new VehicleTable(); // VehicleTable
                    XBinData.ReadFromFile(reader);
                    break;
                //case 0xDC327944DD83627E: // TODO: Fix for M1: DE. look for 0xA for PaintCombination array entries.
                //    XBinData = new PaintCombinationsTable(); // PaintCombinations
                //    XBinData.ReadFromFile(reader);
                //    break;
                default:
                    throw new Exception("We lack the support for this type.");
                    break;
            }

            Debug.Assert(XBinData != null, "XBinData is null, but we should have actually read this.");
            return XBinData;
        }
    }
}
