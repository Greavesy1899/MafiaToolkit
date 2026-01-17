using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Utils.Helpers.Reflection;

namespace ResourceTypes.CCDB
{
    /// <summary>
    /// Represents a CCDB (Compound Database) file - used for NPC spawn profiles in Mafia I: DE
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class CCDBFile
    {
        [Category("Header"), Description("File format version")]
        public int Version { get; set; }

        [Category("Header"), Description("Header version tuple")]
        public string HeaderVersion { get; set; } = "";

        [Category("Header"), Description("Root type identifier")]
        public string RootTypeID { get; set; } = "";

        [Category("Header"), Description("Memory footprint hint")]
        public int MemoryFootprint { get; set; }

        [Category("Table of Contents"), Description("Object references in ToC")]
        public List<string> TableOfContents { get; set; } = new List<string>();

        [Category("Content"), Description("Combinable pieces (mesh parts)")]
        public List<CCDBCombinablePiece> CombinablePieces { get; set; } = new List<CCDBCombinablePiece>();

        [Category("Content"), Description("Piece set mappings")]
        public List<CCDBPieceSetMapping> PieceSetMappings { get; set; } = new List<CCDBPieceSetMapping>();

        [Category("Content"), Description("Spawn profiles")]
        public List<CCDBSpawnProfile> SpawnProfiles { get; set; } = new List<CCDBSpawnProfile>();

        [Category("Content"), Description("Shared choices")]
        public List<CCDBChoice> SharedChoices { get; set; } = new List<CCDBChoice>();

        [Category("Content"), Description("Piece sets")]
        public List<CCDBPieceSet> PieceSets { get; set; } = new List<CCDBPieceSet>();

        [Category("Content"), Description("Ranges")]
        public List<CCDBRange> Ranges { get; set; } = new List<CCDBRange>();

        private XDocument _originalDocument;

        public CCDBFile()
        {
        }

        public void ReadFromFile(FileInfo fileInfo)
        {
            byte[] fileData = File.ReadAllBytes(fileInfo.FullName);
            ReadFromBytes(fileData);
        }

        public void ReadFromStream(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                ReadFromBytes(ms.ToArray());
            }
        }

        public void ReadFromBytes(byte[] data)
        {
            // Check if this is XML or binary format
            // Handle UTF-8 BOM (0xEF 0xBB 0xBF) if present
            int startIndex = 0;
            if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            {
                startIndex = 3; // Skip BOM
            }

            bool isXml = data.Length > startIndex && data[startIndex] == '<';

            if (isXml)
            {
                // XML format
                string xmlContent = Encoding.UTF8.GetString(data);
                ReadFromXmlString(xmlContent);
            }
            else
            {
                // Binary/compiled format - not yet supported
                // For now, create a placeholder structure
                ReadFromBinary(data);
            }
        }

        public void ReadFromXmlString(string xmlContent)
        {
            _originalDocument = XDocument.Parse(xmlContent);
            _isXmlFormat = true;
            ParseDocument(_originalDocument);

            // Debug output
            System.Diagnostics.Debug.WriteLine($"[CCDB] Parsed {SpawnProfiles.Count} profiles, {SharedChoices.Count} shared choices");
            int profilesWithChoices = SpawnProfiles.Count(p => p.Choices.Count > 0);
            int profilesWithPackages = SpawnProfiles.Count(p => p.Packages.Count > 0);
            System.Diagnostics.Debug.WriteLine($"[CCDB] Profiles with choices: {profilesWithChoices}, with packages: {profilesWithPackages}");
        }

        private bool _isXmlFormat = false;

        private void ReadFromBinary(byte[] data)
        {
            _rawBinaryData = data;

            try
            {
                using (MemoryStream ms = new MemoryStream(data))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    long streamLength = reader.BaseStream.Length;

                    // First, dump the header to understand the format
                    System.Diagnostics.Debug.WriteLine($"[CCDB] File size: {streamLength} bytes");
                    System.Diagnostics.Debug.WriteLine($"[CCDB] First 64 bytes hex dump:");
                    int dumpSize = (int)Math.Min(64, streamLength);
                    byte[] header = new byte[dumpSize];
                    reader.Read(header, 0, dumpSize);
                    reader.BaseStream.Position = 0;

                    for (int i = 0; i < dumpSize; i += 16)
                    {
                        string hex = "";
                        string ascii = "";
                        for (int j = 0; j < 16 && (i + j) < dumpSize; j++)
                        {
                            hex += $"{header[i + j]:X2} ";
                            char c = (char)header[i + j];
                            ascii += (c >= 32 && c < 127) ? c : '.';
                        }
                        System.Diagnostics.Debug.WriteLine($"[CCDB]   {i:X4}: {hex,-48} {ascii}");
                    }

                    // Check if this is GENR format at start or has a header
                    uint magic = reader.ReadUInt32();

                    if (magic == 0x524E4547) // "GENR" in little-endian at start
                    {
                        System.Diagnostics.Debug.WriteLine($"[CCDB] Detected GENR format at start");
                        reader.BaseStream.Position = 0;
                        ReadFromBinaryGenr(reader, data, streamLength);
                    }
                    else
                    {
                        // Check if there's a type registry header before GENR
                        // Format: [size/flags 4B][zeros 8B][typeCount 4B][typeHashes][typeCounts][GENR...]
                        reader.BaseStream.Position = 0;

                        // Try to detect the header format by scanning for GENR
                        long genrOffset = FindGenrOffset(data);

                        if (genrOffset > 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"[CCDB] Found GENR at offset 0x{genrOffset:X}, parsing header...");
                            reader.BaseStream.Position = 0;
                            ReadFromBinaryWithHeader(reader, data, streamLength, genrOffset);
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"[CCDB] Unknown format, magic=0x{magic:X8}, trying pattern scan...");
                            reader.BaseStream.Position = 0;
                            ReadFromBinaryUnknown(reader, data, streamLength);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Error in ReadFromBinary: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[CCDB] Stack trace: {ex.StackTrace}");
            }
        }

        private long FindGenrOffset(byte[] data)
        {
            // Search for "GENR" (0x47454E52 in ASCII, 0x524E4547 in little-endian uint32)
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

        private void ReadFromBinaryWithHeader(BinaryReader reader, byte[] data, long streamLength, long genrOffset)
        {
            // Parse the header before GENR
            // Format observed: [unknown 4B][zeros 8B][typeCount 4B][typeHashes...][typeCounts...][GENR...]

            uint unknown1 = reader.ReadUInt32(); // Possibly size or flags (0x0066AF20 in sample)
            reader.ReadUInt64(); // 8 bytes of zeros
            uint typeCount = reader.ReadUInt32();

            System.Diagnostics.Debug.WriteLine($"[CCDB] Header: unknown=0x{unknown1:X8}, typeCount={typeCount}");

            if (typeCount > 50)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Warning: typeCount ({typeCount}) too large, limiting to 50");
                typeCount = 50;
            }

            _binaryTypeHashes = new List<uint>();
            _binaryTypeCounts = new List<ushort>();

            // Read type hashes
            for (int i = 0; i < typeCount; i++)
            {
                uint typeHash = reader.ReadUInt32();
                _binaryTypeHashes.Add(typeHash);
            }

            // Read type instance counts
            for (int i = 0; i < typeCount; i++)
            {
                ushort count = reader.ReadUInt16();
                _binaryTypeCounts.Add(count);
            }

            // Log type registry
            for (int i = 0; i < typeCount; i++)
            {
                string typeName = GetKnownTypeName(_binaryTypeHashes[i]);
                System.Diagnostics.Debug.WriteLine($"[CCDB] Type {i}: 0x{_binaryTypeHashes[i]:X8} [{typeName}] x{_binaryTypeCounts[i]}");
            }

            // Get expected counts
            int profileCount = GetTypeInstanceCount(TYPE_C_SPAWN_PROFILE);
            int choiceCount = GetTypeInstanceCount(TYPE_C_CHOICE);
            System.Diagnostics.Debug.WriteLine($"[CCDB] Expected: {profileCount} profiles, {choiceCount} choices");

            // Now position at GENR and parse
            reader.BaseStream.Position = genrOffset;
            uint genrMagic = reader.ReadUInt32();
            if (genrMagic != 0x524E4547)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Error: GENR magic mismatch at offset 0x{genrOffset:X}");
                return;
            }

