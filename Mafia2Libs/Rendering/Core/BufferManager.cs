using Rendering.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.Direct3D11;

namespace Rendering.Core
{
    public class BufferManager
    {
        private Dictionary<ulong, ID3D11Buffer> IndexBuffers;
        private Dictionary<ulong, ID3D11Buffer> VertexBuffers;

        private GraphicsClass CachedGraphics;
        private DirectX11Class D3D;

        public BufferManager(GraphicsClass InGraphics, DirectX11Class InD3D)
        {
            CachedGraphics = InGraphics;
            D3D = InD3D;

            IndexBuffers = new Dictionary<ulong, ID3D11Buffer>();
            VertexBuffers = new Dictionary<ulong, ID3D11Buffer>();
        }

        public void CreateIndexBuffer<T>(ulong BufferHash, Span<T> BufferData) where T : unmanaged
        {
            if (!HasIndexBuffer(BufferHash))
            {
                ID3D11Buffer NewBuffer = D3D.Device.CreateBuffer<T>(BindFlags.IndexBuffer, BufferData, 0, ResourceUsage.Default, CpuAccessFlags.None);
                IndexBuffers.Add(BufferHash, NewBuffer);
            }
        }

        public void CreateVertexBuffer<T>(ulong BufferHash, Span<T> BufferData) where T : unmanaged
        {
            if(!HasVertexBuffer(BufferHash))
            {
                ID3D11Buffer NewBuffer = D3D.Device.CreateBuffer<T>(BindFlags.VertexBuffer, BufferData, 0, ResourceUsage.Default, CpuAccessFlags.None);
                VertexBuffers.Add(BufferHash, NewBuffer);
            }
        }

        public bool HasVertexBuffer(ulong BufferHash)
        {
            return VertexBuffers.ContainsKey(BufferHash);
        }

        public bool HasIndexBuffer(ulong BufferHash)
        {
            return IndexBuffers.ContainsKey(BufferHash);
        }

        public void SetVertexBuffer(ulong BufferHash, int Slot, int Stride)
        {
            if(VertexBuffers.TryGetValue(BufferHash, out ID3D11Buffer Buffer))
            {
                D3D.SetVertexBuffer(Slot, Buffer, Stride);
            }         
        }

        public void SetIndexBuffer(ulong BufferHash, Vortice.DXGI.Format Format, int Offset = 0)
        {
            if (IndexBuffers.TryGetValue(BufferHash, out ID3D11Buffer Buffer))
            {
                D3D.SetIndexBuffer(Buffer, Format, Offset);
            }
        }

        public void Shutdown()
        {
            foreach(ID3D11Buffer D3DBuffer in IndexBuffers.Values)
            {
                D3DBuffer.Dispose();
            }

            foreach (ID3D11Buffer D3DBuffer in VertexBuffers.Values)
            {
                D3DBuffer.Dispose();
            }

            IndexBuffers.Clear();
            VertexBuffers.Clear();
        }
    }
}
