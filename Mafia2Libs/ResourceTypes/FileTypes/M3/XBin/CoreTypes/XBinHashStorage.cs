using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceTypes.M3.XBin
{
    public static class XBinHashStorage
    {
        private static string[] StringTable;
        private static Dictionary<ulong, uint> FNV64Storage;
        private static Dictionary<uint, uint> FNV32Storage;

        public static void LoadStorage()
        {
            // Load string table for XBins.
            string[] LoadedLines = File.ReadAllLines("Resources//GameData//XBin_Hashes.txt");

            // Create all arrays
            StringTable = new string[LoadedLines.Length];
            FNV32Storage = new Dictionary<uint, uint>();
            FNV64Storage = new Dictionary<ulong, uint>();

            // iterate through all lines and build our storage.
            for(uint i = 0; i < LoadedLines.Length; i++)
            {
                string Line = LoadedLines[i];
                StringTable[i] = Line;

                ulong FNV64Hash = FNV64.Hash(Line);
                uint FNV32Hash = FNV32.Hash(Line);

                FNV64Storage.TryAdd(FNV64Hash, i);
                FNV32Storage.TryAdd(FNV32Hash, i);
            }
        }

        public static string GetNameFromHash(ulong Hash, out bool bSuccessful)
        {
            uint Index = uint.MaxValue;
            bSuccessful = FNV64Storage.TryGetValue(Hash, out Index);

            return GetFromTable(Index);
        }

        public static string GetNameFromHash(uint Hash, out bool bSuccessful)
        {
            uint Index = uint.MaxValue;
            bSuccessful = FNV32Storage.TryGetValue(Hash, out Index);

            return GetFromTable(Index);
        }

        private static string GetFromTable(uint Index)
        {
            if(Index == uint.MaxValue)
            {
                return string.Empty;
            }

            return StringTable[Index];
        }
    }
}
