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

### Available Tools

| Tool | Description |
|------|-------------|
| `list_sds_files` | List all .sds files in a directory |
| `open_sds_file` | Open an SDS file and get metadata + resource list |
| `get_sds_header` | Get SDS header info (version, platform, memory requirements) |
| `list_resources` | List resources with optional type filtering and pagination |
| `get_resource_info` | Get detailed info about a specific resource |
| `search_resources` | Search resources by name pattern |
| `extract_resource` | Extract resource data as base64 |
| `get_sds_stats` | Get summary statistics for an SDS file |

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
