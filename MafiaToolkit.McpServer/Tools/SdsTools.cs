using System.ComponentModel;
using System.Text.Json;
using MafiaToolkit.McpServer.Services;
using ModelContextProtocol.Server;
using Utils.Settings;

namespace MafiaToolkit.McpServer.Tools;

/// <summary>
/// MCP tools for browsing and inspecting Mafia SDS archive files
/// </summary>
[McpServerToolType]
public class SdsTools
{
    private readonly SdsService _sdsService;

    public SdsTools(SdsService sdsService)
    {
        _sdsService = sdsService;
    }

    /// <summary>
    /// Lists all SDS files in a directory
    /// </summary>
    [McpServerTool(Name = "list_sds_files"), Description("List all .sds files in a directory")]
    public string ListSdsFiles(
        [Description("Directory path to search for SDS files")] string directoryPath,
        [Description("Search subdirectories recursively (default: true)")] bool recursive = true)
    {
        try
        {
            var files = _sdsService.ListSdsFiles(directoryPath, recursive);

            if (files.Count == 0)
            {
                return JsonSerializer.Serialize(new
                {
                    success = true,
                    message = "No SDS files found",
                    files = Array.Empty<object>()
                });
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                count = files.Count,
                files = files.Select(f => new
                {
                    path = f.Path,
                    name = f.Name,
                    size = f.Size,
                    sizeFormatted = FormatSize(f.Size)
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Opens an SDS file and returns its metadata and resource list
    /// </summary>
    [McpServerTool(Name = "open_sds_file"), Description("Open an SDS file and get its metadata and resource list")]
    public string OpenSdsFile(
        [Description("Full path to the SDS file")] string filePath,
        [Description("Game type hint: MafiaII, MafiaII_DE, MafiaIII, MafiaI_DE (optional)")] string? gameType = null)
    {
        try
        {
            GamesEnumerator? game = null;
            if (!string.IsNullOrEmpty(gameType))
            {
                if (Enum.TryParse<GamesEnumerator>(gameType, true, out var parsed))
                {
                    game = parsed;
                }
            }

            var info = _sdsService.OpenFile(filePath, game);

            if (info == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = "Failed to open SDS file. File may not exist or may be corrupted."
                });
            }

            // Group resources by type
            var typeGroups = info.Resources
                .GroupBy(r => r.TypeName)
                .Select(g => new { type = g.Key, count = g.Count() })
                .OrderByDescending(g => g.count)
                .ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                file = new
                {
                    path = info.FilePath,
                    name = info.FileName,
                    size = info.FileSize,
                    sizeFormatted = FormatSize(info.FileSize)
                },
                header = new
                {
                    version = info.Version,
                    platform = info.Platform.ToString(),
                    endian = info.Endian.ToString(),
                    slotRamRequired = info.SlotRamRequired,
                    slotVramRequired = info.SlotVramRequired,
                    otherRamRequired = info.OtherRamRequired,
                    otherVramRequired = info.OtherVramRequired,
                    detectedGameType = info.DetectedGameType.ToString()
                },
                resourceTypes = info.ResourceTypes.Select(rt => new
                {
                    id = rt.Id,
                    name = rt.Name,
                    parent = rt.Parent
                }).ToList(),
                resourceSummary = typeGroups,
                resourceCount = info.Resources.Count,
                resources = info.Resources.Take(100).Select(r => new
                {
                    index = r.Index,
                    typeId = r.TypeId,
                    typeName = r.TypeName,
                    version = r.Version,
                    dataSize = r.DataSize,
                    dataSizeFormatted = FormatSize(r.DataSize),
                    name = r.Name
                }).ToList(),
                hasMoreResources = info.Resources.Count > 100
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets header information for an SDS file
    /// </summary>
    [McpServerTool(Name = "get_sds_header"), Description("Get SDS file header information (version, platform, memory requirements)")]
    public string GetSdsHeader(
        [Description("Full path to the SDS file")] string filePath)
    {
        try
        {
            var info = _sdsService.OpenFile(filePath);

            if (info == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = "Failed to open SDS file"
                });
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                header = new
                {
                    version = info.Version,
                    versionDescription = info.Version switch
                    {
                        19 => "Mafia II / Mafia II: Definitive Edition",
                        20 => "Mafia III / Mafia I: Definitive Edition",
                        _ => "Unknown"
                    },
                    platform = info.Platform.ToString(),
                    endian = info.Endian.ToString(),
                    slotRamRequired = info.SlotRamRequired,
                    slotRamFormatted = FormatSize(info.SlotRamRequired),
                    slotVramRequired = info.SlotVramRequired,
                    slotVramFormatted = FormatSize(info.SlotVramRequired),
                    otherRamRequired = info.OtherRamRequired,
                    otherRamFormatted = FormatSize(info.OtherRamRequired),
                    otherVramRequired = info.OtherVramRequired,
                    otherVramFormatted = FormatSize(info.OtherVramRequired),
                    detectedGameType = info.DetectedGameType.ToString()
                },
                resourceTypeCount = info.ResourceTypes.Count,
                resourceCount = info.Resources.Count
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Lists resources in an SDS file with optional filtering
    /// </summary>
    [McpServerTool(Name = "list_resources"), Description("List resources in an SDS file, optionally filtered by type")]
    public string ListResources(
        [Description("Full path to the SDS file")] string filePath,
        [Description("Filter by resource type name (e.g., 'Texture', 'FrameResource')")] string? typeFilter = null,
        [Description("Starting index for pagination (default: 0)")] int offset = 0,
        [Description("Number of resources to return (default: 100, max: 500)")] int limit = 100)
    {
        try
        {
            var info = _sdsService.OpenFile(filePath);

            if (info == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = "Failed to open SDS file"
                });
            }

            var resources = info.Resources.AsEnumerable();

            // Apply type filter
            if (!string.IsNullOrEmpty(typeFilter))
            {
                resources = resources.Where(r => r.TypeName.Contains(typeFilter, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = resources.Count();
            limit = Math.Clamp(limit, 1, 500);

            var pageResources = resources
                .Skip(offset)
                .Take(limit)
                .Select(r => new
                {
                    index = r.Index,
                    typeId = r.TypeId,
                    typeName = r.TypeName,
                    version = r.Version,
                    dataSize = r.DataSize,
                    dataSizeFormatted = FormatSize(r.DataSize),
                    fileHash = r.FileHash != 0 ? $"0x{r.FileHash:X16}" : null,
                    slotRamRequired = r.SlotRamRequired,
                    slotVramRequired = r.SlotVramRequired,
                    name = r.Name
                })
                .ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                pagination = new
                {
                    offset,
                    limit,
                    count = pageResources.Count,
                    totalCount,
                    hasMore = offset + pageResources.Count < totalCount
                },
                typeFilter,
                resources = pageResources
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets detailed information about a specific resource
    /// </summary>
    [McpServerTool(Name = "get_resource_info"), Description("Get detailed information about a specific resource by index")]
    public string GetResourceInfo(
        [Description("Full path to the SDS file")] string filePath,
        [Description("Resource index (0-based)")] int resourceIndex)
    {
        try
        {
            var resource = _sdsService.GetResourceInfo(filePath, resourceIndex);

            if (resource == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = "Resource not found or failed to open SDS file"
                });
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                resource = new
                {
                    index = resource.Index,
                    typeId = resource.TypeId,
                    typeName = resource.TypeName,
                    version = resource.Version,
                    dataSize = resource.DataSize,
                    dataSizeFormatted = FormatSize(resource.DataSize),
                    fileHash = resource.FileHash != 0 ? $"0x{resource.FileHash:X16}" : null,
                    slotRamRequired = resource.SlotRamRequired,
                    slotRamFormatted = FormatSize(resource.SlotRamRequired),
                    slotVramRequired = resource.SlotVramRequired,
                    slotVramFormatted = FormatSize(resource.SlotVramRequired),
                    otherRamRequired = resource.OtherRamRequired,
                    otherRamFormatted = FormatSize(resource.OtherRamRequired),
                    otherVramRequired = resource.OtherVramRequired,
                    otherVramFormatted = FormatSize(resource.OtherVramRequired),
                    name = resource.Name
                }
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Searches resources by name pattern
    /// </summary>
    [McpServerTool(Name = "search_resources"), Description("Search resources by name pattern")]
    public string SearchResources(
        [Description("Full path to the SDS file")] string filePath,
        [Description("Search pattern (case-insensitive substring match)")] string pattern,
        [Description("Filter by resource type name")] string? typeFilter = null,
        [Description("Maximum number of results (default: 50, max: 200)")] int limit = 50)
    {
        try
        {
            var results = _sdsService.SearchResources(filePath, pattern, typeFilter);

            if (results == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = "Failed to open SDS file"
                });
            }

            limit = Math.Clamp(limit, 1, 200);

            return JsonSerializer.Serialize(new
            {
                success = true,
                searchPattern = pattern,
                typeFilter,
                totalMatches = results.Count,
                results = results.Take(limit).Select(r => new
                {
                    index = r.Index,
                    typeName = r.TypeName,
                    version = r.Version,
                    dataSize = r.DataSize,
                    dataSizeFormatted = FormatSize(r.DataSize),
                    name = r.Name
                }).ToList(),
                hasMore = results.Count > limit
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Extracts resource data as base64
    /// </summary>
    [McpServerTool(Name = "extract_resource"), Description("Extract resource data as base64-encoded bytes")]
    public string ExtractResource(
        [Description("Full path to the SDS file")] string filePath,
        [Description("Resource index (0-based)")] int resourceIndex,
        [Description("Return only first N bytes (for large resources, max 1MB)")] int? maxBytes = null)
    {
        try
        {
            var data = _sdsService.ExtractResource(filePath, resourceIndex);

            if (data == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = "Resource not found or failed to open SDS file"
                });
            }

            var resourceInfo = _sdsService.GetResourceInfo(filePath, resourceIndex);

            // Limit extraction size
            const int maxAllowed = 1024 * 1024; // 1 MB
            var actualMax = maxBytes.HasValue ? Math.Min(maxBytes.Value, maxAllowed) : maxAllowed;

            var truncated = data.Length > actualMax;
            var extractedData = truncated ? data.Take(actualMax).ToArray() : data;

            return JsonSerializer.Serialize(new
            {
                success = true,
                resource = new
                {
                    index = resourceIndex,
                    typeName = resourceInfo?.TypeName,
                    name = resourceInfo?.Name,
                    totalSize = data.Length,
                    totalSizeFormatted = FormatSize(data.Length),
                    extractedSize = extractedData.Length,
                    truncated
                },
                data = Convert.ToBase64String(extractedData)
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets summary statistics for an SDS file
    /// </summary>
    [McpServerTool(Name = "get_sds_stats"), Description("Get summary statistics for an SDS file")]
    public string GetSdsStats(
        [Description("Full path to the SDS file")] string filePath)
    {
        try
        {
            var info = _sdsService.OpenFile(filePath);

            if (info == null)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = "Failed to open SDS file"
                });
            }

            var typeStats = info.Resources
                .GroupBy(r => r.TypeName)
                .Select(g => new
                {
                    type = g.Key,
                    count = g.Count(),
                    totalSize = g.Sum(r => (long)r.DataSize),
                    totalSizeFormatted = FormatSize(g.Sum(r => (long)r.DataSize)),
                    avgSize = g.Average(r => r.DataSize),
                    avgSizeFormatted = FormatSize((long)g.Average(r => r.DataSize))
                })
                .OrderByDescending(s => s.totalSize)
                .ToList();

            var totalDataSize = info.Resources.Sum(r => (long)r.DataSize);

            return JsonSerializer.Serialize(new
            {
                success = true,
                file = new
                {
                    name = info.FileName,
                    size = info.FileSize,
                    sizeFormatted = FormatSize(info.FileSize)
                },
                summary = new
                {
                    resourceCount = info.Resources.Count,
                    resourceTypeCount = info.ResourceTypes.Count,
                    totalDataSize,
                    totalDataSizeFormatted = FormatSize(totalDataSize),
                    compressionRatio = info.FileSize > 0 ? Math.Round((double)totalDataSize / info.FileSize, 2) : 0
                },
                typeStatistics = typeStats
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new
            {
                success = false,
                error = ex.Message
            });
        }
    }

    private static string FormatSize(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double size = bytes;
        int order = 0;
        while (size >= 1024 && order < sizes.Length - 1)
        {
            order++;
            size /= 1024;
        }
        return $"{size:0.##} {sizes[order]}";
    }
}
