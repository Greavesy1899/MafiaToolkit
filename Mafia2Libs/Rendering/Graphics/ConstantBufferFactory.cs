using System;
using System.Runtime.CompilerServices;
using Vortice;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public class ConstantBufferFactory
    {
        public static ID3D11Buffer ConstructBuffer<T>(ID3D11Device device, string debugName = "") where T : struct
        {
            var bufferDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Unsafe.SizeOf<T>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            ID3D11Buffer buffer = device.CreateBuffer(bufferDesc);
            buffer.DebugName = debugName;
            return buffer;
        }

        unsafe public static void UpdatePixelBuffer<T>(ID3D11DeviceContext context, ID3D11Buffer buffer, int slot, T data) where T : struct
        {
            MappedSubresource mappedResource = context.Map(buffer, MapMode.WriteDiscard, MapFlags.None);
            Unsafe.Write((void*)mappedResource.DataPointer, data);
            context.Unmap(buffer);
            context.PSSetConstantBuffer(slot, buffer);
        }

        unsafe public static void UpdateVertexBuffer<T>(ID3D11DeviceContext context, ID3D11Buffer buffer, int slot, T data) where T : struct
        {
            MappedSubresource mappedResource = context.Map(buffer, MapMode.WriteDiscard, MapFlags.None);
            Unsafe.Write((void*)mappedResource.DataPointer, data);
            context.Unmap(buffer);
            context.VSSetConstantBuffer(slot, buffer);
        }
    }
}
