using Rendering.Core;
using ResourceTypes.BufferPools;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Utils.Extensions;
using Utils.Models;
using Utils.Settings;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using static Rendering.Graphics.BaseShader;
using Color = System.Drawing.Color;

namespace Rendering.Graphics
{
    public class RenderModel : IRenderer
    {
        public struct ModelPart
        {
            public IMaterial Material;
            public ulong MaterialHash;
            public uint StartIndex;
            public uint NumFaces;
            public BaseShader Shader;
        }

        private HashName aoHash;
        public ID3D11ShaderResourceView AOTexture { get; set; }
        public Color SelectionColour { get; private set; }
        
        public struct SelectionInstance
        {
            public int instanceRefID;
        }

        public SelectionInstance selectionInstance;

        public struct LOD
        {
            public ModelPart[] ModelParts { get; set; }
            public VertexLayouts.NormalLayout.Vertex[] Vertices { get; set; }
            public uint[] Indices { get; set; }
        }

        public LOD[] LODs { get; private set; }

        public Dictionary<int, Matrix4x4> InstanceTransforms { get; set; } = new() { };
        public BVH BVH { get; set; } = new();

        public RenderModel()
        {
            DoRender = true;
            bIsUpdatedNeeded = false;
            Transform = Matrix4x4.Identity;
            SelectionColour = Color.White;
            selectionInstance = new SelectionInstance()
            {
                instanceRefID = -1,
            };
        }

        public void ConvertMTKToRenderModel(M2TStructure structure)
        {
            List<Vertex[]> vertices = new List<Vertex[]>();
            LODs = new LOD[structure.Lods.Length];
            for(int i = 0; i != structure.Lods.Length; i++)
            {
                M2TStructure.Lod lod = structure.Lods[i];
                vertices.Add(lod.Vertices);
                LOD lod2 = new LOD();
                lod2.Indices = lod.Indices;
                lod2.ModelParts = new ModelPart[lod.Parts.Length];
                for (int y = 0; y != lod.Parts.Length; y++)
                {
                    ModelPart part = new ModelPart();
                    part.NumFaces = lod.Parts[y].NumFaces;
                    part.StartIndex = lod.Parts[y].StartIndex;
                    part.MaterialHash = lod.Parts[y].Hash;
                    

                    switch (part.MaterialHash)
                    {
                        case 1337:
                            part.Material = RenderStorageSingleton.Instance.Prefabs.GizmoRed;
                            break;
                        case 1338:
                            part.Material = RenderStorageSingleton.Instance.Prefabs.GizmoBlue;
                            break;
                        case 1339:
                            part.Material = RenderStorageSingleton.Instance.Prefabs.GizmoGreen;
                            break;
                        default:
                            part.Material = MaterialsManager.LookupMaterialByHash(part.MaterialHash);
                            break;
                    }
                    lod2.ModelParts[y] = part;
                }

                lod2.Vertices = new VertexLayouts.NormalLayout.Vertex[lod.Vertices.Length];
                for (int y = 0; y != lod.Vertices.Length; y++)
                {
                    var vertice = new VertexLayouts.NormalLayout.Vertex();
                    vertice.Position = lod.Vertices[y].Position;
                    vertice.Normal = lod.Vertices[y].Normal;
                    vertice.Tangent = lod.Vertices[y].Tangent;
                    vertice.TexCoord0 = lod.Vertices[y].UVs[0];
                    vertice.TexCoord7 = lod.Vertices[y].UVs[3];
                    lod2.Vertices[y] = vertice;
                }
                LODs[i] = lod2;
            }
            BoundingBox = BoundingBoxExtenders.CalculateBounds(vertices);
            SetupShaders();
        }

