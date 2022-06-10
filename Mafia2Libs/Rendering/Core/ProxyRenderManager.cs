using Rendering.Core;
using Rendering.Graphics;
using ResourceTypes.Materials;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Toolkit.Core;
using Utils.ContainerTypes;
using Utils.Extensions;
using Utils.VorticeUtils;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Color = System.Drawing.Color;

namespace Rendering.Core
{
    public struct ProxyRenderRequest
    {
        public RenderModel ParentRenderer { get; set; }
        public IMaterial Material { get; set; }
        public uint StartIndex { get; set; }
        public uint NumFaces { get; set; }
        public ulong IndexBufferHash { get; set; }
        public ulong VertexBufferHash { get; set; }
        public int RefID { get; set; }
    }

    /***
     * Rendering Manager which takes requests to render models using the games material system.
     * Once the request has been accepted, we order the request by material, then batch them up with the same materials.
     */
    public class ProxyRenderManager
    {
        private MultiMap<ulong, ProxyRenderRequest> MaterialRequestBatches;
        private Dictionary<int, ulong> RefIDToMaterialLookup;
        private Dictionary<ulong, BaseShader> MaterialToShaderLookup;
        private Dictionary<ulong, RuntimeMaterialInstance> MaterialHashToInstance;
        private List<ulong> DirtyMaterialLists;
        private GraphicsClass CachedGraphics = null;

        public ProxyRenderManager(GraphicsClass InCachedGraphics)
        {
            CachedGraphics = InCachedGraphics;

            MaterialRequestBatches = new MultiMap<ulong, ProxyRenderRequest>();
            MaterialToShaderLookup = new Dictionary<ulong, BaseShader>();
            RefIDToMaterialLookup = new Dictionary<int, ulong>();
            MaterialHashToInstance = new Dictionary<ulong, RuntimeMaterialInstance>();
            DirtyMaterialLists = new List<ulong>();
        }

        public int AddProxyRenderRequest(ProxyRenderRequest Request)
        {
            // Can only accept a valid material
            if(Request.Material == null)
            {
                // throw error when logging is present
                return -1;
            }

            // Check if we have an active batch
            ulong MaterialHash = Request.Material.GetMaterialHash();
            if (!MaterialRequestBatches.ContainsKey(MaterialHash))
            {
                CreateShaderForMaterial(Request.Material);
                CreateRuntimeMaterialForMaterial(Request.Material);

                MaterialRequestBatches.AddKey(MaterialHash);              
            }

            // We know the material is valid, and we've got a batch.
            // Add it to the Batch.
            Request.RefID = RefManager.GetNewRefID();
            MaterialRequestBatches[MaterialHash].Add(Request);
            if (!DirtyMaterialLists.Contains(MaterialHash))
            {
                DirtyMaterialLists.Add(MaterialHash);
            }

            // We've added, so now we have to make sure we can find it when wanting to access it.
            RefIDToMaterialLookup.Add(Request.RefID, MaterialHash);
            
            return Request.RefID;
        }

        public bool RemoveProxyRenderRequest(int RefID)
        {
            // If we find it in the Lookup, then we can guarantee 
            // its existence in the batch MultiMap.
            bool bHasBeenRemoved = false;
            if(RefIDToMaterialLookup.ContainsKey(RefID))
            {
                ulong MaterialHashKey = RefIDToMaterialLookup[RefID];

                // TODO: Figure out why MultiMap is failing for Foreach
                List<ProxyRenderRequest> RequestBatch = MaterialRequestBatches[MaterialHashKey];
                foreach(ProxyRenderRequest Value in RequestBatch)
                {
                    // Found request, remove and early return out
                    if(Value.RefID == RefID)
                    {
                        RefIDToMaterialLookup.Remove(RefID);
                        bHasBeenRemoved = MaterialRequestBatches.Remove(MaterialHashKey, Value);
                        
                        if(!DirtyMaterialLists.Contains(MaterialHashKey))
                        {
                            DirtyMaterialLists.Add(MaterialHashKey);
                        }

                        break;
                    }
                }

                // If we find that we have no requests for this Material, go ahead
                // and remove. We don't want to leave empty arrays and invalid data around.
                if (bHasBeenRemoved && RequestBatch.Count == 0)
                {
                    MaterialToShaderLookup.Remove(MaterialHashKey);
                    MaterialRequestBatches.Remove(MaterialHashKey);
                    MaterialHashToInstance.Remove(MaterialHashKey);
                    DirtyMaterialLists.Remove(MaterialHashKey);
                }
            }

            return bHasBeenRemoved;
        }

