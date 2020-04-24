using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Rendering.Graphics
{
    public class ConstantBufferFactory
    {
        public static Buffer ConstructBuffer<T>(Device device, string debugName = "") where T : struct
        {
            var bufferDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = Utilities.SizeOf<T>(),
                BindFlags = BindFlags.ConstantBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            Buffer buffer = new Buffer(device, bufferDesc);
            buffer.DebugName = debugName;
            return buffer;
        }

        private static void InternalUpdateBuffer<T>(DeviceContext context, ref Buffer buffer, T data) where T : struct
        {
            DataStream mappedResource;
            context.MapSubresource(buffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            mappedResource.Write(data);
            context.UnmapSubresource(buffer, 0);
        }

        public static void UpdatePixelBuffer<T>(DeviceContext context, Buffer buffer, int slot, T data) where T : struct
        {
            DataStream mappedResource;
            context.MapSubresource(buffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            mappedResource.Write(data);
            context.UnmapSubresource(buffer, 0);
            context.PixelShader.SetConstantBuffer(slot, buffer);
        }

        public static void UpdateVertexBuffer<T>(DeviceContext context, Buffer buffer, int slot, T data) where T : struct
        {
            DataStream mappedResource;
            context.MapSubresource(buffer, MapMode.WriteDiscard, MapFlags.None, out mappedResource);
            mappedResource.Write(data);
            context.UnmapSubresource(buffer, 0);
            context.VertexShader.SetConstantBuffer(slot, buffer);
        }
    }
}
