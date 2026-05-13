using ResourceTypes.M3.XBin;
using ResourceTypes.M3.XBin.TableContainers;
using ResourceTypes.M3.XBin.GfxContainers;
using System;
using System.Diagnostics;
using System.IO;
using ResourceTypes.M3.XBin.GuiContainers;
using Utils.Logging;

namespace ResourceTypes.M3.XBin
{
    public static class XBinFactory
    {
        public static BaseTable ReadXBin(BinaryReader reader, XBin Parent, ulong hash)
        {
            BaseTable XBinData = null;

            switch(hash)
            {
                case 0x5E42EF29E8A3E1D3: // StringTable
                    XBinData = new StringTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0xA869F8A3AC7656E1: // M3
                case 0xA869F8A3ED0CDAFC: // M1: DE
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
                    // TODO: Broken for both games
                case 0x44FA070D73C43CBD: // CarTrafficTuningTable for M1:DE
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
                case 0xB77D0A522C8E12A3: // MissionsTable (Mafia I: DE)
                case 0xB77D0A52FD0225D0: // MissionTable (Mafia III)
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
                case 0x5D69A41CDC82936C: // TableContainer (Mafia III)
                    XBinData = new TableContainer();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x06F579D51CE129A5: // GfxEnvContainer (Mafia III)
                case 0x06F579D595DA02AD: // GfxEnvContainer (Mafia I: DE)
                    XBinData = new GfxEnvContainer();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0xAD5CF0F7FC3717F0: // GameGuiContainer (Mafia I: DE)
                case 0xAD5CF0F764C39370: // GameGuiContainer (Mafia III)
                    XBinData = new GameGuiContainer();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x0E2FBBCF46754F66: // CarTuningPackAvailabilityTable (Mafia I: DE and Mafia III)
                    XBinData = new CarTuningPackAvailabilityTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0xA32C16191BC63EEF: // StreamMap Table (Mafia I: DE)
                case 0xA32C1619D5261223: // StreamMap Table (Mafia III)
                    XBinData = new StreamMapTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x2B673F12DCA4BBF1:
                case 0x2B673F120D288C9A:
                    XBinData = new CitiesTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 0x8982CC5C78136253: // Subtitles Table (Mafia III)
                case 0x8982CC5C46848F6E: // Subtitles Table (Mafia I: DE)
                    XBinData = new SubtitleTable();
                    XBinData.ReadFromFile(reader);
                    break;
                case 15866877770641655778:
                case 0xDC327944DD83627E:
                    XBinData = new PaintCombinationsTable(); // PaintCombinations
                    XBinData.ReadFromFile(reader);
                    break;
                case 0xAB9158912EE359DE: // GameMelee (Mafia I: DE)
                    XBinData = new GameMeleeTable();
                    XBinData.ReadFromFile(reader);
                    break;
                // Empty MP placeholders — Mafia I: DE ships these as a hash
                // reservation with a fixed empty body (88 bytes, identical
                // across all MP tables).
                case 0x4DC22579671437D6: // businesses_mp
                case 0xEF634D2F8A3F6A38: // charactertable_mp
                case 0xDCAAA289821103EC: // cities_mp
                case 0x2C3F50C91D8B2D8B: // game_events_mp
                case 0x908F4C0CFFBEB487: // missions_mp
                case 0x23CA3D65F68F175D: // persistent_characters_mp
                case 0x9C1617718CF12257: // plotlines_mp
                case 0x3D75F99490367D50: // quests_mp
                case 0x9CF8F2FB8911D588: // slots_mp
                case 0xA1408F852F76D27F: // speeches_mp
                case 0xAC124B39C2C72AED: // streammap_mp
                case 0x5E8198F71D5F1B07: // stringtable_mp
                    XBinData = new EmptyMPPlaceholderTable();
                    XBinData.ReadFromFile(reader);
                    break;
                default:
                    throw new Exception("We lack the support for this type.");
                    break;
            }

            ToolkitAssert.Ensure(XBinData != null, "XBinData is null, but we should have actually read this.");
            return XBinData;
        }
    }
}
