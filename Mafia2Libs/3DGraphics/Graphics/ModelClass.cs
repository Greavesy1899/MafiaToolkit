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

namespace ModelViewer.Programming.GraphicClasses
{
    public class ModelClass
    {
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
        public TextureClass Texture { get; private set; }
        public ModelFormat[] ModelObj { get; private set; }

        public ModelClass() { }

        public bool Init(SharpDX.Direct3D11.Device device, string modelFormatFilename, string textureFileName)
        {
            if (!LoadModel(modelFormatFilename))
            {
                MessageBox.Show("unable to load model " + modelFormatFilename);
                return false;
            }
            if (!InitBuffer(device))
            {
                MessageBox.Show("unable to init buffer");
                return false;
            }
            if (!LoadTexture(device, textureFileName))
            {
                MessageBox.Show("unable to load texture");
                return false;
            }
            return true;
        }
        private bool LoadModel(string modelFormatFilename)
        {
            List<string> lines = null;

            try
            {
                lines = File.ReadLines(modelFormatFilename).ToList();

                var vertexCountString = lines[0].Split(new char[] { ':' })[1].Trim();
                VertexCount = int.Parse(vertexCountString);
                IndexCount = VertexCount;
                ModelObj = new ModelFormat[VertexCount];

                for (var i = 4; i < lines.Count && i < 4 + VertexCount; i++)
                {
                    var modelArray = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    ModelObj[i - 4] = new ModelFormat()
                    {
                        x = float.Parse(modelArray[0]),
                        y = float.Parse(modelArray[1]),
                        z = float.Parse(modelArray[2]),
                        tu = float.Parse(modelArray[3]),
                        tv = float.Parse(modelArray[4]),
                        nx = float.Parse(modelArray[5]),
                        ny = float.Parse(modelArray[6]),
                        nz = float.Parse(modelArray[7])
                    };
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
        private bool InitBuffer(SharpDX.Direct3D11.Device device)
        {
            try
            {
                var vertices = new LightShaderClass.Vertex[VertexCount];
                var indices = new int[IndexCount];
                for (var i = 0; i < VertexCount; i++)
                {
                    vertices[i] = new LightShaderClass.Vertex()
                    {
                        position = new Vector3(ModelObj[i].x, ModelObj[i].y, ModelObj[i].z),
                        texture = new Vector2(ModelObj[i].tu, ModelObj[i].tv),
                        normal = new Vector3(ModelObj[i].nx, ModelObj[i].ny, ModelObj[i].nz)
                    };
                    indices[i] = i;
                    
                }
                VertexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, vertices);
                IndexBuffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.IndexBuffer, indices);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool LoadTexture(SharpDX.Direct3D11.Device device, string textureFileName)
        {
            Texture = new TextureClass();
            bool result = Texture.Init(device, textureFileName);
            return result;
        }
        private void ReleaseTextures()
        {
            Texture?.Shutdown();
            Texture = null;
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