            // GENR header - parse all fields
            ushort genrVersionMinor = reader.ReadUInt16();
            ushort genrVersionMajor = reader.ReadUInt16();
            Version = genrVersionMajor;
            HeaderVersion = $"{genrVersionMajor}.{genrVersionMinor}";

            // GENR has more header fields after version
            uint genrRootType = reader.ReadUInt32();
            uint genrFlags = reader.ReadUInt32();

            System.Diagnostics.Debug.WriteLine($"[CCDB] GENR: version={genrVersionMajor}.{genrVersionMinor}, rootType=0x{genrRootType:X8}, flags=0x{genrFlags:X8}");

            // For version >= 9, there might be a memory footprint field
            if (genrVersionMajor >= 9)
            {
                uint memFootprint = reader.ReadUInt32();
                System.Diagnostics.Debug.WriteLine($"[CCDB] GENR memFootprint: {memFootprint}");
            }

            // GENR might have its own type registry - read type count
            uint genrTypeCount = reader.ReadUInt32();
            System.Diagnostics.Debug.WriteLine($"[CCDB] GENR internal typeCount: {genrTypeCount}");

            // Skip GENR's internal type registry if present
            if (genrTypeCount > 0 && genrTypeCount < 100)
            {
                // Skip type hashes and counts
                reader.BaseStream.Position += genrTypeCount * 4; // type hashes
                reader.BaseStream.Position += genrTypeCount * 2; // type counts
            }

            // Now we're at the actual data section
            long dataStart = reader.BaseStream.Position;
            System.Diagnostics.Debug.WriteLine($"[CCDB] Data section starts at offset 0x{dataStart:X}");

            // Dump first 32 bytes of data section for analysis
            byte[] dataSample = new byte[Math.Min(32, streamLength - dataStart)];
            reader.Read(dataSample, 0, dataSample.Length);
            reader.BaseStream.Position = dataStart;
            string sampleHex = BitConverter.ToString(dataSample).Replace("-", " ");
            System.Diagnostics.Debug.WriteLine($"[CCDB] Data sample: {sampleHex}");

