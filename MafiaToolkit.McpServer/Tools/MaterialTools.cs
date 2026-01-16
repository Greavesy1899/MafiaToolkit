using System.ComponentModel;
using System.Text;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace MafiaToolkit.McpServer.Tools;

/// <summary>
/// MCP tools for browsing and inspecting Material Library (MTL) files
/// </summary>
[McpServerToolType]
public class MaterialTools
{
    public MaterialTools()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// Open and parse a material library file
    /// </summary>
    [McpServerTool(Name = "open_mtl_file"), Description("Open a Material Library (.mtl) file and get its contents")]
    public string OpenMtlFile(
        [Description("Full path to the MTL file")] string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });
            }

            var fileInfo = new FileInfo(filePath);

            using var stream = File.OpenRead(filePath);
            using var reader = new BinaryReader(stream);

            // Read header
            var magic = new string(reader.ReadChars(4));
            if (magic != "MTLB")
            {
                return JsonSerializer.Serialize(new { success = false, error = "Invalid MTL file (bad magic)" });
            }

            int version = reader.ReadInt32();
            int materialCount = reader.ReadInt32();
            int unknown = reader.ReadInt32();

            string versionName = version switch
            {
                57 => "V57 (Mafia II)",
                58 => "V58 (Mafia II: DE)",
                63 => "V63 (Mafia III / Mafia I: DE)",
                _ => $"Unknown ({version})"
            };

            // Parse materials
            var materials = new List<object>();
            for (int i = 0; i < materialCount && stream.Position < stream.Length; i++)
            {
                var mat = ParseMaterial(reader, version);
                if (mat != null)
                {
                    materials.Add(mat);
                }
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                file = new
                {
                    path = filePath,
                    name = fileInfo.Name,
                    size = fileInfo.Length,
                    sizeFormatted = FormatSize(fileInfo.Length)
                },
                header = new
                {
                    magic,
                    version,
                    versionName,
                    materialCount,
                    unknown
                },
                materials = materials.Take(100).ToList(),
                hasMore = materials.Count > 100
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// List all MTL files in a directory
    /// </summary>
    [McpServerTool(Name = "list_mtl_files"), Description("List all Material Library files in a directory")]
    public string ListMtlFiles(
        [Description("Directory path to search")] string directoryPath,
        [Description("Search subdirectories recursively")] bool recursive = true)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "Directory not found" });
            }

            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var files = Directory.GetFiles(directoryPath, "*.mtl", searchOption);

            var mtlFiles = new List<object>();
            foreach (var file in files.Take(200))
            {
                try
                {
                    var info = new FileInfo(file);
                    int? version = null;
                    int? materialCount = null;

                    using (var stream = File.OpenRead(file))
                    using (var reader = new BinaryReader(stream))
                    {
                        if (stream.Length >= 16)
                        {
                            var magic = new string(reader.ReadChars(4));
                            if (magic == "MTLB")
                            {
                                version = reader.ReadInt32();
                                materialCount = reader.ReadInt32();
                            }
                        }
                    }

                    mtlFiles.Add(new
                    {
                        path = file,
                        name = info.Name,
                        size = info.Length,
                        sizeFormatted = FormatSize(info.Length),
                        version,
                        versionName = version switch
                        {
                            57 => "V57 (Mafia II)",
                            58 => "V58 (Mafia II: DE)",
                            63 => "V63 (Mafia III / I: DE)",
                            _ => version?.ToString()
                        },
                        materialCount
                    });
                }
                catch
                {
                    // Skip files we can't read
                }
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                directory = directoryPath,
                count = mtlFiles.Count,
                totalFound = files.Length,
                files = mtlFiles
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Get detailed info about a specific material
    /// </summary>
    [McpServerTool(Name = "get_material_info"), Description("Get detailed information about a specific material in an MTL file")]
    public string GetMaterialInfo(
        [Description("Full path to the MTL file")] string filePath,
        [Description("Material index (0-based)")] int materialIndex)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });
            }

            using var stream = File.OpenRead(filePath);
            using var reader = new BinaryReader(stream);

            var magic = new string(reader.ReadChars(4));
            if (magic != "MTLB")
            {
                return JsonSerializer.Serialize(new { success = false, error = "Invalid MTL file" });
            }

            int version = reader.ReadInt32();
            int materialCount = reader.ReadInt32();
            reader.ReadInt32(); // unknown

            if (materialIndex < 0 || materialIndex >= materialCount)
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = $"Material index out of range (0-{materialCount - 1})"
                });
            }

            // Skip to the requested material
            for (int i = 0; i < materialIndex; i++)
            {
                SkipMaterial(reader, version);
            }

            var material = ParseMaterialDetailed(reader, version);

            return JsonSerializer.Serialize(new
            {
                success = true,
                mtlVersion = version,
                materialIndex,
                material
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Search for materials by name
    /// </summary>
    [McpServerTool(Name = "search_materials"), Description("Search for materials by name pattern in an MTL file")]
    public string SearchMaterials(
        [Description("Full path to the MTL file")] string filePath,
        [Description("Search pattern (case-insensitive substring match)")] string pattern,
        [Description("Maximum results to return")] int limit = 50)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });
            }

            using var stream = File.OpenRead(filePath);
            using var reader = new BinaryReader(stream);

            var magic = new string(reader.ReadChars(4));
            if (magic != "MTLB")
            {
                return JsonSerializer.Serialize(new { success = false, error = "Invalid MTL file" });
            }

            int version = reader.ReadInt32();
            int materialCount = reader.ReadInt32();
            reader.ReadInt32(); // unknown

            var matches = new List<object>();

            for (int i = 0; i < materialCount && stream.Position < stream.Length; i++)
            {
                var mat = ParseMaterial(reader, version);
                if (mat != null)
                {
                    var name = mat.GetType().GetProperty("name")?.GetValue(mat)?.ToString() ?? "";
                    if (name.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    {
                        matches.Add(new
                        {
                            index = i,
                            material = mat
                        });

                        if (matches.Count >= limit)
                            break;
                    }
                }
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                searchPattern = pattern,
                totalMaterials = materialCount,
                matchCount = matches.Count,
                matches
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    private static object? ParseMaterial(BinaryReader reader, int version)
    {
        try
        {
            long startPos = reader.BaseStream.Position;

            // Read material name (hash + string)
            ulong nameHash = reader.ReadUInt64();
            ushort nameLength = reader.ReadUInt16();
            string name = nameLength > 0 && nameLength < 1024
                ? Encoding.GetEncoding(1252).GetString(reader.ReadBytes(nameLength))
                : "";

            // Read shader info
            ulong shaderID = reader.ReadUInt64();
            uint shaderHash = reader.ReadUInt32();

            // Read flags
            uint flags = reader.ReadUInt32();

            // Skip the rest of the material data based on version
            SkipMaterialContent(reader, version);

            return new
            {
                name,
                nameHash = $"0x{nameHash:X16}",
                shaderID = $"0x{shaderID:X16}",
                shaderHash = $"0x{shaderHash:X8}",
                flags = $"0x{flags:X8}"
            };
        }
        catch
        {
            return null;
        }
    }

    private static object? ParseMaterialDetailed(BinaryReader reader, int version)
    {
        try
        {
            // Read material name
            ulong nameHash = reader.ReadUInt64();
            ushort nameLength = reader.ReadUInt16();
            string name = nameLength > 0 && nameLength < 1024
                ? Encoding.GetEncoding(1252).GetString(reader.ReadBytes(nameLength))
                : "";

            // Read shader info
            ulong shaderID = reader.ReadUInt64();
            uint shaderHash = reader.ReadUInt32();
            uint flags = reader.ReadUInt32();

            // Read samplers count
            byte samplerCount = reader.ReadByte();
            var samplers = new List<object>();

            for (int i = 0; i < samplerCount; i++)
            {
                var sampler = ParseSampler(reader, version);
                if (sampler != null)
                    samplers.Add(sampler);
            }

            // Read parameters
            int paramCount = reader.ReadInt32();
            var parameters = new List<object>();

            for (int i = 0; i < paramCount && i < 100; i++)
            {
                var param = ParseParameter(reader);
                if (param != null)
                    parameters.Add(param);
            }

            // Collect texture names from samplers
            var textures = samplers
                .Select(s => s.GetType().GetProperty("textureName")?.GetValue(s)?.ToString())
                .Where(t => !string.IsNullOrEmpty(t))
                .ToList();

            return new
            {
                name,
                nameHash = $"0x{nameHash:X16}",
                shaderID = $"0x{shaderID:X16}",
                shaderHash = $"0x{shaderHash:X8}",
                flags = $"0x{flags:X8}",
                flagsDecoded = DecodeMaterialFlags(flags),
                samplerCount,
                samplers,
                parameterCount = paramCount,
                parameters,
                textures
            };
        }
        catch (Exception ex)
        {
            return new { error = ex.Message };
        }
    }

    private static object? ParseSampler(BinaryReader reader, int version)
    {
        try
        {
            // Sampler ID (4 chars like "S000")
            string id = new string(reader.ReadChars(4));

            // Sampler states (6 bytes)
            byte[] states = reader.ReadBytes(6);

            // Texture hash
            ulong textureHash = reader.ReadUInt64();

            // Texture name
            ushort texNameLength = reader.ReadUInt16();
            string textureName = texNameLength > 0 && texNameLength < 1024
                ? Encoding.GetEncoding(1252).GetString(reader.ReadBytes(texNameLength))
                : "";

            return new
            {
                id,
                textureHash = $"0x{textureHash:X16}",
                textureName,
                samplerStates = states.Select(b => $"0x{b:X2}").ToArray()
            };
        }
        catch
        {
            return null;
        }
    }

    private static object? ParseParameter(BinaryReader reader)
    {
        try
        {
            string id = new string(reader.ReadChars(4));
            int paramCount = reader.ReadInt32();

            var values = new float[paramCount];
            for (int i = 0; i < paramCount && i < 16; i++)
            {
                values[i] = reader.ReadSingle();
            }

            return new
            {
                id,
                valueCount = paramCount,
                values = values.Take(paramCount).Select(v => Math.Round(v, 4)).ToArray()
            };
        }
        catch
        {
            return null;
        }
    }

    private static void SkipMaterial(BinaryReader reader, int version)
    {
        try
        {
            // Skip name
            reader.ReadUInt64(); // hash
            ushort nameLength = reader.ReadUInt16();
            if (nameLength > 0 && nameLength < 1024)
                reader.BaseStream.Position += nameLength;

            // Skip shader info and flags
            reader.ReadUInt64(); // shaderID
            reader.ReadUInt32(); // shaderHash
            reader.ReadUInt32(); // flags

            SkipMaterialContent(reader, version);
        }
        catch
        {
            // Best effort skip
        }
    }

    private static void SkipMaterialContent(BinaryReader reader, int version)
    {
        try
        {
            // Skip samplers
            byte samplerCount = reader.ReadByte();
            for (int i = 0; i < samplerCount; i++)
            {
                reader.BaseStream.Position += 4; // ID
                reader.BaseStream.Position += 6; // states
                reader.ReadUInt64(); // texture hash
                ushort texNameLength = reader.ReadUInt16();
                if (texNameLength > 0 && texNameLength < 1024)
                    reader.BaseStream.Position += texNameLength;
            }

            // Skip parameters
            int paramCount = reader.ReadInt32();
            for (int i = 0; i < paramCount && i < 100; i++)
            {
                reader.BaseStream.Position += 4; // ID
                int valueCount = reader.ReadInt32();
                reader.BaseStream.Position += valueCount * 4; // floats
            }
        }
        catch
        {
            // Best effort skip
        }
    }

    private static string[] DecodeMaterialFlags(uint flags)
    {
        var flagNames = new List<string>();

        // Common material flags (based on Mafia II material system)
        if ((flags & 0x1) != 0) flagNames.Add("ALPHA_TEST");
        if ((flags & 0x2) != 0) flagNames.Add("ALPHA_BLEND");
        if ((flags & 0x4) != 0) flagNames.Add("ADDITIVE");
        if ((flags & 0x8) != 0) flagNames.Add("TWO_SIDED");
        if ((flags & 0x10) != 0) flagNames.Add("DEPTH_WRITE");
        if ((flags & 0x20) != 0) flagNames.Add("DEPTH_TEST");
        if ((flags & 0x100) != 0) flagNames.Add("DECAL");
        if ((flags & 0x200) != 0) flagNames.Add("INSTANCED");
        if ((flags & 0x1000) != 0) flagNames.Add("VEGETATION");
        if ((flags & 0x2000) != 0) flagNames.Add("SKIN");
        if ((flags & 0x10000) != 0) flagNames.Add("HAIR");

        return flagNames.ToArray();
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
