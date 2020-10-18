using ResourceTypes.M3.XBin;
using ResourceTypes.M3.XBin.TableContainers;
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
                case 0x3759788EC437536C: // CarGearboxTable
                    XBinData = new CarGearBoxesTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0xF5D56763013A2B0A: // CarInteriorColorsTable
                    XBinData = new CarInteriorColorsTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x44FA070D73C43CBD: // CarTrafficTuningTable for M1:DE
                    XBinData = new CarTrafficTuningTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x44FA070D55388F65: // CarTrafficTuningTable for M3
                    XBinData = new CarTrafficTuningTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x09B5140FA382AF8F: // CarTuningModificatorsTable
                    XBinData = new CarTuningModificatorsTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x2793BB7847F84081: // GameMeshBindingTable
                    XBinData = new GameMeshBindingTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x493DEA76C9A390F0: // SlotTable
                    XBinData = new SlotTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0xB77D0A522C8E12A3: // MissionsTable
                    XBinData = new MissionsTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x3990790678078A1C: // GenericSpeechSituations
                    XBinData = new GenericSpeechSituationsTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0xEF795C84CA85E193: // CharacterCinematicsTable
                    XBinData = new CharacterCinematicsTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x5D69A41C1FBD6565: // TableContainer (Mafia I: DE)
                    XBinData = new TableContainer();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x5D69A41CDC82936C: // TableContainer (Mafia III)
                    XBinData = new TableContainer();
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
