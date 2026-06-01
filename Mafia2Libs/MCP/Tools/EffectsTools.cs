using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using ModelContextProtocol.Server;
using ResourceTypes.Effects;

namespace Mafia2Tool.MCP.Tools;

/// <summary>
/// MCP tools for parsing Mafia II ".eff" files (the SDS "Effects" resource): the effect
/// (pattern) list with names/kinds, and per-effect generations, operators and parameters.
/// </summary>
[McpServerToolType]
public class EffectsTools
{
    [McpServerTool(Name = "parse_effects_file"), Description("Parse a Mafia II .eff (Effects) file. Returns the effect list, or full detail for one effect when effectIndex is set.")]
    public string ParseEffectsFile(
        [Description("Full path to the .eff file")] string filePath,
        [Description("Index of a single effect to return in full detail (default: -1 = list all)")] int effectIndex = -1)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "File not found: " + filePath });
            }

            var eff = new EffectsFile();
            eff.ReadFromFile(filePath);
            return BuildJson(eff, effectIndex);
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    [McpServerTool(Name = "parse_effects_from_bytes"), Description("Parse .eff (Effects) content from base64 bytes (e.g. the output of extract_resource on an 'Effects' resource).")]
    public string ParseEffectsFromBytes(
        [Description("Base64-encoded .eff bytes")] string base64Data,
        [Description("Index of a single effect to return in full detail (default: -1 = list all)")] int effectIndex = -1)
    {
        try
        {
            var bytes = Convert.FromBase64String(base64Data);
            var eff = new EffectsFile();
            eff.ReadFromBytes(bytes);
            return BuildJson(eff, effectIndex);
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    private static string BuildJson(EffectsFile eff, int effectIndex)
    {
        var summary = eff.GetSummary();

        if (eff.IsRawFallback)
        {
            return JsonSerializer.Serialize(new
            {
                success = true,
                rawFallback = true,
                note = "File could not be parsed losslessly; treated as opaque.",
                effectCount = 0
            });
        }

        if (effectIndex >= 0)
        {
            if (effectIndex >= summary.Count)
            {
                return JsonSerializer.Serialize(new { success = false, error = "effectIndex out of range (0.." + (summary.Count - 1) + ")" });
            }
            return JsonSerializer.Serialize(new
            {
                success = true,
                effectCount = summary.Count,
                effect = FullEffect(summary[effectIndex], effectIndex)
            });
        }

        return JsonSerializer.Serialize(new
        {
            success = true,
            effectCount = summary.Count,
            effects = summary.Select((e, i) => new
            {
                index = i,
                id = e.Id,
                name = e.DisplayName,
                kind = e.Kind,
                generations = e.GenerationCount,
                frames = e.FrameCount,
                sounds = e.SoundCount,
                generationNames = e.Generations.Select(g => g.Name).ToList(),
                operators = e.Generations.SelectMany(g => g.Operators.Select(o => o.Type ?? ("Operator" + o.TypeId)))
                                          .Distinct().ToList()
            }).ToList()
        });
    }

    private static object FullEffect(EffectPatternInfo e, int index)
    {
        return new
        {
            index,
            id = e.Id,
            name = e.DisplayName,
            kind = e.Kind,
            generations = e.Generations.Select(g => new
            {
                name = g.Name,
                operators = g.Operators.Select(o => new
                {
                    type = o.Type ?? ("Operator" + o.TypeId),
                    typeId = o.TypeId,
                    enabled = o.Enabled,
                    parameters = o.Parameters.Select(p => new
                    {
                        tag = p.Tag,
                        name = p.Name,
                        animated = p.IsAnimated,
                        keyCount = p.KeyCount,
                        summary = p.Summary,
                        values = p.Values
                    }).ToList()
                }).ToList()
            }).ToList(),
            frames = e.Frames.Select(f => new { type = f.TypeName, classId = f.ClassId, position = f.Transform }).ToList(),
            soundElements = e.Sounds.Select(s => s.Name).ToList()
        };
    }
}
