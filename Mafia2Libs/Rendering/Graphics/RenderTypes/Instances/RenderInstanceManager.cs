using Rendering.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Utils.Extensions;
using Utils.VorticeUtils;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Rendering.Graphics.Instances
{
    public class RenderInstanceManager
    {
        private ModelInstanceManager modelInstanceManager;

        public RenderInstanceManager(ModelInstanceManager modelInstanceManager)
        {
            this.modelInstanceManager = modelInstanceManager;
        }

        // Přidání nové instance modelu s danou transformační maticí
        public void AddInstance(int lodHash, Matrix4x4 transform)
        {
            var modelInstances = modelInstanceManager.GetInstances(lodHash);
            if (modelInstances.Count > 0)
            {
                var instance = modelInstances[0];
                if (instance.Transforms == null)
                {
                    instance.Transforms = new List<Matrix4x4>();
                }

                var m = Matrix4x4.Transpose(transform);

                instance.Transforms.Add(m);
            }
        }

        public void UpdateBuffers(ID3D11Device id3D11Device, ID3D11DeviceContext context)
        {
            foreach (var instance in modelInstanceManager.GetAllInstances().Values)
            {
                foreach (var val in instance.modelInstances)
                {
                    UpdateInstanceBuffer(id3D11Device, context, val);
                }
            }
            //is this relevant? if used only by translokators, then not
            //modelInstanceManager.SetupShaders();
            //modelInstanceManager.InitTextures(id3D11Device,context);
        }

        public void UpdateInstanceBuffer(ID3D11Device device, ID3D11DeviceContext deviceContext, ModelInstance instance)
        {
            if (instance.Transforms == null || instance.Transforms.Count < 2)//if it is 1, if is not instance
                return;

            int newSize = 1024 * Marshal.SizeOf<Matrix4x4>();

            // Create or update buffer only if necessary
            if (instance.InstanceBuffer == null || instance.InstanceBuffer.Description.SizeInBytes < newSize)
            {
                // Buffer description for instance buffer
                var bufferDescription = new BufferDescription
                {
                    SizeInBytes = newSize,
                    Usage = ResourceUsage.Dynamic,
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.Write,
                };

                // Dispose old buffer if necessary
                instance.InstanceBuffer?.Dispose();

                // Convert list to array
                Matrix4x4[] transformsArray1 = instance.Transforms.ToArray();
                Matrix4x4[] transformsArray = new Matrix4x4[1024];
                Array.Copy(transformsArray1, 0, transformsArray, 0, transformsArray1.Length < 1024 ? transformsArray1.Length : 1024);

                // Pin the array in memory
                GCHandle handle = GCHandle.Alloc(transformsArray, GCHandleType.Pinned);
                try
                {
                    IntPtr pointer = handle.AddrOfPinnedObject();
                    // Update the instance buffer
                    instance.InstanceBuffer = device.CreateBuffer(bufferDescription, pointer);
                }
                finally
                {
                    handle.Free();
                }
            }
        }

        private void SetupShaders(ModelInstance instance)
        {
            for (int x = 0; x != instance.LODs[0].ModelParts.Length; x++)
            {
                RenderModel.ModelPart part = instance.LODs[0].ModelParts[x];
                if (part.Material == null)
                    part.Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[0];
                else
                {
                    part.Shader = (RenderStorageSingleton.Instance.ShaderManager.shaders.ContainsKey(instance.LODs[0].ModelParts[x].Material.ShaderHash)
                        ? RenderStorageSingleton.Instance.ShaderManager.shaders[instance.LODs[0].ModelParts[x].Material.ShaderHash]
                        : RenderStorageSingleton.Instance.ShaderManager.shaders[0]);
                }
                instance.LODs[0].ModelParts[x] = part;
            }
        }

        public void PrepareInstanceBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            //foreach (var lod in modelInstanceManager.GetAllInstances())
            //{
            //    foreach (var instance in lod.Value.modelInstances)
            //    {
            //        if (instance.Transforms != null && instance.Transforms.Count > 0)
            //        {
            //            // Create or resize the instance buffer if needed
            //            var bufferSize = instance.Transforms.Count * Marshal.SizeOf<Matrix4x4>();
            //            if (instance.InstanceBuffer == null)
            //            {
            //                // Create buffer description for the instance buffer
            //                var bufferDescription = new BufferDescription
            //                {
            //                    SizeInBytes = bufferSize,
            //                    Usage = ResourceUsage.Dynamic,
            //                    BindFlags = BindFlags.ConstantBuffer,  // Use as a vertex buffer for instancing
            //                    CpuAccessFlags = CpuAccessFlags.Write,
            //                };
            //
            //                // Dispose of the old buffer if it exists
            //                instance.InstanceBuffer?.Dispose();
            //                instance.InstanceBuffer = device.CreateBuffer(bufferDescription);
            //            }
            //        }
            //    }
            //}
        }

        public void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            // Předávejte kamerové matice do shaderu
            foreach (var lod in modelInstanceManager.GetAllInstances())
            {
                foreach (var instance in lod.Value.modelInstances)
                {
                    if (instance.Transforms != null && instance.Transforms.Count > 0)
                    {
                        // Nastavte vertex buffer a instance buffer
                        VertexBufferView vertexBufferView = new VertexBufferView(instance.vertexBuffer, Unsafe.SizeOf<VertexLayouts.NormalLayout.Vertex>(), 0);
                        deviceContext.IASetVertexBuffers(0, vertexBufferView);
                        deviceContext.IASetIndexBuffer(instance.indexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
                        deviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);

                        // Renderujte jednotlivé části modelu
                        RenderInstances(deviceContext, instance, camera, device);
                    }
                }
            }
        }

        private void RenderInstances(ID3D11DeviceContext deviceContext, ModelInstance instance, Camera camera, ID3D11Device device)
        {
            deviceContext.PSSetShaderResource(2, instance.AoTexture);
            for (int i = 0; i < instance.LODs[0].ModelParts.Length; i++)
            {
                RenderModel.ModelPart segment = instance.LODs[0].ModelParts[i];

                // Set the shaders
                deviceContext.VSSetShader(segment.Shader.OurVertexShader);
                deviceContext.PSSetShader(segment.Shader.OurPixelShader);
                deviceContext.PSSetSampler(0, segment.Shader.SamplerState);
                deviceContext.GSSetShader(segment.Shader.OurGeometryShader);

                // Set material parameters
                segment.Shader.SetShaderParameters(device, deviceContext, new BaseShader.MaterialParameters(segment.Material, Color.White.Normalize()));


                // Set instance buffer for transformations
                deviceContext.VSSetConstantBuffers(2, 1, new ID3D11Buffer[] { instance.InstanceBuffer }); // We should move this into a separate vertex buffer so it can be dynamic instead of a fixed 65536 byte size

                // Set input layout and draw
                deviceContext.IASetInputLayout(segment.Shader.Layout);

                // Draw indexed instances
                deviceContext.DrawIndexedInstanced((int)segment.NumFaces * 3, instance.Transforms.Count < 1024 ? instance.Transforms.Count : 1024, (int)segment.StartIndex, 0, 0);
                Profiler.NumDrawCallsThisFrame++;
            }
        }
    }
}