        public bool ConvertFrameToRenderModel(FrameObjectSingleMesh mesh, FrameGeometry geom, FrameMaterial mats, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers)
        {
            if (mesh == null || geom == null || mats == null || indexBuffers[0] == null || vertexBuffers[0] == null)
                return false;

            aoHash = mesh.OMTextureHash;
            SetTransform(mesh.WorldTransform);
            //DoRender = (mesh.SecondaryFlags == 4097 ? true : false);
            BoundingBox = mesh.Boundings;
            LODs = new LOD[geom.NumLods];

            for (int i = 0; i != geom.NumLods; i++)
            {
                LOD lod = new LOD();
                lod.Indices = indexBuffers[i].GetData();
                lod.ModelParts = new ModelPart[mats.LodMatCount[i]];

                for (int z = 0; z != mats.Materials[i].Length; z++)
                {
                    lod.ModelParts[z] = new ModelPart();
                    lod.ModelParts[z].NumFaces = (uint)mats.Materials[i][z].NumFaces;
                    lod.ModelParts[z].StartIndex = (uint)mats.Materials[i][z].StartIndex;
                    lod.ModelParts[z].MaterialHash = mats.Materials[i][z].MaterialHash;
                    lod.ModelParts[z].Material = MaterialsManager.LookupMaterialByHash(lod.ModelParts[z].MaterialHash);
                }

                lod.Vertices = new VertexLayouts.NormalLayout.Vertex[geom.LOD[i].NumVerts];
                int vertexSize;
                Dictionary<VertexFlags, FrameLOD.VertexOffset> vertexOffsets = geom.LOD[i].GetVertexOffsets(out vertexSize);
                try
                {
                    for (int x = 0; x != lod.Vertices.Length; x++)
                    {
                        VertexLayouts.NormalLayout.Vertex vertex = new VertexLayouts.NormalLayout.Vertex();

                        //declare data required and send to decompresser
                        byte[] data = new byte[vertexSize];
                        Array.Copy(vertexBuffers[i].Data, (x * vertexSize), data, 0, vertexSize);
                        Vertex decompressed = VertexTranslator.DecompressVertex(data, geom.LOD[i].VertexDeclaration, geom.DecompressionOffset, geom.DecompressionFactor, vertexOffsets);

                        //retrieve the data we require
                        vertex.Position = decompressed.Position;
                        vertex.Normal = decompressed.Normal;
                        vertex.Tangent = decompressed.Tangent;
                        vertex.Binormal = decompressed.Binormal;
                        vertex.TexCoord0 = decompressed.UVs[0];
                        vertex.TexCoord7 = decompressed.UVs[3];

                        lod.Vertices[x] = vertex;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error when creating renderable {1}!: \n{0}", ex.Message, mesh.Name.ToString()), "Toolkit");
                    return false;
                }
                LODs[i] = lod;
            }

            SetupShaders();
            return true;
        }

        public void UpdateMaterials(FrameMaterial mats)
        {
            for (int i = 0; i != LODs.Length; i++)
            {
                for (int z = 0; z != LODs[i].ModelParts.Length; z++)
                {
                    ulong hash = mats.Materials[i][z].MaterialHash;
                    LODs[i].ModelParts[z].MaterialHash = hash;
                    LODs[i].ModelParts[z].Material = MaterialsManager.LookupMaterialByHash(hash);
                }
            }
            bIsUpdatedNeeded = true;
        }

        private void SetupShaders()
        {
            for (int x = 0; x != LODs[0].ModelParts.Length; x++)
            {
                ModelPart part = LODs[0].ModelParts[x];
                if (part.Material == null)
                    part.Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[0];
                else
                {
                    //Debug.WriteLine(LODs[0].ModelParts[x].Material.MaterialName + "\t" + LODs[0].ModelParts[x].Material.ShaderHash);
                    part.Shader = (RenderStorageSingleton.Instance.ShaderManager.shaders.ContainsKey(LODs[0].ModelParts[x].Material.ShaderHash)
                        ? RenderStorageSingleton.Instance.ShaderManager.shaders[LODs[0].ModelParts[x].Material.ShaderHash]
                        : RenderStorageSingleton.Instance.ShaderManager.shaders[0]);
                }
                LODs[0].ModelParts[x] = part;
            }
        }

        private void InitTextures(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            if (aoHash != null)
            {
                ID3D11ShaderResourceView texture;

                if (!RenderStorageSingleton.Instance.TextureCache.TryGetValue(aoHash.Hash, out texture))
                {
                    if (!string.IsNullOrEmpty(aoHash.String))
                    {
                        texture = TextureLoader.LoadTexture(d3d, d3dContext, aoHash.String);
                        RenderStorageSingleton.Instance.TextureCache.Add(aoHash.Hash, texture);
                    }
                }

                AOTexture = texture;
            }
            else
            {
                AOTexture = RenderStorageSingleton.Instance.TextureCache[0];
            }

            for (int i = 0; i < LODs.Length; i++)
            {
                for(int x = 0; x < LODs[i].ModelParts.Length; x++)
                {
                    ModelPart part = LODs[i].ModelParts[x];
                    
                    if(part.Material != null)
                    {
                        GetTextureFromSampler(d3d, d3dContext, part, "S000");
                        GetTextureFromSampler(d3d, d3dContext, part, "S001");
                        GetTextureFromSampler(d3d, d3dContext, part, "S011");
                    }
                }
            }
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            vertexBuffer = d3d.CreateBuffer(BindFlags.VertexBuffer, LODs[0].Vertices, 0, ResourceUsage.Default, CpuAccessFlags.None);
            indexBuffer = d3d.CreateBuffer(BindFlags.IndexBuffer, LODs[0].Indices, 0, ResourceUsage.Default, CpuAccessFlags.None);

            InitInstanceBuffer(d3d);

            InitTextures(d3d, d3dContext);
        }

        public void InitInstanceBuffer(ID3D11Device d3d)
        {
            int newSize = InstanceTransforms.Count * Marshal.SizeOf<Matrix4x4>();

            if (InstanceTransforms.Count == 0)
            {
                return;
            }

            // Create or update buffer only if necessary
            if (instanceBuffer == null || instanceBuffer.Description.SizeInBytes < newSize)
            {
                // Buffer description for instance buffer
                var bufferDescription = new BufferDescription
                {
                    SizeInBytes = newSize,
                    Usage = ResourceUsage.Dynamic,
                    BindFlags = BindFlags.ShaderResource,
                    OptionFlags = ResourceOptionFlags.BufferStructured,
                    CpuAccessFlags = CpuAccessFlags.Write,
                    StructureByteStride = Marshal.SizeOf<Matrix4x4>(),
                };

                var viewDescription = new ShaderResourceViewDescription()
                {
                    Format = Vortice.DXGI.Format.Unknown,
                    ViewDimension = ShaderResourceViewDimension.Buffer,
                };

                viewDescription.Buffer.FirstElement = 0;
                viewDescription.Buffer.NumElements = InstanceTransforms.Count;

                // Dispose old buffer if necessary
                instanceBuffer?.Dispose();

                // Convert list to array
                Matrix4x4[] transformsArray = InstanceTransforms.Values.ToArray();

                // Pin the array in memory
                GCHandle handle = GCHandle.Alloc(transformsArray, GCHandleType.Pinned);
                try
                {
                    IntPtr pointer = handle.AddrOfPinnedObject();
                    // Update the instance buffer
                    instanceBuffer = d3d.CreateBuffer(bufferDescription, pointer);

                    instanceBufferView = d3d.CreateShaderResourceView(instanceBuffer, viewDescription);
                }
                finally
                {
                    handle.Free();
                }
            }
        }

        public void ReloadInstanceBuffer(ID3D11Device d3d)
        {
            instanceBuffer?.Dispose();
            instanceBuffer = null;
            instanceBufferView?.Dispose();
            instanceBufferView = null;

            InitInstanceBuffer(d3d);
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            Transform = matrix;
        }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
            {
                return;
            }

            bool BuffersSet = false;

            if (InstanceTransforms.Count > 0)
            {
                VertexBufferView VertexBufferView = new VertexBufferView(vertexBuffer, Unsafe.SizeOf<VertexLayouts.NormalLayout.Vertex>(), 0);
                deviceContext.IASetVertexBuffers(0, VertexBufferView);
                deviceContext.IASetIndexBuffer(indexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
                deviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);
                deviceContext.PSSetShaderResource(2, AOTexture);

                BuffersSet = true;

                RenderInstances(deviceContext, camera, device);
            }

            if (!camera.CheckBBoxFrustum(Transform, BoundingBox))
                return;

            if (!BuffersSet)
            {
                VertexBufferView VertexBufferView = new VertexBufferView(vertexBuffer, Unsafe.SizeOf<VertexLayouts.NormalLayout.Vertex>(), 0);
                deviceContext.IASetVertexBuffers(0, VertexBufferView);
                deviceContext.IASetIndexBuffer(indexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
                deviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);
                deviceContext.PSSetShaderResource(2, AOTexture);
                BuffersSet = true;
            }

            for (int i = 0; i != LODs[0].ModelParts.Length; i++)
            {
                ModelPart Segment = LODs[0].ModelParts[i];

                if (InstanceTransforms.Count == 0)
                {
                    Segment.Shader.ResetShaderParameters(device, deviceContext);
                }

                Segment.Shader.SetShaderParameters(device, deviceContext, new MaterialParameters(Segment.Material, SelectionColour.Normalize()));
                Segment.Shader.SetSceneVariables(deviceContext, Transform, camera);
                Segment.Shader.Render(deviceContext, PrimitiveTopology.TriangleList, (int)(Segment.NumFaces * 3), Segment.StartIndex);
            }
        }

