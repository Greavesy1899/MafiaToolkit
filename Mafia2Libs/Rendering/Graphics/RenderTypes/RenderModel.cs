using Rendering.Core;
using ResourceTypes.BufferPools;
using ResourceTypes.FrameResource;
using ResourceTypes.Materials;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Windows;
using Utils.Extensions;
using Utils.Logging;
using Utils.Models;
using Utils.Types;
using Utils.VorticeUtils;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Mathematics;
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
        }

        private HashName aoHash;
        public ID3D11ShaderResourceView AOTexture { get; private set; }
        public Color SelectionColour { get; private set; }

        public struct LOD
        {
            public ModelPart[] ModelParts { get; set; }
            public VertexLayouts.NormalLayout.Vertex[] Vertices { get; set; }
            public uint[] Indices { get; set; }
            public ulong VertexBufferHash { get; set; }
            public ulong IndexBufferHash { get; set; }
        }

        public LOD[] LODs { get; private set; }

        private int[] ProxyPartIDs;

        private enum UpdateRequest
        {
            None,
            Model,
            Visibility
        }

        private UpdateRequest UpdateRequestType = UpdateRequest.None;

        public RenderModel(int InRefID)
        {
            RefID = InRefID;

            DoRender = true;
            bIsUpdatedNeeded = false;
            Transform = Matrix4x4.Identity;
            SelectionColour = Color.White;

            ProxyPartIDs = new int[0];
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
        }

        public bool ConvertFrameToRenderModel(FrameObjectSingleMesh mesh, FrameGeometry geom, FrameMaterial mats, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers)
        {
            if (mesh == null || geom == null || mats == null || indexBuffers[0] == null || vertexBuffers[0] == null)
            {
                return false;
            }

            aoHash = mesh.OMTextureHash;
            SetTransform(mesh.WorldTransform);
            //DoRender = (mesh.SecondaryFlags == 4097 ? true : false);
            BoundingBox = mesh.Boundings;
            LODs = new LOD[geom.NumLods];

            for(int i = 0; i != geom.NumLods; i++)
            {
                LOD lod = new LOD();
                lod.Indices = indexBuffers[i].GetData();
                lod.ModelParts = new ModelPart[mats.LodMatCount[i]];
                lod.IndexBufferHash = geom.LOD[i].IndexBufferRef.Hash;
                lod.VertexBufferHash = geom.LOD[i].VertexBufferRef.Hash;

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
                catch(Exception ex)
                {
                    MessageBox.Show(string.Format("Error when creating renderable {1}!: \n{0}", ex.Message, mesh.Name.ToString()), "Toolkit");
                    return false;
                }
                LODs[i] = lod;
            }

            return true;
        }

        public void UpdateMaterials(FrameMaterial mats)
        {
            for (int i = 0; i < LODs.Length; i++)
            {
                for (int z = 0; z < LODs[i].ModelParts.Length; z++)
                {
                    ulong hash = mats.Materials[i][z].MaterialHash;
                    LODs[i].ModelParts[z].MaterialHash = hash;
                    LODs[i].ModelParts[z].Material = MaterialsManager.LookupMaterialByHash(hash);
                }
            }

            bIsUpdatedNeeded = true;
            UpdateRequestType = UpdateRequest.Model;
        }

        public override bool IsRayIntersecting(Ray PickingRay, out PickOutParams OutParams)
        {
            float lowest = float.MaxValue;
            int lowestRefID = -1;
            Vector3 WorldPosIntersect = Vector3.Zero;

            // Convert to Model world space
            Matrix4x4 vWM = Matrix4x4.Identity;
            Matrix4x4.Invert(Transform, out vWM);
            var localRay = new Ray(
                Vector3Utils.TransformCoordinate(PickingRay.Position, vWM),
                Vector3.TransformNormal(PickingRay.Direction, vWM)
            );

            if (localRay.Intersects(BoundingBox) == 0.0f)
            {
                // We're not intersecting with this mesh
                OutParams = new PickOutParams();
                return false;
            }

            for (var i = 0; i < LODs[0].Indices.Length / 3; i++)
            {
                var v0 = LODs[0].Vertices[LODs[0].Indices[i * 3]].Position;
                var v1 = LODs[0].Vertices[LODs[0].Indices[i * 3 + 1]].Position;
                var v2 = LODs[0].Vertices[LODs[0].Indices[i * 3 + 2]].Position;
                float t = 0.0f;

                // Check if ray is intersecting triangle
                if (!Toolkit.Mathematics.Collision.RayIntersectsTriangle(ref localRay, ref v0, ref v1, ref v2, out t))
                {
                    continue;
                }

                var worldPosition = PickingRay.Position + t * PickingRay.Direction;
                var distance = (worldPosition - PickingRay.Position).LengthSquared();
                if (distance < lowest)
                {
                    lowest = distance;
                    lowestRefID = RefID;
                    WorldPosIntersect = worldPosition;
                }
            }

            OutParams = new PickOutParams();
            OutParams.LowestDistance = lowest;
            OutParams.LowestRefID = lowestRefID;
            OutParams.WorldPosition = WorldPosIntersect;

            return true;
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
            // Ask BufferManager to create ID3D11 buffers
            CachedGraphics.OurBufferManager.CreateIndexBuffer<uint>(LODs[0].IndexBufferHash, LODs[0].Indices);
            CachedGraphics.OurBufferManager.CreateVertexBuffer<VertexLayouts.NormalLayout.Vertex>(LODs[0].VertexBufferHash, LODs[0].Vertices);

            InitTextures(d3d, d3dContext);
            RequestProxies(d3d, d3dContext);
        }

        private void RequestProxies(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            ProxyPartIDs = new int[LODs[0].ModelParts.Length];
            for (int i = 0; i < ProxyPartIDs.Length; i++)
            {
                ModelPart Part = LODs[0].ModelParts[i];

                ProxyRenderRequest ProxyRequest = new ProxyRenderRequest();
                ProxyRequest.IndexBufferHash = LODs[0].IndexBufferHash;
                ProxyRequest.VertexBufferHash = LODs[0].VertexBufferHash;
                ProxyRequest.Material = Part.Material;
                ProxyRequest.NumFaces = Part.NumFaces;
                ProxyRequest.StartIndex = Part.StartIndex;
                ProxyRequest.ParentRenderer = this;

                ProxyPartIDs[i] = CachedGraphics.OurProxyRenderManager.AddProxyRenderRequest(ProxyRequest);
            }
        }

        private void RemoveProxies()
        {
            for (int i = 0; i < ProxyPartIDs.Length; i++)
            {
                bool bHasBeenRemoved = CachedGraphics.OurProxyRenderManager.RemoveProxyRenderRequest(ProxyPartIDs[i]);
                ToolkitAssert.Ensure(bHasBeenRemoved, "Should have removed this ProxyMesh from the Manager");
            }
        }

        public override void SetVisibility(bool bIsVisible)
        {
            base.SetVisibility(bIsVisible);

            bIsUpdatedNeeded = true;
            UpdateRequestType = UpdateRequest.Visibility;
        }

        public override void Shutdown()
        {
            base.Shutdown();

            foreach(int RefID in ProxyPartIDs)
            {
                CachedGraphics.OurProxyRenderManager.RemoveProxyRenderRequest(RefID);
            }

            ProxyPartIDs = new int[0];
            LODs[0].Vertices = null;
            LODs[0].Indices = null;
            AOTexture?.Dispose();
            AOTexture = null;
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if(bIsUpdatedNeeded)
            {
                if (UpdateRequestType == UpdateRequest.Visibility)
                {
                    if (DoRender == true)
                    {
                        RequestProxies(device, deviceContext);
                    }
                    else
                    {
                        RemoveProxies();
                    }
                }
                else if (UpdateRequestType == UpdateRequest.Model)
                {
                    InitTextures(device, deviceContext);

                    RemoveProxies();
                    RequestProxies(device, deviceContext);
                }

                UpdateRequestType = UpdateRequest.None;
                bIsUpdatedNeeded = false;
            }
        }

        public override void Select()
        {
            base.Select();

            SelectionColour = Color.Red;
        }

        public override void Unselect()
        {
            base.Unselect();

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
    }
}
