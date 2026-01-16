using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using Gibbed.Illusion.FileFormats;
using Gibbed.IO;
using Gibbed.Mafia2.FileFormats;
using Gibbed.Mafia2.FileFormats.Archive;
using Utils.Settings;

namespace Mafia2Tool.MCP.Services;

/// <summary>
/// Information about an opened SDS archive
/// </summary>
public class SdsFileInfo
{
    public string FilePath { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public long FileSize { get; init; }
    public uint Version { get; init; }
    public Platform Platform { get; init; }
    public Endian Endian { get; init; }
    public uint SlotRamRequired { get; init; }
    public uint SlotVramRequired { get; init; }
    public uint OtherRamRequired { get; init; }
    public uint OtherVramRequired { get; init; }
    public List<SdsResourceTypeInfo> ResourceTypes { get; init; } = new();
    public List<SdsResourceInfo> Resources { get; init; } = new();
    public string? ResourceInfoXml { get; init; }
    public GamesEnumerator DetectedGameType { get; init; }
}

/// <summary>
/// Information about a resource type in an SDS archive
/// </summary>
public class SdsResourceTypeInfo
{
    public uint Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public uint Parent { get; init; }
}

/// <summary>
/// Information about a resource entry in an SDS archive
/// </summary>
public class SdsResourceInfo
{
    public int Index { get; init; }
    public int TypeId { get; init; }
    public string TypeName { get; init; } = string.Empty;
    public ushort Version { get; init; }
    public int DataSize { get; init; }
    public ulong FileHash { get; init; }
    public uint SlotRamRequired { get; init; }
    public uint SlotVramRequired { get; init; }
    public uint OtherRamRequired { get; init; }
    public uint OtherVramRequired { get; init; }
    public string? Name { get; init; }
}

/// <summary>
/// Service for reading and caching SDS archive files
/// </summary>
public class SdsService
{
    private readonly ConcurrentDictionary<string, CachedSdsFile> _cache = new();
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);

    private class CachedSdsFile
    {
        public SdsFileInfo Info { get; init; } = null!;
        public List<ResourceEntry> Entries { get; init; } = new();
        public DateTime LastAccessed { get; set; }
    }

    /// <summary>
    /// Lists all SDS files in a directory
    /// </summary>
    public List<(string Path, string Name, long Size)> ListSdsFiles(string directoryPath, bool recursive = true)
    {
        var results = new List<(string Path, string Name, long Size)>();

        if (!Directory.Exists(directoryPath))
        {
            return results;
        }

        var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var files = Directory.GetFiles(directoryPath, "*.sds", searchOption);

        foreach (var file in files)
        {
            try
            {
                var info = new FileInfo(file);
                results.Add((file, info.Name, info.Length));
            }
            catch
            {
                // Skip files we can't access
            }
        }

        return results;
    }

    /// <summary>
    /// Last error encountered during file operations
    /// </summary>
    public string? LastError { get; private set; }

    /// <summary>
    /// Opens an SDS file and returns its metadata
    /// </summary>
    public SdsFileInfo? OpenFile(string filePath, GamesEnumerator? gameType = null)
    {
        LastError = null;
        var normalizedPath = Path.GetFullPath(filePath);

        // Check cache first
        if (_cache.TryGetValue(normalizedPath, out var cached))
        {
            cached.LastAccessed = DateTime.UtcNow;
            return cached.Info;
        }

        if (!File.Exists(normalizedPath))
        {
            LastError = $"File not found: {normalizedPath}";
            return null;
        }

        try
        {
            var fileInfo = new FileInfo(normalizedPath);

            // Read the SDS file
            using var inputStream = File.OpenRead(normalizedPath);

            // Try to unwrap encrypted files
            Stream dataStream;
            var unwrapped = ArchiveEncryption.Unwrap(inputStream);
            if (unwrapped != null)
            {
                dataStream = unwrapped;
            }
            else
            {
                inputStream.Position = 0;
                dataStream = inputStream;
            }

            var archive = ReadArchive(dataStream, out var resourceEntries);

            // Try to detect game type from version
            var detectedGame = archive.Version switch
            {
                19 => gameType ?? GamesEnumerator.MafiaII,
                20 => gameType ?? GamesEnumerator.MafiaIII,
                _ => gameType ?? GamesEnumerator.MafiaII
            };

            // Extract resource names from XML if available
            var resourceNames = ExtractResourceNames(archive.ResourceInfoXml, resourceEntries.Count);

            // Build resource info list
            var resources = new List<SdsResourceInfo>();
            for (int i = 0; i < resourceEntries.Count; i++)
            {
                var entry = resourceEntries[i];
                var typeName = entry.TypeId >= 0 && entry.TypeId < archive.ResourceTypes.Count
                    ? archive.ResourceTypes[entry.TypeId].Name
                    : "Unknown";

                resources.Add(new SdsResourceInfo
                {
                    Index = i,
                    TypeId = entry.TypeId,
                    TypeName = typeName,
                    Version = entry.Version,
                    DataSize = entry.Data?.Length ?? 0,
                    FileHash = entry.FileHash,
                    SlotRamRequired = entry.SlotRamRequired,
                    SlotVramRequired = entry.SlotVramRequired,
                    OtherRamRequired = entry.OtherRamRequired,
                    OtherVramRequired = entry.OtherVramRequired,
                    Name = i < resourceNames.Count ? resourceNames[i] : null
                });
            }

            var sdsInfo = new SdsFileInfo
            {
                FilePath = normalizedPath,
                FileName = fileInfo.Name,
                FileSize = fileInfo.Length,
                Version = archive.Version,
                Platform = archive.Platform,
                Endian = archive.Endian,
                SlotRamRequired = archive.SlotRamRequired,
                SlotVramRequired = archive.SlotVramRequired,
                OtherRamRequired = archive.OtherRamRequired,
                OtherVramRequired = archive.OtherVramRequired,
                ResourceTypes = archive.ResourceTypes.Select(rt => new SdsResourceTypeInfo
                {
                    Id = rt.Id,
                    Name = rt.Name,
                    Parent = rt.Parent
                }).ToList(),
                Resources = resources,
                ResourceInfoXml = archive.ResourceInfoXml,
                DetectedGameType = detectedGame
            };

            // Cache the result
            _cache[normalizedPath] = new CachedSdsFile
            {
                Info = sdsInfo,
                Entries = resourceEntries,
                LastAccessed = DateTime.UtcNow
            };

            // Clean up old cache entries
            CleanupCache();

            // Dispose unwrapped stream if we created one
            unwrapped?.Dispose();

            return sdsInfo;
        }
        catch (Exception ex)
        {
            LastError = $"{ex.GetType().Name}: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"[SdsService] Error opening file: {ex}");
            return null;
        }
    }