        private float colorTransitionTime = 0.0f; // timer for distinguishing translokators

        public void RenderInstances(ID3D11DeviceContext deviceContext, Camera camera, ID3D11Device device)
        {
            deviceContext.VSSetShaderResource(0, instanceBufferView);

            colorTransitionTime += 0.1f;


            float t = (float)(Math.Sin(colorTransitionTime) * 0.5 + 0.5);

            Color startColor = Color.White;
            Color endColor = Color.Yellow;

            Color tint = Color.FromArgb(
                (int)(startColor.A + (endColor.A - startColor.A) * t),
                (int)(startColor.R + (endColor.R - startColor.R) * t),
                (int)(startColor.G + (endColor.G - startColor.G) * t),
                (int)(startColor.B + (endColor.B - startColor.B) * t)
            );

            for (int i = 0; i < LODs[0].ModelParts.Length; i++)
            {
                RenderModel.ModelPart segment = LODs[0].ModelParts[i];

                segment.Shader.SetShaderParameters(device, deviceContext, new MaterialParameters(segment.Material, ToolkitSettings.bTranslokatorTint ? tint.Normalize() : startColor.Normalize()));
                segment.Shader.SetSceneVariables(deviceContext, Transform, camera);

                segment.Shader.setHightLightInstance(deviceContext, selectionInstance.instanceRefID);


                segment.Shader.RenderInstanced(deviceContext, PrimitiveTopology.TriangleList, (int)segment.NumFaces * 3, (int)segment.StartIndex, InstanceTransforms.Count);
                Profiler.NumDrawCallsThisFrame++;
            }
        }

