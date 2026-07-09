// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Gibbed.IO;
using Gibbed.Mafia2.ResourceFormats;
using Mafia2Tool.MCP.Services;
using ModelContextProtocol.Server;
using Utils.Settings;

namespace Mafia2Tool.MCP.Tools;

/// <summary>
/// MCP tools for reading the game's data tables (the ".tbl" rows packed into the "Table"
/// resources of tables.sds). Many systems — weapons, cars, physics materials — are table
/// driven, so these expose list / dump / row-lookup without shelling out to an external
/// parser.
///
/// Three input shapes are accepted:
///   * sdsPath    — an SDS archive (e.g. tables.sds); every "Table" resource is enumerated
///                  and its contained tables flattened.
///   * tablePath  — a standalone ".tbl" file (a 4-byte version header + one table), exactly
///                  what the toolkit writes when it unpacks a Table resource.
///   * base64Data — the raw bytes of a "Table" resource payload (the output of
///                  extract_resource); a count-prefixed list of tables. Needs `version`.
///
/// Columns are identified only by a 32-bit FNV name hash (the format stores no column name
/// strings), so cells are returned positionally and columns are reported by hash + type.
/// </summary>
[McpServerToolType]
public class TableTools
{
    private readonly SdsService _sdsService;

    public TableTools(SdsService sdsService)
    {
        _sdsService = sdsService;
    }

    // ---------------------------------------------------------------------------------------
    // Tools
    // ---------------------------------------------------------------------------------------

