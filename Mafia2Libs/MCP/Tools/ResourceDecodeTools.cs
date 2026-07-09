// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using Mafia2Tool.MCP.Services;
using ModelContextProtocol.Server;
using Utils.Settings;

namespace Mafia2Tool.MCP.Tools;

/// <summary>
/// Ergonomic bridge over <c>extract_resource</c>: open an SDS, look at the resource's typeName
/// and run the matching decoder in one call, instead of extract_resource → base64 → decode_*.
/// Routes the resource types that have a bytes decoder (Actors, FrameResource, ItemDesc,
/// Collisions, Effects); for a FrameResource it also auto-pairs the archive's FrameNameTable
/// when there's exactly one.
/// </summary>
[McpServerToolType]
public class ResourceDecodeTools
{
    private static readonly string[] SupportedTypes = { "Actors", "FrameResource", "ItemDesc", "Collisions", "Effects" };

    private readonly SdsService _sdsService;
    private readonly DecodeTools _decode = new();
    private readonly EffectsTools _effects = new();

    public ResourceDecodeTools(SdsService sdsService)
    {
        _sdsService = sdsService;
    }

    [McpServerTool(Name = "decode_resource"), Description("Extract a resource from an SDS and decode it in one step, routing by its typeName (Actors, FrameResource, ItemDesc, Collisions, Effects). Select by resourceIndex, or by typeName to take the first resource of that type. offset/limit page the decoded array where applicable.")]
    public string DecodeResource(
        [Description("Full path to the SDS file.")] string sdsPath,
        [Description("Resource index to decode (0-based). Use -1 to select by typeName instead.")] int resourceIndex = -1,
        [Description("When resourceIndex is -1, decode the first resource of this type (Actors, FrameResource, ItemDesc, Collisions, Effects).")] string? typeName = null,
        [Description("Starting index for pagination of the decoded array (default: 0)")] int offset = 0,
        [Description("Max items for the decoded array (default: 200)")] int limit = 200,
        [Description("Game type hint: MafiaII, MafiaII_DE, MafiaIII, MafiaI_DE (optional)")] string? gameType = null)
    {
        try
        {
            GamesEnumerator? game = null;
            if (!string.IsNullOrEmpty(gameType) && Enum.TryParse<GamesEnumerator>(gameType, true, out var parsed)) game = parsed;

            var info = _sdsService.OpenFile(sdsPath, game);
            if (info == null) return McpError.OpenFailureJson(_sdsService);

            // Resolve which resource to decode.
            SdsResourceInfo resource;
            if (resourceIndex >= 0)
            {
                resource = info.Resources.FirstOrDefault(r => r.Index == resourceIndex);
                if (resource == null)
                    return JsonSerializer.Serialize(new { success = false, error = $"Resource index {resourceIndex} not found (0..{info.Resources.Count - 1})." });
            }
            else if (!string.IsNullOrEmpty(typeName))
            {
                resource = info.Resources.FirstOrDefault(r => r.TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase));
                if (resource == null)
                    return JsonSerializer.Serialize(new { success = false, error = $"No resource of type '{typeName}' in this SDS." });
            }
            else
            {
                return JsonSerializer.Serialize(new { success = false, error = "Provide resourceIndex, or typeName to select the first resource of a type." });
            }

            if (!SupportedTypes.Contains(resource.TypeName, StringComparer.OrdinalIgnoreCase))
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = $"No decoder for resource type '{resource.TypeName}'.",
                    resource = new { index = resource.Index, typeName = resource.TypeName, name = resource.Name },
                    supportedTypes = SupportedTypes
                });
            }

            var data = _sdsService.ExtractResource(sdsPath, resource.Index);
            if (data == null)
                return JsonSerializer.Serialize(new { success = false, error = "Failed to extract resource data." });

            var base64 = Convert.ToBase64String(data);

            string decoded = resource.TypeName.ToLowerInvariant() switch
            {
                "actors" => _decode.DecodeActors(null, base64, offset, limit),
                "frameresource" => _decode.DecodeFrameResource(null, base64, false, true, null, FindLoneFrameNameTable(sdsPath, info), offset, limit),
                "itemdesc" => _decode.DecodeItemDesc(null, base64),
                "collisions" => _decode.DecodeCollisions(null, base64, offset, limit),
                "effects" => _effects.ParseEffectsFromBytes(base64, -1),
                _ => null
            };

            using var decodedDoc = JsonDocument.Parse(decoded);
            return JsonSerializer.Serialize(new
            {
                success = true,
                resource = new { index = resource.Index, typeName = resource.TypeName, name = resource.Name, dataSize = resource.DataSize },
                decoded = decodedDoc.RootElement
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    /// <summary>
    /// A FrameResource's named frames live in a sibling FrameNameTable. When the archive holds
    /// exactly one, pass it through so decode_frame_resource can resolve names; otherwise skip
    /// (pairing is ambiguous).
    /// </summary>
    private string FindLoneFrameNameTable(string sdsPath, SdsFileInfo info)
    {
        var fnts = info.Resources.Where(r => r.TypeName == "FrameNameTable").ToList();
        if (fnts.Count != 1) return null;
        var data = _sdsService.ExtractResource(sdsPath, fnts[0].Index);
        return data == null ? null : Convert.ToBase64String(data);
    }
}
