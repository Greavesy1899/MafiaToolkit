# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MafiaToolkit is a Windows .NET modding toolkit for the Mafia game series (Mafia II, Mafia III, Mafia I: DE, Mafia II: DE). It provides file format parsers, editors, and a 3D map editor with DirectX 11 rendering.

### Supported Games

| Game | SDS | Materials | Map Editor | XBin |
|------|-----|-----------|------------|------|
| Mafia II | ✓ | ✓ | ✓ | - |
| Mafia II: Definitive Edition | ✓ | ✓ | ✓ | - |
| Mafia III | ✓ | ✓ | - | ✓ |
| Mafia I: Definitive Edition | ✓ | ✓ | - | ✓ |

Mafia 1 Classic is **not** supported.

## Build Commands

### Main Toolkit (C# / .NET 7)
```bash
# Build the toolkit
dotnet build MafiaToolkit.sln

# Build with output directory
dotnet build --output "_out"

# Restore dependencies
dotnet restore
```

### M2FBX Native Library (C++ / FBX SDK)
```bash
# Generate VS2022 project files (from M2FBX/M2FBX directory)
cd M2FBX/M2FBX
premake5 vs2022

# Build in Visual Studio or use MSBuild
# Requires: FBX SDK and PhysX 2.8.X installed
```

**Note**: The toolkit is Windows-only and targets x64. Debug builds of M2FBX produce a console app for testing; Release produces a DLL.

## Architecture

### Source Layout
```
Mafia2Libs/                     # Main toolkit source
├── Core/IO/                    # File abstraction layer (FileBase, FileFactory)
├── ResourceTypes/FileTypes/    # Game file format implementations
│   ├── FrameResource/          # 3D model/skeleton data
│   ├── Materials/              # MTL material formats (V_57, V_58, V_63)
│   ├── Collisions/             # PhysX collision data
│   ├── Actors/                 # Game entity definitions
│   ├── Cutscene/               # Cinematic animation with curve parameters
│   ├── Navigation/             # AI/traffic pathfinding (NAV, NHV, NOV)
│   ├── Prefab/                 # Prefabricated object templates
│   ├── SDSConfig/              # Hierarchical game configuration
│   ├── GameParams/             # BitStream-encoded game parameters
│   ├── FrameProps/             # Frame properties with FNV64 hashes
│   └── M3.XBin/                # XBin game data containers
├── Gibbed.*/                   # Archive serialization (Gibbed's code)
├── Rendering/                  # DirectX 11 graphics engine
│   ├── Graphics/               # D3D11, shaders, textures, camera
│   └── Core/                   # RenderableFactory, gizmos, spatial grids
├── Forms/                      # 21 editor windows (WinForms)
├── Controls/                   # Reusable UI components
├── Utils/                      # Settings, logging, language, helpers
├── ThirdParty/                 # ApexSDK (PhysX), BitStream, OPCODE
└── Shaders/                    # HLSL shaders

M2FBX/                          # Native C++ FBX conversion library
├── M2FBX/Source/               # FBX wrangler, MT conversion
└── M2PhysX/                    # Separate physics library
```

### File Format Handling

Uses **Factory + Strategy pattern** via `FileFactory.ConstructFromFileInfo()`:
- Archives: SDS, PCKG → `FileSDS`, `FilePCKG`
- 3D Data: FR (FrameResource) → model/skeleton
- Materials: MTL → `FileMaterialLibrary` (version-aware)
- Actors: ACT, FXA, FAS → entity definitions
- Navigation: NAV, NHV, NOV, GAME, GSD → pathfinding
- Audio: BNK, PCK → Wwise formats
- Data: XBin → game data containers with sub-types

Each format has a dedicated `File*` class in `Core/IO/` that handles serialization.

### Rendering System

```
GraphicsClass (coordinator)
    ↓
DirectX11Class (D3D11 wrapper)
    ↓
RenderableFactory → 14 IRenderer implementations
    ↓
ShaderManager (6 shader types)
```

Key renderers: `RenderModel`, `RenderInstance`, `RenderAIWorld`, `RenderNav`, `RenderStaticCollision`

### Editor Forms

- **MapEditor**: Main 3D scene editor (most complex, integrates rendering + all resource types)
- **MaterialEditor/Browser**: MTL editing
- **CutsceneEditor**: Cinematic animation
- **ActorEditor**: Game entity definitions
- **XBinEditor**: Game data containers
- **TranslokatorEditor**: Object placement
- 15+ additional specialized editors

Forms use `WeifenLuo.WinFormsUI.Docking` for flexible panel layout.

### Key Dependencies

- **Vortice.Direct3D11**: DirectX 11 bindings
- **SharpGLTF**: GLTF model support
- **DockPanelSuite**: WinForms docking
- **Gibbed.IO**: Binary serialization utilities
- **UnluacNET**: Lua decompilation
- **OodleSharp**: Oodle compression

## Important Patterns

### Adding New File Format Support
1. Create `File<Format>` class extending `FileBase` in `Core/IO/`
2. Register extension in `FileFactory.ConstructFromFileInfo()`
3. Create corresponding resource types in `ResourceTypes/FileTypes/`
4. Optionally add renderer in `Rendering/Graphics/RenderTypes/`

### Resource Serialization
All game resources implement `IResourceType` interface with `Serialize`/`Deserialize` methods. Version-aware loading handles format differences across games.

### Rendering Objects
Implement `IRenderer` interface in `Rendering/Graphics/RenderTypes/`. Use `RenderableFactory` to create instances from resource types.

### Adding New Editor Forms
1. Create `<Name>Editor.cs` form in `Forms/` with TreeView + PropertyGrid layout
2. Add constructor accepting `FileInfo` parameter
3. Register in `FileBin.cs` or `FileFactory` based on file extension/magic
4. Add localization strings in `Utils/Language/` resource files
5. Standard menu structure: File (Save, Reload, Exit), Tools (Export XML, Import XML)