    [McpServerTool(Name = "list_tables"), Description("List the data tables in a tables.sds archive, a standalone .tbl file, or a base64 Table-resource payload. Returns each table's name, row/column counts and column hashes+types.")]
    public string ListTables(
        [Description("Path to an SDS archive (e.g. tables.sds) whose 'Table' resources will be enumerated.")] string? sdsPath = null,
        [Description("Path to a standalone .tbl file.")] string? tablePath = null,
        [Description("Base64 of a 'Table' resource payload (extract_resource output). Requires version.")] string? base64Data = null,
        [Description("Table format version for base64Data (1 = Mafia II, 2 = Mafia I: DE). Default 1.")] int version = 1,
        [Description("Game type hint when opening an SDS: MafiaII, MafiaII_DE, MafiaIII, MafiaI_DE (optional)")] string? gameType = null)
    {
        try
        {
            var tables = LoadTables(sdsPath, tablePath, base64Data, version, gameType, out var err);
            if (tables == null) return err;

            return JsonSerializer.Serialize(new
            {
                success = true,
                source = SourceKind(sdsPath, tablePath),
                tableCount = tables.Count,
                tables = tables.Select(t => new
                {
                    resourceIndex = t.ResourceIndex,
                    tableIndex = t.TableIndex,
                    name = t.Table.Name,
                    nameHash = Hex64(t.Table.NameHash),
                    rowCount = t.Table.Rows.Count,
                    columnCount = t.Table.Columns.Count,
                    columns = t.Table.Columns.Select(c => new { nameHash = Hex32(c.NameHash), type = c.Type.ToString() }).ToList()
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    [McpServerTool(Name = "dump_rows"), Description("Dump the rows of a data table. Provide sdsPath/tablePath/base64Data plus tableName when the source holds more than one table. Cells are positional, aligned to the reported columns.")]
    public string DumpRows(
        [Description("Path to an SDS archive (e.g. tables.sds).")] string? sdsPath = null,
        [Description("Path to a standalone .tbl file.")] string? tablePath = null,
        [Description("Base64 of a 'Table' resource payload. Requires version.")] string? base64Data = null,
        [Description("Table format version for base64Data (default 1).")] int version = 1,
        [Description("Name of the table to dump. Required when the source contains more than one table; matched case-insensitively (exact, or ignoring a .tbl extension).")] string? tableName = null,
        [Description("Starting row index for pagination (default: 0)")] int offset = 0,
        [Description("Max rows to return (default: 100, max: 2000)")] int limit = 100,
        [Description("Game type hint when opening an SDS (optional)")] string? gameType = null)
    {
        try
        {
            var tables = LoadTables(sdsPath, tablePath, base64Data, version, gameType, out var err);
            if (tables == null) return err;

            var picked = PickTable(tables, tableName, out var pickErr);
            if (picked == null) return pickErr;
            var table = picked.Table;

            limit = Math.Clamp(limit, 1, 2000);
            offset = Math.Max(0, offset);

            var rows = table.Rows.Skip(offset).Take(limit).Select((r, i) => new
            {
                index = offset + i,
                values = r.Values.Select((v, c) => FormatCell(v, table.Columns[c].Type)).ToList()
            }).ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                table = new { name = table.Name, nameHash = Hex64(table.NameHash), rowCount = table.Rows.Count, columnCount = table.Columns.Count },
                columns = table.Columns.Select(c => new { nameHash = Hex32(c.NameHash), type = c.Type.ToString() }).ToList(),
                pagination = new { offset, limit, count = rows.Count, totalCount = table.Rows.Count, hasMore = offset + rows.Count < table.Rows.Count },
                rows
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    [McpServerTool(Name = "lookup_by_row"), Description("Look up a single table row by index, returning each cell paired with its column hash and type. Provide tableName when the source holds more than one table.")]
    public string LookupByRow(
        [Description("Path to an SDS archive (e.g. tables.sds).")] string? sdsPath = null,
        [Description("Path to a standalone .tbl file.")] string? tablePath = null,
        [Description("Base64 of a 'Table' resource payload. Requires version.")] string? base64Data = null,
        [Description("Table format version for base64Data (default 1).")] int version = 1,
        [Description("Name of the table to read. Required when the source contains more than one table.")] string? tableName = null,
        [Description("Row index to look up (0-based).")] int rowIndex = 0,
        [Description("Game type hint when opening an SDS (optional)")] string? gameType = null)
    {
        try
        {
            var tables = LoadTables(sdsPath, tablePath, base64Data, version, gameType, out var err);
            if (tables == null) return err;

            var picked = PickTable(tables, tableName, out var pickErr);
            if (picked == null) return pickErr;
            var table = picked.Table;

            if (rowIndex < 0 || rowIndex >= table.Rows.Count)
            {
                return JsonSerializer.Serialize(new { success = false, error = $"rowIndex out of range (0..{table.Rows.Count - 1})" });
            }

            var row = table.Rows[rowIndex];
            return JsonSerializer.Serialize(new
            {
                success = true,
                table = new { name = table.Name, nameHash = Hex64(table.NameHash), rowCount = table.Rows.Count, columnCount = table.Columns.Count },
                rowIndex,
                cells = row.Values.Select((v, c) => new
                {
                    columnHash = Hex32(table.Columns[c].NameHash),
                    type = table.Columns[c].Type.ToString(),
                    value = FormatCell(v, table.Columns[c].Type)
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    // ---------------------------------------------------------------------------------------
    // Loading
    // ---------------------------------------------------------------------------------------

    private sealed class LoadedTable
    {
        public int? ResourceIndex;
        public int TableIndex;
        public TableData Table;
    }

    /// <summary>
    /// Resolves the input to a flat list of tables. On failure sets <paramref name="error"/> to a
    /// ready-to-return JSON error and returns null.
    /// </summary>
    private List<LoadedTable> LoadTables(string sdsPath, string tablePath, string base64Data, int version, string gameType, out string error)
    {
        error = null;
        int provided = (string.IsNullOrEmpty(sdsPath) ? 0 : 1) + (string.IsNullOrEmpty(tablePath) ? 0 : 1) + (string.IsNullOrEmpty(base64Data) ? 0 : 1);
        if (provided != 1)
        {
            error = JsonSerializer.Serialize(new { success = false, error = "Provide exactly one of sdsPath, tablePath or base64Data." });
            return null;
        }

        var result = new List<LoadedTable>();

        if (!string.IsNullOrEmpty(sdsPath))
        {
            GamesEnumerator? game = null;
            if (!string.IsNullOrEmpty(gameType) && Enum.TryParse<GamesEnumerator>(gameType, true, out var parsed)) game = parsed;

            var info = _sdsService.OpenFile(sdsPath, game);
            if (info == null)
            {
                error = McpError.OpenFailureJson(_sdsService);
                return null;
            }

            var tableResources = info.Resources.Where(r => r.TypeName == "Table").ToList();
            if (tableResources.Count == 0)
            {
                error = JsonSerializer.Serialize(new { success = false, error = "No 'Table' resources found in this SDS." });
                return null;
            }

            foreach (var res in tableResources)
            {
                var data = _sdsService.ExtractResource(sdsPath, res.Index);
                if (data == null) continue;

                var container = new TableResource();
                using var ms = new MemoryStream(data, false);
                container.Deserialize(res.Version, ms, Endian.Little);
                for (int i = 0; i < container.Tables.Count; i++)
                    result.Add(new LoadedTable { ResourceIndex = res.Index, TableIndex = i, Table = container.Tables[i] });
            }
            return result;
        }

        if (!string.IsNullOrEmpty(tablePath))
        {
            if (!File.Exists(tablePath))
            {
                error = JsonSerializer.Serialize(new { success = false, error = "File not found: " + tablePath });
                return null;
            }

            using var reader = new BinaryReader(File.OpenRead(tablePath));
            ushort fileVersion = (ushort)reader.ReadInt32();
            var td = new TableData();
            td.Deserialize(fileVersion, reader.BaseStream, Endian.Little);
            result.Add(new LoadedTable { ResourceIndex = null, TableIndex = 0, Table = td });
            return result;
        }

        // base64Data: a Table resource payload (count-prefixed list of tables).
        var bytes = Convert.FromBase64String(base64Data);
        var resource = new TableResource();
        using (var ms = new MemoryStream(bytes, false))
        {
            resource.Deserialize((ushort)version, ms, Endian.Little);
        }
        for (int i = 0; i < resource.Tables.Count; i++)
            result.Add(new LoadedTable { ResourceIndex = null, TableIndex = i, Table = resource.Tables[i] });
        return result;
    }

    private static LoadedTable PickTable(List<LoadedTable> tables, string tableName, out string error)
    {
        error = null;

        if (string.IsNullOrEmpty(tableName))
        {
            if (tables.Count == 1) return tables[0];
            error = JsonSerializer.Serialize(new
            {
                success = false,
                error = $"Source contains {tables.Count} tables; specify tableName.",
                availableTables = tables.Select(t => t.Table.Name).ToList()
            });
            return null;
        }

        var matches = tables.Where(t => NameMatches(t.Table.Name, tableName)).ToList();
        if (matches.Count == 0)
        {
            error = JsonSerializer.Serialize(new
            {
                success = false,
                error = $"No table named '{tableName}'.",
                availableTables = tables.Select(t => t.Table.Name).ToList()
            });
            return null;
        }
        return matches[0];
    }

    private static bool NameMatches(string actual, string query)
    {
        if (string.IsNullOrEmpty(actual)) return false;
        if (actual.Equals(query, StringComparison.OrdinalIgnoreCase)) return true;
        string strip(string s) => Path.GetFileNameWithoutExtension(s);
        return strip(actual).Equals(strip(query), StringComparison.OrdinalIgnoreCase);
    }

    // ---------------------------------------------------------------------------------------
    // Formatting
    // ---------------------------------------------------------------------------------------

    private static string SourceKind(string sdsPath, string tablePath) =>
        !string.IsNullOrEmpty(sdsPath) ? "sds" : !string.IsNullOrEmpty(tablePath) ? "tbl" : "resource";

    private static string Hex32(uint value) => $"0x{value:X8}";
    private static string Hex64(ulong value) => $"0x{value:X16}";

    /// <summary>
    /// Hash64 and Flags32 cells are surfaced as hex strings (they read as identifiers/bitfields);
    /// every other column type keeps its native JSON value (bool / number / string).
    /// </summary>
    private static object FormatCell(object value, TableData.ColumnType type)
    {
        if (value == null) return null;
        return type switch
        {
            TableData.ColumnType.Hash64 => value is ulong u ? Hex64(u) : value,
            TableData.ColumnType.Flags32 => value is uint f ? Hex32(f) : value,
            _ => value
        };
    }
}
