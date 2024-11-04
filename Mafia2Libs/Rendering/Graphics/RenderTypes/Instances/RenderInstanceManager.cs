using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rendering.Core;
using Utils.Extensions;
using Vortice;
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

                instance.Transforms.Add(transform);
            }
        }

        public void PrepareInstanceBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            foreach (var lod in modelInstanceManager.GetAllInstances())
            {
                foreach (var instance in lod.Value.modelInstances)
                {
                    if (instance.Transforms != null && instance.Transforms.Count > 0)
                    {
                        if (instance.InstanceBuffer == null || instance.Transforms.Count > 1)
                        {
                            // Inicializujte nebo přenačtěte buffer pokud potřebujeme více místa
                            var bufferDescription = new BufferDescription
                            {
                                SizeInBytes = instance.Transforms.Count * Marshal.SizeOf<Matrix4x4>(),
                                Usage = ResourceUsage.Dynamic,
                                BindFlags = BindFlags.ConstantBuffer,
                                CpuAccessFlags = CpuAccessFlags.Write,
                            };

                            instance.InstanceBuffer?.Dispose();
                            instance.InstanceBuffer = device.CreateBuffer(bufferDescription);
                            //instance.InstanceBufferCapacity = instance.Transforms.Count;
                        }

                        var mappedResource = deviceContext.Map(instance.InstanceBuffer, MapMode.WriteDiscard, MapFlags.None);

                        unsafe
                        {
                            fixed (Matrix4x4* dataPtr = instance.Transforms.ToArray())
                            {
                                Buffer.MemoryCopy(dataPtr, mappedResource.DataPointer.ToPointer(),
                                    instance.Transforms.Count * Marshal.SizeOf<Matrix4x4>(),
                                    instance.Transforms.Count * Marshal.SizeOf<Matrix4x4>());
                            }
                        }

                        deviceContext.Unmap(instance.InstanceBuffer);
                    }
                }
            }
        }



        public void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            PrepareInstanceBuffers(device, deviceContext);

            foreach (var lod in modelInstanceManager.GetAllInstances())
            {
                foreach (var instance in lod.Value.modelInstances)
                {
                    if (instance.Transforms != null && instance.Transforms.Count > 0)
                    {
                        VertexBufferView vertexBufferView = new VertexBufferView(instance.vertexBuffer, Unsafe.SizeOf<VertexLayouts.NormalLayout.Vertex>(), 0);
                        deviceContext.IASetVertexBuffers(0, vertexBufferView);
                        deviceContext.IASetIndexBuffer(instance.indexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
                        deviceContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);

                        // Nastavíme instance buffer pro shader
                        deviceContext.VSSetConstantBuffers(1, 1, new ID3D11Buffer[] { instance.InstanceBuffer });

                        // Vykreslení s instancemi
                        RenderInstances(deviceContext, instance, camera,device);
                    }
                }
            }
        }



        private void RenderInstances(ID3D11DeviceContext deviceContext, ModelInstance instance, Camera camera,ID3D11Device device)
        {

            for (int i = 0; i < instance.LODs[0].ModelParts.Length; i++)
            {

                
                RenderModel.ModelPart segment = instance.LODs[0].ModelParts[i];
                
                            //Blob VertexBytecode =  segment.Shader.ConstructBytecode(new ShaderInitParams.ShaderFileEntryPoint("mrdka.hlsl", "LightVertexShader", "vs_4_0"));
                            //ID3D11VertexShader hovno = device.CreateVertexShader(VertexBytecode);                

                deviceContext.IASetInputLayout(segment.Shader.Layout);
                deviceContext.VSSetShader(segment.Shader.OurVertexShader);

                deviceContext.PSSetShader(segment.Shader.OurPixelShader);
                deviceContext.PSSetSampler(0, segment.Shader.SamplerState);

                deviceContext.GSSetShader(segment.Shader.OurGeometryShader);
                
                // Nastavení parametrů materiálu
                segment.Shader.SetShaderParameters(deviceContext.Device, deviceContext, new BaseShader.MaterialParameters(segment.Material, Color.White.Normalize()));
                
                
                // Nastavení instance bufferu pro transformace
                deviceContext.VSSetConstantBuffers(1, 1, new ID3D11Buffer[] { instance.InstanceBuffer });

                // Vykreslete všechny transformace jako instancované geometrie
                deviceContext.DrawIndexedInstanced((int)segment.NumFaces * 3, instance.Transforms.Count, (int)segment.StartIndex, 0, 0);
                Profiler.NumDrawCallsThisFrame++;
            }
        }
    }
}
