using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // Use the existing ArchiveFile class which handles Oodle compression
            var archive = new ArchiveFile();

            // Set game type if provided
            if (gameType.HasValue)
            {
                archive.SetGameType(gameType.Value);
            }

            // Read the SDS file using existing toolkit code
            using var inputStream = File.OpenRead(normalizedPath);
            archive.Deserialize(inputStream);

            // Try to detect game type from version
            var detectedGame = archive.Version switch
            {
                19 => gameType ?? GamesEnumerator.MafiaII,
                20 => gameType ?? GamesEnumerator.MafiaIII,
                _ => gameType ?? GamesEnumerator.MafiaII
            };

            // Build resource info list
            var resources = new List<SdsResourceInfo>();
            for (int i = 0; i < archive.ResourceEntries.Count; i++)
            {
                var entry = archive.ResourceEntries[i];
                var typeName = entry.TypeId >= 0 && entry.TypeId < archive.ResourceTypes.Count
                    ? archive.ResourceTypes[entry.TypeId].Name
                    : "Unknown";

                var name = i < archive.ResourceNames.Count ? archive.ResourceNames[i] : null;

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
                    Name = name
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

            // Cache the result with the archive's resource entries
            _cache[normalizedPath] = new CachedSdsFile
            {
                Info = sdsInfo,
                Entries = archive.ResourceEntries,
                LastAccessed = DateTime.UtcNow
            };

            // Clean up old cache entries
            CleanupCache();

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
}
