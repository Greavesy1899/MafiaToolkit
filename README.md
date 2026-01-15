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

**Requirements:** Windows, .NET 7 SDK, Visual Studio 2022

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

## Project Structure

```
Mafia2Libs/           # Main C# toolkit
├── Core/IO/          # File format handlers
├── ResourceTypes/    # Game data structures
├── Rendering/        # DirectX 11 engine
├── Forms/            # Editor windows
└── Gibbed.*/         # Archive serialization

M2FBX/                # Native C++ FBX library
```

## Links

- [Issues](https://github.com/Greavesy1899/Mafia2Toolkit/issues)
- [Discord](https://discord.gg/HFCksVXXWy)
