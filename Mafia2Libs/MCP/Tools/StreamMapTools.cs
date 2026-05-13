using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using ModelContextProtocol.Server;
using ResourceTypes.Misc;

namespace Mafia2Tool.MCP.Tools;

/// <summary>
/// MCP tools for browsing and inspecting StreamMapa.bin (streaming configuration) files
/// </summary>
[McpServerToolType]
public class StreamMapTools
{
    [McpServerTool(Name = "open_stream_map"), Description("Parse StreamMapa.bin and return header overview: group/line/loader/block counts and group list. Main file is at <GameRoot>\\EDIT\\tables\\StreamMapa.bin; DLCs have their own copies (e.g. dlcs\\<dlc_name>\\game_edit\\tables\\), but prefer the main file unless searching DLC-specific content")]
    public string OpenStreamMap([Description("Full path to StreamMapa.bin (main: <GameRoot>\\EDIT\\tables\\StreamMapa.bin; DLCs have copies under dlcs\\<dlc_name>\\game_edit\\tables\\)")] string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var streamMap = new StreamMapLoader(new FileInfo(filePath));
            return JsonSerializer.Serialize(new
            {
                success = true,
                groupCount = streamMap.Groups.Length,
                groupHeaderCount = streamMap.GroupHeaders.Length,
                lineCount = streamMap.Lines.Length,
                loaderCount = streamMap.Loaders.Length,
                blockCount = streamMap.Blocks.Length,
                groups = streamMap.Groups.Select(g => new { g.Name, type = g.Type.ToString() }).ToList()
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    [McpServerTool(Name = "get_stream_groups"), Description("List all StreamGroup entries with type, offsets, and loader index range")]
    public string GetStreamGroups([Description("Full path to StreamMapa.bin (main: <GameRoot>\\EDIT\\tables\\StreamMapa.bin; DLCs have copies under dlcs\\<dlc_name>\\game_edit\\tables\\)")] string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var streamMap = new StreamMapLoader(new FileInfo(filePath));
            var groups = streamMap.Groups.Select((g, i) => new
            {
                index = i,
                name = g.Name,
                type = g.Type.ToString(),
                unk01 = g.Unk01,
                loaderStartOffset = g.startOffset,
                loaderEndOffset = g.endOffset,
                loaderCount = g.endOffset - g.startOffset,
                unk05 = g.Unk05
            }).ToList();

            return JsonSerializer.Serialize(new { success = true, count = groups.Count, groups });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    [McpServerTool(Name = "get_stream_lines"), Description("List StreamLine entries with pagination and optional group name filter")]
    public string GetStreamLines(
        [Description("Full path to StreamMapa.bin (main: <GameRoot>\\EDIT\\tables\\StreamMapa.bin; DLCs have copies under dlcs\\<dlc_name>\\game_edit\\tables\\)")] string filePath,
        [Description("Filter by group name (case-insensitive substring, optional)")] string? groupFilter = null,
        [Description("Starting index for pagination (default: 0)")] int offset = 0,
        [Description("Number of lines to return (default: 100)")] int limit = 100)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var streamMap = new StreamMapLoader(new FileInfo(filePath));
            var lines = streamMap.Lines.AsEnumerable();

            if (!string.IsNullOrEmpty(groupFilter))
                lines = lines.Where(l => l.Group != null && l.Group.Contains(groupFilter, StringComparison.OrdinalIgnoreCase));

            limit = Math.Clamp(limit, 1, 500);
            var totalCount = lines.Count();
            var page = lines.Skip(offset).Take(limit).Select((l, i) => new
            {
                index = offset + i,
                name = l.Name,
                group = l.Group,
                flags = l.Flags,
                loadType = l.LoadType,
                lineID = l.lineID,
                groupID = l.groupID
            }).ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                pagination = new { offset, limit, count = page.Count, totalCount, hasMore = offset + page.Count < totalCount },
                groupFilter,
                lines = page
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    [McpServerTool(Name = "get_stream_line_detail"), Description("Get full detail for a single StreamLine by index")]
    public string GetStreamLineDetail(
        [Description("Full path to StreamMapa.bin (main: <GameRoot>\\EDIT\\tables\\StreamMapa.bin; DLCs have copies under dlcs\\<dlc_name>\\game_edit\\tables\\)")] string filePath,
        [Description("Line index (0-based)")] int lineIndex)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var streamMap = new StreamMapLoader(new FileInfo(filePath));
            if (lineIndex < 0 || lineIndex >= streamMap.Lines.Length)
                return JsonSerializer.Serialize(new { success = false, error = $"Line index {lineIndex} out of range (0-{streamMap.Lines.Length - 1})" });

            var l = streamMap.Lines[lineIndex];
            return JsonSerializer.Serialize(new
            {
                success = true,
                index = lineIndex,
                name = l.Name,
                group = l.Group,
                flags = l.Flags,
                loadType = l.LoadType,
                lineID = l.lineID,
                groupID = l.groupID,
                nameIDX = l.nameIDX,
                flagIDX = l.flagIDX,
                unk5 = l.Unk5,
                unk10 = $"0x{l.Unk10:X16}",
                unk11 = $"0x{l.Unk11:X16}",
                unk12 = l.Unk12,
                unk13 = l.Unk13,
                unk14 = l.Unk14,
                unk15 = l.Unk15
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    [McpServerTool(Name = "get_stream_loaders"), Description("List all StreamLoader entries (assets loaded per streaming group) with pagination")]
    public string GetStreamLoaders(
        [Description("Full path to StreamMapa.bin (main: <GameRoot>\\EDIT\\tables\\StreamMapa.bin; DLCs have copies under dlcs\\<dlc_name>\\game_edit\\tables\\)")] string filePath,
        [Description("Starting index for pagination (default: 0)")] int offset = 0,
        [Description("Number of loaders to return (default: 100)")] int limit = 100)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var streamMap = new StreamMapLoader(new FileInfo(filePath));
            limit = Math.Clamp(limit, 1, 500);
            var totalCount = streamMap.Loaders.Length;
            var page = streamMap.Loaders.Skip(offset).Take(limit).Select((l, i) => new
            {
                index = offset + i,
                path = l.Path,
                entity = l.Entity,
                type = l.Type.ToString(),
                loadType = l.LoadType,
                loaderID = l.LoaderID,
                loaderSubID = l.LoaderSubID,
                groupID = l.GroupID,
                assignedGroup = l.AssignedGroup
            }).ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                pagination = new { offset, limit, count = page.Count, totalCount, hasMore = offset + page.Count < totalCount },
                loaders = page
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    [McpServerTool(Name = "get_line_load_list"), Description("Get the list of assets (loaders) that will be loaded when a specific StreamLine is activated, by matching loader start/end range against the line's lineID")]
    public string GetLineLoadList(
        [Description("Full path to StreamMapa.bin (main: <GameRoot>\\EDIT\\tables\\StreamMapa.bin; DLCs have copies under dlcs\\<dlc_name>\\game_edit\\tables\\)")] string filePath,
        [Description("Line index (0-based)")] int lineIndex)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var streamMap = new StreamMapLoader(new FileInfo(filePath));
            if (lineIndex < 0 || lineIndex >= streamMap.Lines.Length)
                return JsonSerializer.Serialize(new { success = false, error = $"Line index {lineIndex} out of range (0-{streamMap.Lines.Length - 1})" });

            var line = streamMap.Lines[lineIndex];
            var loadList = streamMap.Loaders
                .Select((loader, i) => new { loader, i })
                .Where(x => line.lineID >= x.loader.start && line.lineID <= x.loader.end)
                .Select(x => new
                {
                    index = x.i,
                    path = x.loader.Path,
                    entity = x.loader.Entity,
                    type = x.loader.Type.ToString(),
                    loadType = x.loader.LoadType,
                    loaderID = x.loader.LoaderID,
                    loaderSubID = x.loader.LoaderSubID,
                    start = x.loader.start,
                    end = x.loader.end
                })
                .ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                lineIndex,
                lineName = line.Name,
                lineID = line.lineID,
                group = line.Group,
                loadListCount = loadList.Count,
                loadList
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    [McpServerTool(Name = "search_stream_lines"), Description("Search StreamLine names and StreamLoader paths/entities by case-insensitive substring pattern")]
    public string SearchStreamLines(
        [Description("Full path to StreamMapa.bin (main: <GameRoot>\\EDIT\\tables\\StreamMapa.bin; DLCs have copies under dlcs\\<dlc_name>\\game_edit\\tables\\)")] string filePath,
        [Description("Search pattern (case-insensitive substring)")] string pattern,
        [Description("Maximum results per category (default: 50)")] int limit = 50)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var streamMap = new StreamMapLoader(new FileInfo(filePath));
            limit = Math.Clamp(limit, 1, 200);

            var matchedLines = streamMap.Lines
                .Select((l, i) => new { l, i })
                .Where(x => x.l.Name != null && x.l.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                .Select(x => new { index = x.i, name = x.l.Name, group = x.l.Group, flags = x.l.Flags, loadType = x.l.LoadType })
                .ToList();

            var matchedLoaders = streamMap.Loaders
                .Select((l, i) => new { l, i })
                .Where(x => (x.l.Path != null && x.l.Path.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                         || (x.l.Entity != null && x.l.Entity.Contains(pattern, StringComparison.OrdinalIgnoreCase)))
                .Select(x => new { index = x.i, path = x.l.Path, entity = x.l.Entity, type = x.l.Type.ToString(), loadType = x.l.LoadType })
                .ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                pattern,
                lines = new
                {
                    totalMatches = matchedLines.Count,
                    results = matchedLines.Take(limit).ToList(),
                    hasMore = matchedLines.Count > limit
                },
                loaders = new
                {
                    totalMatches = matchedLoaders.Count,
                    results = matchedLoaders.Take(limit).ToList(),
                    hasMore = matchedLoaders.Count > limit
                }
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }
}
