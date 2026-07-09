# Mafia Toolkit

[![Build status](https://ci.appveyor.com/api/projects/status/62dtija7vekn7htn/branch/master?svg=true)](https://ci.appveyor.com/project/Greavesy1899/mafia2toolkit)
[![.NET](https://github.com/Greavesy1899/MafiaToolkit/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Greavesy1899/MafiaToolkit/actions/workflows/dotnet.yml)

A Windows modding toolkit for the Mafia series with file format parsers, editors, and a 3D map editor.

## Supported Games

| Game | SDS | Materials | Map Editor | XBin |
|------|-----|-----------|------------|------|
| Mafia II | ✓ | ✓ | ✓ | - |
| Mafia II: Definitive Edition | ✓ | ✓ | ✓ | - |
| Mafia III | ✓ | ✓ | - | ✓ |
| Mafia I: Definitive Edition | ✓ | ✓ | - | ✓ |

Mafia 1 Classic is **not** supported.

## Building

**Requirements:** Windows, .NET 8 SDK, Visual Studio 2022

```bash
# Build the toolkit
dotnet build MafiaToolkit.sln

# Or with output directory
dotnet build --output "_out"
```

### M2FBX (Optional Native Library)

Required for FBX model import/export and collision cooking.

**Additional requirements:** [Autodesk FBX SDK](https://adsdk.autodesk.com/fbx), PhysX 2.8.X

```bash
cd M2FBX/M2FBX
premake5 vs2022
# Open M2FBX.sln in Visual Studio and build
```

## Editors

| Editor | File Types | Description |
|--------|------------|-------------|
| **Map Editor** | `FrameResource.fr` | 3D scene editor with DirectX 11 rendering. Edit geometry, collisions, materials. Export/import via FBX. |
| **Material Editor** | `.mtl` | Edit materials, parameters, samplers. Supports MTL versions 57, 58, 63. Merge materials between files. |
| **XBin Editor** | `.xbin` | Edit game data containers. XML export/import. |
| **Translokator Editor** | `Translokator_*.tra` | Object placement and instancing. |
| **Table Editor** | Game tables | Edit game data tables. |
| **City Editors** | `cityareas.bin`, `cityshops.bin` | Edit city regions and shops. |
| **Actor Editor** | `.act`, `.fxa`, `.fas` | Edit game entities and animations. |
| **Cutscene Editor** | `.cut` | Edit cinematic sequences. |

## SDS Archive Support

- Unpack/repack SDS archives (based on [Gibbed.Illusion](https://github.com/gibbed/Gibbed.Illusion))
- Automatic XML and table decompilation
- Optional Lua decompilation
- Compressed and uncompressed output
- ZModeler3-compatible export format

## MCP Server (AI Integration)

MafiaToolkit includes an integrated MCP (Model Context Protocol) server that allows AI assistants like Claude to browse and inspect Mafia game files programmatically. The server starts automatically when MafiaToolkit launches.

### Configuration

Create a `.mcp.json` file in your project directory (or add to your Claude Code settings):

```json
{
  "mcpServers": {
    "mafia-toolkit": {
      "type": "http",
      "url": "http://localhost:5123/"
    }
  }
}
```

### Usage

1. Start MafiaToolkit (the MCP server starts automatically on port 5123)
2. In Claude Code, run `/mcp` to connect to the server
3. Use the available tools to browse SDS archives and game files

### Available Tools (36 total)

#### SDS Archive Tools (8 tools)

| Tool | Parameters | Description |
|------|------------|-------------|
| `list_sds_files` | `directoryPath`, `recursive` | List all `.sds` files in a directory. Returns file path, name, and size for each archive found. |
| `open_sds_file` | `filePath`, `gameType?` | Open an SDS archive and get full metadata including version, platform, resource types, and complete resource list with sizes. |
| `get_sds_header` | `filePath` | Get SDS header info: version (19=Mafia II, 20=Mafia III), platform, endianness, RAM/VRAM requirements. |
| `list_resources` | `filePath`, `typeFilter?`, `offset`, `limit` | List resources with optional type filtering (e.g., "Texture", "Sound") and pagination support. |
| `get_resource_info` | `filePath`, `resourceIndex` | Get detailed info about a specific resource: type, version, data size, memory requirements, file hash. |
| `search_resources` | `filePath`, `pattern`, `typeFilter?`, `limit` | Search resources by name pattern (case-insensitive). Optionally filter by resource type. |
| `extract_resource` | `filePath`, `resourceIndex`, `maxBytes?` | Extract raw resource data as base64. Supports size limiting for large resources. |
| `get_sds_stats` | `filePath` | Get summary statistics: total resources, type breakdown with counts and sizes, compression info. |

#### Texture Tools (4 tools)

| Tool | Parameters | Description |
|------|------------|-------------|
| `inspect_dds_file` | `filePath` | Inspect a standalone DDS texture file. Returns dimensions, format (DXT1/DXT5/BC7/etc.), mip count, flags. |
| `inspect_sds_texture` | `sdsPath`, `resourceIndex` | Inspect a texture resource inside an SDS archive. Shows texture header (name hash, mip flag) and DDS metadata. |
| `list_sds_textures` | `sdsPath`, `includeMetadata?` | List all texture resources in an SDS. Optionally include full DDS metadata (dimensions, format) for each. |
| `inspect_dds_bytes` | `base64Data` | Inspect DDS texture from base64-encoded bytes. Useful for analyzing extracted texture data. |

#### Material Tools (4 tools)

| Tool | Parameters | Description |
|------|------------|-------------|
| `open_mtl_file` | `filePath` | Open a Material Library (.mtl) file. Returns version (V57/V58/V63), material count, and material list with shader info. |
| `list_mtl_files` | `directoryPath`, `recursive` | List all MTL files in a directory with quick metadata (version, material count) for each file. |
| `get_material_info` | `filePath`, `materialIndex` | Get detailed material info: shader ID/hash, flags, samplers with texture names, and all parameters. |
| `search_materials` | `filePath`, `pattern`, `limit` | Search materials by name pattern (case-insensitive substring match). |

#### Utility Tools (8 tools)

| Tool | Parameters | Description |
|------|------------|-------------|
| `get_configured_games` | *(none)* | Get all games configured in MafiaToolkit with their installation paths, selected game, and path validity status. |
| `hash_fnv32` | `input`, `useCodePage1252?` | Compute FNV32 hash for a string. Used by Mafia games for resource identification. Returns decimal, hex, and signed values. |
| `hash_fnv64` | `input`, `useCodePage1252?` | Compute FNV64 hash for a string. Used for file/texture identification. Returns decimal, hex, and signed values. |
| `hash_batch` | `inputs` | Compute FNV32 and FNV64 hashes for multiple strings (comma or newline separated). |
| `detect_file_format` | `filePath` | Detect file format from magic bytes. Identifies SDS, MTL, DDS, BNK, LUA, XML, PNG, and game-specific formats. |
| `detect_format_from_bytes` | `base64Data`, `extensionHint?` | Detect file format from base64-encoded bytes. Useful for identifying extracted resources. |
| `convert_number` | `input` | Convert between decimal, hex (0x), and binary (0b). Returns all representations plus signed values and byte array. |
| `list_game_files` | `directoryPath`, `extensionFilter?`, `recursive` | List game files matching common Mafia extensions (.sds, .mtl, .dds, .act, .nav, .xbin, etc.). |

#### Payload Decode Tools (5 tools)

Decode the SDS resource payloads the browsing tools only expose as opaque bytes into structured JSON. The `decode_*` tools each accept either a standalone `filePath` (the file the toolkit unpacks) or `base64Data` (the raw output of `extract_resource` — no wrapper is added, so it maps 1:1). `decode_resource` is the ergonomic shortcut: it opens the SDS, routes by the resource's typeName, and decodes in one call.

| Tool | Parameters | Description |
|------|------------|-------------|
| `decode_resource` | `sdsPath`, `resourceIndex?`, `typeName?`, `offset`, `limit`, `gameType?` | Extract a resource from an SDS and decode it in one step, routing by typeName (Actors, FrameResource, ItemDesc, Collisions, Effects). Select by `resourceIndex` or by `typeName` (first of that type); for a FrameResource it auto-pairs the archive's lone FrameNameTable. Removes the manual extract → base64 → decode hop. |
| `decode_actors` | `filePath?`, `base64Data?`, `offset`, `limit` | Decode an Actors (`.act`) resource: spawn/slot definitions and actor entries (entity/definition names, actor type, position/rotation/scale, frame links, extra-data type). Paginated. |
| `decode_frame_resource` | `filePath?`, `base64Data?`, `isBigEndian?`, `includeObjects?`, `frameNameTablePath?`, `frameNameTableBase64?`, `offset`, `limit` | Decode a FrameResource (`.fr`) scene graph: header counts, scene folders, and frame objects (name, type, parents, transform). Optionally resolve top-level named frames from a FrameNameTable. Paginated. |
| `decode_itemdesc` | `filePath?`, `base64Data?` | Decode an ItemDesc (`.ids`) resource: frame link, collision type, hashes, transform, and the decoded collision shape (box/sphere/capsule/convex). |
| `decode_collisions` | `filePath?`, `base64Data?`, `offset`, `limit` | Decode a Collisions (`.col`) resource: platform, placements (instance transforms + hashes), and collision models (PhysX triangle meshes with vertex/triangle/section counts). Paginated. |

#### Effects Tools (2 tools)

| Tool | Parameters | Description |
|------|------------|-------------|
| `parse_effects_file` | `filePath`, `effectIndex?` | Parse a Mafia II `.eff` (Effects) file. Returns the effect/pattern list, or full detail (generations, operators, parameters, frames, sounds) for one effect when `effectIndex` is set. |
| `parse_effects_from_bytes` | `base64Data`, `effectIndex?` | Parse `.eff` content from base64 bytes (e.g. the output of `extract_resource` on an 'Effects' resource). |

#### Table Tools (3 tools)

Read the game's data tables (the `.tbl` rows packed into the "Table" resources of `tables.sds` — weapons, cars, physics materials, etc.). Each tool accepts one of `sdsPath` (an SDS whose Table resources are enumerated), `tablePath` (a standalone `.tbl`), or `base64Data` (a Table-resource payload from `extract_resource`, with `version`). Columns carry only a 32-bit FNV name hash, so cells are positional and columns are reported by hash + type.

| Tool | Parameters | Description |
|------|------------|-------------|
| `list_tables` | `sdsPath?`, `tablePath?`, `base64Data?`, `version?`, `gameType?` | List the data tables in the source: each table's name, name hash, row/column counts, and column hashes + types. |
| `dump_rows` | `sdsPath?`, `tablePath?`, `base64Data?`, `version?`, `tableName?`, `offset`, `limit`, `gameType?` | Dump a table's rows (cells aligned positionally to the reported columns). `tableName` required when the source holds more than one table; matched case-insensitively, ignoring path/`.tbl` extension. Paginated. |
| `lookup_by_row` | `sdsPath?`, `tablePath?`, `base64Data?`, `version?`, `tableName?`, `rowIndex`, `gameType?` | Look up a single row by index, returning each cell paired with its column hash and type. |

#### Stream Map Tools (2 tools)

Read and edit `StreamMapa.bin` (magic `StrM`, version 6) — the table that maps each stream line to the SDS/assets it loads.

| Tool | Parameters | Description |
|------|------------|-------------|
| `parse_stream_map` | `filePath?`, `base64Data?`, `section?`, `offset`, `limit` | Decode a StreamMapa.bin. `section=summary` returns counts + all groups; `section=lines\|loaders\|blocks` returns that paginated array (loader path/entity/type, line name/flags/hashes, block hashes). |
| `edit_stream_map` | `filePath`, `find`, `replace`, `mode?`, `fields?`, `dryRun?` | Find/replace across the string fields (`path`, `entity`, `lineName`, `flags`, `groupName`) and save — the way assets get re-pointed. `dryRun` (default true) previews the changes; set `dryRun=false` to write. Keeps a `<name>_old.bin` backup. |

### Supported Formats

- **SDS Archives**: Version 19 (Mafia II/DE), Version 20 (Mafia III/I:DE)
- **Compression**: zlib and Oodle (requires `oo2core_8_win64.dll` from game directory)
- **Encrypted SDS**: Auto-decrypted using TEA cipher

## Project Structure

```
Mafia2Libs/               # Main C# toolkit
├── Core/IO/              # File format handlers
├── ResourceTypes/        # Game data structures
├── Rendering/            # DirectX 11 engine
├── Forms/                # Editor windows
├── MCP/                  # Integrated MCP server
│   ├── Services/         # SDS parsing service
│   └── Tools/            # MCP tool definitions
└── Gibbed.*/             # Archive serialization

M2FBX/                    # Native C++ FBX library
```

## Links

- [Issues](https://github.com/Greavesy1899/Mafia2Toolkit/issues)
- [Discord](https://discord.gg/HFCksVXXWy)
