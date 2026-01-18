using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ResourceTypes.CCDB
{
    /// <summary>
    /// Deserializes GENR (Generic Resource) binary format used in Mafia I:DE/Mafia III.
    /// Based on IDA reverse engineering of C_GenericResource::LoadResource and C_Deserializer.
    /// </summary>
    public class GenrDeserializer
    {
        // Known type hashes from IDA analysis
        public const uint TYPE_C_SPAWN_PROFILE_DB_RESOURCE = 0x097E0ECB;
        public const uint TYPE_C_SPAWN_PROFILE_DB = 0x65F20A93;
        public const uint TYPE_C_SPAWN_PROFILE = 0x340319E3;
        public const uint TYPE_C_CHOICE = 0x33BA57D7;
        public const uint TYPE_S_WEIGHTED_CHOICE = 0x7D5A48C0;
        public const uint TYPE_C_RANGE_WITH_CHANCE = 0x3E29F1D9;
        public const uint TYPE_C_PIECE_SET = 0x42694B5F;
        public const uint TYPE_C_COMBINABLE_PIECE = 0x42964A0F;

        private BinaryReader _reader;
        private long _dataStart;
        private long _streamLength;
        private Dictionary<uint, ushort> _typeInstanceCounts;
        private Dictionary<ulong, object> _objectCache; // Object ID -> parsed object

        public List<CCDBSpawnProfile> Profiles { get; } = new List<CCDBSpawnProfile>();
        public List<CCDBChoice> SharedChoices { get; } = new List<CCDBChoice>();
        public List<CCDBRange> RangeValues { get; } = new List<CCDBRange>();
        public List<CCDBPieceSet> PieceSets { get; } = new List<CCDBPieceSet>();

        public GenrDeserializer()
        {
            _typeInstanceCounts = new Dictionary<uint, ushort>();
            _objectCache = new Dictionary<ulong, object>();
        }

        public static string GetTypeName(uint hash)
        {
            return hash switch
            {
                TYPE_C_SPAWN_PROFILE_DB_RESOURCE => "C_SpawnProfileDBResource",
                TYPE_C_SPAWN_PROFILE_DB => "C_SpawnProfileDB",
                TYPE_C_SPAWN_PROFILE => "C_SpawnProfile",
                TYPE_C_CHOICE => "C_Choice",
                TYPE_S_WEIGHTED_CHOICE => "S_WeightedChoice",
                TYPE_C_RANGE_WITH_CHANCE => "C_RangeWithChance",
                TYPE_C_PIECE_SET => "C_PieceSet",
                TYPE_C_COMBINABLE_PIECE => "C_CombinablePiece",
                _ => $"Unknown_0x{hash:X8}"
            };
        }

        public int GetExpectedCount(uint typeHash)
        {
            return _typeInstanceCounts.TryGetValue(typeHash, out ushort count) ? count : 0;
        }

        /// <summary>
        /// Parse the pre-GENR type registry header.
        /// Format: [field 4B][zeros 8B][typeCount 4B][typeHashes...][typeCounts...]
        /// </summary>
        public void ParsePreGenrHeader(BinaryReader reader, long genrOffset)
        {
            reader.BaseStream.Position = 0;

            uint headerField = reader.ReadUInt32();
            reader.ReadUInt64(); // Skip 8 zero bytes
            uint typeCount = reader.ReadUInt32();

            System.Diagnostics.Debug.WriteLine($"[GENR] Pre-header: field=0x{headerField:X8}, typeCount={typeCount}");

            if (typeCount > 100)
            {
                System.Diagnostics.Debug.WriteLine("[GENR] Pre-header typeCount too large, skipping");
                return;
            }

            var typeHashes = new List<uint>();
            var typeCounts = new List<ushort>();

            for (int i = 0; i < typeCount; i++)
                typeHashes.Add(reader.ReadUInt32());

            for (int i = 0; i < typeCount; i++)
                typeCounts.Add(reader.ReadUInt16());

            // Store in dictionary
            for (int i = 0; i < typeCount; i++)
            {
                _typeInstanceCounts[typeHashes[i]] = typeCounts[i];
                System.Diagnostics.Debug.WriteLine($"[GENR] Type[{i}]: 0x{typeHashes[i]:X8} [{GetTypeName(typeHashes[i])}] x{typeCounts[i]}");
            }
        }

        /// <summary>
        /// Parse the GENR header and data section start.
        /// </summary>
        public bool ParseGenrHeader(BinaryReader reader)
        {
            uint magic = reader.ReadUInt32();
            if (magic != 0x524E4547) // "GENR" in little-endian
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Invalid magic: 0x{magic:X8}");
                return false;
            }

            ushort versionMinor = reader.ReadUInt16();
            ushort versionMajor = reader.ReadUInt16();
            uint rootTypeId = reader.ReadUInt32();
            uint flags = reader.ReadUInt32();

            System.Diagnostics.Debug.WriteLine($"[GENR] Version: {versionMajor}.{versionMinor}, rootType=0x{rootTypeId:X8}");

            // Memory footprint for version >= 9
            if (versionMajor >= 9)
            {
                int memFootprint = reader.ReadInt32();
                System.Diagnostics.Debug.WriteLine($"[GENR] MemFootprint: {memFootprint}");
            }

            // Internal type registry (skip if we already have the outer one)
            uint genrTypeCount = reader.ReadUInt32();
            if (genrTypeCount > 0 && genrTypeCount < 200)
            {
                if (_typeInstanceCounts.Count == 0)
                {
                    var typeHashes = new List<uint>();
                    for (int i = 0; i < genrTypeCount; i++)
                        typeHashes.Add(reader.ReadUInt32());
                    for (int i = 0; i < genrTypeCount; i++)
                    {
                        ushort count = reader.ReadUInt16();
                        _typeInstanceCounts[typeHashes[i]] = count;
                    }
                }
                else
                {
                    // Skip GENR's internal registry
                    reader.BaseStream.Position += genrTypeCount * 4; // hashes
                    reader.BaseStream.Position += genrTypeCount * 2; // counts
                }
            }

            _dataStart = reader.BaseStream.Position;
            System.Diagnostics.Debug.WriteLine($"[GENR] Data section starts at 0x{_dataStart:X}");
            return true;
        }

        /// <summary>
        /// Search for a field name pattern in the binary data.
        /// GENR format includes field names like "m_Profiles" as length-prefixed strings.
        /// </summary>
        public long FindFieldByName(byte[] data, string fieldName, long searchStart = 0)
        {
            byte[] pattern = Encoding.ASCII.GetBytes(fieldName);
            byte lengthByte = (byte)fieldName.Length;

            for (long pos = searchStart; pos < data.Length - pattern.Length - 10; pos++)
            {
                // Look for length byte followed by field name
                if (data[pos] == lengthByte)
                {
                    bool match = true;
                    for (int i = 0; i < pattern.Length && match; i++)
                    {
                        if (data[pos + 1 + i] != pattern[i])
                            match = false;
                    }
                    if (match)
                    {
                        System.Diagnostics.Debug.WriteLine($"[GENR] Found '{fieldName}' at offset 0x{pos:X}");
                        return pos;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Read a vector count from the binary stream at the current position.
        /// Returns the count and advances past the count + any type info.
        /// </summary>
        private uint ReadVectorHeader(BinaryReader reader)
        {
            // Vector serialization format varies - try common patterns
            long startPos = reader.BaseStream.Position;

            // First try: direct count as uint32
            uint count = reader.ReadUInt32();

            // Sanity check - counts shouldn't be huge
            if (count > 100000)
            {
                // Reset and try alternative format
                reader.BaseStream.Position = startPos;
                return 0;
            }

            return count;
        }

        /// <summary>
        /// Skip type info bytes that precede element data in GENR format.
        /// </summary>
        private void SkipTypeInfo(BinaryReader reader, int bytes = 8)
        {
            if (reader.BaseStream.Position + bytes <= _streamLength)
            {
                reader.BaseStream.Position += bytes;
            }
        }
    }
}
