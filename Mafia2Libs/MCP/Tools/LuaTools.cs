// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Gibbed.IO;
using Gibbed.Mafia2.ResourceFormats;
using Mafia2Tool.MCP.Services;
using ModelContextProtocol.Server;
using Utils.Lua;
using Utils.Settings;

namespace Mafia2Tool.MCP.Tools;

/// <summary>
/// MCP tools that expose the toolkit's Lua decompiler (UnluacNET). Game logic ships as compiled
/// Lua bytecode inside an SDS "Script" resource, whose payload is a <see cref="ScriptResource"/>
/// holding one-or-more named <see cref="ScriptData"/> chunks. <see cref="DecompileLua"/> handles a
/// single chunk (file or base64); <see cref="DecompileScriptResource"/> unwraps a Script resource
/// from an SDS. Output is UnluacNET's verbatim reconstruction, without the GUI <c>LuaHelper</c> fix-ups.
/// </summary>
[McpServerToolType]
public class LuaTools
{
    private readonly SdsService _sdsService;

    public LuaTools(SdsService sdsService)
    {
        _sdsService = sdsService;
    }

    [McpServerTool(Name = "decompile_lua"), Description("Decompile a single compiled Lua chunk (bytecode) back to source. Pass either filePath (a '.lua'/'.AP' script) or base64Data (e.g. one script's raw bytes). Only compiled bytecode can be decompiled — plain-text Lua and non-Lua data are rejected.")]
    public string DecompileLua(
        [Description("Full path to a compiled Lua file (.lua/.AP). Omit if using base64Data.")] string? filePath = null,
        [Description("Base64 of a single compiled Lua chunk. Omit if using filePath.")] string? base64Data = null)
    {
        try
        {
            byte[] bytes = LoadBytes(filePath, base64Data, out var err);
            if (bytes == null) return err;

            if (!LuaHelper.IsBytecode(bytes))
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = "Data is not compiled Lua bytecode (missing '\\x1BLua' magic). Only compiled chunks can be decompiled."
                });

            string source = LuaHelper.DecompileBytecode(bytes);
            return JsonSerializer.Serialize(new
            {
                success = true,
                byteSize = bytes.Length,
                lineCount = CountLines(source),
                source
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    [McpServerTool(Name = "decompile_script_resource"), Description("Open an SDS, unwrap its Script resource and decompile the compiled Lua it contains. With scriptIndex = -1 (default) returns the script list (names, sizes, whether each is bytecode) without decompiling; set scriptIndex to decompile that single script's source. Select the resource by resourceIndex, or by leaving it -1 to take the first 'Script' resource.")]
    public string DecompileScriptResource(
        [Description("Full path to the SDS file.")] string sdsPath,
        [Description("Index of the script inside the resource to decompile (0-based). Use -1 to just list the contained scripts.")] int scriptIndex = -1,
        [Description("Resource index of the Script resource (0-based). Use -1 to take the first 'Script' resource in the archive.")] int resourceIndex = -1,
        [Description("Game type hint: MafiaII, MafiaII_DE, MafiaIII, MafiaI_DE (optional)")] string? gameType = null)
    {
        try
        {
            GamesEnumerator? game = null;
            if (!string.IsNullOrEmpty(gameType) && Enum.TryParse<GamesEnumerator>(gameType, true, out var parsed)) game = parsed;

            var info = _sdsService.OpenFile(sdsPath, game);
            if (info == null) return McpError.OpenFailureJson(_sdsService);

            SdsResourceInfo resource;
            if (resourceIndex >= 0)
            {
                resource = info.Resources.FirstOrDefault(r => r.Index == resourceIndex);
                if (resource == null)
                    return JsonSerializer.Serialize(new { success = false, error = $"Resource index {resourceIndex} not found (0..{info.Resources.Count - 1})." });
                if (!resource.TypeName.Equals("Script", StringComparison.OrdinalIgnoreCase))
                    return JsonSerializer.Serialize(new { success = false, error = $"Resource {resourceIndex} is '{resource.TypeName}', not a Script resource." });
            }
            else
            {
                resource = info.Resources.FirstOrDefault(r => r.TypeName.Equals("Script", StringComparison.OrdinalIgnoreCase));
                if (resource == null)
                    return JsonSerializer.Serialize(new { success = false, error = "No Script resource in this SDS." });
            }

            var data = _sdsService.ExtractResource(sdsPath, resource.Index);
            if (data == null)
                return JsonSerializer.Serialize(new { success = false, error = "Failed to extract Script resource data." });

            var scriptResource = new ScriptResource();
            using (var stream = new MemoryStream(data, false))
            {
                scriptResource.Deserialize(resource.Version, stream, Endian.Little);
            }
            var scripts = scriptResource.Scripts;

            if (scriptIndex < 0)
            {
                return JsonSerializer.Serialize(new
                {
                    success = true,
                    resource = new { index = resource.Index, typeName = resource.TypeName, version = resource.Version },
                    path = scriptResource.Path,
                    scriptCount = scripts.Count,
                    scripts = scripts.Select((s, i) => new
                    {
                        index = i,
                        name = s.Name,
                        byteSize = s.Data?.Length ?? 0,
                        isBytecode = LuaHelper.IsBytecode(s.Data)
                    }).ToList()
                });
            }

            if (scriptIndex >= scripts.Count)
                return JsonSerializer.Serialize(new { success = false, error = $"scriptIndex {scriptIndex} out of range (0..{scripts.Count - 1})." });

            var script = scripts[scriptIndex];
            if (!LuaHelper.IsBytecode(script.Data))
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = $"Script '{script.Name}' is not compiled Lua bytecode; nothing to decompile.",
                    script = new { index = scriptIndex, name = script.Name, byteSize = script.Data?.Length ?? 0 }
                });

            string source = LuaHelper.DecompileBytecode(script.Data);
            return JsonSerializer.Serialize(new
            {
                success = true,
                resource = new { index = resource.Index, typeName = resource.TypeName, version = resource.Version },
                path = scriptResource.Path,
                scriptCount = scripts.Count,
                script = new
                {
                    index = scriptIndex,
                    name = script.Name,
                    byteSize = script.Data.Length,
                    lineCount = CountLines(source),
                    source
                }
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    /// <summary>
    /// Resolves the payload bytes from exactly one of filePath / base64Data. On failure, sets
    /// <paramref name="error"/> to a ready-to-return JSON error and returns null.
    /// </summary>
    private static byte[] LoadBytes(string filePath, string base64Data, out string error)
    {
        error = null;
        bool hasPath = !string.IsNullOrEmpty(filePath);
        bool hasData = !string.IsNullOrEmpty(base64Data);

        if (hasPath == hasData)
        {
            error = JsonSerializer.Serialize(new { success = false, error = "Provide exactly one of filePath or base64Data." });
            return null;
        }

        if (hasPath)
        {
            if (!File.Exists(filePath))
            {
                error = JsonSerializer.Serialize(new { success = false, error = "File not found: " + filePath });
                return null;
            }
            return File.ReadAllBytes(filePath);
        }

        return Convert.FromBase64String(base64Data);
    }

    private static int CountLines(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;
        int count = 1;
        foreach (var c in s) if (c == '\n') count++;
        return count;
    }
}
