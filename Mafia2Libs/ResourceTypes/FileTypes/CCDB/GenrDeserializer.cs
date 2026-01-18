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
            System.Diagnostics.Debug.WriteLine($"[GENR] Parsing {count} choices at position 0x{reader.BaseStream.Position:X}");

            // Dump first 32 bytes after count to understand format
            long afterCount = reader.BaseStream.Position;
            if (afterCount + 32 < _streamLength)
            {
                byte[] preview = new byte[32];
                reader.Read(preview, 0, 32);
                reader.BaseStream.Position = afterCount;

                System.Diagnostics.Debug.WriteLine($"[GENR] First 32 bytes after count:");
                System.Diagnostics.Debug.WriteLine($"  {preview[0]:X2} {preview[1]:X2} {preview[2]:X2} {preview[3]:X2} " +
                    $"{preview[4]:X2} {preview[5]:X2} {preview[6]:X2} {preview[7]:X2} " +
                    $"{preview[8]:X2} {preview[9]:X2} {preview[10]:X2} {preview[11]:X2} " +
                    $"{preview[12]:X2} {preview[13]:X2} {preview[14]:X2} {preview[15]:X2}");
                System.Diagnostics.Debug.WriteLine($"  {preview[16]:X2} {preview[17]:X2} {preview[18]:X2} {preview[19]:X2} " +
                    $"{preview[20]:X2} {preview[21]:X2} {preview[22]:X2} {preview[23]:X2} " +
                    $"{preview[24]:X2} {preview[25]:X2} {preview[26]:X2} {preview[27]:X2} " +
                    $"{preview[28]:X2} {preview[29]:X2} {preview[30]:X2} {preview[31]:X2}");
            }

            int parsed = 0;
            int failures = 0;

            // vector<C_OwningPtr<C_Choice>> format:
            // Each element is a C_OwningPtr which has 8-byte header + 8-byte reference
            // The actual C_Choice data follows inline (not via reference resolution)

            while (parsed < count && failures < 50 && reader.BaseStream.Position + 20 < _streamLength)
            {
                long elementStart = reader.BaseStream.Position;

                // Skip C_OwningPtr header (8 bytes) + reference (8 bytes) = 16 bytes total
                // Then read inline C_Choice data
                if (reader.BaseStream.Position + 16 > _streamLength)
                    break;

                // Read and log first few element headers to understand pattern
                if (parsed < 3)
                {
                    ulong header = reader.ReadUInt64();
                    ulong reference = reader.ReadUInt64();
                    System.Diagnostics.Debug.WriteLine($"[GENR] Element {parsed}: header=0x{header:X16}, ref=0x{reference:X16}");
                    // After header+ref, try to parse inline C_Choice
                }
                else
                {
                    reader.BaseStream.Position += 16; // Skip header + reference
                }

                var choice = ParseChoice(reader);
                if (choice != null)
                {
                    choice.Ref = $"Choice_{parsed}";
                    SharedChoices.Add(choice);
                    parsed++;
                    failures = 0;

                    if (parsed % 500 == 0)
                        System.Diagnostics.Debug.WriteLine($"[GENR] Parsed {parsed} choices at 0x{reader.BaseStream.Position:X}");
                }
                else
                {
                    failures++;
                    // Try skipping different amounts to find correct alignment
                    reader.BaseStream.Position = elementStart + 4;
                }
            }

            System.Diagnostics.Debug.WriteLine($"[GENR] ParseChoicesArray complete: {parsed} choices (failures={failures})");
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

                // Parse range values
                int expectedRanges = GetExpectedCount(TYPE_C_RANGE_WITH_CHANCE);
                if (expectedRanges > 0)
                {
                    ParseRangeValues(data, _reader, expectedRanges);
                }

                System.Diagnostics.Debug.WriteLine($"[GENR] Result: {Profiles.Count} profiles, {SharedChoices.Count} choices, {RangeValues.Count} ranges");
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
        /// Find and parse range values from m_RangeValues field.
        /// </summary>
        public void ParseRangeValues(byte[] data, BinaryReader reader, int expectedCount)
        {
            if (expectedCount <= 0) return;

            long fieldPos = FindFieldByName(data, "m_RangeValues", _dataStart);

            if (fieldPos > 0 && TryFindVectorStart(data, fieldPos + 14, expectedCount, out long vectorStart))
            {
                reader.BaseStream.Position = vectorStart + 4;

                for (int i = 0; i < expectedCount && reader.BaseStream.Position + 12 <= _streamLength; i++)
                {
                    var range = ParseRangeWithChance(reader);
                    if (range != null)
                        RangeValues.Add(range);
                }

                System.Diagnostics.Debug.WriteLine($"[GENR] Parsed {RangeValues.Count} range values");
            }
        }
    }
}
