using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public const uint TYPE_S_WEIGHTED_CHOICE = 0x245A5C55;  // Updated from IDA analysis
        public const uint TYPE_C_RANGE_WITH_CHANCE = 0x62554927;  // Updated from IDA analysis
        public const uint TYPE_C_RANGE_WITH_CHANCE_ALT = 0x21232777;  // Alternate hash from GENR files
        public const uint TYPE_C_PIECE_SET = 0x75DD667F;  // Updated from IDA analysis
        public const uint TYPE_C_COMBINABLE_PIECE = 0x42964A0F;
        public const uint TYPE_C_RANGE = 0x77B959E0;  // New from IDA analysis

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
                TYPE_C_RANGE_WITH_CHANCE_ALT => "C_RangeWithChance",
                TYPE_C_PIECE_SET => "C_PieceSet",
                TYPE_C_COMBINABLE_PIECE => "C_CombinablePiece",
                TYPE_C_RANGE => "C_Range",
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

        /// <summary>
        /// Parse a C_SpawnProfile from the binary stream.
        /// Structure (80 bytes):
        /// - m_Id: C_HashName (8B) at offset 0
        /// - m_Choices: vector&lt;S_WeightedChoice&gt; at offset 8
        /// - m_Packages: vector&lt;C_HashName&gt; at offset 32
        /// - m_Priority: int at offset 56
        /// - m_MinimumPackagesToLoad: uint at offset 60
        /// - m_DesiredPackageMinimum: uint at offset 64
        /// - m_Flags: uint at offset 68
        /// </summary>
        private CCDBSpawnProfile ParseSpawnProfile(BinaryReader reader, HashSet<ulong> seenIds)
        {
            if (reader.BaseStream.Position + 28 > _streamLength)
                return null;

            long profileStart = reader.BaseStream.Position;

            // m_Id: 8 bytes hash
            ulong id = reader.ReadUInt64();
            if (!IsValidProfileHash(id) || seenIds.Contains(id))
            {
                reader.BaseStream.Position = profileStart;
                return null;
            }

            // m_Choices: count (4 bytes) + array of S_WeightedChoice (8 bytes each)
            uint choicesCount = reader.ReadUInt32();
            if (choicesCount > 200)
            {
                reader.BaseStream.Position = profileStart;
                return null;
            }

            long afterChoices = reader.BaseStream.Position + (choicesCount * 8L);
            if (afterChoices + 20 > _streamLength)
            {
                reader.BaseStream.Position = profileStart;
                return null;
            }

            var choices = new List<CCDBWeightedChoice>();
            for (int j = 0; j < choicesCount; j++)
            {
                uint choiceIndex = reader.ReadUInt32();
                float weight = reader.ReadSingle();

                // Validate weight
                if (float.IsNaN(weight) || float.IsInfinity(weight) || weight < 0 || weight > 100)
                {
                    reader.BaseStream.Position = profileStart;
                    return null;
                }

                choices.Add(new CCDBWeightedChoice { ChoiceIndex = choiceIndex, Weight = weight });
            }

            // m_Packages: count (4 bytes) + array of C_HashName (8 bytes each)
            uint packagesCount = reader.ReadUInt32();
            if (packagesCount > 200)
            {
                reader.BaseStream.Position = profileStart;
                return null;
            }

            long afterPackages = reader.BaseStream.Position + (packagesCount * 8L);
            if (afterPackages + 16 > _streamLength)
            {
                reader.BaseStream.Position = profileStart;
                return null;
            }

            var packages = new List<CCDBPackage>();
            for (int j = 0; j < packagesCount; j++)
            {
                ulong pkgHash = reader.ReadUInt64();
                packages.Add(new CCDBPackage { Id = new CCDBHashName(pkgHash) });
            }

            // Tail fields
            int priority = reader.ReadInt32();
            uint minPkg = reader.ReadUInt32();
            uint desiredMin = reader.ReadUInt32();
            uint flags = reader.ReadUInt32();

            // Validation
            if (minPkg > 500 || desiredMin > 500)
            {
                reader.BaseStream.Position = profileStart;
                return null;
            }

            return new CCDBSpawnProfile
            {
                Id = new CCDBHashName(id),
                Choices = choices,
                Packages = packages,
                Priority = priority,
                MinimumPackagesToLoad = minPkg,
                DesiredPackageMinimum = desiredMin,
                Flags = flags
            };
        }

        private bool IsValidProfileHash(ulong value)
        {
            if (value == 0 || value == ulong.MaxValue)
                return false;

            // Count set bits - FNV64 hashes have good distribution
            int setBits = 0;
            ulong v = value;
            while (v != 0)
            {
                setBits += (int)(v & 1);
                v >>= 1;
            }

            if (setBits < 10 || setBits > 54)
                return false;

            // Must have high bits set
            if ((value & 0xFFFF000000000000UL) == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Parse a C_Choice from the binary stream.
        /// Structure (104 bytes):
        /// - m_PieceSets: vector&lt;uint&gt; at offset 0
        /// - m_RangeIndexes: vector&lt;ushort&gt; at offset 24
        /// - m_Channels: vector&lt;uint&gt; at offset 48
        /// - m_Tags: map at offset 72 (complex, simplified parsing)
        /// - m_Flags: uint at offset 96
        /// </summary>
        private CCDBChoice ParseChoice(BinaryReader reader)
        {
            if (reader.BaseStream.Position + 20 > _streamLength)
                return null;

            long choiceStart = reader.BaseStream.Position;

            try
            {
                // m_PieceSets: count + array of uint
                uint pieceSetsCount = reader.ReadUInt32();
                if (pieceSetsCount > 500)
                {
                    reader.BaseStream.Position = choiceStart;
                    return null;
                }

                var pieceSets = new List<uint>();
                for (int i = 0; i < pieceSetsCount; i++)
                {
                    if (reader.BaseStream.Position + 4 > _streamLength) break;
                    pieceSets.Add(reader.ReadUInt32());
                }

                // m_RangeIndexes: count + array of ushort
                uint rangeIndexCount = reader.ReadUInt32();
                if (rangeIndexCount > 500)
                {
                    reader.BaseStream.Position = choiceStart;
                    return null;
                }

                var rangeIndexes = new List<ushort>();
                for (int i = 0; i < rangeIndexCount; i++)
                {
                    if (reader.BaseStream.Position + 2 > _streamLength) break;
                    rangeIndexes.Add(reader.ReadUInt16());
                }

                // m_Channels: count + array of uint
                uint channelsCount = reader.ReadUInt32();
                if (channelsCount > 500)
                {
                    reader.BaseStream.Position = choiceStart;
                    return null;
                }

                var channels = new List<uint>();
                for (int i = 0; i < channelsCount; i++)
                {
                    if (reader.BaseStream.Position + 4 > _streamLength) break;
                    channels.Add(reader.ReadUInt32());
                }

                // m_Tags: map - complex structure, skip for now by reading size
                // Maps are serialized as count + (key, value) pairs
                uint tagsCount = reader.ReadUInt32();
                if (tagsCount > 100)
                {
                    reader.BaseStream.Position = choiceStart;
                    return null;
                }

                var tags = new List<CCDBTag>();
                // Skip tags parsing for now - too complex without full type info
                // Would need to parse C_HashNameString + C_TagValueList for each pair

                // m_Flags: uint
                if (reader.BaseStream.Position + 4 > _streamLength)
                {
                    reader.BaseStream.Position = choiceStart;
                    return null;
                }
                uint flags = reader.ReadUInt32();

                return new CCDBChoice
                {
                    PieceSets = pieceSets,
                    RangeIndexes = rangeIndexes,
                    Channels = channels,
                    Tags = tags,
                    Flags = flags
                };
            }
            catch
            {
                reader.BaseStream.Position = choiceStart;
                return null;
            }
        }

        /// <summary>
        /// Find and parse all C_Choice objects in the data section.
        /// Uses field name search to locate the m_SharedChoices array.
        ///
        /// GENR vector format (from IDA DeserializeContainer):
        /// - Field name: 1 byte length + N bytes name
        /// - Type info: 4B type hash + 1B flags + 4B val1 + 4B val2 = 13 bytes
        /// - Container header: 8 bytes
        /// - Count: 4 bytes
        /// - Type infos for elements (variable length, ends with marker)
        /// - Elements: each C_OwningPtr has 8B header + 8B ref + inline object data
        /// </summary>
        public void ParseSharedChoices(byte[] data, BinaryReader reader, int expectedCount)
        {
            // Search for "m_SharedChoices" field name in binary
            long fieldPos = FindFieldByName(data, "m_SharedChoices", _dataStart);

            if (fieldPos > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Found m_SharedChoices at 0x{fieldPos:X}");

                // Calculate positions based on GENR format from IDA
                long afterName = fieldPos + 1 + 15; // After length byte + "m_SharedChoices"

                // Dump 48 bytes for debugging
                if (afterName + 48 < data.Length)
                {
                    System.Diagnostics.Debug.WriteLine($"[GENR] Bytes at 0x{afterName:X}:");
                    for (int row = 0; row < 3; row++)
                    {
                        long off = afterName + row * 16;
                        System.Diagnostics.Debug.WriteLine($"  0x{off:X4}: " +
                            $"{data[off]:X2} {data[off+1]:X2} {data[off+2]:X2} {data[off+3]:X2} " +
                            $"{data[off+4]:X2} {data[off+5]:X2} {data[off+6]:X2} {data[off+7]:X2} " +
                            $"{data[off+8]:X2} {data[off+9]:X2} {data[off+10]:X2} {data[off+11]:X2} " +
                            $"{data[off+12]:X2} {data[off+13]:X2} {data[off+14]:X2} {data[off+15]:X2}");
                    }

                    // Parse known offsets
                    uint typeHash = BitConverter.ToUInt32(data, (int)afterName);
                    byte flags = data[afterName + 4];
                    uint val1 = BitConverter.ToUInt32(data, (int)afterName + 5);
                    uint val2 = BitConverter.ToUInt32(data, (int)afterName + 9);

                    System.Diagnostics.Debug.WriteLine($"[GENR] TypeInfo: hash=0x{typeHash:X8} flags=0x{flags:X2} val1={val1} val2={val2}");

                    // Container header at offset +13
                    ulong containerHeader = BitConverter.ToUInt64(data, (int)afterName + 13);
                    System.Diagnostics.Debug.WriteLine($"[GENR] Container header: 0x{containerHeader:X16}");

                    // Count at offset +21
                    uint count = BitConverter.ToUInt32(data, (int)afterName + 21);
                    System.Diagnostics.Debug.WriteLine($"[GENR] Count at +21: {count}");

                    // Also try other offsets in case format varies
                    for (int testOff = 13; testOff <= 30; testOff += 4)
                    {
                        uint testCount = BitConverter.ToUInt32(data, (int)afterName + testOff);
                        if (testCount == expectedCount)
                        {
                            System.Diagnostics.Debug.WriteLine($"[GENR] Found expected count {expectedCount} at offset +{testOff}");
                            reader.BaseStream.Position = afterName + testOff;
                            ParseChoicesArray(reader, expectedCount);
                            return;
                        }
                    }
                }

                // Search entire file for the count value to understand format
                byte[] countBytes = BitConverter.GetBytes((uint)expectedCount);
                System.Diagnostics.Debug.WriteLine($"[GENR] Searching for count {expectedCount} (0x{expectedCount:X4}) in entire file...");

                for (long pos = 0; pos < data.Length - 4; pos++)
                {
                    if (data[pos] == countBytes[0] && data[pos + 1] == countBytes[1] &&
                        data[pos + 2] == countBytes[2] && data[pos + 3] == countBytes[3])
                    {
                        System.Diagnostics.Debug.WriteLine($"[GENR] Found count {expectedCount} at offset 0x{pos:X}");

                        // Show context: 16 bytes before and 16 bytes after
                        long ctxStart = Math.Max(0, pos - 16);
                        long ctxEnd = Math.Min(data.Length, pos + 20);

                        StringBuilder sb = new StringBuilder();
                        sb.Append($"  Context at 0x{ctxStart:X}: ");
                        for (long i = ctxStart; i < ctxEnd; i++)
                        {
                            if (i == pos) sb.Append("[");
                            sb.Append($"{data[i]:X2}");
                            if (i == pos + 3) sb.Append("]");
                            sb.Append(" ");
                        }
                        System.Diagnostics.Debug.WriteLine(sb.ToString());

                        // Try parsing from this position
                        reader.BaseStream.Position = pos;
                        ParseChoicesArray(reader, expectedCount);
                        if (SharedChoices.Count > 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"[GENR] Successfully parsed {SharedChoices.Count} choices from offset 0x{pos:X}");
                            return;
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine($"[GENR] Could not find count {expectedCount} anywhere in file");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[GENR] m_SharedChoices field not found");
            }
        }

        private bool TryFindVectorStart(byte[] data, long searchStart, int expectedCount, out long vectorStart)
        {
            vectorStart = -1;
            byte[] countBytes = BitConverter.GetBytes((uint)expectedCount);

            // Search for the count value near the field name
            for (long pos = searchStart; pos < Math.Min(searchStart + 1000, data.Length - 4); pos++)
            {
                if (data[pos] == countBytes[0] &&
                    data[pos + 1] == countBytes[1] &&
                    data[pos + 2] == countBytes[2] &&
                    data[pos + 3] == countBytes[3])
                {
                    vectorStart = pos;
                    System.Diagnostics.Debug.WriteLine($"[GENR] Found count {expectedCount} at 0x{pos:X}");
                    return true;
                }
            }
            return false;
        }

        private void ParseChoicesArray(BinaryReader reader, int expectedCount)
        {
            uint count = reader.ReadUInt32();
            long dataStart = reader.BaseStream.Position;
            System.Diagnostics.Debug.WriteLine($"[GENR] Parsing {count} choices at position 0x{dataStart:X}");

            // Reset debug counter for new parsing session
            _choiceDebugCount = 0;

            // GENR vector<C_OwningPtr<T>> format from IDA analysis:
            // - Count: 4 bytes (already read)
            // - Type info section: field names and types stored ONCE for all elements
            // - Element data: each element's values WITHOUT field names
            //
            // From IDA DeserializeContainer:
            // 1. ReadTypeInfos is called ONCE before the element loop
            // 2. DeserializeInternal is called for EACH element, reading raw data

            // Find the first m_PieceSets pattern which marks the type info section
            byte[] pieceSetsPattern = { 0x0B, 0x6D, 0x5F, 0x50, 0x69, 0x65, 0x63, 0x65, 0x53, 0x65, 0x74, 0x73 };
            long typeInfoStart = FindNextChoiceStart(reader, dataStart, Math.Min(dataStart + 50000, _streamLength));

            if (typeInfoStart < 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Could not find m_PieceSets in data section");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] Found type info at 0x{typeInfoStart:X}");

            // Skip the type info section by finding the m_Flags field (last field in type info)
            // Type info structure for each field: length(1) + name(N) + type_info(16)
            // Fields in order: m_PieceSets, m_RangeIndexes, m_Channels, m_Tags, m_Flags

            reader.BaseStream.Position = typeInfoStart;
            long typeInfoEnd = SkipTypeInfoSection(reader);

            if (typeInfoEnd < 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Failed to find end of type info section");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] Type info section ends at 0x{typeInfoEnd:X}");

            // Now parse elements as raw data
            // Each C_Choice element (without field names):
            // - m_PieceSets: count(4) + uint32 array
            // - m_RangeIndexes: count(4) + ushort array
            // - m_Channels: count(4) + uint32 array
            // - m_Tags: count(4) + map entries (complex)
            // - m_Flags: uint32(4)

            reader.BaseStream.Position = typeInfoEnd;
            int parsed = 0;
            int failures = 0;

            // GENR format for C_OwningPtr<C_Choice> elements:
            // Each element has:
            // - 12 bytes header (element reference/ID, often zeros)
            // - 4 bytes type hash (TYPE_C_CHOICE = 0x33BA57D7)
            // - Then actual C_Choice field data
            //
            // We'll locate elements by searching for the TYPE_C_CHOICE marker

            byte[] choiceTypeMarker = BitConverter.GetBytes(TYPE_C_CHOICE); // 0x33BA57D7

            // Find first TYPE_C_CHOICE marker after type info section
            long searchStart = typeInfoEnd;
            long firstMarker = FindTypeMarker(reader, searchStart, TYPE_C_CHOICE, 500);

            if (firstMarker < 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Could not find TYPE_C_CHOICE marker after type info");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] Found first TYPE_C_CHOICE marker at 0x{firstMarker:X}");

            // Calculate element header size (bytes before the type marker)
            long headerSize = firstMarker - typeInfoEnd;
            System.Diagnostics.Debug.WriteLine($"[GENR] Element header size: {headerSize} bytes");

            while (parsed < count && failures < 100 && reader.BaseStream.Position + 20 < _streamLength)
            {
                long elemStart = reader.BaseStream.Position;

                // Find the TYPE_C_CHOICE marker for this element
                // Use larger search range after failures since elements with many tags can be far apart
                int searchRange = failures > 0 ? 2000 : 200;
                long typeMarkerPos = FindTypeMarker(reader, elemStart, TYPE_C_CHOICE, searchRange);

                if (typeMarkerPos < 0)
                {
                    failures++;
                    if (failures <= 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] No TYPE_C_CHOICE marker found near 0x{elemStart:X} (range={searchRange})");
                    reader.BaseStream.Position = elemStart + 4;
                    continue;
                }

                // Position reader right after the type marker (4 bytes)
                reader.BaseStream.Position = typeMarkerPos + 4;
                long elemDataStart = reader.BaseStream.Position;

                var choice = ParseChoiceRaw(reader);

                if (choice != null)
                {
                    choice.Ref = $"Choice_{parsed}";
                    SharedChoices.Add(choice);
                    parsed++;
                    failures = 0;

                    if (parsed <= 3 || parsed % 1000 == 0)
                        System.Diagnostics.Debug.WriteLine($"[GENR] Parsed choice {parsed} at 0x{elemStart:X}, now at 0x{reader.BaseStream.Position:X}");
                }
                else
                {
                    failures++;
                    // Always log failures when we've parsed many choices (late failures)
                    bool isLateFailure = parsed > 5000;
                    if (failures <= 5 || isLateFailure)
                    {
                        System.Diagnostics.Debug.WriteLine($"[GENR] Failed to parse choice at 0x{elemStart:X}, failures={failures}, parsed={parsed}");

                        // Dump raw bytes at failure location - use 160 bytes for more context
                        long savedDbg = reader.BaseStream.Position;
                        reader.BaseStream.Position = elemStart;
                        byte[] failBytes = new byte[Math.Min(160, _streamLength - elemStart)];
                        int read = reader.Read(failBytes, 0, failBytes.Length);
                        reader.BaseStream.Position = savedDbg;
                        System.Diagnostics.Debug.WriteLine($"[GENR] Bytes at failure 0x{elemStart:X}:");
                        for (int row = 0; row < 4 && row * 40 < read; row++)
                        {
                            int rowStart = row * 40;
                            int rowLen = Math.Min(40, read - rowStart);
                            System.Diagnostics.Debug.WriteLine($"  +{rowStart:X2}: {BitConverter.ToString(failBytes, rowStart, rowLen).Replace("-", " ")}");
                        }
                    }

                    // Try to recover by searching for next valid element
                    long recovered = TryRecoverNextElement(reader, elemStart);
                    if (recovered > 0)
                    {
                        reader.BaseStream.Position = recovered;
                        failures--; // Don't count recovery as failure
                    }
                    else
                    {
                        reader.BaseStream.Position = elemStart + 4;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] ParseChoicesArray complete: {parsed} choices (expected {count})");
        }

        /// <summary>
        /// Find a type marker (4-byte type hash) in the binary stream.
        /// </summary>
        private long FindTypeMarker(BinaryReader reader, long searchStart, uint typeHash, int maxSearch)
        {
            byte[] pattern = BitConverter.GetBytes(typeHash);
            long savedPos = reader.BaseStream.Position;

            for (long pos = searchStart; pos < Math.Min(searchStart + maxSearch, _streamLength - 4); pos++)
            {
                reader.BaseStream.Position = pos;
                bool match = true;
                for (int i = 0; i < 4 && match; i++)
                {
                    if (reader.ReadByte() != pattern[i])
                        match = false;
                }
                if (match)
                {
                    reader.BaseStream.Position = savedPos;
                    return pos;
                }
            }

            reader.BaseStream.Position = savedPos;
            return -1;
        }

        /// <summary>
        /// Find a field header with format=4 and specified type hash.
        /// Returns the position of the format field (start of the 12-byte header).
        /// </summary>
        private long FindTypeMarkerWithFormat(BinaryReader reader, long searchStart, uint typeHash, int maxSearch)
        {
            long savedPos = reader.BaseStream.Position;

            for (long pos = searchStart; pos < Math.Min(searchStart + maxSearch, _streamLength - 12); pos++)
            {
                reader.BaseStream.Position = pos;
                uint format = reader.ReadUInt32();
                uint type = reader.ReadUInt32();

                // Look for format=4 followed by the type hash
                if (format == 4 && type == typeHash)
                {
                    reader.BaseStream.Position = savedPos;
                    return pos;
                }
            }

            reader.BaseStream.Position = savedPos;
            return -1;
        }

        /// <summary>
        /// Try to recover by finding the next valid element start position.
        /// Looks for patterns that indicate a valid C_Choice element.
        /// </summary>
        private long TryRecoverNextElement(BinaryReader reader, long fromPos)
        {
            // Look for a position where we have small counts for the first 3 fields
            // Pattern: small_count + data + small_count + data + small_count + data + small_count + small_data + uint32
            const int maxSearch = 500;

            for (long pos = fromPos + 1; pos < Math.Min(fromPos + maxSearch, _streamLength - 20); pos++)
            {
                reader.BaseStream.Position = pos;

                uint count1 = reader.ReadUInt32();
                if (count1 > 50) continue;

                // Skip pieceSets data
                long afterPieceSets = reader.BaseStream.Position + (count1 * 4);
                if (afterPieceSets + 16 > _streamLength) continue;

                reader.BaseStream.Position = afterPieceSets;
                uint count2 = reader.ReadUInt32();
                if (count2 > 100) continue;

                // Skip rangeIndexes data (each ushort is 2 bytes)
                long afterRangeIndexes = reader.BaseStream.Position + (count2 * 2);
                if (afterRangeIndexes + 12 > _streamLength) continue;

                reader.BaseStream.Position = afterRangeIndexes;
                uint count3 = reader.ReadUInt32();
                if (count3 > 50) continue;

                // Skip channels data
                long afterChannels = reader.BaseStream.Position + (count3 * 4);
                if (afterChannels + 8 > _streamLength) continue;

                reader.BaseStream.Position = afterChannels;
                uint count4 = reader.ReadUInt32();
                if (count4 > 20) continue;

                // This looks like a valid element start
                System.Diagnostics.Debug.WriteLine($"[GENR] Recovery found potential element at 0x{pos:X} (counts: {count1}, {count2}, {count3}, {count4})");
                return pos;
            }

            return -1;
        }

        /// <summary>
        /// Skip the type info section and return the position where element data starts.
        /// Based on IDA analysis of ReadTypeInfos:
        /// - Type info entries are read in a loop
        /// - Each entry has: token(4) before reading, then name_len(1) + name(N) + type_data(~13 bytes)
        /// - Token 0 = continue, Token 1 = end of type infos
        /// - After the last field's type info, there's a token 0x01 marking the end
        /// </summary>
        private long SkipTypeInfoSection(BinaryReader reader)
        {
            // Find the last field name "m_Flags" in the type info section
            byte[] mFlagsPattern = { 0x07, 0x6D, 0x5F, 0x46, 0x6C, 0x61, 0x67, 0x73 }; // 0x07 "m_Flags"
            long startPos = reader.BaseStream.Position;
            long mFlagsPos = -1;

            // Search for m_Flags pattern (it should be the last field)
            for (long pos = startPos; pos < Math.Min(startPos + 2000, _streamLength - mFlagsPattern.Length); pos++)
            {
                reader.BaseStream.Position = pos;
                bool match = true;
                for (int i = 0; i < mFlagsPattern.Length && match; i++)
                {
                    if (reader.ReadByte() != mFlagsPattern[i])
                        match = false;
                }
                if (match)
                {
                    mFlagsPos = pos;
                    System.Diagnostics.Debug.WriteLine($"[GENR] Found m_Flags at 0x{mFlagsPos:X}");
                    break;
                }
            }

            if (mFlagsPos < 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Could not find m_Flags in type info section");
                return -1;
            }

            // Position after "m_Flags" string (length byte + 7 chars)
            long afterMFlags = mFlagsPos + mFlagsPattern.Length;
            reader.BaseStream.Position = afterMFlags;

            // Dump bytes after m_Flags to understand the structure
            if (afterMFlags + 100 < _streamLength)
            {
                byte[] postFlags = new byte[100];
                reader.Read(postFlags, 0, 100);
                reader.BaseStream.Position = afterMFlags;

                System.Diagnostics.Debug.WriteLine($"[GENR] Bytes after m_Flags at 0x{afterMFlags:X}:");
                for (int row = 0; row < 4; row++)
                {
                    int off = row * 25;
                    StringBuilder sb = new StringBuilder($"  +{off:X2}: ");
                    for (int i = 0; i < 25 && off + i < 100; i++)
                    {
                        sb.Append($"{postFlags[off + i]:X2} ");
                    }
                    System.Diagnostics.Debug.WriteLine(sb.ToString());
                }

                // From IDA analysis:
                // After "m_Flags" name, there's type info data:
                // - 8 bytes (hash check data)
                // - 4 bytes (more type data)
                // Then comes the end token (0x01000000 = 1 as uint32)
                // Then element data starts

                // Search for end token (0x01) followed by reasonable element data
                for (int testOff = 8; testOff < 60; testOff++)
                {
                    if (testOff + 8 < 100)
                    {
                        uint token = BitConverter.ToUInt32(postFlags, testOff);

                        // Token 0x01 marks end of type infos
                        if (token == 1)
                        {
                            // Element data starts after the token (4 bytes)
                            long elementStart = afterMFlags + testOff + 4;

                            // Validate: next uint32 should be first field count (m_PieceSets)
                            if (testOff + 8 < 100)
                            {
                                uint firstCount = BitConverter.ToUInt32(postFlags, testOff + 4);
                                if (firstCount <= 100)
                                {
                                    System.Diagnostics.Debug.WriteLine($"[GENR] Found end token 0x01 at +{testOff}, element start at 0x{elementStart:X}, first count={firstCount}");
                                    return elementStart;
                                }
                            }
                        }
                    }
                }

                // Fallback: try to find first reasonable element by pattern matching
                // Look for small counts that could be field arrays
                for (int testOff = 12; testOff < 80; testOff += 4)
                {
                    if (testOff + 20 < 100)
                    {
                        uint count1 = BitConverter.ToUInt32(postFlags, testOff);

                        // First count (m_PieceSets) should be small
                        if (count1 <= 50)
                        {
                            // After first array, check second count (m_RangeIndexes)
                            int nextOffset = testOff + 4 + (int)(count1 * 4);
                            if (nextOffset + 4 < 100)
                            {
                                uint count2 = BitConverter.ToUInt32(postFlags, nextOffset);
                                // Second count should also be small
                                if (count2 <= 100)
                                {
                                    long elementStart = afterMFlags + testOff;
                                    System.Diagnostics.Debug.WriteLine($"[GENR] Pattern match: element start at +{testOff} (0x{elementStart:X}), counts={count1},{count2}");
                                    return elementStart;
                                }
                            }
                        }
                    }
                }
            }

            // Fallback: skip a fixed amount after m_Flags
            // Type info for uint32: ~13 bytes, then end token: 4 bytes
            long fallback = afterMFlags + 17;
            System.Diagnostics.Debug.WriteLine($"[GENR] Using fallback position: 0x{fallback:X}");
            return fallback;
        }

        /// <summary>
        /// Parse a C_Choice from raw data (without field names).
        /// </summary>
        private CCDBChoice ParseChoiceRaw(BinaryReader reader)
        {
            if (reader.BaseStream.Position + 20 > _streamLength)
                return null;

            long choiceStart = reader.BaseStream.Position;

            try
            {
                var choice = new CCDBChoice();
                choice.PieceSets = new List<uint>();
                choice.RangeIndexes = new List<ushort>();
                choice.Channels = new List<uint>();
                choice.Tags = new List<CCDBTag>();

                // Dump bytes for first few choices
                if (_choiceDebugCount < 5)
                {
                    byte[] preview = new byte[80];
                    long savedPos = reader.BaseStream.Position;
                    int bytesRead = reader.Read(preview, 0, 80);
                    reader.BaseStream.Position = savedPos;
                    System.Diagnostics.Debug.WriteLine($"[GENR] Raw choice #{_choiceDebugCount} at 0x{choiceStart:X}:");
                    System.Diagnostics.Debug.WriteLine($"  Row 0: {BitConverter.ToString(preview, 0, Math.Min(bytesRead, 40)).Replace("-", " ")}");
                    if (bytesRead > 40)
                        System.Diagnostics.Debug.WriteLine($"  Row 1: {BitConverter.ToString(preview, 40, Math.Min(bytesRead - 40, 40)).Replace("-", " ")}");
                }

                // GENR vector field format: [flags:4] [type_hash:4] [size:4] [count:4] [elements]
                // Each field has a 12-byte header before the count+elements

                // m_PieceSets: vector<uint32>
                long posPieceSets = reader.BaseStream.Position;
                uint pieceSetsFlags = reader.ReadUInt32();
                uint pieceSetsType = reader.ReadUInt32();
                uint pieceSetsSize = reader.ReadUInt32();
                uint pieceSetsCount = reader.ReadUInt32();

                if (_choiceDebugCount < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   @0x{posPieceSets:X}: pieceSets flags={pieceSetsFlags} type=0x{pieceSetsType:X8} size={pieceSetsSize} count={pieceSetsCount}");

                if (pieceSetsCount > 500 || pieceSetsType != 0xC2725761) // TYPE_PIECESET
                {
                    // Check if we hit an element header with format != 4
                    // Format types from IDA analysis:
                    // - format=2: skip 8+4=12 bytes after format field
                    // - format=4: standard value header
                    // - format=0: type info header follows

                    // Always log format-2 detection for debugging
                    if (pieceSetsFlags == 2)
                        System.Diagnostics.Debug.WriteLine($"[GENR] Detected format=2 at 0x{choiceStart:X}, pieceSetsType=0x{pieceSetsType:X8}");

                    if (pieceSetsFlags == 2)
                    {
                        // Format 2: skip remaining header (we read 16 bytes already, but format-2 has 16 byte header)
                        // Total format-2 header: [format:4][8 bytes][4 bytes] = 16 bytes, then actual data
                        long newStart = choiceStart + 16;
                        System.Diagnostics.Debug.WriteLine($"[GENR]   Format=2 element header at 0x{choiceStart:X}, adjusting start to 0x{newStart:X}");

                        reader.BaseStream.Position = newStart;
                        pieceSetsFlags = reader.ReadUInt32();
                        pieceSetsType = reader.ReadUInt32();
                        pieceSetsSize = reader.ReadUInt32();
                        pieceSetsCount = reader.ReadUInt32();

                        System.Diagnostics.Debug.WriteLine($"[GENR]   Adjusted @0x{newStart:X}: pieceSets flags={pieceSetsFlags} type=0x{pieceSetsType:X8} size={pieceSetsSize} count={pieceSetsCount}");

                        // Validate again
                        if (pieceSetsCount > 500 || pieceSetsType != 0xC2725761)
                        {
                            System.Diagnostics.Debug.WriteLine($"[GENR] pieceSets still invalid after format-2 adjustment");
                            reader.BaseStream.Position = choiceStart;
                            _choiceDebugCount++;
                            return null;
                        }
                    }
                    else
                    {
                        bool isLate = SharedChoices.Count > 5000;
                        if (_choiceDebugCount < 5 || isLate)
                            System.Diagnostics.Debug.WriteLine($"[GENR] pieceSets invalid at 0x{posPieceSets:X}: flags={pieceSetsFlags} count={pieceSetsCount} type=0x{pieceSetsType:X8} size={pieceSetsSize} (parsed={SharedChoices.Count})");
                        reader.BaseStream.Position = choiceStart;
                        _choiceDebugCount++;
                        return null;
                    }
                }

                for (int i = 0; i < pieceSetsCount; i++)
                    choice.PieceSets.Add(reader.ReadUInt32());

                // m_RangeIndexes: vector<unsigned short>
                long posRangeIdx = reader.BaseStream.Position;
                uint rangeIdxFlags = reader.ReadUInt32();
                uint rangeIdxType = reader.ReadUInt32();
                uint rangeIdxSize = reader.ReadUInt32();

                if (_choiceDebugCount < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   @0x{posRangeIdx:X}: rangeIdx flags={rangeIdxFlags} type=0x{rangeIdxType:X8} size={rangeIdxSize}");

                // Handle format=0 (empty/null field) or invalid header
                // When format=0 or type doesn't match, try to find next valid header
                if (rangeIdxFlags == 0 || rangeIdxType != 0x73517674 || rangeIdxSize > 10000)
                {
                    if (_choiceDebugCount < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   rangeIdx has unusual format, scanning for next field...");

                    // Seek back to start of suspected header and scan for TYPE_PIECESET (channels)
                    reader.BaseStream.Position = posRangeIdx;
                    long channelsPos = FindTypeMarkerWithFormat(reader, posRangeIdx, 0xC2725761, 200);

                    if (channelsPos > posRangeIdx)
                    {
                        // Found channels, rangeIdx is empty
                        reader.BaseStream.Position = channelsPos;
                        if (_choiceDebugCount < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Found channels at 0x{channelsPos:X}, treating rangeIdx as empty");
                    }
                    else
                    {
                        // Can't find channels, fail
                        bool isLate = SharedChoices.Count > 5000;
                        if (_choiceDebugCount < 5 || isLate)
                            System.Diagnostics.Debug.WriteLine($"[GENR] rangeIdx invalid at 0x{posRangeIdx:X} and no channels found (flags={rangeIdxFlags} type=0x{rangeIdxType:X8} size={rangeIdxSize}, parsed={SharedChoices.Count})");
                        reader.BaseStream.Position = choiceStart;
                        _choiceDebugCount++;
                        return null;
                    }
                }
                else
                {
                    // Normal rangeIdx field - read the data
                    uint rangeIndexCount = reader.ReadUInt32();

                    if (_choiceDebugCount < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   rangeIdx count={rangeIndexCount}");

                    if (rangeIndexCount > 500)
                    {
                        bool isLate = SharedChoices.Count > 5000;
                        if (_choiceDebugCount < 5 || isLate)
                            System.Diagnostics.Debug.WriteLine($"[GENR] rangeIdx count={rangeIndexCount} too large at 0x{posRangeIdx:X} (parsed={SharedChoices.Count})");
                        reader.BaseStream.Position = choiceStart;
                        _choiceDebugCount++;
                        return null;
                    }

                    // Read ushort elements - stored as 2 bytes each (no padding)
                    // Use size field to determine actual data bytes: size - 4 (for count) = element bytes
                    uint rangeIdxDataBytes = rangeIdxSize - 4;
                    uint actualRangeCount = rangeIdxDataBytes / 2;
                    if (actualRangeCount != rangeIndexCount && _choiceDebugCount < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   rangeIdx count mismatch: header says {rangeIndexCount}, size says {actualRangeCount}");

                    for (int i = 0; i < actualRangeCount; i++)
                    {
                        ushort value = reader.ReadUInt16();
                        choice.RangeIndexes.Add(value);
                    }
                }

                // m_Channels: vector<uint32> - uses TYPE_PIECESET (0xC2725761)
                long posChannels = reader.BaseStream.Position;
                uint channelsFlags = reader.ReadUInt32();
                uint channelsType = reader.ReadUInt32();
                uint channelsSize = reader.ReadUInt32();
                uint channelsCount = reader.ReadUInt32();

                if (_choiceDebugCount < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   @0x{posChannels:X}: channels flags={channelsFlags} type=0x{channelsType:X8} size={channelsSize} count={channelsCount}");

                if (channelsCount > 500 || channelsType != 0xC2725761) // TYPE_PIECESET for channels
                {
                    bool isLate = SharedChoices.Count > 5000;
                    if (_choiceDebugCount < 5 || isLate)
                        System.Diagnostics.Debug.WriteLine($"[GENR] channels invalid at 0x{posChannels:X}: count={channelsCount} type=0x{channelsType:X8} flags={channelsFlags} size={channelsSize} (parsed={SharedChoices.Count})");
                    reader.BaseStream.Position = choiceStart;
                    _choiceDebugCount++;
                    return null;
                }

                for (int i = 0; i < channelsCount; i++)
                {
                    choice.Channels.Add(reader.ReadUInt32());
                }

                if (_choiceDebugCount < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   After channels: pos=0x{reader.BaseStream.Position:X}");

                // m_Tags: count + map entries
                // Tag format is complex - map<C_HashNameString, C_TagValueList>
                // For elements with tags, we'll find m_Flags by scanning backwards from next TYPE_C_CHOICE
                long posTags = reader.BaseStream.Position;
                uint tagsCount = reader.ReadUInt32();
                if (_choiceDebugCount < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   @0x{posTags:X}: tagsCount={tagsCount}");

                if (tagsCount > 100)
                {
                    bool isLate = SharedChoices.Count > 5000;
                    if (_choiceDebugCount < 5 || isLate)
                        System.Diagnostics.Debug.WriteLine($"[GENR] tagsCount={tagsCount} too large at 0x{posTags:X} (parsed={SharedChoices.Count})");
                    reader.BaseStream.Position = choiceStart;
                    _choiceDebugCount++;
                    return null;
                }

                if (tagsCount > 0)
                {
                    if (_choiceDebugCount < 3)
                    {
                        // Dump bytes at tag start for debugging
                        long tagStartPos = reader.BaseStream.Position;
                        byte[] tagPreview = new byte[60];
                        int bytesRead = reader.Read(tagPreview, 0, 60);
                        reader.BaseStream.Position = tagStartPos;
                        System.Diagnostics.Debug.WriteLine($"[GENR] Tags section ({tagsCount} tags) at 0x{tagStartPos:X}:");
                        System.Diagnostics.Debug.WriteLine($"  {BitConverter.ToString(tagPreview, 0, Math.Min(bytesRead, 60)).Replace("-", " ")}");
                    }

                    // Skip tags by finding the next TYPE_C_CHOICE marker and backing up 4 bytes for m_Flags
                    long tagStart = reader.BaseStream.Position;
                    long nextChoice = FindTypeMarker(reader, tagStart, TYPE_C_CHOICE, 2000);

                    if (nextChoice > tagStart)
                    {
                        // The element header before TYPE_C_CHOICE is typically 12 bytes
                        // So m_Flags should be at (nextChoice - 12 - 4)
                        // But to be safe, let's read the 4 bytes just before the element header
                        long flagsPos = nextChoice - 16; // 12 bytes header + 4 bytes for flags

                        if (flagsPos > tagStart)
                        {
                            reader.BaseStream.Position = flagsPos;
                            choice.Flags = reader.ReadUInt32();

                            if (_choiceDebugCount < 3)
                                System.Diagnostics.Debug.WriteLine($"[GENR] Skipped tags, found flags=0x{choice.Flags:X} at 0x{flagsPos:X}");

                            // Position at the start of next element's header
                            reader.BaseStream.Position = nextChoice - 12;
                            _choiceDebugCount++;
                            return choice;
                        }
                    }

                    // Fallback: For the last element(s) where no next TYPE_C_CHOICE exists
                    // The tag structure is complex and the simplified parsing doesn't work.
                    // Instead, try to find m_Flags using size-based calculation or accept default.

                    bool isLateChoice = SharedChoices.Count > 5000;
                    if (isLateChoice)
                    {
                        System.Diagnostics.Debug.WriteLine($"[GENR] Last choice fallback: no next TYPE_C_CHOICE marker found, tags={tagsCount} at 0x{tagStart:X}");

                        // For the last choice, try to estimate tag section size using the pattern:
                        // Each tag entry appears to have variable size based on a size field
                        // Look for a known pattern that indicates end of tags (e.g., m_Flags is often 0x80000000 or 0x00000000)

                        // Search for a reasonable flags value after tags
                        // Tags typically end with m_Flags which is often 0x80000000 or small values
                        long searchStart = tagStart;
                        long searchEnd = Math.Min(tagStart + 500, _streamLength - 4);
                        bool foundFlags = false;

                        for (long pos = searchStart; pos < searchEnd; pos += 4)
                        {
                            reader.BaseStream.Position = pos;
                            uint potentialFlags = reader.ReadUInt32();

                            // m_Flags for choices is typically 0x80000000 or 0x00000000
                            if (potentialFlags == 0x80000000 || potentialFlags == 0x00000000)
                            {
                                // Verify this looks like end of tags by checking what follows
                                // After the last choice, there should be end-of-section data or next section
                                if (reader.BaseStream.Position + 4 <= _streamLength)
                                {
                                    uint nextValue = reader.ReadUInt32();
                                    // If next value is a reasonable pattern (not another tag hash), this might be flags
                                    if (nextValue < 0x10000 || nextValue == 0 || (nextValue & 0xFF000000) != 0)
                                    {
                                        choice.Flags = potentialFlags;
                                        System.Diagnostics.Debug.WriteLine($"[GENR] Last choice: found flags=0x{potentialFlags:X} at 0x{pos:X}");
                                        foundFlags = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!foundFlags)
                        {
                            // Accept with default flags if we can't find them
                            choice.Flags = 0x80000000; // Common default
                            System.Diagnostics.Debug.WriteLine($"[GENR] Last choice: using default flags=0x80000000");
                        }

                        // Position reader at end of data for this choice
                        reader.BaseStream.Position = _streamLength;
                        _choiceDebugCount++;
                        return choice;
                    }

                    // Original fallback for early choices (should rarely hit)
                    for (int i = 0; i < tagsCount; i++)
                    {
                        if (reader.BaseStream.Position + 12 > _streamLength)
                        {
                            if (_choiceDebugCount < 5)
                                System.Diagnostics.Debug.WriteLine($"[GENR] Tags overflow at tag {i}/{tagsCount}, pos=0x{reader.BaseStream.Position:X} (parsed={SharedChoices.Count})");
                            reader.BaseStream.Position = choiceStart;
                            _choiceDebugCount++;
                            return null;
                        }

                        // C_HashNameString: just the hash (8 bytes)
                        reader.ReadUInt64();

                        // C_TagValueList: count + values (each value is 8 bytes hash/value)
                        uint valueCount = reader.ReadUInt32();
                        if (valueCount > 100)
                        {
                            if (_choiceDebugCount < 3)
                                System.Diagnostics.Debug.WriteLine($"[GENR] Tag valueCount={valueCount} too large at pos 0x{reader.BaseStream.Position:X} (parsed={SharedChoices.Count})");
                            reader.BaseStream.Position = choiceStart;
                            _choiceDebugCount++;
                            return null;
                        }

                        // Skip values (assume 8 bytes each for hash-based values)
                        reader.BaseStream.Position += valueCount * 8;

                        if (reader.BaseStream.Position > _streamLength)
                        {
                            if (_choiceDebugCount < 5)
                                System.Diagnostics.Debug.WriteLine($"[GENR] Tag values overflow at tag {i}, valueCount={valueCount}, pos=0x{reader.BaseStream.Position:X} (parsed={SharedChoices.Count})");
                            reader.BaseStream.Position = choiceStart;
                            _choiceDebugCount++;
                            return null;
                        }
                    }
                }

                // m_Flags: uint32
                choice.Flags = reader.ReadUInt32();

                if (_choiceDebugCount < 3)
                {
                    System.Diagnostics.Debug.WriteLine($"[GENR] Parsed choice #{_choiceDebugCount}: " +
                        $"pieceSets={pieceSetsCount}, rangeIndexes={choice.RangeIndexes.Count}, " +
                        $"channels={channelsCount}, tags={tagsCount}, flags=0x{choice.Flags:X}");
                }

                _choiceDebugCount++;
                return choice;
            }
            catch (Exception ex)
            {
                bool isLate = SharedChoices.Count > 5000;
                if (_choiceDebugCount < 5 || isLate)
                    System.Diagnostics.Debug.WriteLine($"[GENR] ParseChoiceRaw exception at 0x{choiceStart:X}: {ex.Message} (parsed={SharedChoices.Count})");
                reader.BaseStream.Position = choiceStart;
                _choiceDebugCount++;
                return null;
            }
        }

        /// <summary>
        /// Skip the type info section to find where element data starts.
        /// </summary>
        private long SkipToFirstElement(BinaryReader reader, long dataStart, byte[] typeHash)
        {
            // Read ahead to understand the structure
            reader.BaseStream.Position = dataStart;

            // Dump first 100 bytes for debugging
            if (_choiceDebugCount < 1 && dataStart + 100 < _streamLength)
            {
                byte[] preview = new byte[100];
                reader.Read(preview, 0, 100);
                reader.BaseStream.Position = dataStart;

                System.Diagnostics.Debug.WriteLine($"[GENR] First 100 bytes after count at 0x{dataStart:X}:");
                for (int row = 0; row < 4; row++)
                {
                    int off = row * 25;
                    StringBuilder sb = new StringBuilder($"  +{off:X2}: ");
                    for (int i = 0; i < 25 && off + i < 100; i++)
                    {
                        sb.Append($"{preview[off + i]:X2} ");
                    }
                    System.Diagnostics.Debug.WriteLine(sb.ToString());
                }
            }

            // Search for the first occurrence of m_PieceSets field name
            // which marks the start of actual C_Choice data
            byte[] pieceSetsPattern = { 0x0B, 0x6D, 0x5F, 0x50, 0x69, 0x65, 0x63, 0x65, 0x53, 0x65, 0x74, 0x73 };

            for (long pos = dataStart; pos < Math.Min(dataStart + 50000, _streamLength - pieceSetsPattern.Length); pos++)
            {
                reader.BaseStream.Position = pos;
                bool match = true;
                for (int i = 0; i < pieceSetsPattern.Length && match; i++)
                {
                    if (reader.ReadByte() != pieceSetsPattern[i])
                        match = false;
                }
                if (match)
                {
                    System.Diagnostics.Debug.WriteLine($"[GENR] Found first m_PieceSets at 0x{pos:X}");
                    return pos;
                }
            }

            return -1;
        }

        /// <summary>
        /// Find the next choice start position by looking for m_PieceSets field name.
        /// Uses buffered reading for efficiency.
        /// </summary>
        private long FindNextChoiceStart(BinaryReader reader, long searchStart, long searchEnd)
        {
            // Search for m_PieceSets pattern which marks start of each C_Choice
            byte[] pieceSetsPattern = { 0x0B, 0x6D, 0x5F, 0x50, 0x69, 0x65, 0x63, 0x65, 0x53, 0x65, 0x74, 0x73 };

            // Read in chunks for efficiency
            const int chunkSize = 65536;
            byte[] buffer = new byte[chunkSize + pieceSetsPattern.Length];

            long pos = searchStart;
            while (pos < searchEnd)
            {
                reader.BaseStream.Position = pos;
                int bytesToRead = (int)Math.Min(chunkSize + pieceSetsPattern.Length, searchEnd - pos);
                int bytesRead = reader.Read(buffer, 0, bytesToRead);

                if (bytesRead < pieceSetsPattern.Length)
                    break;

                // Search within buffer
                for (int i = 0; i <= bytesRead - pieceSetsPattern.Length; i++)
                {
                    bool match = true;
                    for (int j = 0; j < pieceSetsPattern.Length && match; j++)
                    {
                        if (buffer[i + j] != pieceSetsPattern[j])
                            match = false;
                    }
                    if (match)
                    {
                        return pos + i;
                    }
                }

                // Move to next chunk, overlapping by pattern length to avoid missing patterns at boundaries
                pos += chunkSize;
            }

            return -1;
        }

        /// <summary>
        /// Parse a C_Choice from the current position (starting at m_PieceSets field).
        /// </summary>
        private CCDBChoice ParseChoiceFromPosition(BinaryReader reader)
        {
            return ParseChoiceFields(reader);
        }

        private int _choiceDebugCount = 0;

        /// <summary>
        /// Search ahead for a field name within a limited range.
        /// Returns the position after the field name, or -1 if not found.
        /// </summary>
        private long SearchForFieldName(BinaryReader reader, string fieldName, int maxSearch = 100)
        {
            long startPos = reader.BaseStream.Position;
            byte expectedLen = (byte)fieldName.Length;
            byte[] expectedBytes = Encoding.ASCII.GetBytes(fieldName);

            for (int offset = 0; offset < maxSearch && startPos + offset + fieldName.Length + 1 < _streamLength; offset++)
            {
                reader.BaseStream.Position = startPos + offset;
                byte len = reader.ReadByte();
                if (len == expectedLen)
                {
                    bool match = true;
                    for (int i = 0; i < len && match; i++)
                    {
                        if (reader.ReadByte() != expectedBytes[i])
                            match = false;
                    }
                    if (match)
                    {
                        return reader.BaseStream.Position; // Position after field name
                    }
                }
            }

            reader.BaseStream.Position = startPos;
            return -1;
        }

        /// <summary>
        /// Check if the next bytes look like a field name (length byte followed by "m_")
        /// </summary>
        private bool IsFieldNameNext(BinaryReader reader)
        {
            long pos = reader.BaseStream.Position;
            if (pos + 3 >= _streamLength) return false;

            byte len = reader.ReadByte();
            byte m = reader.ReadByte();
            byte underscore = reader.ReadByte();
            reader.BaseStream.Position = pos;

            // Field names start with "m_" and have reasonable length
            return len >= 5 && len <= 30 && m == 0x6D && underscore == 0x5F;
        }

        /// <summary>
        /// Read a GENR vector field: type info + count, but data may be stored elsewhere.
        /// Returns the count value (for reference purposes).
        /// </summary>
        private uint ReadVectorFieldHeader(BinaryReader reader)
        {
            SkipTypeInfo(reader, 16);
            uint countOrRef = reader.ReadUInt32();

            // In GENR, if the next field name follows immediately, the "count" is actually
            // a reference/offset and data is stored elsewhere. Return 0 for inline data count.
            if (IsFieldNameNext(reader))
            {
                return 0; // No inline data
            }

            return countOrRef;
        }

        private CCDBChoice ParseChoiceFields(BinaryReader reader)
        {
            try
            {
                long startPos = reader.BaseStream.Position;
                var choice = new CCDBChoice();
                choice.PieceSets = new List<uint>();
                choice.RangeIndexes = new List<ushort>();
                choice.Channels = new List<uint>();
                choice.Tags = new List<CCDBTag>();

                // Diagnostic: dump bytes at start position for first few attempts
                if (_choiceDebugCount < 3)
                {
                    byte[] preview = new byte[64];
                    long savedPos = reader.BaseStream.Position;
                    int bytesRead = reader.Read(preview, 0, 64);
                    reader.BaseStream.Position = savedPos;

                    System.Diagnostics.Debug.WriteLine($"[GENR] Choice #{_choiceDebugCount} at 0x{startPos:X}:");
                    System.Diagnostics.Debug.WriteLine($"  Bytes: {BitConverter.ToString(preview, 0, Math.Min(bytesRead, 64)).Replace("-", " ")}");
                }

                // We're positioned at the m_PieceSets field name (0x0B "m_PieceSets")
                // Read and verify the field name
                byte len = reader.ReadByte();
                if (len != 11) // "m_PieceSets" is 11 chars
                {
                    if (_choiceDebugCount < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] ParseChoiceFields: Expected m_PieceSets length 11, got {len} at 0x{startPos:X}");
                    _choiceDebugCount++;
                    return null;
                }

                byte[] nameBytes = reader.ReadBytes(len);
                string fieldName = Encoding.ASCII.GetString(nameBytes);
                if (fieldName != "m_PieceSets")
                {
                    if (_choiceDebugCount < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] ParseChoiceFields: Expected 'm_PieceSets', got '{fieldName}' at 0x{startPos:X}");
                    _choiceDebugCount++;
                    return null;
                }

                // Now read the type info and vector data
                uint pieceSetsCount = ReadVectorFieldHeader(reader);

                if (_choiceDebugCount < 3)
                    System.Diagnostics.Debug.WriteLine($"[GENR] m_PieceSets: pos=0x{reader.BaseStream.Position:X}, inlineCount={pieceSetsCount}");

                if (pieceSetsCount > 0 && pieceSetsCount <= 100)
                {
                    for (int i = 0; i < pieceSetsCount; i++)
                        choice.PieceSets.Add(reader.ReadUInt32());
                }

                // Search for m_RangeIndexes
                long afterRangeIndexes = SearchForFieldName(reader, "m_RangeIndexes", 100);
                if (afterRangeIndexes > 0)
                {
                    reader.BaseStream.Position = afterRangeIndexes;
                    uint rangeCount = ReadVectorFieldHeader(reader);
                    if (rangeCount > 0 && rangeCount <= 100)
                    {
                        for (int i = 0; i < rangeCount; i++)
                            choice.RangeIndexes.Add(reader.ReadUInt16());
                    }
                    if (_choiceDebugCount < 3)
                        System.Diagnostics.Debug.WriteLine($"[GENR] m_RangeIndexes: inlineCount={rangeCount}");
                }

                // Search for m_Channels
                long afterChannels = SearchForFieldName(reader, "m_Channels", 100);
                if (afterChannels > 0)
                {
                    reader.BaseStream.Position = afterChannels;
                    uint channelCount = ReadVectorFieldHeader(reader);
                    if (channelCount > 0 && channelCount <= 100)
                    {
                        for (int i = 0; i < channelCount; i++)
                            choice.Channels.Add(reader.ReadUInt32());
                    }
                    if (_choiceDebugCount < 3)
                        System.Diagnostics.Debug.WriteLine($"[GENR] m_Channels: inlineCount={channelCount}");
                }

                // Search for m_Tags (skip for now, complex map)
                long afterTags = SearchForFieldName(reader, "m_Tags", 100);
                if (afterTags > 0)
                {
                    reader.BaseStream.Position = afterTags;
                    // Skip type info for map - format is more complex
                    SkipTypeInfo(reader, 16);
                    uint tagCount = reader.ReadUInt32();
                    // Skip tag data for now
                    if (_choiceDebugCount < 3)
                        System.Diagnostics.Debug.WriteLine($"[GENR] m_Tags: count={tagCount}");
                }

                // Search for m_Flags
                long afterFlags = SearchForFieldName(reader, "m_Flags", 100);
                if (afterFlags > 0)
                {
                    reader.BaseStream.Position = afterFlags;
                    SkipTypeInfo(reader, 8); // Simpler type info for primitive
                    choice.Flags = reader.ReadUInt32();
                    if (_choiceDebugCount < 3)
                        System.Diagnostics.Debug.WriteLine($"[GENR] m_Flags: {choice.Flags}");
                }
                else
                {
                    choice.Flags = 0;
                }

                // Dump bytes after this choice to see what comes next
                if (_choiceDebugCount < 3)
                {
                    long endPos = reader.BaseStream.Position;
                    System.Diagnostics.Debug.WriteLine($"[GENR] Choice #{_choiceDebugCount} parsed OK, pos now 0x{endPos:X}");

                    // Show next 50 bytes to understand structure between choices
                    if (endPos + 50 < _streamLength)
                    {
                        byte[] nextBytes = new byte[50];
                        reader.Read(nextBytes, 0, 50);
                        reader.BaseStream.Position = endPos;
                        System.Diagnostics.Debug.WriteLine($"[GENR] Next 50 bytes after choice:");
                        System.Diagnostics.Debug.WriteLine($"  {BitConverter.ToString(nextBytes, 0, 50).Replace("-", " ")}");
                    }
                }
                _choiceDebugCount++;

                return choice;
            }
            catch (Exception ex)
            {
                if (_choiceDebugCount < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR] ParseChoiceFields exception: {ex.Message}");
                _choiceDebugCount++;
                return null;
            }
        }

        private bool TryReadFieldName(BinaryReader reader, string expectedName)
        {
            if (reader.BaseStream.Position + expectedName.Length + 1 > _streamLength)
                return false;

            byte len = reader.ReadByte();
            if (len != expectedName.Length)
            {
                reader.BaseStream.Position--; // Put back the byte
                return false;
            }

            byte[] nameBytes = reader.ReadBytes(len);
            string name = Encoding.ASCII.GetString(nameBytes);
            return name == expectedName;
        }

        /// <summary>
        /// Main entry point - deserialize a CCDB binary file.
        /// </summary>
        public bool Deserialize(byte[] data)
        {
            if (data == null || data.Length < 100)
                return false;

            _streamLength = data.Length;

            using (var ms = new MemoryStream(data))
            using (_reader = new BinaryReader(ms))
            {
                // Find GENR magic
                long genrOffset = FindGenrOffset(data);
                if (genrOffset < 0)
                {
                    System.Diagnostics.Debug.WriteLine("[GENR] No GENR magic found");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"[GENR] Found GENR at offset 0x{genrOffset:X}");

                // Parse pre-GENR header
                if (genrOffset > 0)
                {
                    ParsePreGenrHeader(_reader, genrOffset);
                }

                // Parse GENR header
                _reader.BaseStream.Position = genrOffset;
                if (!ParseGenrHeader(_reader))
                    return false;

                // Get expected counts
                int expectedProfiles = GetExpectedCount(TYPE_C_SPAWN_PROFILE);
                int expectedChoices = GetExpectedCount(TYPE_C_CHOICE);

                System.Diagnostics.Debug.WriteLine($"[GENR] Expected: {expectedProfiles} profiles, {expectedChoices} choices");

                // Parse profiles using scan method
                ParseProfilesFromData(data, _reader, expectedProfiles);

                // Parse shared choices
                if (expectedChoices > 0)
                {
                    ParseSharedChoices(data, _reader, expectedChoices);
                }

                // Parse range values - try both type hashes (different game versions use different hashes)
                int expectedRanges = GetExpectedCount(TYPE_C_RANGE_WITH_CHANCE);
                if (expectedRanges == 0)
                    expectedRanges = GetExpectedCount(TYPE_C_RANGE_WITH_CHANCE_ALT);
                if (expectedRanges > 0)
                {
                    ParseRangeValues(data, _reader, expectedRanges);
                }

                // Parse piece sets
                int expectedPieceSets = GetExpectedCount(TYPE_C_PIECE_SET);
                if (expectedPieceSets > 0)
                {
                    ParsePieceSets(data, _reader, expectedPieceSets);
                }

                System.Diagnostics.Debug.WriteLine($"[GENR] Result: {Profiles.Count} profiles, {SharedChoices.Count} choices, {RangeValues.Count} ranges, {PieceSets.Count} piece sets");
                return true;
            }
        }

        private long FindGenrOffset(byte[] data)
        {
            // Search for "GENR" magic
            byte[] pattern = { 0x47, 0x45, 0x4E, 0x52 }; // "GENR"
            for (int i = 0; i < Math.Min(data.Length - 4, 1024); i++)
            {
                if (data[i] == pattern[0] && data[i + 1] == pattern[1] &&
                    data[i + 2] == pattern[2] && data[i + 3] == pattern[3])
                {
                    return i;
                }
            }
            return -1;
        }

        private void ParseProfilesFromData(byte[] data, BinaryReader reader, int expectedCount)
        {
            System.Diagnostics.Debug.WriteLine("[GENR] Parsing profiles...");

            // First try to find m_Profiles field
            long fieldPos = FindFieldByName(data, "m_Profiles", _dataStart);

            HashSet<ulong> seenIds = new HashSet<ulong>();
            int parsed = 0;

            if (fieldPos > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Found m_Profiles at 0x{fieldPos:X}");

                // Search for count near field
                long searchStart = fieldPos + 1 + 10; // Skip name
                if (TryFindVectorStart(data, searchStart, expectedCount, out long vectorStart))
                {
                    reader.BaseStream.Position = vectorStart + 4; // Skip count

                    int failures = 0;
                    while (parsed < expectedCount && failures < 50 && reader.BaseStream.Position + 40 < _streamLength)
                    {
                        long profileStart = reader.BaseStream.Position;
                        var profile = ParseSpawnProfile(reader, seenIds);

                        if (profile != null)
                        {
                            profile.Ref = $"Profile_{parsed}";
                            Profiles.Add(profile);
                            seenIds.Add(profile.Id.Hash);
                            parsed++;
                            failures = 0;

                            if (parsed % 500 == 0)
                                System.Diagnostics.Debug.WriteLine($"[GENR] Parsed {parsed} profiles");
                        }
                        else
                        {
                            failures++;
                            reader.BaseStream.Position = profileStart + 8;
                        }
                    }
                }
            }

            // Fallback: scan entire data section
            if (parsed < expectedCount / 2)
            {
                System.Diagnostics.Debug.WriteLine("[GENR] Field search insufficient, using full scan");
                ScanForProfiles(data, reader, expectedCount, seenIds);
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] Profile parsing complete: {Profiles.Count} profiles");
        }

        private void ScanForProfiles(byte[] data, BinaryReader reader, int expectedCount, HashSet<ulong> seenIds)
        {
            int parsed = Profiles.Count;
            long maxScan = Math.Min(_streamLength - 100, _dataStart + 20 * 1024 * 1024);

            for (long pos = _dataStart; pos < maxScan && parsed < expectedCount; pos += 4)
            {
                reader.BaseStream.Position = pos;

                var profile = ParseSpawnProfile(reader, seenIds);
                if (profile != null)
                {
                    profile.Ref = $"Profile_{parsed}";
                    Profiles.Add(profile);
                    seenIds.Add(profile.Id.Hash);
                    parsed++;

                    pos = reader.BaseStream.Position - 4; // Continue from end of profile

                    if (parsed % 500 == 0)
                        System.Diagnostics.Debug.WriteLine($"[GENR] Scan: {parsed} profiles at 0x{reader.BaseStream.Position:X}");
                }
            }
        }

        /// <summary>
        /// Parse a C_RangeWithChance from the binary stream.
        /// Structure (12 bytes):
        /// - m_MinValue: float at offset 0
        /// - m_MaxValue: float at offset 4
        /// - m_Chance: float at offset 8
        /// </summary>
        private CCDBRange ParseRangeWithChance(BinaryReader reader)
        {
            if (reader.BaseStream.Position + 12 > _streamLength)
                return null;

            float minVal = reader.ReadSingle();
            float maxVal = reader.ReadSingle();
            float chance = reader.ReadSingle();

            // Validate
            if (float.IsNaN(minVal) || float.IsNaN(maxVal) || float.IsNaN(chance))
                return null;
            if (float.IsInfinity(minVal) || float.IsInfinity(maxVal) || float.IsInfinity(chance))
                return null;
            if (chance < 0 || chance > 1.0f)
                return null;

            return new CCDBRange
            {
                Min = minVal,
                Max = maxVal,
                Chance = chance
            };
        }

        /// <summary>
        /// Find and parse range values from m_RangeValues field in C_SpawnProfileDB.
        /// C_RangeWithChance structure from IDA:
        /// - m_MinValue (float) at offset 0
        /// - m_MaxValue (float) at offset 4
        /// - m_Chance (float) at offset 8
        /// Total size: 12 bytes
        /// </summary>
        public void ParseRangeValues(byte[] data, BinaryReader reader, int expectedCount)
        {
            if (expectedCount <= 0) return;

            System.Diagnostics.Debug.WriteLine($"[GENR] Looking for {expectedCount} C_RangeWithChance objects...");

            // PRIORITY 1: Find the m_RangeValues field directly (this is where the actual array lives)
            long fieldPos = FindFieldByName(data, "m_RangeValues", _dataStart);
            if (fieldPos > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Found m_RangeValues field at 0x{fieldPos:X}");

                // After field name, look for the vector header
                // Format: [flags:4][type:4][size:4][count:4][elements...]
                reader.BaseStream.Position = fieldPos + 14; // Skip "m_RangeValues" (13 chars + null or length byte)

                // Search for the vector start pattern within 200 bytes
                long searchEnd = Math.Min(reader.BaseStream.Position + 200, _streamLength - 20);
                bool foundArray = false;

                while (reader.BaseStream.Position < searchEnd)
                {
                    long testPos = reader.BaseStream.Position;
                    uint flags = reader.ReadUInt32();

                    // Vector field typically has flags=4
                    if (flags == 4)
                    {
                        uint vectorTypeHash = reader.ReadUInt32();
                        uint size = reader.ReadUInt32();

                        // Check if size looks reasonable for an array of Ranges
                        // Each Range could be 12 bytes (3 floats) packed, or larger with headers
                        if (size > 0 && size < 500000)
                        {
                            // Try to find count - might be in the size field or after
                            uint possibleCount = size / 12; // If packed 12-byte elements

                            // Or the count might be the next uint
                            long afterHeader = reader.BaseStream.Position;
                            uint explicitCount = reader.ReadUInt32();

                            // Check if explicit count matches expected
                            if (explicitCount == expectedCount || (explicitCount > 0 && explicitCount <= expectedCount + 10))
                            {
                                System.Diagnostics.Debug.WriteLine($"[GENR] Found RangeValues array: count={explicitCount} at 0x{afterHeader:X}");

                                // Parse the array elements
                                for (int i = 0; i < explicitCount && reader.BaseStream.Position + 12 <= _streamLength; i++)
                                {
                                    var range = ParseRangeElement(reader, i);
                                    if (range != null)
                                    {
                                        RangeValues.Add(range);
                                        if (i < 5 || i % 200 == 0)
                                            System.Diagnostics.Debug.WriteLine($"[GENR] Range[{i}]: Min={range.Min:F2}, Max={range.Max:F2}, Chance={range.Chance:F2}");
                                    }
                                }
                                foundArray = true;
                                break;
                            }

                            reader.BaseStream.Position = afterHeader; // Reset and continue searching
                        }
                    }

                    reader.BaseStream.Position = testPos + 1; // Move forward and try again
                }

                if (foundArray)
                {
                    System.Diagnostics.Debug.WriteLine($"[GENR] Parsed {RangeValues.Count} ranges from m_RangeValues field");
                    return;
                }
            }

            // PRIORITY 2: Find TYPE_C_RANGE_WITH_CHANCE markers and filter to actual instances
            System.Diagnostics.Debug.WriteLine($"[GENR] m_RangeValues field not found, using type marker approach...");

            uint typeHash = TYPE_C_RANGE_WITH_CHANCE_ALT; // 0x21232777 is the hash seen in GENR files
            byte[] typeHashBytes = BitConverter.GetBytes(typeHash);
            List<long> markerPositions = new List<long>();

            for (long pos = _dataStart; pos < data.Length - 4; pos++)
            {
                if (data[pos] == typeHashBytes[0] && data[pos + 1] == typeHashBytes[1] &&
                    data[pos + 2] == typeHashBytes[2] && data[pos + 3] == typeHashBytes[3])
                {
                    markerPositions.Add(pos);
                }
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] Found {markerPositions.Count} TYPE_C_RANGE_WITH_CHANCE markers");

            if (markerPositions.Count == 0) return;

            // Show first few markers for debugging
            for (int i = 0; i < Math.Min(5, markerPositions.Count); i++)
            {
                DumpBytes(data, markerPositions[i], 48, $"TYPE_C_RANGE_WITH_CHANCE marker[{i}]");
            }

            // Parse each marker
            HashSet<string> seenValues = new HashSet<string>(); // Track unique values
            foreach (var markerPos in markerPositions)
            {
                reader.BaseStream.Position = markerPos + 4; // Skip type hash
                var range = ParseRangeAfterMarker(reader, RangeValues.Count);
                if (range != null)
                {
                    // Track unique values to detect if we're parsing duplicates
                    string key = $"{range.Min:F4}_{range.Max:F4}_{range.Chance:F4}";
                    seenValues.Add(key);

                    RangeValues.Add(range);
                    if (RangeValues.Count <= 5 || RangeValues.Count % 200 == 0)
                        System.Diagnostics.Debug.WriteLine($"[GENR] Range[{RangeValues.Count - 1}]: Min={range.Min:F2}, Max={range.Max:F2}, Chance={range.Chance:F2} at 0x{markerPos:X}");
                }
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] Final Range count: {RangeValues.Count} (expected {expectedCount}), unique values: {seenValues.Count}");

            // If we have very few unique values compared to total, warn about possible duplicate parsing
            if (seenValues.Count < RangeValues.Count / 10 && RangeValues.Count > 100)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] WARNING: Only {seenValues.Count} unique Range values out of {RangeValues.Count} - may be parsing duplicates!");
            }
        }

        /// <summary>
        /// Parse a single C_RangeWithChance element from a contiguous array.
        /// Elements are stored as: [min:float][max:float][chance:float] = 12 bytes each
        /// </summary>
        private CCDBRange ParseRangeElement(BinaryReader reader, int index)
        {
            try
            {
                // In a packed array, elements are just 3 consecutive floats
                float min = reader.ReadSingle();
                float max = reader.ReadSingle();
                float chance = reader.ReadSingle();

                return new CCDBRange { Min = min, Max = max, Chance = chance };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Parse a C_RangeWithChance after its type marker.
        /// The format varies based on flags:
        /// - flags=0: Inline object reference, skip (not a real instance)
        /// - flags=2: Reference format [flags:4][ref:8] - values are 0,0,0 if ref=0, or check for inline data
        /// - flags=4: Value format [flags:4][type:4][size:4][value] for each field
        /// </summary>
        private CCDBRange ParseRangeAfterMarker(BinaryReader reader, int debugId)
        {
            long startPos = reader.BaseStream.Position;

            try
            {
                uint firstFlags = reader.ReadUInt32();

                // flags=0 means inline object marker - not a real Range instance
                // This appears in the data stream as part of other structures
                if (firstFlags == 0)
                {
                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   Skipping flags=0 Range marker (inline reference)");
                    return null;
                }

                // flags=2 means reference/inline format
                // Format: [flags:4][8-byte ref]
                // For C_RangeWithChance with ref=0, values default to 0 OR there's inline data
                if (firstFlags == 2)
                {
                    ulong refValue = reader.ReadUInt64();

                    // If ref is 0, check if there's inline data or use defaults
                    if (refValue == 0)
                    {
                        // Peek at all potential values to determine how many are actually inline
                        long peekPos = reader.BaseStream.Position;
                        uint raw1 = reader.ReadUInt32();  // Potential min (or next token flags)
                        uint raw2 = reader.ReadUInt32();  // Potential max (or next token type/flags)
                        uint raw3 = reader.ReadUInt32();  // Potential chance (or next token data)
                        reader.BaseStream.Position = peekPos;  // Reset position

                        // Check if raw1 is flags (0, 2, 4) - if so, there's NO inline data
                        if ((raw1 == 0 || raw1 == 2 || raw1 == 4) && raw2 > 0x01000000)
                        {
                            // No inline data - use default values
                            if (debugId < 5)
                                System.Diagnostics.Debug.WriteLine($"[GENR]   Range (flags=2, ref=0): no inline data, defaults 0,0,0");
                            return new CCDBRange { Min = 0, Max = 0, Chance = 0 };
                        }

                        // raw1 is likely a float - read it
                        float minValue = reader.ReadSingle();

                        // Check if the NEXT uint (raw2) is flags for a new token
                        // Flags are small values (0, 2, 4), type hashes are large (>0x01000000)
                        bool nextIsToken = (raw2 == 0 || raw2 == 2 || raw2 == 4) && raw3 > 0x01000000;

                        if (nextIsToken)
                        {
                            // Only 1 float was inline - this is the "value" for the Range
                            // Interpret as: min=value, max=value, chance=1.0 (100%)
                            if (debugId < 5)
                                System.Diagnostics.Debug.WriteLine($"[GENR]   Range (flags=2, single): value={minValue:F4} -> min=max={minValue:F4}, chance=1.0");
                            return new CCDBRange { Min = minValue, Max = minValue, Chance = 1.0f };
                        }

                        // More than 1 float - read max
                        float maxValue = reader.ReadSingle();

                        // Check if the THIRD value (raw3) is flags for a new token
                        bool thirdIsToken = (raw3 == 0 || raw3 == 2 || raw3 == 4);

                        if (thirdIsToken)
                        {
                            // Only 2 floats were inline
                            if (debugId < 5)
                                System.Diagnostics.Debug.WriteLine($"[GENR]   Range (flags=2, double): min={minValue:F4}, max={maxValue:F4}, chance=1.0");
                            return new CCDBRange { Min = minValue, Max = maxValue, Chance = 1.0f };
                        }

                        // All 3 floats are inline
                        float chance = reader.ReadSingle();

                        if (debugId < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Range (flags=2, full): min={minValue:F4}, max={maxValue:F4}, chance={chance:F4}");
                        return new CCDBRange { Min = minValue, Max = maxValue, Chance = chance };
                    }
                    else
                    {
                        // Non-zero ref - values are stored elsewhere, use defaults for now
                        if (debugId < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Range (flags=2, ref=0x{refValue:X}): external ref, using defaults");
                        return new CCDBRange { Min = 0, Max = 0, Chance = 0 };
                    }
                }

                // flags=4 means value format with full headers
                if (firstFlags == 4)
                {
                    uint minType = reader.ReadUInt32();
                    uint minSize = reader.ReadUInt32();

                    if (minSize != 4)
                    {
                        if (debugId < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid m_MinValue size={minSize}");
                        return null;
                    }

                    float minValue = reader.ReadSingle();

                    // m_MaxValue
                    uint maxFlags = reader.ReadUInt32();
                    uint maxType = reader.ReadUInt32();
                    uint maxSize = reader.ReadUInt32();

                    if (maxFlags != 4 || maxSize != 4)
                    {
                        if (debugId < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid m_MaxValue header (flags={maxFlags}, size={maxSize})");
                        return null;
                    }

                    float maxValue = reader.ReadSingle();

                    // m_Chance
                    uint chanceFlags = reader.ReadUInt32();
                    uint chanceType = reader.ReadUInt32();
                    uint chanceSize = reader.ReadUInt32();

                    if (chanceFlags != 4 || chanceSize != 4)
                    {
                        if (debugId < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid m_Chance header (flags={chanceFlags}, size={chanceSize})");
                        return null;
                    }

                    float chance = reader.ReadSingle();

                    // Validate
                    if (minValue >= -10000 && minValue <= 10000 &&
                        maxValue >= -10000 && maxValue <= 10000 &&
                        chance >= 0 && chance <= 2.0f)
                    {
                        if (debugId < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Range (flags=4): min={minValue:F2}, max={maxValue:F2}, chance={chance:F2}");

                        return new CCDBRange
                        {
                            Min = minValue,
                            Max = maxValue,
                            Chance = chance
                        };
                    }
                    else
                    {
                        if (debugId < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid range values (flags=4): min={minValue}, max={maxValue}, chance={chance}");
                        return null;
                    }
                }

                // Unknown flags - skip this marker
                if (debugId < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   Unknown flags={firstFlags} for C_RangeWithChance at 0x{startPos:X}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Error parsing Range at 0x{startPos:X}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Find and parse piece sets from m_PieceSets field (at C_SpawnProfileDB level).
        /// Based on IDA analysis of C_SpawnProfileDB::RegisterAttributes:
        /// - m_PieceSets is at offset 112
        /// - Type: vector&lt;C_OwningPtr&lt;C_PieceSet&gt;&gt;
        /// C_PieceSet structure from IDA analysis of C_PieceSet::C_ThisTypeInfo:
        /// - offset 4: m_Weight (uint)
        /// - offset 8: m_ChanceToSpawn (float)
        /// - offset 16: m_PieceIds (vector&lt;C_HashName&gt;)
        /// </summary>
        /// <summary>
        /// Parse PieceSets from m_PieceSets field in C_SpawnProfileDB.
        /// C_SpawnProfileDB has m_PieceSets: vector&lt;C_OwningPtr&lt;C_PieceSet&gt;&gt; at offset 112
        /// Each C_PieceSet has:
        /// - m_Weight (uint) at offset 4
        /// - m_ChanceToSpawn (float) at offset 8
        /// - m_PieceIds (vector&lt;C_HashName&gt;) at offset 16
        /// </summary>
        public void ParsePieceSets(byte[] data, BinaryReader reader, int expectedCount)
        {
            if (expectedCount <= 0) return;

            System.Diagnostics.Debug.WriteLine($"[GENR] Looking for {expectedCount} C_PieceSet objects...");

            // PRIORITY 1: Find the m_PieceSets field directly
            long fieldPos = FindFieldByName(data, "m_PieceSets", _dataStart);
            if (fieldPos > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Found m_PieceSets field at 0x{fieldPos:X}");

                // After field name, look for the vector header
                // Format: [type_hash:4][flags:1][size:4] or [flags:4][type_hash:4][size:4][count:4]
                reader.BaseStream.Position = fieldPos + 1 + 11; // Skip length byte + "m_PieceSets"

                // Search for the vector start pattern within 200 bytes
                long searchEnd = Math.Min(reader.BaseStream.Position + 200, _streamLength - 20);
                bool foundArray = false;

                // Dump bytes for debugging
                if (reader.BaseStream.Position + 64 < _streamLength)
                {
                    long dumpPos = reader.BaseStream.Position;
                    System.Diagnostics.Debug.WriteLine($"[GENR] Bytes after m_PieceSets field at 0x{dumpPos:X}:");
                    for (int row = 0; row < 4; row++)
                    {
                        long off = dumpPos + row * 16;
                        if (off + 16 > _streamLength) break;
                        reader.BaseStream.Position = off;
                        byte[] rowData = reader.ReadBytes(16);
                        System.Diagnostics.Debug.WriteLine($"  0x{off:X4}: {BitConverter.ToString(rowData).Replace("-", " ")}");
                    }
                    reader.BaseStream.Position = dumpPos;
                }

                while (reader.BaseStream.Position < searchEnd)
                {
                    long testPos = reader.BaseStream.Position;
                    uint flags = reader.ReadUInt32();

                    // Vector field typically has flags=4
                    if (flags == 4)
                    {
                        uint typeHash = reader.ReadUInt32();
                        uint size = reader.ReadUInt32();

                        System.Diagnostics.Debug.WriteLine($"[GENR] Testing vector at 0x{testPos:X}: flags={flags} type=0x{typeHash:X8} size={size}");

                        if (size > 0 && size < 5000000)
                        {
                            // The count should be the next uint
                            long afterHeader = reader.BaseStream.Position;
                            uint explicitCount = reader.ReadUInt32();

                            System.Diagnostics.Debug.WriteLine($"[GENR] Potential count at 0x{afterHeader:X}: {explicitCount}");

                            if (explicitCount == expectedCount || (explicitCount > 0 && explicitCount <= expectedCount + 10))
                            {
                                System.Diagnostics.Debug.WriteLine($"[GENR] Found PieceSets array: count={explicitCount} at 0x{afterHeader:X}");

                                // Parse the array elements
                                int parsed = 0;
                                int failures = 0;

                                while (parsed < explicitCount && failures < 20 && reader.BaseStream.Position + 16 <= _streamLength)
                                {
                                    var pieceSet = ParsePieceSetFromArray(reader, parsed);
                                    if (pieceSet != null)
                                    {
                                        PieceSets.Add(pieceSet);
                                        parsed++;
                                        failures = 0;

                                        if (parsed <= 5 || parsed % 500 == 0)
                                            System.Diagnostics.Debug.WriteLine($"[GENR] PieceSet[{parsed-1}]: Weight={pieceSet.Weight}, Chance={pieceSet.ChanceToSpawn:F2}, PieceIds={pieceSet.PieceIds.Count}");
                                    }
                                    else
                                    {
                                        failures++;
                                    }
                                }

                                if (PieceSets.Count > expectedCount / 2)
                                {
                                    foundArray = true;
                                    break;
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"[GENR] Only parsed {PieceSets.Count}, continuing search...");
                                    PieceSets.Clear();
                                }
                            }

                            reader.BaseStream.Position = afterHeader;
                        }
                    }

                    reader.BaseStream.Position = testPos + 1;
                }

                if (foundArray)
                {
                    System.Diagnostics.Debug.WriteLine($"[GENR] Parsed {PieceSets.Count} piece sets from m_PieceSets field");
                    return;
                }
            }

            // PRIORITY 2: Fall back to type marker approach
            System.Diagnostics.Debug.WriteLine($"[GENR] m_PieceSets field not found, using type marker approach...");
            System.Diagnostics.Debug.WriteLine($"[GENR] TYPE_C_PIECE_SET = 0x{TYPE_C_PIECE_SET:X8}");

            byte[] typeHashBytes = BitConverter.GetBytes(TYPE_C_PIECE_SET);
            List<long> markerPositions = new List<long>();

            for (long pos = _dataStart; pos < data.Length - 4; pos++)
            {
                if (data[pos] == typeHashBytes[0] && data[pos + 1] == typeHashBytes[1] &&
                    data[pos + 2] == typeHashBytes[2] && data[pos + 3] == typeHashBytes[3])
                {
                    markerPositions.Add(pos);
                }
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] Found {markerPositions.Count} TYPE_C_PIECE_SET markers in data section");

            if (markerPositions.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("[GENR] No TYPE_C_PIECE_SET markers found, trying alternate approach...");
                ScanForPieceSetsFromTypeHash(data, reader, expectedCount);
                System.Diagnostics.Debug.WriteLine($"[GENR] Final PieceSet count: {PieceSets.Count}");
                return;
            }

            // Show first few markers for debugging
            for (int i = 0; i < Math.Min(5, markerPositions.Count); i++)
            {
                DumpBytes(data, markerPositions[i], 48, $"TYPE_C_PIECE_SET marker[{i}]");
            }

            // Parse each marker, tracking unique values
            int parsedCount = 0;
            int failureCount = 0;
            HashSet<string> seenValues = new HashSet<string>();

            foreach (var markerPos in markerPositions)
            {
                if (parsedCount >= expectedCount) break;
                if (failureCount >= 50) break;

                reader.BaseStream.Position = markerPos + 4;

                var pieceSet = ParsePieceSetAfterMarker(reader);

                if (pieceSet != null)
                {
                    string key = $"{pieceSet.Weight}_{pieceSet.ChanceToSpawn:F4}_{pieceSet.PieceIds.Count}";
                    seenValues.Add(key);

                    PieceSets.Add(pieceSet);
                    parsedCount++;
                    failureCount = 0;

                    if (parsedCount <= 5 || parsedCount % 200 == 0)
                        System.Diagnostics.Debug.WriteLine($"[GENR] PieceSet[{parsedCount-1}]: Weight={pieceSet.Weight}, Chance={pieceSet.ChanceToSpawn:F2}, PieceIds={pieceSet.PieceIds.Count} at 0x{markerPos:X}");
                }
                else
                {
                    failureCount++;
                    if (failureCount <= 10)
                        System.Diagnostics.Debug.WriteLine($"[GENR] Failed to parse PieceSet at marker 0x{markerPos:X}");
                }
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] Final PieceSet count: {PieceSets.Count} (expected {expectedCount}), unique values: {seenValues.Count}");

            if (seenValues.Count < PieceSets.Count / 10 && PieceSets.Count > 100)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] WARNING: Only {seenValues.Count} unique PieceSet values out of {PieceSets.Count} - may be parsing duplicates!");
            }
        }

        /// <summary>
        /// Parse a C_PieceSet from a contiguous array (field-based approach).
        /// Each element in the array is a C_OwningPtr&lt;C_PieceSet&gt; serialized inline.
        /// </summary>
        private CCDBPieceSet ParsePieceSetFromArray(BinaryReader reader, int index)
        {
            long startPos = reader.BaseStream.Position;

            try
            {
                // Dump first few for debugging
                if (index < 5)
                {
                    byte[] preview = new byte[80];
                    long savedPos = reader.BaseStream.Position;
                    int bytesRead = reader.Read(preview, 0, Math.Min(80, (int)(_streamLength - savedPos)));
                    reader.BaseStream.Position = savedPos;
                    System.Diagnostics.Debug.WriteLine($"[GENR] Raw PieceSet[{index}] at 0x{startPos:X}:");
                    System.Diagnostics.Debug.WriteLine($"  {BitConverter.ToString(preview, 0, Math.Min(bytesRead, 40)).Replace("-", " ")}");
                }

                // Check if this is a C_OwningPtr (might have flags first)
                uint firstValue = reader.ReadUInt32();

                // If firstValue looks like flags (0, 2, 4), handle accordingly
                if (firstValue == 0 || firstValue == 2)
                {
                    // Null/empty pointer
                    if (index < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] PieceSet[{index}] is empty (flags={firstValue})");

                    // Skip reference bytes
                    if (firstValue == 2)
                        reader.ReadUInt64(); // 8-byte ref

                    return new CCDBPieceSet(); // Return empty
                }

                // Check for TYPE_C_PIECE_SET marker
                if (firstValue == TYPE_C_PIECE_SET)
                {
                    // Found type marker, parse the PieceSet fields
                    return ParsePieceSetAfterMarker(reader);
                }

                // firstValue might be flags=4 for inline object
                if (firstValue == 4)
                {
                    uint typeHash = reader.ReadUInt32();

                    if (index < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] PieceSet[{index}] inline: type=0x{typeHash:X8}");

                    // Check if this is the C_PieceSet type
                    if (typeHash == TYPE_C_PIECE_SET)
                    {
                        uint size = reader.ReadUInt32();
                        // Now parse the actual PieceSet fields
                        return ParsePieceSetAfterMarker(reader);
                    }
                }

                // Reset and try parsing as raw PieceSet data
                reader.BaseStream.Position = startPos;

                var pieceSet = new CCDBPieceSet();

                // Try parsing as direct field values
                // Format: [flags:4][type:4][size:4][value] for each field

                // m_Weight
                uint weightFlags = reader.ReadUInt32();
                if (weightFlags != 4)
                {
                    if (index < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] PieceSet[{index}] unexpected weightFlags: {weightFlags}");
                    reader.BaseStream.Position = startPos + 4;
                    return null;
                }

                uint weightType = reader.ReadUInt32();
                uint weightSize = reader.ReadUInt32();
                if (weightSize != 4)
                {
                    if (index < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] PieceSet[{index}] unexpected weightSize: {weightSize}");
                    return null;
                }
                pieceSet.Weight = reader.ReadUInt32();

                // m_ChanceToSpawn
                uint chanceFlags = reader.ReadUInt32();
                if (chanceFlags != 4)
                {
                    if (index < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] PieceSet[{index}] unexpected chanceFlags: {chanceFlags}");
                    return null;
                }

                uint chanceType = reader.ReadUInt32();
                uint chanceSize = reader.ReadUInt32();
                if (chanceSize != 4)
                {
                    if (index < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR] PieceSet[{index}] unexpected chanceSize: {chanceSize}");
                    return null;
                }
                pieceSet.ChanceToSpawn = reader.ReadSingle();

                // m_PieceIds
                uint pieceIdsFlags = reader.ReadUInt32();
                if (pieceIdsFlags == 2)
                {
                    // Empty vector
                    reader.ReadUInt64(); // reference
                    reader.ReadUInt32(); // type hash
                }
                else if (pieceIdsFlags == 4)
                {
                    uint pieceIdsType = reader.ReadUInt32();
                    uint pieceIdsSize = reader.ReadUInt32();
                    int pieceIdCount = reader.ReadInt32();

                    if (pieceIdCount > 0 && pieceIdCount <= 100)
                    {
                        for (int i = 0; i < pieceIdCount; i++)
                        {
                            if (reader.BaseStream.Position + 8 > _streamLength)
                                break;
                            ulong hash = reader.ReadUInt64();
                            pieceSet.PieceIds.Add(new CCDBHashName { Hash = hash });
                        }
                    }
                }

                // Validate
                if (pieceSet.Weight == 0 || pieceSet.Weight > 100000)
                    return null;
                if (pieceSet.ChanceToSpawn < 0 || pieceSet.ChanceToSpawn > 2.0f)
                    return null;

                return pieceSet;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Error parsing PieceSet[{index}] at 0x{startPos:X}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Fallback method to find PieceSets by searching for the type hash in type info.
        /// </summary>
        private void ScanForPieceSetsFromTypeHash(byte[] data, BinaryReader reader, int expectedCount)
        {
            // Search for the C_PieceSet type hash (0x75DD667F)
            byte[] typeHashBytes = BitConverter.GetBytes(TYPE_C_PIECE_SET);

            for (long pos = _dataStart; pos < data.Length - 20; pos++)
            {
                if (data[pos] == typeHashBytes[0] && data[pos + 1] == typeHashBytes[1] &&
                    data[pos + 2] == typeHashBytes[2] && data[pos + 3] == typeHashBytes[3])
                {
                    // Found type hash, look for count within next 64 bytes
                    for (int offset = 5; offset <= 64; offset += 4)
                    {
                        if (pos + offset + 4 >= data.Length) break;

                        uint potentialCount = BitConverter.ToUInt32(data, (int)(pos + offset));
                        if (potentialCount == expectedCount)
                        {
                            System.Diagnostics.Debug.WriteLine($"[GENR] Found type hash at 0x{pos:X}, count at offset +{offset}");

                            long parseStart = pos + offset + 4;
                            reader.BaseStream.Position = parseStart;

                            // Try parsing
                            int parsed = 0;
                            int failures = 0;
                            while (parsed < expectedCount && failures < 10 && reader.BaseStream.Position + 16 <= _streamLength)
                            {
                                long objStart = reader.BaseStream.Position;
                                var pieceSet = ParsePieceSetAfterMarker(reader);

                                if (pieceSet != null)
                                {
                                    PieceSets.Add(pieceSet);
                                    parsed++;
                                    failures = 0;
                                }
                                else
                                {
                                    failures++;
                                    reader.BaseStream.Position = objStart + 4;
                                }
                            }

                            if (PieceSets.Count > expectedCount / 2)
                            {
                                System.Diagnostics.Debug.WriteLine($"[GENR] Fallback found {PieceSets.Count} piece sets");
                                return;
                            }
                            else
                            {
                                PieceSets.Clear();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Debug helper to dump bytes at a position.
        /// </summary>
        private void DumpBytes(byte[] data, long pos, int count, string label)
        {
            if (pos < 0) pos = 0;
            if (pos >= data.Length) return;

            System.Diagnostics.Debug.WriteLine($"[GENR] {label} at 0x{pos:X}:");
            for (int row = 0; row < count / 16 && pos + row * 16 < data.Length; row++)
            {
                long off = pos + row * 16;
                var sb = new StringBuilder($"  0x{off:X4}: ");
                for (int i = 0; i < 16 && off + i < data.Length; i++)
                    sb.Append($"{data[off + i]:X2} ");
                sb.Append(" | ");
                for (int i = 0; i < 16 && off + i < data.Length; i++)
                {
                    char c = (char)data[off + i];
                    sb.Append(c >= 32 && c < 127 ? c : '.');
                }
                System.Diagnostics.Debug.WriteLine(sb.ToString());
            }
        }

        /// <summary>
        /// Parse a C_PieceSet after finding its TYPE_C_PIECE_SET marker.
        /// GENR format for class attributes uses: [flags:4][type_hash:4][size:4][data]
        /// For simple values: data = the value
        /// For vectors: data = [count:4][elements...]
        ///
        /// C_PieceSet attributes from IDA:
        /// - m_Weight (uint)
        /// - m_ChanceToSpawn (float)
        /// - m_PieceIds (vector&lt;C_HashName&gt;)
        /// </summary>
        private CCDBPieceSet ParsePieceSetAfterMarker(BinaryReader reader)
        {
            long startPos = reader.BaseStream.Position;
            int debugId = PieceSets.Count;

            try
            {
                // Dump bytes for first few
                if (debugId < 5)
                {
                    byte[] preview = new byte[80];
                    long savedPos = reader.BaseStream.Position;
                    int bytesRead = reader.Read(preview, 0, 80);
                    reader.BaseStream.Position = savedPos;
                    System.Diagnostics.Debug.WriteLine($"[GENR] Raw PieceSet #{debugId} at 0x{startPos:X}:");
                    System.Diagnostics.Debug.WriteLine($"  Row 0: {BitConverter.ToString(preview, 0, Math.Min(bytesRead, 40)).Replace("-", " ")}");
                    if (bytesRead > 40)
                        System.Diagnostics.Debug.WriteLine($"  Row 1: {BitConverter.ToString(preview, 40, Math.Min(bytesRead - 40, 40)).Replace("-", " ")}");
                }

                var pieceSet = new CCDBPieceSet();

                // m_Weight: [flags:4][type_hash:4][size:4][value:4]
                uint weightFlags = reader.ReadUInt32();
                uint weightType = reader.ReadUInt32();
                uint weightSize = reader.ReadUInt32();

                if (debugId < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   @0x{startPos:X}: m_Weight flags={weightFlags} type=0x{weightType:X8} size={weightSize}");

                // Weight flags should be 4 (value type), size should be 4
                if (weightFlags != 4 || weightSize != 4)
                {
                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid m_Weight header");
                    return null;
                }

                pieceSet.Weight = reader.ReadUInt32();

                // m_ChanceToSpawn: [flags:4][type_hash:4][size:4][value:4]
                uint chanceFlags = reader.ReadUInt32();
                uint chanceType = reader.ReadUInt32();
                uint chanceSize = reader.ReadUInt32();

                if (debugId < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   m_ChanceToSpawn flags={chanceFlags} type=0x{chanceType:X8} size={chanceSize}");

                if (chanceFlags != 4 || chanceSize != 4)
                {
                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid m_ChanceToSpawn header");
                    return null;
                }

                pieceSet.ChanceToSpawn = reader.ReadSingle();

                // m_PieceIds: [flags:4][type_hash:4][size:4][count:4][elements...]
                // OR for empty: [flags=2:4][reference:8][type_hash:4] (no count/elements)
                uint pieceIdsFlags = reader.ReadUInt32();

                if (debugId < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   m_PieceIds flags={pieceIdsFlags}");

                if (pieceIdsFlags == 2)
                {
                    // flags=2 means empty vector or object reference
                    // Format: [flags=2:4][reference:8][type_hash:4]
                    reader.ReadUInt64(); // skip 8-byte reference (usually zeros for empty)
                    reader.ReadUInt32(); // skip 4-byte type hash
                    // PieceIds remains empty (already initialized empty)
                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   m_PieceIds is empty (flags=2 format)");
                }
                else if (pieceIdsFlags == 4)
                {
                    uint pieceIdsType = reader.ReadUInt32();
                    uint pieceIdsSize = reader.ReadUInt32();

                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   m_PieceIds type=0x{pieceIdsType:X8} size={pieceIdsSize}");

                    int pieceIdCount = reader.ReadInt32();

                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   m_PieceIds count={pieceIdCount}");

                    if (pieceIdCount < 0 || pieceIdCount > 100)
                    {
                        if (debugId < 5)
                            System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid pieceIdCount: {pieceIdCount}");
                        return null;
                    }

                    for (int i = 0; i < pieceIdCount; i++)
                    {
                        if (reader.BaseStream.Position + 8 > _streamLength)
                            return null;
                        ulong hash = reader.ReadUInt64();
                        pieceSet.PieceIds.Add(new CCDBHashName { Hash = hash });
                    }
                }
                else
                {
                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid m_PieceIds flags: {pieceIdsFlags}");
                    return null;
                }

                // Validate final values
                if (pieceSet.Weight == 0 || pieceSet.Weight > 100000)
                {
                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid Weight value: {pieceSet.Weight}");
                    return null;
                }

                if (pieceSet.ChanceToSpawn < 0 || pieceSet.ChanceToSpawn > 2.0f)
                {
                    if (debugId < 5)
                        System.Diagnostics.Debug.WriteLine($"[GENR]   Invalid ChanceToSpawn value: {pieceSet.ChanceToSpawn}");
                    return null;
                }

                if (debugId < 5)
                    System.Diagnostics.Debug.WriteLine($"[GENR]   Parsed: Weight={pieceSet.Weight}, Chance={pieceSet.ChanceToSpawn:F4}, PieceIds={pieceSet.PieceIds.Count}");

                return pieceSet;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GENR] Error parsing PieceSet at 0x{startPos:X}: {ex.Message}");
                return null;
            }
        }

    }
}