            // Scan for spawn profile structures
            ScanForSpawnProfilesInGenr(reader, data, streamLength, dataStart, profileCount);
        }

        private void ScanForSpawnProfilesInGenr(BinaryReader reader, byte[] data, long streamLength, long dataStart, int expectedCount)
        {
            // Scan within GENR data for spawn profile patterns
            System.Diagnostics.Debug.WriteLine($"[CCDB] Scanning for profiles from offset 0x{dataStart:X}...");

            HashSet<ulong> seenIds = new HashSet<ulong>();
            int foundCount = 0;
            int maxToFind = Math.Max(expectedCount + 500, 3000);

            // Scan through the GENR data
            for (long pos = dataStart; pos < streamLength - 40 && foundCount < maxToFind; pos += 4)
            {
                reader.BaseStream.Position = pos;

                try
                {
                    ulong id = reader.ReadUInt64();

                    // Check if this looks like a valid FNV64 hash
                    if (!IsValidFNV64Hash(id) || seenIds.Contains(id))
                        continue;

                    // Read potential choices count
                    uint choicesCount = reader.ReadUInt32();
                    if (choicesCount > 50)
                        continue;

                    // Calculate bytes needed for rest of profile
                    long neededBytes = (choicesCount * 8) + 4 + 16;
                    if (reader.BaseStream.Position + neededBytes > streamLength)
                        continue;

                    // Save position and skip choices
                    long choicesStart = reader.BaseStream.Position;
                    reader.BaseStream.Position += choicesCount * 8;

                    // Read packages count
                    uint packagesCount = reader.ReadUInt32();
                    if (packagesCount > 50)
                        continue;

                    neededBytes = (packagesCount * 8) + 16;
                    if (reader.BaseStream.Position + neededBytes > streamLength)
                        continue;

                    // Save position and skip packages
                    long packagesStart = reader.BaseStream.Position;
                    reader.BaseStream.Position += packagesCount * 8;

                    // Read tail fields
                    int priority = reader.ReadInt32();
                    uint minPkg = reader.ReadUInt32();
                    uint desiredMin = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();

                    // Validate
                    if (priority < 0 || priority > 10000 || minPkg > 100 || desiredMin > 100)
                        continue;

                    // This looks valid! Re-read to extract data
                    var choices = new List<CCDBWeightedChoice>();
                    reader.BaseStream.Position = choicesStart;
                    for (int i = 0; i < choicesCount; i++)
                    {
                        uint choiceIdx = reader.ReadUInt32();
                        float weight = reader.ReadSingle();
                        choices.Add(new CCDBWeightedChoice { ChoiceIndex = choiceIdx, Weight = weight });
                    }

                    reader.ReadUInt32(); // Skip packagesCount
                    var packages = new List<CCDBPackage>();
                    for (int i = 0; i < packagesCount; i++)
                    {
                        ulong pkgHash = reader.ReadUInt64();
                        packages.Add(new CCDBPackage { Id = new CCDBHashName(pkgHash) });
                    }

                    var profile = new CCDBSpawnProfile
                    {
                        Id = new CCDBHashName(id),
                        Choices = choices,
                        Packages = packages,
                        Priority = priority,
                        MinimumPackagesToLoad = minPkg,
                        DesiredPackageMinimum = desiredMin,
                        Flags = flags,
                        Ref = $"Profile_{foundCount}"
                    };

                    SpawnProfiles.Add(profile);
                    seenIds.Add(id);
                    foundCount++;

                    // Move to end of profile for next iteration
                    pos = reader.BaseStream.Position - 4;
                }
                catch
                {
                    // Continue scanning
                }
            }

            int withChoices = SpawnProfiles.Count(p => p.Choices.Count > 0);
            int withPackages = SpawnProfiles.Count(p => p.Packages.Count > 0);
            System.Diagnostics.Debug.WriteLine($"[CCDB] Found {foundCount} profiles ({withChoices} with choices, {withPackages} with packages)");
        }

        private void ReadFromBinaryUnknown(BinaryReader reader, byte[] data, long streamLength)
        {
            // Unknown binary format - try to scan for spawn profile patterns
            System.Diagnostics.Debug.WriteLine($"[CCDB] Attempting pattern scan on unknown format...");

            // Scan for valid FNV64 hashes followed by reasonable array counts
            // This is a heuristic approach when we don't know the exact format
            _binaryTypeHashes = new List<uint>();
            _binaryTypeCounts = new List<ushort>();

            // Try to find spawn profile patterns in the data
            reader.BaseStream.Position = 0;
            ScanForSpawnProfiles(reader, data, streamLength);

            if (SpawnProfiles.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Pattern scan found {SpawnProfiles.Count} profiles");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] No profiles found in pattern scan");
            }
        }

        private void ScanForSpawnProfiles(BinaryReader reader, byte[] data, long streamLength)
        {
            // Look for patterns that match C_SpawnProfile structure:
            // - 8 bytes FNV64 hash (id)
            // - 4 bytes choices count (should be 0-20 typically)
            // - variable choices data (count * 8 bytes)
            // - 4 bytes packages count (should be 0-10 typically)
            // - variable packages data (count * 8 bytes)
            // - 4 bytes priority (0-1000 typically)
            // - 4 bytes min packages
            // - 4 bytes desired min
            // - 4 bytes flags

            HashSet<ulong> seenIds = new HashSet<ulong>();
            int foundCount = 0;
            int maxToFind = 3000;

            // Scan through the file looking for valid profile patterns
            for (long pos = 0; pos < streamLength - 40 && foundCount < maxToFind; pos += 8)
            {
                reader.BaseStream.Position = pos;

                try
                {
                    ulong id = reader.ReadUInt64();

                    // Check if this looks like a valid FNV64 hash
                    if (!IsValidFNV64Hash(id) || seenIds.Contains(id))
                        continue;

                    // Read potential choices count
                    uint choicesCount = reader.ReadUInt32();
                    if (choicesCount > 50)
                        continue;

                    // Calculate bytes needed for rest of profile
                    long neededBytes = (choicesCount * 8) + 4 + 16; // choices + packagesCount + tail
                    if (reader.BaseStream.Position + neededBytes > streamLength)
                        continue;

                    // Skip choices
                    reader.BaseStream.Position += choicesCount * 8;

                    // Read packages count
                    uint packagesCount = reader.ReadUInt32();
                    if (packagesCount > 50)
                        continue;

                    neededBytes = (packagesCount * 8) + 16;
                    if (reader.BaseStream.Position + neededBytes > streamLength)
                        continue;

                    // Skip packages
                    reader.BaseStream.Position += packagesCount * 8;

                    // Read tail fields
                    int priority = reader.ReadInt32();
                    uint minPkg = reader.ReadUInt32();
                    uint desiredMin = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();

                    // Validate
                    if (priority < 0 || priority > 10000 || minPkg > 100 || desiredMin > 100)
                        continue;

                    // This looks like a valid profile! Now re-read to extract data
                    reader.BaseStream.Position = pos + 8 + 4; // After id and choicesCount

                    var choices = new List<CCDBWeightedChoice>();
                    for (int i = 0; i < choicesCount; i++)
                    {
                        uint choiceIdx = reader.ReadUInt32();
                        float weight = reader.ReadSingle();
                        choices.Add(new CCDBWeightedChoice { ChoiceIndex = choiceIdx, Weight = weight });
                    }

                    reader.ReadUInt32(); // Skip packagesCount (already read)
                    var packages = new List<CCDBPackage>();
                    for (int i = 0; i < packagesCount; i++)
                    {
                        ulong pkgHash = reader.ReadUInt64();
                        packages.Add(new CCDBPackage { Id = new CCDBHashName(pkgHash) });
                    }

                    var profile = new CCDBSpawnProfile
                    {
                        Id = new CCDBHashName(id),
                        Choices = choices,
                        Packages = packages,
                        Priority = priority,
                        MinimumPackagesToLoad = minPkg,
                        DesiredPackageMinimum = desiredMin,
                        Flags = flags,
                        Ref = $"Profile_{foundCount}"
                    };

                    SpawnProfiles.Add(profile);
                    seenIds.Add(id);
                    foundCount++;

                    // Skip to end of this profile for next iteration
                    pos = reader.BaseStream.Position - 8;
                }
                catch
                {
                    // Continue scanning
                }
            }
        }

        private void ReadFromBinaryGenr(BinaryReader reader, byte[] data, long streamLength)
        {
            // Generic Resource (GENR) format header
            // Based on IDA analysis of ue::sys::core::C_GenericResource::DeserializeBin

            // Minimum header size check (magic + version + typeId + extra + typeCount = 20 bytes)
            if (streamLength < 20)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Error: File too small ({streamLength} bytes), minimum 20 bytes required");
                return;
            }

            // Read and check magic (0x524E4547 = "GENR")
            uint magic = reader.ReadUInt32();
            if (magic != 0x524E4547) // "GENR" in little-endian
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Warning: Unexpected magic 0x{magic:X8}, expected GENR");
                return;
            }

            // Version tuple (2 x uint16) - version must be <= 9
            ushort versionMinor = reader.ReadUInt16();
            ushort versionMajor = reader.ReadUInt16();
            Version = versionMajor;
            HeaderVersion = $"{versionMajor}.{versionMinor}";

            // Root type ID (type hash of root object)
            uint rootTypeId = reader.ReadUInt32();
            RootTypeID = $"0x{rootTypeId:X8}";

            // Another field (possibly flags or second type info)
            uint extraField = reader.ReadUInt32();

            // Memory footprint (for version >= 9)
            if (versionMajor >= 9)
            {
                if (reader.BaseStream.Position + 4 > streamLength)
                {
                    System.Diagnostics.Debug.WriteLine($"[CCDB] Error: Not enough bytes for MemoryFootprint");
                    return;
                }
                MemoryFootprint = reader.ReadInt32();
            }

            // Read type count
            if (reader.BaseStream.Position + 4 > streamLength)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Error: Not enough bytes for typeCount");
                return;
            }
            uint typeCount = reader.ReadUInt32();
            _binaryTypeHashes = new List<uint>();
            _binaryTypeCounts = new List<ushort>();

            System.Diagnostics.Debug.WriteLine($"[CCDB] GENR header: version={versionMajor}.{versionMinor}, rootType=0x{rootTypeId:X8}, typeCount={typeCount}");

            // Sanity check type count
            if (typeCount > 100)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Warning: typeCount ({typeCount}) seems too large, limiting to 100");
                typeCount = 100;
            }

            // Check if we have enough bytes for type hashes and counts
            long requiredForRegistry = (typeCount * 4) + (typeCount * 2); // 4 bytes per hash + 2 bytes per count
            if (reader.BaseStream.Position + requiredForRegistry > streamLength)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Error: Not enough bytes for type registry (need {requiredForRegistry}, have {streamLength - reader.BaseStream.Position})");
                return;
            }

            // Read type information (type hashes)
            for (int i = 0; i < typeCount; i++)
            {
                uint typeHash = reader.ReadUInt32();
                _binaryTypeHashes.Add(typeHash);
            }

            // Read type instance counts
            for (int i = 0; i < typeCount; i++)
            {
                ushort count = reader.ReadUInt16();
                _binaryTypeCounts.Add(count);
            }

            // Log type registry (limited)
            for (int i = 0; i < Math.Min(10, (int)typeCount); i++)
            {
                string typeName = GetKnownTypeName(_binaryTypeHashes[i]);
                System.Diagnostics.Debug.WriteLine($"[CCDB] Type {i}: 0x{_binaryTypeHashes[i]:X8} [{typeName}] x{_binaryTypeCounts[i]}");
            }
            if (typeCount > 10)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] ... and {typeCount - 10} more types");
            }

            // The data section follows the type registry
            long dataStart = reader.BaseStream.Position;
            System.Diagnostics.Debug.WriteLine($"[CCDB] Data section starts at offset 0x{dataStart:X}");

            // Parse the data section properly based on type registry
            ParseBinaryDataSection(data, reader, dataStart);
        }

        private List<uint> _binaryTypeHashes;
        private List<ushort> _binaryTypeCounts;

        // Known type hashes from IDA analysis
        private const uint TYPE_C_COMBINABLE_PIECE = 0x42964A0F;
        private const uint TYPE_C_SPAWN_PROFILE_DB_RESOURCE = 0x097E0ECB;
        private const uint TYPE_C_SPAWN_PROFILE_DB = 0x65F20A93;
        private const uint TYPE_C_SPAWN_PROFILE = 0x340319E3;
        private const uint TYPE_C_CHOICE = 0x33BA57D7;
        private const uint TYPE_C_PIECE_SET = 0x42694B5F;
        private const uint TYPE_S_TOC_OBJ = 0x799A4041;
        private const uint TYPE_S_WEIGHTED_CHOICE = 0x7D5A48C0; // S_WeightedChoice
        private const uint TYPE_C_RANGE_WITH_CHANCE = 0x3E29F1D9;

        /// <summary>
        /// Parse the binary data section based on the GENR format structure.
        /// The data section contains serialized type instances in order of type registry.
        /// </summary>
        private void ParseBinaryDataSection(byte[] data, BinaryReader reader, long dataStart)
        {
            try
            {
                long streamLength = reader.BaseStream.Length;

                // The GENR format stores data in a specific layout:
                // - Type instances are stored based on the type descriptor's serialization
                // - Arrays are stored as: count (4 bytes) + elements (count * element_size)
                // - Strings are stored as: length (2 bytes) + chars

                // Get counts from type registry
                int profileCount = GetTypeInstanceCount(TYPE_C_SPAWN_PROFILE);
                int choiceCount = GetTypeInstanceCount(TYPE_C_CHOICE);
                int pieceCount = GetTypeInstanceCount(TYPE_C_COMBINABLE_PIECE);
                int pieceSetCount = GetTypeInstanceCount(TYPE_C_PIECE_SET);
                int rangeCount = GetTypeInstanceCount(TYPE_C_RANGE_WITH_CHANCE);
                int tocCount = GetTypeInstanceCount(TYPE_S_TOC_OBJ);

                System.Diagnostics.Debug.WriteLine($"[CCDB] Expected counts - Profiles: {profileCount}, Choices: {choiceCount}, Pieces: {pieceCount}");
                System.Diagnostics.Debug.WriteLine($"[CCDB] Data section: start=0x{dataStart:X}, length={streamLength}");

                // Now we need to understand the data layout
                // Based on GENR format, the data is typically organized by type
                // Let's read the first part of data to understand the structure
                reader.BaseStream.Position = dataStart;

                // Try to detect the data layout by scanning for patterns
                // First, let's dump some bytes for analysis
                int sampleSize = (int)Math.Min(256, streamLength - dataStart);
                if (sampleSize > 0)
                {
                    byte[] sampleData = new byte[sampleSize];
                    reader.Read(sampleData, 0, sampleData.Length);
                    reader.BaseStream.Position = dataStart;

                    System.Diagnostics.Debug.WriteLine($"[CCDB] First 64 bytes of data section:");
                    for (int i = 0; i < Math.Min(64, sampleData.Length); i += 16)
                    {
                        string hex = "";
                        for (int j = 0; j < 16 && (i + j) < sampleData.Length; j++)
                        {
                            hex += $"{sampleData[i + j]:X2} ";
                        }
                        System.Diagnostics.Debug.WriteLine($"[CCDB]   {i:X4}: {hex}");
                    }
                }

                // The data section starts with strings (ToC entries), then other data
                // Let's try to parse string table first
                ParseBinaryStringTable(reader, tocCount);

                // After strings, there should be the main data
                // We need to find where each type's data begins
                // For now, let's scan for C_SpawnProfile patterns (8-byte hash followed by array counts)
                long afterStrings = reader.BaseStream.Position;
                System.Diagnostics.Debug.WriteLine($"[CCDB] After string table: offset 0x{afterStrings:X}");

                // Try structured parsing of remaining data
                ParseBinaryStructuredData(data, reader, profileCount, choiceCount, pieceCount);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Error in ParseBinaryDataSection: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[CCDB] Stack trace: {ex.StackTrace}");
            }
        }

        private int GetTypeInstanceCount(uint typeHash)
        {
            if (_binaryTypeHashes == null || _binaryTypeCounts == null)
                return 0;

            int index = _binaryTypeHashes.IndexOf(typeHash);
            if (index >= 0 && index < _binaryTypeCounts.Count)
                return _binaryTypeCounts[index];
            return 0;
        }

        private void ParseBinaryStringTable(BinaryReader reader, int expectedCount)
        {
            // ToC entries are typically strings with length prefix
            // Try to read string entries
            try
            {
                long startPos = reader.BaseStream.Position;
                long streamLength = reader.BaseStream.Length;
                int stringsRead = 0;

                // Limit how many strings we try to read
                int maxStrings = Math.Min(expectedCount, 1000);

                while (stringsRead < maxStrings && reader.BaseStream.Position + 2 <= streamLength)
                {
                    long pos = reader.BaseStream.Position;

                    // Try reading as length-prefixed string
                    ushort length = reader.ReadUInt16();

                    // Sanity check: length should be reasonable (< 500 chars) and we need enough bytes
                    if (length > 0 && length < 500 && reader.BaseStream.Position + length <= streamLength)
                    {
                        byte[] strBytes = reader.ReadBytes(length);
                        string str = System.Text.Encoding.UTF8.GetString(strBytes);

                        // Check if it looks like a valid path/identifier (printable ASCII)
                        bool isValid = str.Length > 0 && !str.Contains('\0');
                        if (isValid)
                        {
                            foreach (char c in str)
                            {
                                if (c < 32 || c > 126)
                                {
                                    isValid = false;
                                    break;
                                }
                            }
                        }

                        if (isValid)
                        {
                            TableOfContents.Add(str);
                            stringsRead++;
                            continue;
                        }
                    }

                    // If that didn't work, stop reading strings
                    reader.BaseStream.Position = pos;
                    break;
                }

                System.Diagnostics.Debug.WriteLine($"[CCDB] Read {stringsRead} ToC entries, stopped at offset 0x{reader.BaseStream.Position:X}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Error reading string table: {ex.Message}");
            }
        }

        private void ParseBinaryStructuredData(byte[] data, BinaryReader reader, int profileCount, int choiceCount, int pieceCount)
        {
            // Based on IDA analysis, C_SpawnProfile structure in binary:
            // - m_Id: 8 bytes (FNV64 hash)
            // - m_Choices: array stored as count (4 bytes) + elements (count * 8 bytes for S_WeightedChoice)
            // - m_Packages: array stored as count (4 bytes) + elements (count * 8 bytes per package hash)
            // - m_Priority: 4 bytes (s32)
            // - m_MinimumPackagesToLoad: 4 bytes (u32)
            // - m_DesiredPackageMinimum: 4 bytes (u32)
            // - m_Flags: 4 bytes (u32)

            // S_WeightedChoice structure (8 bytes):
            // - m_ChoiceIndex: 4 bytes (u32)
            // - m_Weight: 4 bytes (float)

            long scanStart = reader.BaseStream.Position;
            long streamLength = reader.BaseStream.Length;

            System.Diagnostics.Debug.WriteLine($"[CCDB] ParseBinaryStructuredData: scanStart=0x{scanStart:X}, streamLength={streamLength}");

            // Try to find the start of spawn profile data by looking for the pattern
            // Scan through data looking for valid profile structures
            List<long> potentialProfileStarts = new List<long>();

            reader.BaseStream.Position = scanStart;

            // Limit scanning to avoid performance issues - scan first 1MB of data section
            long maxScanEnd = Math.Min(scanStart + 1024 * 1024, streamLength - 32);

            while (reader.BaseStream.Position < maxScanEnd)
            {
                long pos = reader.BaseStream.Position;

                // Try reading as C_SpawnProfile
                try
                {
                    // Need at least 32 bytes for minimum profile structure
                    if (pos + 32 > streamLength)
                        break;

                    ulong hash = reader.ReadUInt64();
                    uint choicesCount = reader.ReadUInt32();

                    // Validate: hash should look valid and choices count should be reasonable
                    if (IsValidFNV64Hash(hash) && choicesCount <= 100)
                    {
                        // Calculate total required size for this potential profile
                        long afterChoices = reader.BaseStream.Position + (choicesCount * 8);
                        if (afterChoices + 20 <= streamLength)
                        {
                            reader.BaseStream.Position = afterChoices;
                            uint packagesCount = reader.ReadUInt32();

                            if (packagesCount <= 100)
                            {
                                long afterPackages = reader.BaseStream.Position + (packagesCount * 8);
                                if (afterPackages + 16 <= streamLength)
                                {
                                    reader.BaseStream.Position = afterPackages;
                                    int priority = reader.ReadInt32();
                                    uint minPkg = reader.ReadUInt32();
                                    uint desiredMin = reader.ReadUInt32();
                                    uint flags = reader.ReadUInt32();

                                    // Validate priority and other fields
                                    if (priority >= 0 && priority <= 10000 && minPkg <= 100 && desiredMin <= 100)
                                    {
                                        potentialProfileStarts.Add(pos);

                                        // If we found enough candidates, stop scanning
                                        if (potentialProfileStarts.Count >= 10)
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[CCDB] Scan exception at 0x{pos:X}: {ex.Message}");
                }

                // Move to next 8-byte aligned position for faster scanning
                reader.BaseStream.Position = pos + 8;
            }

            System.Diagnostics.Debug.WriteLine($"[CCDB] Found {potentialProfileStarts.Count} potential profile starts");

            // If we found potential starts, try parsing profiles
            if (potentialProfileStarts.Count > 0)
            {
                // Find a sequence of consecutive profiles
                TryParseProfilesFromPosition(reader, potentialProfileStarts[0], profileCount);
            }

            // If no profiles found yet, fall back to legacy scanning (but with bounds checking)
            if (SpawnProfiles.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine($"[CCDB] Structured parsing found no profiles, skipping legacy scan to avoid errors");
                // Don't call the old scanning methods as they may have bounds issues
            }
        }

        private bool IsValidFNV64Hash(ulong value)
        {
            if (value == 0 || value == ulong.MaxValue)
                return false;

            // FNV64 hashes have good bit distribution
            // Check that it's not a simple pattern
            int setBits = 0;
            ulong v = value;
            while (v != 0)
            {
                setBits += (int)(v & 1);
                v >>= 1;
            }

            // Valid FNV64 hashes typically have 24-48 bits set
            return setBits >= 20 && setBits <= 50;
        }

        private void TryParseProfilesFromPosition(BinaryReader reader, long startPos, int expectedCount)
        {
            reader.BaseStream.Position = startPos;
            int parsedCount = 0;
            int maxProfiles = Math.Max(expectedCount, 3000);
            long streamLength = reader.BaseStream.Length;

            System.Diagnostics.Debug.WriteLine($"[CCDB] Trying to parse profiles from offset 0x{startPos:X}, stream length: {streamLength}");

            while (parsedCount < maxProfiles && reader.BaseStream.Position + 32 <= streamLength)
            {
                long profileStart = reader.BaseStream.Position;

                try
                {
                    // Check we have enough bytes for minimum profile (8 + 4 + 4 + 16 = 32 bytes minimum)
                    if (reader.BaseStream.Position + 32 > streamLength)
                        break;

                    // Read C_SpawnProfile fields
                    ulong id = reader.ReadUInt64();

                    if (!IsValidFNV64Hash(id))
                        break;

                    // Read choices array
                    uint choicesCount = reader.ReadUInt32();
                    if (choicesCount > 100)
                        break;

                    // Check we have enough bytes for choices + remaining data
                    long requiredBytes = (choicesCount * 8) + 4 + 16; // choices + packagesCount + tail fields
                    if (reader.BaseStream.Position + requiredBytes > streamLength)
                        break;

                    List<CCDBWeightedChoice> choices = new List<CCDBWeightedChoice>();
                    for (int i = 0; i < choicesCount; i++)
                    {
                        uint choiceIndex = reader.ReadUInt32();
                        float weight = reader.ReadSingle();
                        choices.Add(new CCDBWeightedChoice { ChoiceIndex = choiceIndex, Weight = weight });
                    }

                    // Read packages array
                    uint packagesCount = reader.ReadUInt32();
                    if (packagesCount > 100)
                        break;

                    // Check we have enough bytes for packages + remaining data
                    requiredBytes = (packagesCount * 8) + 16; // packages + tail fields
                    if (reader.BaseStream.Position + requiredBytes > streamLength)
                        break;

                    List<CCDBPackage> packages = new List<CCDBPackage>();
                    for (int i = 0; i < packagesCount; i++)
                    {
                        ulong packageHash = reader.ReadUInt64();
                        packages.Add(new CCDBPackage { Id = new CCDBHashName(packageHash) });
                    }

                    // Read remaining fields
                    int priority = reader.ReadInt32();
                    uint minPackages = reader.ReadUInt32();
                    uint desiredMin = reader.ReadUInt32();
                    uint flags = reader.ReadUInt32();

                    // Validate
                    if (priority < 0 || priority > 10000)
                        break;

                    // Create profile
                    CCDBSpawnProfile profile = new CCDBSpawnProfile
                    {
                        Id = new CCDBHashName(id),
                        Choices = choices,
                        Packages = packages,
                        Priority = priority,
                        MinimumPackagesToLoad = minPackages,
                        DesiredPackageMinimum = desiredMin,
                        Flags = flags,
                        Ref = $"Profile_{parsedCount}"
                    };

                    SpawnProfiles.Add(profile);
                    parsedCount++;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[CCDB] Exception at offset 0x{profileStart:X}: {ex.Message}");
                    break;
                }
            }

            System.Diagnostics.Debug.WriteLine($"[CCDB] Parsed {parsedCount} profiles with structured parsing");

            int withChoices = SpawnProfiles.Count(p => p.Choices.Count > 0);
            int withPackages = SpawnProfiles.Count(p => p.Packages.Count > 0);
            System.Diagnostics.Debug.WriteLine($"[CCDB] Profiles with choices: {withChoices}, with packages: {withPackages}");
        }

        private void ParseBinaryContent(byte[] data, BinaryReader reader, long dataStart)
        {
            // Find C_CombinablePiece instances
            int combinablePieceIndex = _binaryTypeHashes?.IndexOf(TYPE_C_COMBINABLE_PIECE) ?? -1;
            int combinablePieceCount = (combinablePieceIndex >= 0 && _binaryTypeCounts != null && combinablePieceIndex < _binaryTypeCounts.Count)
                ? _binaryTypeCounts[combinablePieceIndex]
                : 0;

            // Find C_SpawnProfile instances
            int spawnProfileIndex = _binaryTypeHashes?.IndexOf(TYPE_C_SPAWN_PROFILE) ?? -1;
            int spawnProfileCount = (spawnProfileIndex >= 0 && _binaryTypeCounts != null && spawnProfileIndex < _binaryTypeCounts.Count)
                ? _binaryTypeCounts[spawnProfileIndex]
                : 0;

            // Find C_Choice instances
            int choiceIndex = _binaryTypeHashes?.IndexOf(TYPE_C_CHOICE) ?? -1;
            int choiceCount = (choiceIndex >= 0 && _binaryTypeCounts != null && choiceIndex < _binaryTypeCounts.Count)
                ? _binaryTypeCounts[choiceIndex]
                : 0;

            // Find ToC entries
            int tocIndex = _binaryTypeHashes?.IndexOf(TYPE_S_TOC_OBJ) ?? -1;
            int tocCount = (tocIndex >= 0 && _binaryTypeCounts != null && tocIndex < _binaryTypeCounts.Count)
                ? _binaryTypeCounts[tocIndex]
                : 0;

            // Use heuristic pattern matching to find data
            ParseBinaryCombinables(data, reader);
            ParseBinarySpawnProfiles(data, reader);

            // Add summary info to header
            int parsedProfiles = SpawnProfiles.Count;
            int profilesWithData = SpawnProfiles.Count(p => p.Choices.Count > 0 || p.Packages.Count > 0);
            HeaderVersion = $"v{Version} (Types: {_binaryTypeHashes?.Count ?? 0}, " +
                $"Expected: {spawnProfileCount} profiles, {choiceCount} choices, {combinablePieceCount} pieces | " +
                $"Parsed: {parsedProfiles} profiles, {profilesWithData} with data)";
        }

        private long FindPattern(byte[] data, byte[] pattern, long startOffset)
        {
            for (long i = startOffset; i < data.Length - pattern.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (data[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match) return i;
            }
            return -1;
        }

        private long FindString(byte[] data, string str)
        {
            byte[] pattern = Encoding.ASCII.GetBytes(str);
            return FindPattern(data, pattern, 0);
        }

        private void ParseBinaryCombinables(byte[] data, BinaryReader reader)
        {
            // C_CombinablePiece structure (from IDA):
            // - m_PieceId at offset 0 (8 bytes)
            // - m_TargetPackageId at offset 8 (8 bytes)
            // - m_TemplateId at offset 16 (8 bytes)
            // Total size: 24 bytes

            // Get expected count from type registry
            int expectedCount = 0;
            int typeIndex = _binaryTypeHashes?.IndexOf(TYPE_C_COMBINABLE_PIECE) ?? -1;
            if (typeIndex >= 0 && _binaryTypeCounts != null && typeIndex < _binaryTypeCounts.Count)
            {
                expectedCount = _binaryTypeCounts[typeIndex];
            }

            // Find the data section containing combinable pieces
            // Look for array header patterns or scan for valid hash sequences
            long searchStart = reader.BaseStream.Position;
            HashSet<ulong> seenHashes = new HashSet<ulong>();
            int foundPieces = 0;
            int maxPieces = Math.Max(expectedCount * 2, 5000); // Allow some margin

            // Scan through data looking for valid piece patterns
            reader.BaseStream.Position = searchStart;
            while (reader.BaseStream.Position < data.Length - CCDBCombinablePiece.BinarySize && foundPieces < maxPieces)
            {
                long pos = reader.BaseStream.Position;

                try
                {
                    ulong hash1 = reader.ReadUInt64();
                    ulong hash2 = reader.ReadUInt64();
                    ulong hash3 = reader.ReadUInt64();

                    // Validate: all three should be valid FNV64 hashes and not duplicates
                    if (hash1 != 0 && hash2 != 0 && hash3 != 0 &&
                        !seenHashes.Contains(hash1) &&
                        IsLikelyHash(hash1) && IsLikelyHash(hash2) && IsLikelyHash(hash3))
                    {
                        CCDBCombinablePiece piece = new CCDBCombinablePiece();
                        piece.PieceId = new CCDBHashName(hash1);
                        piece.TargetPackageId = new CCDBHashName(hash2);
                        piece.TemplateId = new CCDBHashName(hash3);
                        CombinablePieces.Add(piece);
                        seenHashes.Add(hash1);
                        foundPieces++;

                        // Continue from end of this piece
                        continue;
                    }
                }
                catch
                {
                    break;
                }

                // Move to next 8-byte aligned position
                reader.BaseStream.Position = pos + 8;
            }
        }

        private void ParseBinarySpawnProfiles(byte[] data, BinaryReader reader)
        {
            // C_SpawnProfile structure (from IDA):
            // - m_Id at offset 0 (8 bytes, C_HashName)
            // - m_Choices at offset 8 (array - 24 bytes for std::vector)
            // - m_Packages at offset 32 (array - 24 bytes for std::vector)
            // - m_Priority at offset 56 (int, 4 bytes)
            // - m_MinimumPackagesToLoad at offset 60 (int, 4 bytes)
            // - m_DesiredPackageMinimum at offset 64 (int, 4 bytes)
            // - m_Flags at offset 68 (uint, 4 bytes)

            // Get expected count from type registry
            int expectedCount = 0;
            int typeIndex = _binaryTypeHashes?.IndexOf(TYPE_C_SPAWN_PROFILE) ?? -1;
            if (typeIndex >= 0 && _binaryTypeCounts != null && typeIndex < _binaryTypeCounts.Count)
            {
                expectedCount = _binaryTypeCounts[typeIndex];
            }

            // For now, scan for potential profile data patterns
            // Profiles start with a valid hash ID, followed by array headers
            HashSet<ulong> seenIds = new HashSet<ulong>();
            int foundProfiles = 0;
            int maxProfiles = Math.Max(expectedCount * 2, 1000);

            // Reset reader position to start of data section
            long searchStart = reader.BaseStream.Position;
            reader.BaseStream.Position = searchStart;

            while (reader.BaseStream.Position < data.Length - 72 && foundProfiles < maxProfiles)
            {
                long pos = reader.BaseStream.Position;

                try
                {
                    // Try to read a profile ID (8 bytes hash)
                    ulong profileId = reader.ReadUInt64();

                    // Profile IDs should be valid FNV64 hashes
                    if (profileId != 0 && !seenIds.Contains(profileId) && IsLikelyHash(profileId))
                    {
                        // Skip to priority field (offset 56 from start)
                        reader.BaseStream.Position = pos + 56;

                        // Read profile settings
                        int priority = reader.ReadInt32();
                        int minPackages = reader.ReadInt32();
                        int desiredMin = reader.ReadInt32();
                        uint flags = reader.ReadUInt32();

                        // Validate: priority should be reasonable (0-1000 typical range)
                        if (priority >= 0 && priority <= 10000 && minPackages >= 0 && desiredMin >= 0)
                        {
                            CCDBSpawnProfile profile = new CCDBSpawnProfile();
                            profile.Id = new CCDBHashName(profileId);
                            profile.Priority = priority;
                            profile.MinimumPackagesToLoad = (uint)minPackages;
                            profile.DesiredPackageMinimum = (uint)desiredMin;
                            profile.Flags = flags;
                            profile.Ref = $"Profile_{foundProfiles}";

                            SpawnProfiles.Add(profile);
                            seenIds.Add(profileId);
                            foundProfiles++;

                            // Move past this profile structure
                            reader.BaseStream.Position = pos + 72;
                            continue;
                        }
                    }
                }
                catch
                {
                    break;
                }

                // Move to next 8-byte aligned position
                reader.BaseStream.Position = pos + 8;
            }

            // If no profiles found via scanning, create placeholders from type registry count
            if (SpawnProfiles.Count == 0 && expectedCount > 0)
            {
                for (int i = 0; i < Math.Min(expectedCount, 1000); i++)
                {
                    CCDBSpawnProfile profile = new CCDBSpawnProfile();
                    profile.Ref = $"Profile_{i}";
                    profile.Id = new CCDBHashName((ulong)(0x1000000000000000 + i));
                    profile.Priority = 100;
                    SpawnProfiles.Add(profile);
                }
            }
        }

        private bool IsLikelyHash(ulong value)
        {
            // FNV64 hashes typically have good bit distribution
            // Reject values that are too simple (sequential, all same nibble, etc)
            if (value == 0 || value == ulong.MaxValue) return false;

            // Count set bits - good hashes have roughly half set
            int bits = 0;
            ulong v = value;
            while (v != 0)
            {
                bits += (int)(v & 1);
                v >>= 1;
            }

            return bits >= 16 && bits <= 48; // Reasonable range for FNV64
        }

        private byte[] _rawBinaryData;

        private void ParseDocument(XDocument doc)
        {
            XElement root = doc.Root;
            if (root == null || root.Name.LocalName != "Compound")
            {
                throw new InvalidDataException("Invalid CCDB file: missing Compound root element");
            }

            // Parse header attributes
            Version = int.TryParse(root.Attribute("Version")?.Value, out int ver) ? ver : 0;
            HeaderVersion = root.Attribute("HeaderVersion")?.Value ?? "";
            RootTypeID = root.Attribute("RootTypeID")?.Value ?? "";
            MemoryFootprint = int.TryParse(root.Attribute("MemoryFootprint")?.Value, out int mem) ? mem : 0;

            System.Diagnostics.Debug.WriteLine($"[CCDB] Parsing document, root element: {root.Name.LocalName}");
            System.Diagnostics.Debug.WriteLine($"[CCDB] Number of 'root' children: {root.Elements("root").Count()}");

            // Find the ToC and Content sections
            foreach (XElement rootSection in root.Elements("root"))
            {
                string compoundId = rootSection.Attribute("CompoundId")?.Value;
                System.Diagnostics.Debug.WriteLine($"[CCDB] Found root section: CompoundId={compoundId}");

                if (compoundId == "ToC")
                {
                    ParseTableOfContents(rootSection);
                }
                else if (compoundId == "Content")
                {
                    ParseContent(rootSection);
                    System.Diagnostics.Debug.WriteLine($"[CCDB] After ParseContent: {SpawnProfiles.Count} profiles");
                }
            }
        }

        private void ParseTableOfContents(XElement tocSection)
        {
            TableOfContents.Clear();

            // Navigate to Items element
            XElement items = tocSection.Descendants("Items").FirstOrDefault();
            if (items != null)
            {
                foreach (XElement tocObj in items.Elements("S_ToCObj"))
                {
                    TableOfContents.Add(tocObj.Value);
                }
            }
        }

        private void ParseContent(XElement contentSection)
        {
            CombinablePieces.Clear();
            PieceSetMappings.Clear();
            SpawnProfiles.Clear();
            SharedChoices.Clear();
            PieceSets.Clear();
            Ranges.Clear();

            // Parse combinable pieces
            var combinablePieceElements = contentSection.Descendants("C_CombinablePiece").ToList();
            foreach (XElement pieceElem in combinablePieceElements)
            {
                CombinablePieces.Add(CCDBCombinablePiece.FromXElement(pieceElem));
            }

            // Parse piece sets
            var pieceSetElements = contentSection.Descendants("C_PieceSet").ToList();
            foreach (XElement pieceSetElem in pieceSetElements)
            {
                PieceSets.Add(CCDBPieceSet.FromXElement(pieceSetElem));
            }

            // Parse piece set mappings - search for pair elements within m_PieceSetsByCombinableId
            var pieceSetMappingContainer = contentSection.Descendants("m_PieceSetsByCombinableId").FirstOrDefault();
            if (pieceSetMappingContainer != null)
            {
                var itemsElement = pieceSetMappingContainer.Element("Items");
                if (itemsElement != null)
                {
                    foreach (XElement pairElem in itemsElement.Elements("pair"))
                    {
                        PieceSetMappings.Add(CCDBPieceSetMapping.FromXElement(pairElem));
                    }
                }
            }

            // Parse spawn profiles
            var spawnProfileElements = contentSection.Descendants("C_SpawnProfile").ToList();
            System.Diagnostics.Debug.WriteLine($"[CCDB] Found {spawnProfileElements.Count} C_SpawnProfile elements");
            foreach (XElement profileElem in spawnProfileElements)
            {
                var profile = CCDBSpawnProfile.FromXElement(profileElem);
                SpawnProfiles.Add(profile);
            }
            // Debug: count profiles with choices
            int withChoices = SpawnProfiles.Count(p => p.Choices.Count > 0);
            int withPackages = SpawnProfiles.Count(p => p.Packages.Count > 0);
            System.Diagnostics.Debug.WriteLine($"[CCDB] Profiles with choices: {withChoices}, with packages: {withPackages}");

            // Parse shared choices
            var choiceElements = contentSection.Descendants("C_Choice").ToList();
            System.Diagnostics.Debug.WriteLine($"[CCDB] Found {choiceElements.Count} C_Choice elements");
            foreach (XElement choiceElem in choiceElements)
            {
                SharedChoices.Add(CCDBChoice.FromXElement(choiceElem));
            }

            // Parse ranges (C_RangeWithChance or C_Range)
            var rangeElements = contentSection.Descendants("C_RangeWithChance").ToList();
            foreach (XElement rangeElem in rangeElements)
            {
                Ranges.Add(CCDBRange.FromXElement(rangeElem));
            }
        }

        public void WriteToFile(FileInfo fileInfo)
        {
            // For now, we save the original document/data (preserving structure)
            if (_originalDocument != null)
            {
                _originalDocument.Save(fileInfo.FullName);
            }
            else if (_rawBinaryData != null)
            {
                // Binary format - just save the raw data back
                File.WriteAllBytes(fileInfo.FullName, _rawBinaryData);
            }
        }

        public bool IsBinaryFormat => _rawBinaryData != null;
        public bool IsXmlFormat => _isXmlFormat;

        /// <summary>
        /// Builds tree nodes for display in the editor
        /// </summary>
        public TreeNode GetAsTreeNodes()
        {
            string formatType = IsBinaryFormat ? "Binary" : (IsXmlFormat ? "XML" : "Unknown");
            TreeNode rootNode = new TreeNode($"CCDB [{formatType}] (v{Version})");
            rootNode.Tag = this;

            // Header info node
            TreeNode headerNode = new TreeNode("Header");
            headerNode.Tag = this;
            headerNode.Nodes.Add(new TreeNode($"Format: {formatType}"));
            headerNode.Nodes.Add(new TreeNode($"Version: {Version}"));
            headerNode.Nodes.Add(new TreeNode($"HeaderVersion: {HeaderVersion}"));
            headerNode.Nodes.Add(new TreeNode($"RootTypeID: {RootTypeID}"));
            if (_originalDocument != null)
            {
                headerNode.Nodes.Add(new TreeNode($"XML Root: {_originalDocument.Root?.Name.LocalName ?? "null"}"));
                var rootSections = _originalDocument.Root?.Elements("root").ToList();
                headerNode.Nodes.Add(new TreeNode($"Root sections found: {rootSections?.Count ?? 0}"));
            }
            rootNode.Nodes.Add(headerNode);

            // Table of Contents node
            TreeNode tocNode = new TreeNode($"Table of Contents ({TableOfContents.Count} entries)");
            for (int i = 0; i < Math.Min(TableOfContents.Count, 100); i++)
            {
                TreeNode entryNode = new TreeNode(TableOfContents[i]);
                tocNode.Nodes.Add(entryNode);
            }
            if (TableOfContents.Count > 100)
            {
                tocNode.Nodes.Add(new TreeNode($"... and {TableOfContents.Count - 100} more"));
            }
            rootNode.Nodes.Add(tocNode);

            // Combinable Pieces node
            TreeNode piecesNode = new TreeNode($"Combinable Pieces ({CombinablePieces.Count})");
            foreach (CCDBCombinablePiece piece in CombinablePieces)
            {
                TreeNode pieceNode = new TreeNode(piece.ToString());
                pieceNode.Tag = piece;
                piecesNode.Nodes.Add(pieceNode);
            }
            rootNode.Nodes.Add(piecesNode);

            // Piece Set Mappings node
            TreeNode mappingsNode = new TreeNode($"Piece Set Mappings ({PieceSetMappings.Count})");
            foreach (CCDBPieceSetMapping mapping in PieceSetMappings)
            {
                TreeNode mappingNode = new TreeNode(mapping.ToString());
                mappingNode.Tag = mapping;
                mappingsNode.Nodes.Add(mappingNode);
            }
            rootNode.Nodes.Add(mappingsNode);

            // Spawn Profiles node - sort so profiles with choices/packages appear first
            int withData = SpawnProfiles.Count(p => p.Choices.Count > 0 || p.Packages.Count > 0);
            TreeNode profilesNode = new TreeNode($"Spawn Profiles ({SpawnProfiles.Count}, {withData} with data)");
            var sortedProfiles = SpawnProfiles.OrderByDescending(p => p.Choices.Count + p.Packages.Count);
            foreach (CCDBSpawnProfile profile in sortedProfiles)
            {
                TreeNode profileNode = new TreeNode(profile.ToString());
                profileNode.Tag = profile;

                // Add Choices as child nodes (these are weighted choice references)
                if (profile.Choices.Count > 0)
                {
                    TreeNode choicesChildNode = new TreeNode($"Choices ({profile.Choices.Count})");
                    for (int i = 0; i < profile.Choices.Count; i++)
                    {
                        CCDBWeightedChoice wc = profile.Choices[i];
                        TreeNode choiceChildNode = new TreeNode($"[{i}] {wc.ToString()}");
                        choiceChildNode.Tag = wc;
                        choicesChildNode.Nodes.Add(choiceChildNode);
                    }
                    profileNode.Nodes.Add(choicesChildNode);
                }

                // Add Packages as child nodes
                if (profile.Packages.Count > 0)
                {
                    TreeNode packagesChildNode = new TreeNode($"Packages ({profile.Packages.Count})");
                    for (int i = 0; i < profile.Packages.Count; i++)
                    {
                        CCDBPackage package = profile.Packages[i];
                        TreeNode packageChildNode = new TreeNode($"[{i}] {package.ToString()}");
                        packageChildNode.Tag = package;
                        packagesChildNode.Nodes.Add(packageChildNode);
                    }
                    profileNode.Nodes.Add(packagesChildNode);
                }

                profilesNode.Nodes.Add(profileNode);
            }
            rootNode.Nodes.Add(profilesNode);

            // Shared Choices node
            TreeNode choicesNode = new TreeNode($"Shared Choices ({SharedChoices.Count})");
            for (int i = 0; i < SharedChoices.Count; i++)
            {
                CCDBChoice choice = SharedChoices[i];
                TreeNode choiceNode = new TreeNode($"[{i}] {choice.ToString()}");
                choiceNode.Tag = choice;

                // Add Tags as child nodes
                if (choice.Tags.Count > 0)
                {
                    TreeNode tagsChildNode = new TreeNode($"Tags ({choice.Tags.Count})");
                    for (int j = 0; j < choice.Tags.Count; j++)
                    {
                        CCDBTag tag = choice.Tags[j];
                        TreeNode tagChildNode = new TreeNode($"[{j}] {tag.ToString()}");
                        tagChildNode.Tag = tag;

                        // Add tag values as child nodes
                        if (tag.Values.Count > 0)
                        {
                            foreach (CCDBTagValue value in tag.Values)
                            {
                                TreeNode valueNode = new TreeNode(value.ToString());
                                valueNode.Tag = value;
                                tagChildNode.Nodes.Add(valueNode);
                            }
                        }
                        tagsChildNode.Nodes.Add(tagChildNode);
                    }
                    choiceNode.Nodes.Add(tagsChildNode);
                }

                choicesNode.Nodes.Add(choiceNode);
            }
            rootNode.Nodes.Add(choicesNode);

            // Piece Sets node
            TreeNode pieceSetsNode = new TreeNode($"Piece Sets ({PieceSets.Count})");
            foreach (CCDBPieceSet pieceSet in PieceSets)
            {
                TreeNode pieceSetNode = new TreeNode(pieceSet.ToString());
                pieceSetNode.Tag = pieceSet;
                pieceSetsNode.Nodes.Add(pieceSetNode);
            }
            rootNode.Nodes.Add(pieceSetsNode);

            // Ranges node
            TreeNode rangesNode = new TreeNode($"Ranges ({Ranges.Count})");
            foreach (CCDBRange range in Ranges)
            {
                TreeNode rangeNode = new TreeNode(range.ToString());
                rangeNode.Tag = range;
                rangesNode.Nodes.Add(rangeNode);
            }
            rootNode.Nodes.Add(rangesNode);

            // Type Registry node (for binary files)
            if (IsBinaryFormat && _binaryTypeHashes != null && _binaryTypeHashes.Count > 0)
            {
                TreeNode typeRegNode = new TreeNode($"Type Registry ({_binaryTypeHashes.Count} types)");
                for (int i = 0; i < _binaryTypeHashes.Count; i++)
                {
                    uint hash = _binaryTypeHashes[i];
                    ushort count = (i < _binaryTypeCounts?.Count) ? _binaryTypeCounts[i] : (ushort)0;
                    string typeName = GetKnownTypeName(hash);
                    TreeNode typeNode = new TreeNode($"0x{hash:X8} [{typeName}] x{count}");
                    typeRegNode.Nodes.Add(typeNode);
                }
                rootNode.Nodes.Add(typeRegNode);
            }

            return rootNode;
        }

        private string GetKnownTypeName(uint hash)
        {
            return hash switch
            {
                TYPE_C_COMBINABLE_PIECE => "C_CombinablePiece",
                TYPE_C_SPAWN_PROFILE_DB_RESOURCE => "C_SpawnProfileDBResource",
                TYPE_C_SPAWN_PROFILE_DB => "C_SpawnProfileDB",
                TYPE_C_SPAWN_PROFILE => "C_SpawnProfile",
                TYPE_C_CHOICE => "C_Choice",
                TYPE_C_PIECE_SET => "C_PieceSet",
                TYPE_S_TOC_OBJ => "S_ToCObj",
                _ => "Unknown"
            };
        }
    }
}