        public IRenderer GetObject(int RefID)
        {
            foreach (List<ProxyRenderRequest> Entry in MaterialRequestBatches.Values)
            {
                foreach (ProxyRenderRequest Value in Entry)
                {
                    if(Value.ParentRenderer.RefID == RefID)
                    {
                        return Value.ParentRenderer;
                    }
                }
            }

            return null;
        }

        public PickOutParams Pick(Ray PickingRay)
        {
            float Lowest = float.MaxValue;
            int LowestRefID = -1;
            Vector3 WorldPosIntersect = Vector3.Zero;

            Dictionary<int, float> ProcessedRenderModels = new Dictionary<int, float>();
            foreach (List<ProxyRenderRequest> Entry in MaterialRequestBatches.Values)
            {
                foreach (ProxyRenderRequest Value in Entry)
                {
                    if (!ProcessedRenderModels.ContainsKey(Value.ParentRenderer.RefID))
                    {
                        PickOutParams ProxyPickOutParams = new PickOutParams();
                        if (Value.ParentRenderer.IsRayIntersecting(PickingRay, out ProxyPickOutParams))
                        {
                            if (ProxyPickOutParams.LowestDistance < Lowest)
                            {
                                Lowest = ProxyPickOutParams.LowestDistance;
                                LowestRefID = ProxyPickOutParams.LowestRefID;
                                WorldPosIntersect = ProxyPickOutParams.WorldPosition;
                            }
                        }

                        ProcessedRenderModels.Add(Value.ParentRenderer.RefID, 0.0f);
                    }
                }
            }

            PickOutParams OutParams = new PickOutParams();
            OutParams.LowestRefID = LowestRefID;
            OutParams.LowestDistance = Lowest;
            OutParams.WorldPosition = WorldPosIntersect;

            return OutParams;
        }

        public void RenderProxies(DirectX11Class Dx11Object, Camera InCamera)
        {
            SortMaterialLists();

            foreach (var Entry in MaterialRequestBatches)
            {
                // Cache the hash as well as the Material Shader
                ulong MaterialHash = Entry.Key;
                BaseShader MaterialShader = MaterialToShaderLookup[MaterialHash];

                BaseShader.MaterialParameters MatParams = new BaseShader.MaterialParameters(MaterialHashToInstance[MaterialHash], Vector3.Zero);
                MaterialShader.SetShaderParameters(Dx11Object.Device, Dx11Object.DeviceContext, MatParams);

                // Iterate through all proxies and render them
                foreach (ProxyRenderRequest Value in Entry.Value)
                {
                    MatParams.SelectionColour = (Value.ParentRenderer.IsSelected() ? Color.Red : Color.White).Normalize();
                    MaterialShader.SetEditorParameters(Dx11Object.Device, Dx11Object.DeviceContext, MatParams);
                    MaterialShader.SetSceneVariables(Dx11Object.DeviceContext, Value.ParentRenderer.Transform, InCamera);
                    CachedGraphics.OurBufferManager.SetVertexBuffer(Value.VertexBufferHash, 0, Unsafe.SizeOf<VertexLayouts.NormalLayout.Vertex>());
                    CachedGraphics.OurBufferManager.SetIndexBuffer(Value.IndexBufferHash, Vortice.DXGI.Format.R32_UInt, 0);
                    Dx11Object.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
                    Dx11Object.DeviceContext.PSSetShaderResource(2, Value.ParentRenderer.AOTexture);

                    MaterialShader.Render(Dx11Object, PrimitiveTopology.TriangleList, (int)(Value.NumFaces * 3), Value.StartIndex);
                }
            }
        }

        private void SortMaterialLists()
        {
            foreach (ulong Hash in DirtyMaterialLists)
            {
                MaterialRequestBatches[Hash].OrderBy(item => item.IndexBufferHash);
            }

            if (DirtyMaterialLists.Count > 0)
            {
                DirtyMaterialLists.Clear();
            }
        }

        private void CreateShaderForMaterial(IMaterial InMaterial)
        {
            ulong ShaderHash = InMaterial.ShaderHash;
            BaseShader MaterialShader = (RenderStorageSingleton.Instance.ShaderManager.shaders.ContainsKey(ShaderHash)
                ? RenderStorageSingleton.Instance.ShaderManager.shaders[ShaderHash]
                : RenderStorageSingleton.Instance.ShaderManager.shaders[0]);

            MaterialToShaderLookup.Add(InMaterial.GetMaterialHash(), MaterialShader);
        }

        private void CreateRuntimeMaterialForMaterial(IMaterial InMaterial)
        {
            RuntimeMaterialInstance MatInstance = new RuntimeMaterialInstance();
            MatInstance.Initialise(InMaterial);

            MaterialHashToInstance.Add(InMaterial.GetMaterialHash(), MatInstance);
        }

        public void Shutdown()
        {
        }
    }
}
