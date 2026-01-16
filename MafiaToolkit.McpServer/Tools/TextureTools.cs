using System.ComponentModel;
using System.Text.Json;
using MafiaToolkit.McpServer.Services;
using ModelContextProtocol.Server;

namespace MafiaToolkit.McpServer.Tools;

/// <summary>
/// MCP tools for inspecting textures (DDS files) within SDS archives and standalone
/// </summary>
[McpServerToolType]
public class TextureTools
{
    private readonly SdsService _sdsService;

    public TextureTools(SdsService sdsService)
    {
        _sdsService = sdsService;
    }

    /// <summary>
    /// Inspect a DDS texture file
    /// </summary>
    [McpServerTool(Name = "inspect_dds_file"), Description("Inspect a DDS texture file and get its metadata")]
    public string InspectDdsFile(
        [Description("Full path to the DDS file")] string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });
            }

            var fileInfo = new FileInfo(filePath);
            byte[] header = new byte[Math.Min(148, fileInfo.Length)];

            using (var stream = File.OpenRead(filePath))
            {
                stream.Read(header, 0, header.Length);
            }

            var ddsInfo = ParseDdsHeader(header);

            if (ddsInfo == null)
            {
                return JsonSerializer.Serialize(new { success = false, error = "Invalid DDS file" });
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
                texture = ddsInfo
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Inspect a texture resource within an SDS file
    /// </summary>
    [McpServerTool(Name = "inspect_sds_texture"), Description("Inspect a texture resource within an SDS archive")]
    public string InspectSdsTexture(
        [Description("Full path to the SDS file")] string sdsPath,
        [Description("Resource index of the texture")] int resourceIndex)
    {
        try
        {
            var resourceInfo = _sdsService.GetResourceInfo(sdsPath, resourceIndex);
            if (resourceInfo == null)
            {
                return JsonSerializer.Serialize(new { success = false, error = "Resource not found" });
            }

            if (!resourceInfo.TypeName.Contains("Texture", StringComparison.OrdinalIgnoreCase))
            {
                return JsonSerializer.Serialize(new
                {
                    success = false,
                    error = $"Resource is not a texture (type: {resourceInfo.TypeName})"
                });
            }

            var data = _sdsService.ExtractResource(sdsPath, resourceIndex);
            if (data == null || data.Length < 10)
            {
                return JsonSerializer.Serialize(new { success = false, error = "Failed to extract resource data" });
            }

            // Parse Mafia texture resource format
            // First 8 bytes: name hash (FNV64)
            // Byte 8: Unknown
            // Byte 9: HasMIP flag (version 2)
            // Rest: DDS data

            ulong nameHash = BitConverter.ToUInt64(data, 0);
            byte unknown8 = data[8];
            byte hasMip = resourceInfo.Version == 2 && data.Length > 9 ? data[9] : (byte)0;

            int ddsOffset = resourceInfo.Version == 2 ? 10 : 9;
            if (data.Length <= ddsOffset + 128)
            {
                return JsonSerializer.Serialize(new { success = false, error = "Texture data too small" });
            }

            var ddsData = data.Skip(ddsOffset).ToArray();
            var ddsInfo = ParseDdsHeader(ddsData);

            return JsonSerializer.Serialize(new
            {
                success = true,
                resource = new
                {
                    index = resourceIndex,
                    name = resourceInfo.Name,
                    typeName = resourceInfo.TypeName,
                    version = resourceInfo.Version,
                    totalSize = data.Length,
                    totalSizeFormatted = FormatSize(data.Length)
                },
                textureHeader = new
                {
                    nameHash = $"0x{nameHash:X16}",
                    unknown8,
                    hasMip = hasMip == 1
                },
                dds = ddsInfo
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// List all textures in an SDS file
    /// </summary>
    [McpServerTool(Name = "list_sds_textures"), Description("List all texture resources in an SDS archive")]
    public string ListSdsTextures(
        [Description("Full path to the SDS file")] string sdsPath,
        [Description("Include DDS metadata for each texture (slower)")] bool includeMetadata = false)
    {
        try
        {
            var sdsInfo = _sdsService.OpenFile(sdsPath);
            if (sdsInfo == null)
            {
                return JsonSerializer.Serialize(new { success = false, error = "Failed to open SDS file" });
            }

            var textures = sdsInfo.Resources
                .Where(r => r.TypeName.Contains("Texture", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var textureInfos = new List<object>();

            foreach (var tex in textures)
            {
                object texInfo;

                if (includeMetadata)
                {
                    var data = _sdsService.ExtractResource(sdsPath, tex.Index);
                    DdsInfo? ddsInfo = null;

                    if (data != null && data.Length > 20)
                    {
                        int ddsOffset = tex.Version == 2 ? 10 : 9;
                        if (data.Length > ddsOffset + 128)
                        {
                            var ddsData = data.Skip(ddsOffset).ToArray();
                            ddsInfo = ParseDdsHeader(ddsData);
                        }
                    }

                    texInfo = new
                    {
                        index = tex.Index,
                        name = tex.Name,
                        typeName = tex.TypeName,
                        version = tex.Version,
                        dataSize = tex.DataSize,
                        dataSizeFormatted = FormatSize(tex.DataSize),
                        dds = ddsInfo != null ? new
                        {
                            ddsInfo.Width,
                            ddsInfo.Height,
                            ddsInfo.Format,
                            ddsInfo.MipCount
                        } : null
                    };
                }
                else
                {
                    texInfo = new
                    {
                        index = tex.Index,
                        name = tex.Name,
                        typeName = tex.TypeName,
                        version = tex.Version,
                        dataSize = tex.DataSize,
                        dataSizeFormatted = FormatSize(tex.DataSize)
                    };
                }

                textureInfos.Add(texInfo);
            }

            // Group by type
            var typeSummary = textures
                .GroupBy(t => t.TypeName)
                .Select(g => new { type = g.Key, count = g.Count() })
                .ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                sdsFile = sdsInfo.FileName,
                textureCount = textures.Count,
                typeSummary,
                textures = textureInfos
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Inspect DDS data from base64
    /// </summary>
    [McpServerTool(Name = "inspect_dds_bytes"), Description("Inspect DDS texture from base64-encoded bytes")]
    public string InspectDdsBytes(
        [Description("Base64-encoded DDS data (at least first 148 bytes)")] string base64Data)
    {
        try
        {
            var data = Convert.FromBase64String(base64Data);
            var ddsInfo = ParseDdsHeader(data);

            if (ddsInfo == null)
            {
                return JsonSerializer.Serialize(new { success = false, error = "Invalid DDS data" });
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                dataSize = data.Length,
                dataSizeFormatted = FormatSize(data.Length),
                dds = ddsInfo
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    private class DdsInfo
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint Depth { get; set; }
        public uint MipCount { get; set; }
        public string Format { get; set; } = "Unknown";
        public string FormatDetails { get; set; } = "";
        public uint Flags { get; set; }
        public string[] FlagNames { get; set; } = Array.Empty<string>();
        public bool IsCubemap { get; set; }
        public bool IsVolumeTexture { get; set; }
        public bool IsDX10 { get; set; }
        public uint? DX10Format { get; set; }
        public uint? DX10Dimension { get; set; }
        public uint PitchOrLinearSize { get; set; }
        public uint DataOffset { get; set; }
        public long EstimatedDataSize { get; set; }
    }

    private static DdsInfo? ParseDdsHeader(byte[] data)
    {
        if (data.Length < 128)
            return null;

        // Check DDS magic
        uint magic = BitConverter.ToUInt32(data, 0);
        if (magic != 0x20534444) // 'DDS '
            return null;

        var info = new DdsInfo();

        // DDS_HEADER starts at offset 4
        uint headerSize = BitConverter.ToUInt32(data, 4);
        info.Flags = BitConverter.ToUInt32(data, 8);
        info.Height = BitConverter.ToUInt32(data, 12);
        info.Width = BitConverter.ToUInt32(data, 16);
        info.PitchOrLinearSize = BitConverter.ToUInt32(data, 20);
        info.Depth = BitConverter.ToUInt32(data, 24);
        info.MipCount = BitConverter.ToUInt32(data, 28);

        if (info.MipCount == 0) info.MipCount = 1;

        // Parse flags
        var flagNames = new List<string>();
        if ((info.Flags & 0x1) != 0) flagNames.Add("CAPS");
        if ((info.Flags & 0x2) != 0) flagNames.Add("HEIGHT");
        if ((info.Flags & 0x4) != 0) flagNames.Add("WIDTH");
        if ((info.Flags & 0x8) != 0) flagNames.Add("PITCH");
        if ((info.Flags & 0x1000) != 0) flagNames.Add("PIXELFORMAT");
        if ((info.Flags & 0x20000) != 0) flagNames.Add("MIPMAPCOUNT");
        if ((info.Flags & 0x80000) != 0) flagNames.Add("LINEARSIZE");
        if ((info.Flags & 0x800000) != 0) flagNames.Add("DEPTH");
        info.FlagNames = flagNames.ToArray();

        // DDS_PIXELFORMAT at offset 76
        uint pfFlags = BitConverter.ToUInt32(data, 80);
        uint fourCC = BitConverter.ToUInt32(data, 84);
        uint rgbBitCount = BitConverter.ToUInt32(data, 88);

        // Parse format
        if ((pfFlags & 0x4) != 0) // DDPF_FOURCC
        {
            info.Format = FourCCToString(fourCC);

            if (fourCC == 0x30315844) // 'DX10'
            {
                info.IsDX10 = true;
                info.DataOffset = 148;

                if (data.Length >= 148)
                {
                    info.DX10Format = BitConverter.ToUInt32(data, 128);
                    info.DX10Dimension = BitConverter.ToUInt32(data, 132);
                    info.Format = GetDX10FormatName(info.DX10Format.Value);
                }
            }
            else
            {
                info.DataOffset = 128;
            }

            info.FormatDetails = GetFormatDetails(info.Format);
        }
        else if ((pfFlags & 0x40) != 0) // DDPF_RGB
        {
            info.Format = $"RGB{rgbBitCount}";
            info.DataOffset = 128;
            info.FormatDetails = $"{rgbBitCount}-bit uncompressed RGB";
        }
        else if ((pfFlags & 0x20000) != 0) // DDPF_LUMINANCE
        {
            info.Format = $"L{rgbBitCount}";
            info.DataOffset = 128;
            info.FormatDetails = $"{rgbBitCount}-bit luminance";
        }
        else
        {
            info.Format = "Unknown";
            info.DataOffset = 128;
        }

        // Check caps2 for cubemap/volume
        uint caps2 = BitConverter.ToUInt32(data, 112);
        info.IsCubemap = (caps2 & 0x200) != 0;
        info.IsVolumeTexture = (caps2 & 0x200000) != 0;

        // Estimate data size
        info.EstimatedDataSize = EstimateTextureSize(info);

        return info;
    }

    private static string FourCCToString(uint fourCC)
    {
        return fourCC switch
        {
            0x31545844 => "DXT1",
            0x32545844 => "DXT2",
            0x33545844 => "DXT3",
            0x34545844 => "DXT4",
            0x35545844 => "DXT5",
            0x30315844 => "DX10",
            0x31495441 => "ATI1",
            0x32495441 => "ATI2",
            0x55344342 => "BC4U",
            0x53344342 => "BC4S",
            0x55354342 => "BC5U",
            0x53354342 => "BC5S",
            _ => $"0x{fourCC:X8}"
        };
    }

    private static string GetDX10FormatName(uint format)
    {
        return format switch
        {
            71 => "BC1_UNORM",
            72 => "BC1_UNORM_SRGB",
            74 => "BC2_UNORM",
            75 => "BC2_UNORM_SRGB",
            77 => "BC3_UNORM",
            78 => "BC3_UNORM_SRGB",
            80 => "BC4_UNORM",
            81 => "BC4_SNORM",
            83 => "BC5_UNORM",
            84 => "BC5_SNORM",
            95 => "BC6H_UF16",
            96 => "BC6H_SF16",
            98 => "BC7_UNORM",
            99 => "BC7_UNORM_SRGB",
            28 => "R8G8B8A8_UNORM",
            29 => "R8G8B8A8_UNORM_SRGB",
            87 => "B8G8R8A8_UNORM",
            91 => "B8G8R8A8_UNORM_SRGB",
            _ => $"DXGI_{format}"
        };
    }

    private static string GetFormatDetails(string format)
    {
        return format switch
        {
            "DXT1" => "BC1 - 4bpp, 1-bit alpha",
            "DXT3" => "BC2 - 8bpp, explicit alpha",
            "DXT5" => "BC3 - 8bpp, interpolated alpha",
            "ATI1" or "BC4U" or "BC4_UNORM" => "BC4 - 4bpp, single channel",
            "ATI2" or "BC5U" or "BC5_UNORM" => "BC5 - 8bpp, two channels (normal maps)",
            "BC1_UNORM" or "BC1_UNORM_SRGB" => "BC1 - 4bpp, 1-bit alpha",
            "BC2_UNORM" or "BC2_UNORM_SRGB" => "BC2 - 8bpp, explicit alpha",
            "BC3_UNORM" or "BC3_UNORM_SRGB" => "BC3 - 8bpp, interpolated alpha",
            "BC6H_UF16" or "BC6H_SF16" => "BC6H - 8bpp, HDR",
            "BC7_UNORM" or "BC7_UNORM_SRGB" => "BC7 - 8bpp, high quality",
            _ => ""
        };
    }

    private static long EstimateTextureSize(DdsInfo info)
    {
        int bpp = info.Format switch
        {
            "DXT1" or "BC1_UNORM" or "BC1_UNORM_SRGB" or "ATI1" or "BC4U" or "BC4_UNORM" => 4,
            _ => 8
        };

        long size = 0;
        uint w = info.Width, h = info.Height;

        for (int i = 0; i < info.MipCount; i++)
        {
            uint blocksW = Math.Max(1, (w + 3) / 4);
            uint blocksH = Math.Max(1, (h + 3) / 4);
            size += blocksW * blocksH * bpp * 2;

            w = Math.Max(1, w / 2);
            h = Math.Max(1, h / 2);
        }

        if (info.IsCubemap) size *= 6;

        return size;
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
