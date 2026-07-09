// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using ModelContextProtocol.Server;
using ResourceTypes.Actors;
using ResourceTypes.Collisions;
using ResourceTypes.ItemDesc;
using FR = ResourceTypes.FrameResource;
using FNT = ResourceTypes.FrameNameTable;

namespace Mafia2Tool.MCP.Tools;

/// <summary>
/// MCP tools that decode the SDS payloads the toolkit already parses but that the browsing
/// tools only expose as opaque bytes: Actors (spawn/slot defs), FrameResource + FrameNameTable
/// (the scene graph), ItemDesc (collision pickups) and Collisions (PhysX collision meshes).
///
/// Each tool takes either a path to a standalone file (the ".act"/".fr"/".ids"/".col" the
/// toolkit writes when it unpacks an SDS) or base64 bytes. For every one of these resource
/// types the toolkit stores the SDS resource <c>Data</c> verbatim — there is no wrapper — so
/// the base64 form accepts the output of <c>extract_resource</c> directly, exactly like the
/// Effects tools.
/// </summary>
[McpServerToolType]
public class DecodeTools
{
    // ---------------------------------------------------------------------------------------
    // Actors
    // ---------------------------------------------------------------------------------------

    [McpServerTool(Name = "decode_actors"), Description("Decode an Actors ('.act') resource: the spawn/slot definitions and actor entries (positions, types, frame links). Pass either filePath or base64Data.")]
    public string DecodeActors(
        [Description("Full path to the Actors (.act) file. Omit if using base64Data.")] string? filePath = null,
        [Description("Base64 of the Actors payload (e.g. extract_resource on an 'Actors' resource). Omit if using filePath.")] string? base64Data = null,
        [Description("Starting index into the actor entry list for pagination (default: 0)")] int offset = 0,
        [Description("Max actor entries to return (default: 200, max: 2000)")] int limit = 200)
    {
        try
        {
            byte[] bytes = LoadBytes(filePath, base64Data, out var err);
            if (bytes == null) return err;

            var actor = new Actor();
            using (var reader = new BinaryReader(new MemoryStream(bytes, false)))
            {
                actor.ReadFromFile(reader);
            }

            limit = Math.Clamp(limit, 1, 2000);
            offset = Math.Max(0, offset);
            var items = actor.Items;
            var page = items.Skip(offset).Take(limit).Select((it, i) => new
            {
                index = offset + i,
                entityName = it.EntityName,
                actorTypeName = it.ActorTypeName,
                actorTypeId = it.ActorTypeID,
                actorType = Enum.IsDefined(typeof(ActorTypes), it.ActorTypeID) ? ((ActorTypes)it.ActorTypeID).ToString() : null,
                definitionName = it.DefinitionName,
                frameName = it.FrameName,
                frameNameHash = Hex(it.FrameNameHash),
                entityHash = Hex(it.EntityHash),
                position = Vec3(it.Position),
                rotation = Quat(it.Rotation),
                scale = Vec3(it.Scale),
                activateOnInit = it.bActivateOnInit,
                dataId = it.DataID,
                hasExtraData = it.Data != null,
                extraDataType = it.Data?.BufferType.ToString()
            }).ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                definitionCount = actor.Definitions.Count,
                itemCount = items.Count,
                extraDataCount = actor.ExtraData.Count,
                definitions = actor.Definitions.Select(d => new
                {
                    name = d.Name,
                    frameNameHash = Hex(d.FrameNameHash),
                    frameIndex = d.FrameIndex
                }).ToList(),
                pagination = new { offset, limit, count = page.Count, totalCount = items.Count, hasMore = offset + page.Count < items.Count },
                items = page
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    // ---------------------------------------------------------------------------------------
    // FrameResource (+ optional FrameNameTable)
    // ---------------------------------------------------------------------------------------

    [McpServerTool(Name = "decode_frame_resource"), Description("Decode a FrameResource ('.fr') scene graph: header counts, scene folders, and frame objects (name, type, parents, transform). Optionally pass a FrameNameTable to resolve the top-level named frames. Pass either filePath or base64Data.")]
    public string DecodeFrameResource(
        [Description("Full path to the FrameResource (.fr) file. Omit if using base64Data.")] string? filePath = null,
        [Description("Base64 of the FrameResource payload (e.g. extract_resource on a 'FrameResource' resource). Omit if using filePath.")] string? base64Data = null,
        [Description("Big-endian (console) data (default: false = PC)")] bool isBigEndian = false,
        [Description("Include the per-object list (default: true). Set false for just header/counts.")] bool includeObjects = true,
        [Description("Optional path to a FrameNameTable (.fnt) file to resolve named frames.")] string? frameNameTablePath = null,
        [Description("Optional base64 of a FrameNameTable payload to resolve named frames.")] string? frameNameTableBase64 = null,
        [Description("Starting index into the frame object list for pagination (default: 0)")] int offset = 0,
        [Description("Max frame objects to return (default: 500, max: 5000)")] int limit = 500)
    {
        try
        {
            byte[] bytes = LoadBytes(filePath, base64Data, out var err);
            if (bytes == null) return err;

            var frame = new FR.FrameResource();
            using (var stream = new MemoryStream(bytes, false))
            {
                frame.ReadFromFile(stream, isBigEndian);
            }

            var header = frame.Header;
            object nameTable = null;

            byte[] fntBytes = LoadOptionalBytes(frameNameTablePath, frameNameTableBase64);
            if (fntBytes != null)
            {
                var fnt = new FNT.FrameNameTable();
                using (var stream = new MemoryStream(fntBytes, false))
                {
                    fnt.ReadFromFile(stream, isBigEndian);
                }
                nameTable = new
                {
                    entryCount = fnt.FrameData.Length,
                    entries = fnt.FrameData.Select(d => new
                    {
                        name = d.Name,
                        parentName = d.ParentName,
                        frameIndex = d.FrameIndex,
                        flags = d.Flags.ToString()
                    }).ToList()
                };
            }

            object objects = null;
            var frameObjects = frame.FrameObjects;
            if (includeObjects)
            {
                limit = Math.Clamp(limit, 1, 5000);
                offset = Math.Max(0, offset);
                objects = frameObjects.Skip(offset).Take(limit).Select((kv, i) =>
                {
                    var fb = kv.Value as FR.FrameObjectBase;
                    return new
                    {
                        index = offset + i,
                        refId = Hex((uint)kv.Key),
                        name = fb?.Name?.String,
                        type = fb?.GetType().Name,
                        parentIndex1 = fb?.ParentIndex1?.Index,
                        parentIndex2 = fb?.ParentIndex2?.Index,
                        isOnFrameTable = fb?.IsOnFrameTable,
                        position = fb != null ? Vec3(fb.LocalTransform.Translation) : null
                    };
                }).ToList();
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                header = new
                {
                    isScene = header.IsScene,
                    sceneName = header.SceneName?.String,
                    numFolderNames = header.NumFolderNames,
                    numGeometries = header.NumGeometries,
                    numMaterialResources = header.NumMaterialResources,
                    numBlendInfos = header.NumBlendInfos,
                    numSkeletons = header.NumSkeletons,
                    numSkelHierachies = header.NumSkelHierachies,
                    numObjects = header.NumObjects
                },
                sceneFolders = header.SceneFolders?.Select(s => s.Name?.String).ToList(),
                counts = new
                {
                    geometries = frame.FrameGeometries.Count,
                    materials = frame.FrameMaterials.Count,
                    blendInfos = frame.FrameBlendInfos.Count,
                    skeletons = frame.FrameSkeletons.Count,
                    skeletonHierarchies = frame.FrameSkeletonHierachies.Count,
                    scenes = frame.FrameScenes.Count,
                    objects = frameObjects.Count
                },
                nameTable,
                pagination = includeObjects
                    ? new { offset, limit, count = ((System.Collections.ICollection)objects).Count, totalCount = frameObjects.Count, hasMore = offset + ((System.Collections.ICollection)objects).Count < frameObjects.Count }
                    : null,
                objects
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    // ---------------------------------------------------------------------------------------
    // ItemDesc
    // ---------------------------------------------------------------------------------------

    [McpServerTool(Name = "decode_itemdesc"), Description("Decode an ItemDesc ('.ids') resource: the frame link, collision type, hashes, transform and collision shape detail. Pass either filePath or base64Data.")]
    public string DecodeItemDesc(
        [Description("Full path to the ItemDesc (.ids) file. Omit if using base64Data.")] string? filePath = null,
        [Description("Base64 of the ItemDesc payload (e.g. extract_resource on an 'ItemDesc' resource). Omit if using filePath.")] string? base64Data = null)
    {
        try
        {
            byte[] bytes = LoadBytes(filePath, base64Data, out var err);
            if (bytes == null) return err;

            var item = new ItemDescLoader();
            using (var reader = new BinaryReader(new MemoryStream(bytes, false)))
            {
                item.ReadFromFile(reader);
            }

            return JsonSerializer.Serialize(new
            {
                success = true,
                frameRef = Hex(item.frameRef),
                collisionType = item.colType.ToString(),
                collisionTypeId = (int)item.colType,
                idHash = Hex(item.idHash),
                collisionMaterial = item.colMaterial,
                transform = Matrix(item.Matrix),
                collision = Describe(item.collision)
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    // ---------------------------------------------------------------------------------------
    // Collisions
    // ---------------------------------------------------------------------------------------

    [McpServerTool(Name = "decode_collisions"), Description("Decode a Collisions ('.col') resource: platform, placements (instances) and collision models (PhysX triangle meshes with vertex/triangle/section counts). Pass either filePath or base64Data.")]
    public string DecodeCollisions(
        [Description("Full path to the Collisions (.col) file. Omit if using base64Data.")] string? filePath = null,
        [Description("Base64 of the Collisions payload (e.g. extract_resource on a 'Collisions' resource). Omit if using filePath.")] string? base64Data = null,
        [Description("Starting index for pagination of placements and models (default: 0)")] int offset = 0,
        [Description("Max placements and models to return (default: 200, max: 2000)")] int limit = 200)
    {
        try
        {
            byte[] bytes = LoadBytes(filePath, base64Data, out var err);
            if (bytes == null) return err;

            var col = new Collision();
            using (var reader = new BinaryReader(new MemoryStream(bytes, false)))
            {
                col.ReadFromFile(reader);
            }

            limit = Math.Clamp(limit, 1, 2000);
            offset = Math.Max(0, offset);

            var placements = col.Placements.Skip(offset).Take(limit).Select(p => new
            {
                hash = Hex(p.Hash),
                position = Vec3(p.Position),
                rotationDegrees = Vec3(p.RotationDegrees),
                unk4 = p.Unk4,
                unk5 = p.Unk5
            }).ToList();

            var models = col.Models.Values.Skip(offset).Take(limit).Select(m => new
            {
                hash = Hex(m.Hash),
                numVertices = m.Mesh?.NumVertices ?? 0,
                numTriangles = m.Mesh?.NumTriangles ?? 0,
                sectionCount = m.Sections?.Count ?? 0,
                sections = m.Sections?.Select(s => new
                {
                    start = s.Start,
                    numEdges = s.NumEdges,
                    material = s.Material,
                    unk2 = s.Unk2
                }).ToList()
            }).ToList();

            return JsonSerializer.Serialize(new
            {
                success = true,
                platform = col.Platform,
                placementCount = col.Placements.Count,
                modelCount = col.Models.Count,
                pagination = new { offset, limit, placementsReturned = placements.Count, modelsReturned = models.Count },
                placements,
                models
            });
        }
        catch (Exception ex)
        {
            return McpError.FailJson(ex);
        }
    }

    // ---------------------------------------------------------------------------------------
    // Shared helpers
    // ---------------------------------------------------------------------------------------

    /// <summary>
    /// Resolves the payload bytes from exactly one of filePath / base64Data. On failure, sets
    /// <paramref name="error"/> to a ready-to-return JSON error and returns null.
    /// </summary>
    private static byte[] LoadBytes(string filePath, string base64Data, out string error)
    {
        error = null;
        bool hasPath = !string.IsNullOrEmpty(filePath);
        bool hasData = !string.IsNullOrEmpty(base64Data);

        if (hasPath == hasData)
        {
            error = JsonSerializer.Serialize(new { success = false, error = "Provide exactly one of filePath or base64Data." });
            return null;
        }

        if (hasPath)
        {
            if (!File.Exists(filePath))
            {
                error = JsonSerializer.Serialize(new { success = false, error = "File not found: " + filePath });
                return null;
            }
            return File.ReadAllBytes(filePath);
        }

        return Convert.FromBase64String(base64Data);
    }

    /// <summary>Optional secondary payload (e.g. a FrameNameTable alongside a FrameResource).</summary>
    private static byte[] LoadOptionalBytes(string filePath, string base64Data)
    {
        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath)) return File.ReadAllBytes(filePath);
        if (!string.IsNullOrEmpty(base64Data)) return Convert.FromBase64String(base64Data);
        return null;
    }

    private static string Hex(ulong value) => $"0x{value:X16}";
    private static string Hex(uint value) => $"0x{value:X8}";

    private static object Vec3(Vector3 v) => new { x = v.X, y = v.Y, z = v.Z };
    private static object Quat(Quaternion q) => new { x = q.X, y = q.Y, z = q.Z, w = q.W };

    private static object Matrix(Matrix4x4 m)
    {
        Matrix4x4.Decompose(m, out var scale, out var rotation, out var translation);
        return new
        {
            translation = Vec3(translation),
            scale = Vec3(scale),
            rotation = Quat(rotation)
        };
    }

    /// <summary>
    /// Compact reflection dump of a collision-shape object. Scalars and vectors are emitted
    /// inline; arrays are reduced to a <c>{field}Count</c> plus the first few elements so a
    /// convex hull's large vertex/index buffers don't flood the response.
    /// </summary>
    private static object Describe(object obj)
    {
        if (obj == null) return null;

        const int MaxArrayPreview = 8;
        var result = new Dictionary<string, object> { ["type"] = obj.GetType().Name };

        foreach (var field in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            object value = field.GetValue(obj);
            if (value == null) { result[field.Name] = null; continue; }

            switch (value)
            {
                case Vector3 v: result[field.Name] = Vec3(v); break;
                case Quaternion q: result[field.Name] = Quat(q); break;
                case string or bool or byte or short or ushort or int or uint or long or ulong or float or double:
                    result[field.Name] = value;
                    break;
                case Array arr:
                    var preview = new List<object>();
                    for (int i = 0; i < Math.Min(arr.Length, MaxArrayPreview); i++)
                    {
                        object el = arr.GetValue(i);
                        preview.Add(el is Vector3 ev ? Vec3(ev) : el);
                    }
                    result[field.Name] = new { count = arr.Length, preview };
                    break;
                default:
                    result[field.Name] = value.ToString();
                    break;
            }
        }

        return result;
    }
}