    /// <summary>
    /// Gets detailed info about a specific resource
    /// </summary>
    public SdsResourceInfo? GetResourceInfo(string filePath, int resourceIndex)
    {
        var sdsInfo = OpenFile(filePath);
        if (sdsInfo == null || resourceIndex < 0 || resourceIndex >= sdsInfo.Resources.Count)
        {
            return null;
        }

        return sdsInfo.Resources[resourceIndex];
    }

    /// <summary>
    /// Extracts resource data as bytes
    /// </summary>
    public byte[]? ExtractResource(string filePath, int resourceIndex)
    {
        var normalizedPath = Path.GetFullPath(filePath);

        // Ensure file is opened/cached
        if (!_cache.TryGetValue(normalizedPath, out var cached))
        {
            OpenFile(filePath);
            if (!_cache.TryGetValue(normalizedPath, out cached))
            {
                return null;
            }
        }

        if (resourceIndex < 0 || resourceIndex >= cached.Entries.Count)
        {
            return null;
        }

        cached.LastAccessed = DateTime.UtcNow;
        return cached.Entries[resourceIndex].Data;
    }

    /// <summary>
    /// Searches resources by name pattern
    /// </summary>
    public List<SdsResourceInfo> SearchResources(string filePath, string pattern, string? typeFilter = null)
    {
        var sdsInfo = OpenFile(filePath);
        if (sdsInfo == null)
        {
            return new List<SdsResourceInfo>();
        }

        var results = sdsInfo.Resources.AsEnumerable();

        // Filter by type if specified
        if (!string.IsNullOrEmpty(typeFilter))
        {
            results = results.Where(r => r.TypeName.Contains(typeFilter, StringComparison.OrdinalIgnoreCase));
        }

        // Filter by name pattern
        if (!string.IsNullOrEmpty(pattern))
        {
            results = results.Where(r =>
                r.Name?.Contains(pattern, StringComparison.OrdinalIgnoreCase) == true ||
                r.TypeName.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }

        return results.ToList();
    }

    /// <summary>
    /// Closes a cached file
    /// </summary>
    public void CloseFile(string filePath)
    {
        var normalizedPath = Path.GetFullPath(filePath);
        _cache.TryRemove(normalizedPath, out _);
    }

    private ArchiveData ReadArchive(Stream input, out List<ResourceEntry> resourceEntries)
    {
        resourceEntries = new List<ResourceEntry>();
        var archive = new ArchiveData();

        var basePosition = input.Position;

        // Check magic
        var magic = input.ReadValueU32(Endian.Big);
        if (magic != 0x53445300) // 'SDS\0'
        {
            throw new FormatException($"Invalid SDS signature: 0x{magic:X8}");
        }

        input.Seek(8, SeekOrigin.Begin);
        var platform = (Platform)input.ReadValueU32(Endian.Big);
        var endian = platform == Platform.PC ? Endian.Little : Endian.Big;

        input.Seek(4, SeekOrigin.Begin);
        var version = input.ReadValueU32(endian);

        if (version != 19 && version != 20)
        {
            throw new FormatException($"Unsupported SDS version: {version}");
        }

        input.Seek(12, SeekOrigin.Begin);

        // Read file header
        using (var headerData = input.ReadToMemoryStreamSafe(52, endian))
        {
            archive.FileHeader = FileHeader.Read(headerData, endian);
        }

        // Read resource types
        input.Position = basePosition + archive.FileHeader.ResourceTypeTableOffset;
        var resourceTypeCount = input.ReadValueU32(endian);
        archive.ResourceTypes = new List<ResourceType>();
        for (uint i = 0; i < resourceTypeCount; i++)
        {
            archive.ResourceTypes.Add(ResourceType.Read(input, endian));
        }

        // Read resources from block stream
        input.Position = basePosition + archive.FileHeader.BlockTableOffset;
        var blockStream = BlockReaderStream.FromStream(input, endian);

        for (uint i = 0; i < archive.FileHeader.ResourceCount; i++)
        {
            var headerSize = version == 20 ? 34 : 26;
            ResourceHeader resourceHeader;
            using (var data = blockStream.ReadToMemoryStreamSafe(headerSize, endian))
            {
                resourceHeader = ResourceHeader.Read(data, endian, version);
            }

            if (resourceHeader.Size < 30)
            {
                throw new FormatException("Invalid resource header size");
            }

            var dataSize = (int)resourceHeader.Size - (headerSize + 4);
            var resourceData = blockStream.ReadBytes(dataSize);

            resourceEntries.Add(new ResourceEntry
            {
                TypeId = (int)resourceHeader.TypeId,
                Version = resourceHeader.Version,
                Data = resourceData,
                FileHash = resourceHeader.FileHash,
                SlotRamRequired = resourceHeader.SlotRamRequired,
                SlotVramRequired = resourceHeader.SlotVramRequired,
                OtherRamRequired = resourceHeader.OtherRamRequired,
                OtherVramRequired = resourceHeader.OtherVramRequired
            });
        }

        // Read XML info if present
        if (archive.FileHeader.XmlOffset != 0)
        {
            input.Position = basePosition + archive.FileHeader.XmlOffset;
            var xmlLength = (int)(input.Length - input.Position);
            if (xmlLength > 0)
            {
                archive.ResourceInfoXml = input.ReadString(xmlLength, Encoding.ASCII);
            }
        }

        archive.Version = version;
        archive.Platform = platform;
        archive.Endian = endian;
        archive.SlotRamRequired = archive.FileHeader.SlotRamRequired;
        archive.SlotVramRequired = archive.FileHeader.SlotVramRequired;
        archive.OtherRamRequired = archive.FileHeader.OtherRamRequired;
        archive.OtherVramRequired = archive.FileHeader.OtherVramRequired;

        return archive;
    }

    private List<string?> ExtractResourceNames(string? xml, int count)
    {
        var names = new List<string?>(new string?[count]);

        if (string.IsNullOrEmpty(xml))
        {
            return names;
        }

        try
        {
            using var reader = new StringReader(xml);
            var doc = new XPathDocument(reader);
            var nav = doc.CreateNavigator();
            var nodes = nav.Select("/xml/ResourceInfo/SourceDataDescription");

            int index = 0;
            while (nodes.MoveNext() && index < count)
            {
                var name = nodes.Current?.Value;
                if (!string.IsNullOrEmpty(name) && name != "not available")
                {
                    names[index] = name;
                }
                index++;
            }
        }
        catch
        {
            // Failed to parse XML, return empty names
        }

        return names;
    }

    private void CleanupCache()
    {
        var cutoff = DateTime.UtcNow - _cacheExpiration;
        var keysToRemove = _cache
            .Where(kvp => kvp.Value.LastAccessed < cutoff)
            .Select(kvp => kvp.Key)
            .ToList();

        foreach (var key in keysToRemove)
        {
            _cache.TryRemove(key, out _);
        }
    }

    private class ArchiveData
    {
        public uint Version { get; set; }
        public Platform Platform { get; set; }
        public Endian Endian { get; set; }
        public uint SlotRamRequired { get; set; }
        public uint SlotVramRequired { get; set; }
        public uint OtherRamRequired { get; set; }
        public uint OtherVramRequired { get; set; }
        public FileHeader FileHeader { get; set; }
        public List<ResourceType> ResourceTypes { get; set; } = new();
        public string? ResourceInfoXml { get; set; }
    }

    // Internal ResourceHeader struct since original is internal
    private struct ResourceHeader
    {
        public uint TypeId;
        public uint Size;
        public ushort Version;
        public ulong FileHash;
        public uint SlotRamRequired;
        public uint SlotVramRequired;
        public uint OtherRamRequired;
        public uint OtherVramRequired;

        public static ResourceHeader Read(Stream input, Endian endian, uint version)
        {
            ResourceHeader instance;
            instance.FileHash = 0;
            instance.TypeId = input.ReadValueU32(endian);
            instance.Size = input.ReadValueU32(endian);
            instance.Version = input.ReadValueU16(endian);

            if (version == 20)
            {
                instance.FileHash = input.ReadValueU64(endian);
            }

            instance.SlotRamRequired = input.ReadValueU32(endian);
            instance.SlotVramRequired = input.ReadValueU32(endian);
            instance.OtherRamRequired = input.ReadValueU32(endian);
            instance.OtherVramRequired = input.ReadValueU32(endian);

            return instance;
        }
    }
}
