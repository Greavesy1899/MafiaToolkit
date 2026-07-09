// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using ModelContextProtocol.Server;
using ResourceTypes.Misc;

namespace Mafia2Tool.MCP.Tools;

/// <summary>
/// MCP tools for the streaming map (StreamMapa.bin — magic "StrM", version 6): the table that
/// tells the game which SDS/asset each stream line loads. Two tools:
///   * parse_stream_map — decode the groups / lines / loaders / blocks to JSON.
///   * edit_stream_map  — find/replace across the string fields (loader paths/entities, line
///     names/flags, group names) and save. This is what the legacy deploy did by hand to
///     re-point assets; WriteToFile keeps a "<name>_old.bin" backup of the original.
/// </summary>
[McpServerToolType]
public class StreamMapTools
{
    // ---------------------------------------------------------------------------------------
    // parse
    // ---------------------------------------------------------------------------------------

    [McpServerTool(Name = "parse_stream_map"), Description("Parse a StreamMapa.bin (StrM v6). section=summary returns counts + all groups; section=lines|loaders|blocks returns that paginated array. Pass either filePath or base64Data.")]
    public string ParseStreamMap(
        [Description("Full path to the StreamMapa.bin file. Omit if using base64Data.")] string? filePath = null,
        [Description("Base64 of the StreamMapa.bin bytes. Omit if using filePath.")] string? base64Data = null,
        [Description("Which part to return: summary | groups | lines | loaders | blocks (default: summary)")] string section = "summary",
        [Description("Starting index for pagination of lines/loaders/blocks (default: 0)")] int offset = 0,
        [Description("Max items to return for lines/loaders/blocks (default: 200, max: 5000)")] int limit = 200)
    {
        try
        {
            var loader = Load(filePath, base64Data, out var err);
            if (loader == null) return err;

            limit = Math.Clamp(limit, 1, 5000);
            offset = Math.Max(0, offset);

            var counts = new
            {
                groups = loader.Groups.Length,
                groupHeaders = loader.GroupHeaders.Length,
                lines = loader.Lines.Length,
                loaders = loader.Loaders.Length,
                blocks = loader.Blocks.Length,
                hashes = loader.Blocks.Sum(b => b.Hashes?.Length ?? 0)
            };

            switch (section.ToLowerInvariant())
            {
                case "groups":
                    return JsonSerializer.Serialize(new { success = true, version = 6, counts, groups = loader.Groups.Select(GroupJson).ToList() });

                case "lines":
                {
                    var page = loader.Lines.Skip(offset).Take(limit).Select((l, i) => LineJson(l, offset + i)).ToList();
                    return JsonSerializer.Serialize(new { success = true, version = 6, counts, pagination = Page(offset, limit, page.Count, loader.Lines.Length), lines = page });
                }

                case "loaders":
                {
                    var page = loader.Loaders.Skip(offset).Take(limit).Select((l, i) => LoaderJson(l, offset + i)).ToList();
                    return JsonSerializer.Serialize(new { success = true, version = 6, counts, pagination = Page(offset, limit, page.Count, loader.Loaders.Length), loaders = page });
                }

                case "blocks":
                {
                    var page = loader.Blocks.Skip(offset).Take(limit).Select((b, i) => new
                    {
                        index = offset + i,
                        startOffset = b.startOffset,
                        endOffset = b.endOffset,
                        hashCount = b.Hashes?.Length ?? 0,
                        hashes = b.Hashes?.Select(h => $"0x{h:X16}").ToList()
                    }).ToList();
                    return JsonSerializer.Serialize(new { success = true, version = 6, counts, pagination = Page(offset, limit, page.Count, loader.Blocks.Length), blocks = page });
                }

                default: // summary
                    return JsonSerializer.Serialize(new
                    {
                        success = true,
                        version = 6,
                        counts,
                        groups = loader.Groups.Select(GroupJson).ToList(),
                        groupHeaders = loader.GroupHeaders,
                        note = "Use section=lines|loaders|blocks for the paginated arrays."
                    });
            }
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    // ---------------------------------------------------------------------------------------
    // edit (string-swap)
    // ---------------------------------------------------------------------------------------

    [McpServerTool(Name = "edit_stream_map"), Description("Find/replace across a StreamMapa.bin's string fields and save (this is how assets get re-pointed). dryRun (default true) previews the changes without writing; set dryRun=false to apply. WriteToFile keeps a '<name>_old.bin' backup.")]
    public string EditStreamMap(
        [Description("Full path to the StreamMapa.bin file to edit in place.")] string filePath,
        [Description("Substring (or, with mode=exact, whole value) to find.")] string find,
        [Description("Replacement text.")] string replace,
        [Description("Match mode: substring (default, replaces occurrences within a field) or exact (whole-field match).")] string mode = "substring",
        [Description("Comma-separated fields to edit: path, entity, lineName, flags, groupName (default: path,entity).")] string fields = "path,entity",
        [Description("Preview only without writing (default: true). Set false to apply the edit.")] bool dryRun = true)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "File not found: " + filePath });
            }
            if (string.IsNullOrEmpty(find))
            {
                return JsonSerializer.Serialize(new { success = false, error = "'find' must not be empty." });
            }

            var fieldSet = new HashSet<string>((fields ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries), StringComparer.OrdinalIgnoreCase);
            if (fieldSet.Count == 0)
            {
                return JsonSerializer.Serialize(new { success = false, error = "No valid fields selected." });
            }
            bool exact = mode.Equals("exact", StringComparison.OrdinalIgnoreCase);

            var loader = new StreamMapLoader(new FileInfo(filePath));
            if (loader.Loaders == null)
            {
                return JsonSerializer.Serialize(new { success = false, error = "Not a valid StreamMapa.bin (StrM v6)." });
            }

            var changes = new List<object>();
            int total = 0;

            string Apply(string value)
            {
                if (value == null) return null;
                if (exact) return value.Equals(find, StringComparison.Ordinal) ? replace : value;
                return value.Contains(find, StringComparison.Ordinal) ? value.Replace(find, replace) : value;
            }

            void Try(string field, int index, string before, Action<string> setter)
            {
                var after = Apply(before);
                if (!string.Equals(before, after, StringComparison.Ordinal))
                {
                    setter(after);
                    total++;
                    if (changes.Count < 200) changes.Add(new { field, index, before, after });
                }
            }

            if (fieldSet.Contains("path") || fieldSet.Contains("entity"))
            {
                for (int i = 0; i < loader.Loaders.Length; i++)
                {
                    var l = loader.Loaders[i];
                    if (fieldSet.Contains("path")) Try("path", i, l.Path, v => l.Path = v);
                    if (fieldSet.Contains("entity")) Try("entity", i, l.Entity, v => l.Entity = v);
                }
            }
            if (fieldSet.Contains("linename") || fieldSet.Contains("flags"))
            {
                for (int i = 0; i < loader.Lines.Length; i++)
                {
                    var l = loader.Lines[i];
                    if (fieldSet.Contains("linename")) Try("lineName", i, l.Name, v => l.Name = v);
                    if (fieldSet.Contains("flags")) Try("flags", i, l.Flags, v => l.Flags = v);
                }
            }
            if (fieldSet.Contains("groupname"))
            {
                for (int i = 0; i < loader.Groups.Length; i++)
                {
                    var g = loader.Groups[i];
                    Try("groupName", i, g.Name, v => g.Name = v);
                }
                // Group names are also duplicated on each line as its group header; keep them in sync.
                for (int i = 0; i < loader.Lines.Length; i++)
                {
                    var l = loader.Lines[i];
                    Try("lineGroup", i, l.Group, v => l.Group = v);
                }
            }

            bool applied = false;
            if (!dryRun && total > 0)
            {
                loader.WriteToFile();
                applied = true;
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                dryRun,
                applied,
                find,
                replace,
                mode = exact ? "exact" : "substring",
                fields = fieldSet.ToList(),
                totalChanges = total,
                backup = applied ? Path.ChangeExtension(filePath, null) + "_old.bin" : null,
                note = dryRun && total > 0 ? "Preview only — set dryRun=false to write these changes." : null,
                changes
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    // ---------------------------------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------------------------------

    private static StreamMapLoader Load(string filePath, string base64Data, out string error)
    {
        error = null;
        bool hasPath = !string.IsNullOrEmpty(filePath);
        bool hasData = !string.IsNullOrEmpty(base64Data);
        if (hasPath == hasData)
        {
            error = JsonSerializer.Serialize(new { success = false, error = "Provide exactly one of filePath or base64Data." });
            return null;
        }

        var loader = new StreamMapLoader();
        if (hasPath)
        {
            if (!File.Exists(filePath))
            {
                error = JsonSerializer.Serialize(new { success = false, error = "File not found: " + filePath });
                return null;
            }
            loader.ReadFromBytes(File.ReadAllBytes(filePath));
        }
        else
        {
            loader.ReadFromBytes(Convert.FromBase64String(base64Data));
        }

        if (loader.Groups == null || loader.Loaders == null)
        {
            error = JsonSerializer.Serialize(new { success = false, error = "Not a valid StreamMapa.bin (expected magic 'StrM' and version 6)." });
            return null;
        }
        return loader;
    }

    private static object Page(int offset, int limit, int count, int total) =>
        new { offset, limit, count, totalCount = total, hasMore = offset + count < total };

    private static object GroupJson(StreamMapLoader.StreamGroup g) => new
    {
        name = g.Name,
        type = g.Type.ToString(),
        typeId = (int)g.Type,
        unk01 = g.Unk01,
        startOffset = g.startOffset,
        loaderCount = g.endOffset,
        unk5 = g.Unk05
    };

    private static object LineJson(StreamMapLoader.StreamLine l, int index) => new
    {
        index,
        name = l.Name,
        group = l.Group,
        groupId = l.groupID,
        lineId = l.lineID,
        loadType = l.LoadType,
        flags = l.Flags,
        hash1 = $"0x{l.Unk10:X16}",
        hash2 = $"0x{l.Unk11:X16}",
        unk5 = l.Unk5
    };

    private static object LoaderJson(StreamMapLoader.StreamLoader l, int index) => new
    {
        index,
        path = l.Path,
        entity = l.Entity,
        type = l.Type.ToString(),
        typeId = (int)l.Type,
        loadType = l.LoadType,
        loaderSubId = l.LoaderSubID,
        loaderId = l.LoaderID,
        start = l.start,
        end = l.end
    };
}
