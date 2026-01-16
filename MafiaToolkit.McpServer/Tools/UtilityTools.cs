using System.ComponentModel;
using System.Text;
using System.Text.Json;
using ModelContextProtocol.Server;

namespace MafiaToolkit.McpServer.Tools;

/// <summary>
/// MCP tools for hash computation, file detection, and other utilities
/// </summary>
[McpServerToolType]
public class UtilityTools
{
    // FNV hash constants
    private const uint FNV32_INITIAL = 0x811C9DC5;
    private const uint FNV32_PRIME = 0x1000193;
    private const ulong FNV64_INITIAL = 0xCBF29CE484222325;
    private const ulong FNV64_PRIME = 0x00000100000001B3;

    public UtilityTools()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// Compute FNV32 hash for a string
    /// </summary>
    [McpServerTool(Name = "hash_fnv32"), Description("Compute FNV32 hash for a string (used by Mafia games for resource identification)")]
    public string HashFnv32(
        [Description("String to hash")] string input,
        [Description("Use Windows-1252 encoding (default: true, matches game behavior)")] bool useCodePage1252 = true)
    {
        try
        {
            var encoding = useCodePage1252 ? Encoding.GetEncoding(1252) : Encoding.UTF8;
            var bytes = encoding.GetBytes(input);
            var hash = ComputeFnv32(bytes);

            return JsonSerializer.Serialize(new
            {
                success = true,
                input,
                hash = hash,
                hashHex = $"0x{hash:X8}",
                hashSigned = unchecked((int)hash)
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Compute FNV64 hash for a string
    /// </summary>
    [McpServerTool(Name = "hash_fnv64"), Description("Compute FNV64 hash for a string (used by Mafia games for file/texture identification)")]
    public string HashFnv64(
        [Description("String to hash")] string input,
        [Description("Use Windows-1252 encoding (default: true, matches game behavior)")] bool useCodePage1252 = true)
    {
        try
        {
            var encoding = useCodePage1252 ? Encoding.GetEncoding(1252) : Encoding.UTF8;
            var bytes = encoding.GetBytes(input);
            var hash = ComputeFnv64(bytes);

            return JsonSerializer.Serialize(new
            {
                success = true,
                input,
                hash = hash,
                hashHex = $"0x{hash:X16}",
                hashSigned = unchecked((long)hash)
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Batch compute hashes for multiple strings
    /// </summary>
    [McpServerTool(Name = "hash_batch"), Description("Compute FNV32 and FNV64 hashes for multiple strings")]
    public string HashBatch(
        [Description("Strings to hash, separated by newlines or commas")] string inputs)
    {
        try
        {
            var encoding = Encoding.GetEncoding(1252);
            var strings = inputs.Split(new[] { '\n', '\r', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            var results = strings.Select(input =>
            {
                var bytes = encoding.GetBytes(input);
                return new
                {
                    input,
                    fnv32 = $"0x{ComputeFnv32(bytes):X8}",
                    fnv64 = $"0x{ComputeFnv64(bytes):X16}"
                };
            }).ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                count = results.Count,
                results
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Detect file format from magic bytes
    /// </summary>
    [McpServerTool(Name = "detect_file_format"), Description("Detect file format from magic bytes/signature")]
    public string DetectFileFormat(
        [Description("Full path to the file")] string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "File not found" });
            }

            var fileInfo = new FileInfo(filePath);
            byte[] header = new byte[Math.Min(128, fileInfo.Length)];

            using (var stream = File.OpenRead(filePath))
            {
                stream.Read(header, 0, header.Length);
            }

            var detection = DetectFormat(header, fileInfo.Extension);

            return JsonSerializer.Serialize(new
            {
                success = true,
                file = new
                {
                    path = filePath,
                    name = fileInfo.Name,
                    extension = fileInfo.Extension,
                    size = fileInfo.Length,
                    sizeFormatted = FormatSize(fileInfo.Length)
                },
                detection = new
                {
                    format = detection.Format,
                    description = detection.Description,
                    game = detection.Game,
                    magicBytes = BitConverter.ToString(header.Take(16).ToArray()).Replace("-", " "),
                    confidence = detection.Confidence
                }
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Detect format from raw bytes (base64 encoded)
    /// </summary>
    [McpServerTool(Name = "detect_format_from_bytes"), Description("Detect file format from base64-encoded bytes")]
    public string DetectFormatFromBytes(
        [Description("Base64-encoded bytes (at least first 16 bytes)")] string base64Data,
        [Description("File extension hint (optional)")] string? extensionHint = null)
    {
        try
        {
            var bytes = Convert.FromBase64String(base64Data);
            var detection = DetectFormat(bytes, extensionHint ?? "");

            return JsonSerializer.Serialize(new
            {
                success = true,
                detection = new
                {
                    format = detection.Format,
                    description = detection.Description,
                    game = detection.Game,
                    magicBytes = BitConverter.ToString(bytes.Take(16).ToArray()).Replace("-", " "),
                    confidence = detection.Confidence
                }
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Convert between number formats
    /// </summary>
    [McpServerTool(Name = "convert_number"), Description("Convert between decimal, hexadecimal, and binary representations")]
    public string ConvertNumber(
        [Description("Number to convert (supports 0x prefix for hex, 0b for binary)")] string input)
    {
        try
        {
            ulong value;

            if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = Convert.ToUInt64(input[2..], 16);
            }
            else if (input.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
            {
                value = Convert.ToUInt64(input[2..], 2);
            }
            else
            {
                value = ulong.Parse(input);
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                @decimal = value,
                hex = $"0x{value:X}",
                hex8 = $"0x{value:X8}",
                hex16 = $"0x{value:X16}",
                binary = $"0b{Convert.ToString((long)value, 2)}",
                signed32 = value <= uint.MaxValue ? unchecked((int)(uint)value) : (int?)null,
                signed64 = unchecked((long)value),
                bytes = BitConverter.GetBytes(value).Select(b => $"0x{b:X2}").ToArray()
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// List files matching a pattern in a directory
    /// </summary>
    [McpServerTool(Name = "list_game_files"), Description("List game files in a directory matching common Mafia file extensions")]
    public string ListGameFiles(
        [Description("Directory path to search")] string directoryPath,
        [Description("File extension filter (e.g., '.sds', '.mtl', '.dds'). Leave empty for all game files")] string? extensionFilter = null,
        [Description("Search subdirectories recursively")] bool recursive = true)
    {
        try
        {
            if (!Directory.Exists(directoryPath))
            {
                return JsonSerializer.Serialize(new { success = false, error = "Directory not found" });
            }

            var gameExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".sds", ".mtl", ".dds", ".act", ".nav", ".nhv", ".nov",
                ".bnk", ".pck", ".xml", ".lua", ".spe", ".fr", ".cut",
                ".gsd", ".prf", ".xbin", ".buf", ".an2", ".tbl"
            };

            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var pattern = string.IsNullOrEmpty(extensionFilter) ? "*.*" : $"*{extensionFilter}";

            var files = Directory.GetFiles(directoryPath, pattern, searchOption)
                .Where(f =>
                {
                    var ext = Path.GetExtension(f);
                    if (!string.IsNullOrEmpty(extensionFilter))
                        return ext.Equals(extensionFilter, StringComparison.OrdinalIgnoreCase);
                    return gameExtensions.Contains(ext);
                })
                .Take(500)
                .Select(f =>
                {
                    var info = new FileInfo(f);
                    return new
                    {
                        path = f,
                        name = info.Name,
                        extension = info.Extension,
                        size = info.Length,
                        sizeFormatted = FormatSize(info.Length)
                    };
                })
                .ToList();

            var extensionSummary = files
                .GroupBy(f => f.extension.ToLowerInvariant())
                .Select(g => new { extension = g.Key, count = g.Count() })
                .OrderByDescending(g => g.count)
                .ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                directory = directoryPath,
                filter = extensionFilter,
                count = files.Count,
                extensionSummary,
                files
            });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { success = false, error = ex.Message });
        }
    }

    private static uint ComputeFnv32(byte[] data)
    {
        uint hash = FNV32_INITIAL;
        foreach (var b in data)
        {
            hash *= FNV32_PRIME;
            hash ^= b;
        }
        return hash;
    }

    private static ulong ComputeFnv64(byte[] data)
    {
        ulong hash = FNV64_INITIAL;
        foreach (var b in data)
        {
            hash *= FNV64_PRIME;
            hash ^= b;
        }
        return hash;
    }

    private static (string Format, string Description, string Game, string Confidence) DetectFormat(byte[] header, string extension)
    {
        if (header.Length < 4)
            return ("Unknown", "File too small", "Unknown", "Low");

        uint magic32 = BitConverter.ToUInt32(header, 0);
        uint magic32BE = (uint)((header[0] << 24) | (header[1] << 16) | (header[2] << 8) | header[3]);

        // SDS Archive
        if (magic32BE == 0x53445300) // 'SDS\0'
            return ("SDS", "Mafia SDS Archive", "Mafia II/III/DE", "High");

        // Material Library
        if (magic32 == 0x424C544D) // 'MTLB'
            return ("MTL", "Material Library", "Mafia II/III/DE", "High");

        // DDS Texture
        if (magic32 == 0x20534444) // 'DDS '
        {
            var ddsInfo = ParseDdsHeader(header);
            return ("DDS", $"DirectDraw Surface ({ddsInfo})", "Any", "High");
        }

        // Wwise BNK
        if (magic32 == 0x444E4B42) // 'BKHD'
            return ("BNK", "Wwise Sound Bank", "Mafia II/III/DE", "High");

        // Wwise PCK
        if (magic32 == 0x4450424B) // 'AKPK'
            return ("PCK", "Wwise Package", "Mafia II/III/DE", "High");

        // XML
        if (header[0] == '<' || (header[0] == 0xEF && header[1] == 0xBB && header[2] == 0xBF && header[3] == '<'))
            return ("XML", "XML Document", "Any", "High");

        // Lua bytecode
        if (header[0] == 0x1B && header[1] == 0x4C && header[2] == 0x75 && header[3] == 0x61)
            return ("LUA", "Compiled Lua Bytecode", "Mafia II/III/DE", "High");

        // PNG
        if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
            return ("PNG", "PNG Image", "Any", "High");

        // FSB (FMOD)
        if (magic32 == 0x35425346) // 'FSB5'
            return ("FSB", "FMOD Sound Bank", "Mafia II", "High");

        // XBin
        if (extension.Equals(".xbin", StringComparison.OrdinalIgnoreCase))
            return ("XBin", "XBin Data Container", "Mafia III/I:DE", "Medium");

        // Frame Resource
        if (extension.Equals(".fr", StringComparison.OrdinalIgnoreCase))
            return ("FrameResource", "3D Model/Scene Data", "Mafia II/DE", "Medium");

        // Actor
        if (extension.Equals(".act", StringComparison.OrdinalIgnoreCase))
            return ("Actor", "Actor Definition", "Mafia II/DE", "Medium");

        // Navigation
        if (extension.Equals(".nav", StringComparison.OrdinalIgnoreCase))
            return ("NAV", "Navigation AI Data", "Mafia II/III/DE", "Medium");

        // Cutscene
        if (extension.Equals(".cut", StringComparison.OrdinalIgnoreCase))
            return ("Cutscene", "Cutscene Animation", "Mafia II/DE", "Medium");

        return ("Unknown", "Unknown format", "Unknown", "Low");
    }

    private static string ParseDdsHeader(byte[] header)
    {
        if (header.Length < 128)
            return "incomplete header";

        try
        {
            uint height = BitConverter.ToUInt32(header, 12);
            uint width = BitConverter.ToUInt32(header, 16);
            uint mipCount = BitConverter.ToUInt32(header, 28);
            uint fourCC = BitConverter.ToUInt32(header, 84);

            string format;
            if (fourCC == 0x31545844) format = "DXT1";
            else if (fourCC == 0x33545844) format = "DXT3";
            else if (fourCC == 0x35545844) format = "DXT5";
            else if (fourCC == 0x30315844) format = "DX10";
            else if (fourCC == 0x31495441) format = "ATI1";
            else if (fourCC == 0x32495441) format = "ATI2";
            else format = $"0x{fourCC:X8}";

            return $"{width}x{height}, {format}, {mipCount} mips";
        }
        catch
        {
            return "parse error";
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