        public override void Shutdown()
        {
            LODs[0].Vertices = null;
            LODs[0].Indices = null;
            AOTexture?.Dispose();
            AOTexture = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
            indexBuffer?.Dispose();
            indexBuffer = null;
            instanceBuffer?.Dispose();
            instanceBuffer = null;
            instanceBufferView?.Dispose();
            instanceBufferView = null;
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if(bIsUpdatedNeeded)
            {
                SetupShaders();
                InitTextures(device, deviceContext);
                bIsUpdatedNeeded = false;
            }
        }

        public override void Select()
        {
            SelectionColour = Color.Red;
        }
        public void SelectInstance(int instanceRefID)
        {
            List<int> instanceRefIDs = InstanceTransforms.Keys.ToList();
            for (int i = 0; i < instanceRefIDs.Count; i++)
            {
                if (instanceRefIDs[i] == instanceRefID)
                {
                    selectionInstance.instanceRefID = i;
                    return;
                }
            }
        }
        
        public void UnselectInstance()
        {
            selectionInstance.instanceRefID = -1;
        }

        public override void Unselect()
        {
            SelectionColour = Color.White;
        }

        private void GetTextureFromSampler(ID3D11Device d3d, ID3D11DeviceContext d3dContext, ModelPart part, string SamplerKey)
        {
            HashName sampler = part.Material.GetTextureByID(SamplerKey);
            if (sampler != null)
            {
                ID3D11ShaderResourceView texture;

                ulong SamplerHash = sampler.Hash;
                string SamplerName = sampler.String;

                if (!RenderStorageSingleton.Instance.TextureCache.TryGetValue(SamplerHash, out texture))
                {
                    if (!string.IsNullOrEmpty(SamplerName))
                    {
                        texture = TextureLoader.LoadTexture(d3d, d3dContext, SamplerName);
                        RenderStorageSingleton.Instance.TextureCache.Add(SamplerHash, texture);
                    }
                }
            }
        }

        public bool ContainsInstanceTransform(int instanceID)
        {
            return InstanceTransforms.ContainsKey(instanceID);
        }

        // Building all BVH structures at once can be slow so we progressively build
        // them in the background while the map editor is open
        public Task GetBVHBuildingTask()
        {
            // Don't want to rebuild or attempt to build a BVH while it is being built
            // We will need to rebuild BVH for animations later on though
            if (LODs.Length == 0 || BVH.FinishedBuilding || BVH.IsBuilding)
            {
                return null;
            }

            BVH.IsBuilding = true;

            return Task.Run(() => BVH.Build(LODs[0].Vertices, LODs[0].Indices));
        }

        public void RemoveInstance(int instanceRefId,ID3D11Device d3d)
        {
            if (InstanceTransforms.ContainsKey(instanceRefId))
            {
                InstanceTransforms.Remove(instanceRefId);
                ReloadInstanceBuffer(d3d);
            }
        }

        public ID3D11Buffer GetVB()
        {
            return vertexBuffer;
        }
        public ID3D11Buffer GetIB()
        {
            return indexBuffer;
        }
    }
}
