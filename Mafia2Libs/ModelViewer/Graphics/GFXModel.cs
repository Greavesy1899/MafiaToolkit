using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ModelViewer.System;
using Mafia2;

namespace ModelViewer.Graphics
{
    public class ModelClass
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Vertex
        {
            public SharpDX.Vector3 position;
            public SharpDX.Vector2 texture;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct ModelFormat
        {
            public float x, y, z;
            public float tu, tv;
            public float nx, ny, nz;
        }
        private SharpDX.Direct3D11.Buffer VertexBuffer { get; set; }
        private SharpDX.Direct3D11.Buffer IndexBuffer { get; set; }
        private int VertexCount { get; set; }
        public int IndexCount { get; private set; }
        public TextureArrayClass Textures { get; private set; }
        public ModelFormat[] ModelObj { get; private set; }

        public ModelClass() { }

        public bool Init(SharpDX.Direct3D11.Device device, CustomEDM model, string[] textureFileNames)
        {
            if (!InitBuffer(device, model))
            {
                MessageBox.Show("Unable to load the model into the buffer.");
                return false;
            }
            if (!LoadTexture(device, textureFileNames))
            {
                MessageBox.Show("unable to load texture");
                return false;
            }
            return true;
        }
        public void Shutdown()
        {
            ReleaseTextures();
            ShutdownBuffers();
        }
        public void Render(DeviceContext deviceContext)
        {
            RenderBuffers(deviceContext);
        }
        private bool InitBuffer(SharpDX.Direct3D11.Device device, CustomEDM model)
        {
            try
            {
                int part = 2;
                VertexCount = model.Parts[part].Vertices.Length;
                IndexCount = model.Parts[part].Indices.Count * 3;

                LightShaderClass.Vertex[] vertices = new LightShaderClass.Vertex[VertexCount];
                int[] indices = new int[IndexCount];

                for (int i = 0; i != VertexCount; i++)
                {
                    SharpDX.Vector3 vert = new SharpDX.Vector3(model.Parts[part].Vertices[i].X, model.Parts[part].Vertices[i].Y, model.Parts[part].Vertices[i].Z);
                    SharpDX.Vector3 norm = new SharpDX.Vector3(model.Parts[part].Normals[i].X, model.Parts[part].Normals[i].Y, model.Parts[part].Normals[i].Z);
                    SharpDX.Vector2 uv = new SharpDX.Vector2(model.Parts[part].UVs[i].X, model.Parts[part].UVs[i].Y);

                    vertices[i] = new LightShaderClass.Vertex(vert, uv,  norm);
                }
                int index = 0;
                foreach(Short3 s in model.Parts[0].Indices)
                {
                    indices[index] = s.s1;
                    index++;
                    indices[index] = s.s2;
                    index++;
                    indices[index] = s.s3;
                    index++;
                }
                VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, vertices);
                IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indices);
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private bool LoadTexture(SharpDX.Direct3D11.Device device, string[] textureFileNames)
        {
            Textures = new TextureArrayClass();
            Textures.Init(device, textureFileNames);
            return true;
        }
        private void ReleaseTextures()
        {
            Textures?.Shutdown();
            Textures = null;
        }
        private void ReleaseModel()
        {
            ModelObj = null;
        }
        private void ShutdownBuffers()
        {
            IndexBuffer?.Dispose();
            IndexBuffer = null;
            VertexBuffer?.Dispose();
            VertexBuffer = null;
        }
        private void RenderBuffers(DeviceContext deviceContext)
        {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<LightShaderClass.Vertex>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, Format.R32_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }
    }
}