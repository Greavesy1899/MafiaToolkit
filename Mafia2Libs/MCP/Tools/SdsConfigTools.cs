using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using ModelContextProtocol.Server;
using ResourceTypes.SDSConfig;

namespace Mafia2Tool.MCP.Tools;

/// <summary>
/// MCP tools for browsing and inspecting sdsconfig.bin (SDS resource configuration) files
/// </summary>
[McpServerToolType]
public class SdsConfigTools
{
    [McpServerTool(Name = "open_sds_config"), Description("Parse sdsconfig.bin and return overview: magic, version, and list of template names with counts. The file is located at <GameRoot>\\EDIT\\sdsconfig.bin")]
    public string OpenSdsConfig([Description("Full path to sdsconfig.bin (located at <GameRoot>\\EDIT\\sdsconfig.bin)")] string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var config = new SdsConfigFile(new FileInfo(filePath));
            return JsonSerializer.Serialize(new
            {
                success = true,
                magic = $"0x{SdsConfigFile.Magic:X8}",
                version = SdsConfigFile.Version,
                templateCount = config.Template.Length,
                templates = config.Template.Select((t, i) => new
                {
                    index = i,
                    name = t.Name,
                    baseSDSReferenceCount = t.BaseSDSReferences.Length,
                    virtualSlotCount = t.VirtualSlots.Length
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    [McpServerTool(Name = "get_sds_config_template"), Description("Get full detail for one template by index or name: BaseSDSReferences (Group[]) and VirtualSlots with nested SDSItems and SDSReferences. The file is located at <GameRoot>\\EDIT\\sdsconfig.bin")]
    public string GetSdsConfigTemplate(
        [Description("Full path to sdsconfig.bin (located at <GameRoot>\\EDIT\\sdsconfig.bin)")] string filePath,
        [Description("Template index (0-based); use -1 to search by name instead")] int templateIndex = -1,
        [Description("Template name to look up (used when templateIndex is -1)")] string? templateName = null)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var config = new SdsConfigFile(new FileInfo(filePath));

            Template? template = null;
            int resolvedIndex = templateIndex;

            if (templateIndex >= 0)
            {
                if (templateIndex >= config.Template.Length)
                    return JsonSerializer.Serialize(new { success = false, error = $"Template index {templateIndex} out of range (0-{config.Template.Length - 1})" });

                template = config.Template[templateIndex];
            }
            else if (!string.IsNullOrEmpty(templateName))
            {
                resolvedIndex = Array.FindIndex(config.Template, t => t.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase));
                if (resolvedIndex < 0)
                    return JsonSerializer.Serialize(new { success = false, error = $"Template '{templateName}' not found" });

                template = config.Template[resolvedIndex];
            }
            else
            {
                return JsonSerializer.Serialize(new { success = false, error = "Provide templateIndex >= 0 or a templateName" });
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                index = resolvedIndex,
                name = template.Name,
                baseSDSReferences = template.BaseSDSReferences.Select(g => new
                {
                    name = g.Name,
                    count = g.Count,
                    typeId = g.TypeId,
                    priority = g.Priority,
                    memorySize = g.MemorySize,
                    auxSize = g.AuxSize
                }).ToList(),
                virtualSlots = template.VirtualSlots.Select(vs => new
                {
                    name = vs.Name,
                    memoryBudget1 = vs.MemoryBudget1,
                    memoryBudget2 = vs.MemoryBudget2,
                    sdsItems = vs.SDSItems.Select(item => new
                    {
                        name = item.Name,
                        totalSize1 = item.TotalSize1,
                        totalSize2 = item.TotalSize2,
                        flags = item.Flags,
                        isDefault = item.IsDefault,
                        sdsReferences = item.SDSReferences.Select(r => new
                        {
                            name = r.Name,
                            count = r.Count,
                            typeId = r.TypeId,
                            priority = r.Priority,
                            memorySize = r.MemorySize,
                            auxSize = r.AuxSize
                        }).ToList()
                    }).ToList()
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    [McpServerTool(Name = "search_sds_config"), Description("Search all template/slot/item/reference names in sdsconfig.bin by case-insensitive substring pattern. The file is located at <GameRoot>\\EDIT\\sdsconfig.bin")]
    public string SearchSdsConfig(
        [Description("Full path to sdsconfig.bin (located at <GameRoot>\\EDIT\\sdsconfig.bin)")] string filePath,
        [Description("Search pattern (case-insensitive substring)")] string pattern,
        [Description("Maximum results per category (default: 50)")] int limit = 50)
    {
        try
        {
            if (!File.Exists(filePath))
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });

            var config = new SdsConfigFile(new FileInfo(filePath));
            limit = Math.Clamp(limit, 1, 200);

            var matchedTemplates = config.Template
                .Select((t, i) => new { t, i })
                .Where(x => x.t.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                .Select(x => new { index = x.i, name = x.t.Name })
                .ToList();

            var matchedBaseRefs = config.Template
                .SelectMany((t, ti) => t.BaseSDSReferences
                    .Where(g => g.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    .Select(g => new { templateIndex = ti, templateName = t.Name, name = g.Name }))
                .ToList();

            var matchedSlots = config.Template
                .SelectMany((t, ti) => t.VirtualSlots
                    .Select((vs, vi) => new { ti, t, vs, vi })
                    .Where(x => x.vs.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    .Select(x => new { templateIndex = x.ti, templateName = x.t.Name, slotIndex = x.vi, name = x.vs.Name }))
                .ToList();

            var matchedItems = config.Template
                .SelectMany((t, ti) => t.VirtualSlots
                    .SelectMany((vs, vi) => vs.SDSItems
                        .Select((item, ii) => new { ti, t, vi, vs, item, ii })
                        .Where(x => x.item.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                        .Select(x => new { templateIndex = x.ti, templateName = x.t.Name, slotIndex = x.vi, slotName = x.vs.Name, itemIndex = x.ii, name = x.item.Name })))
                .ToList();

            var matchedSdsRefs = config.Template
                .SelectMany((t, ti) => t.VirtualSlots
                    .SelectMany((vs, vi) => vs.SDSItems
                        .SelectMany((item, ii) => item.SDSReferences
                            .Where(r => r.Name.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                            .Select(r => new { templateIndex = ti, templateName = t.Name, slotIndex = vi, slotName = vs.Name, itemIndex = ii, itemName = item.Name, name = r.Name }))))
                .ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                pattern,
                templates = new { totalMatches = matchedTemplates.Count, results = matchedTemplates.Take(limit).ToList(), hasMore = matchedTemplates.Count > limit },
                baseSDSReferences = new { totalMatches = matchedBaseRefs.Count, results = matchedBaseRefs.Take(limit).ToList(), hasMore = matchedBaseRefs.Count > limit },
                virtualSlots = new { totalMatches = matchedSlots.Count, results = matchedSlots.Take(limit).ToList(), hasMore = matchedSlots.Count > limit },
                sdsItems = new { totalMatches = matchedItems.Count, results = matchedItems.Take(limit).ToList(), hasMore = matchedItems.Count > limit },
                sdsReferences = new { totalMatches = matchedSdsRefs.Count, results = matchedSdsRefs.Take(limit).ToList(), hasMore = matchedSdsRefs.Count > limit }
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }
}
