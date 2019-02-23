﻿using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace Rendering.Graphics
{
    public class RenderBoundingBox
    {
        public BoundingBox Boundings { get; private set; }
        public Buffer VertexBuffer { get; private set; }
        public Buffer IndexBuffer { get; private set; }
        private VertexLayouts.BBoxLayout.Vertex[] Vertices { get; set; }
        public  ushort[] Indices { get; private set; }
        public BaseShader Shader;

        public RenderBoundingBox()
        {

        }

        public bool Init(BoundingBox bbox)
        {
            Boundings = bbox;

            Vertices = new VertexLayouts.BBoxLayout.Vertex[8];
            //1
            Vertices[0].Position = new Vector3(Boundings.Minimum.X, Boundings.Minimum.Y, Boundings.Maximum.Z);
            Vertices[0].Colour = new Vector3(1.0f);

            //2
            Vertices[1].Position = new Vector3(Boundings.Maximum.X, Boundings.Minimum.Y, Boundings.Maximum.Z);
            Vertices[1].Colour = new Vector3(1.0f);

            //3
            Vertices[2].Position = new Vector3(Boundings.Minimum.X, Boundings.Minimum.Y, Boundings.Minimum.Z);
            Vertices[2].Colour = new Vector3(1.0f);

            //4
            Vertices[3].Position = new Vector3(Boundings.Maximum.X, Boundings.Minimum.Y, Boundings.Minimum.Z);
            Vertices[3].Colour = new Vector3(1.0f);

            //5
            Vertices[4].Position = new Vector3(Boundings.Minimum.X, Boundings.Maximum.Y, Boundings.Maximum.Z);
            Vertices[4].Colour = new Vector3(1.0f);

            //6
            Vertices[5].Position = new Vector3(Boundings.Maximum.X, Boundings.Maximum.Y, Boundings.Maximum.Z);
            Vertices[5].Colour = new Vector3(1.0f);


            //7
            Vertices[6].Position = new Vector3(Boundings.Minimum.X, Boundings.Maximum.Y, Boundings.Minimum.Z);
            Vertices[6].Colour = new Vector3(1.0f);

            //8
            Vertices[7].Position = new Vector3(Boundings.Maximum.X, Boundings.Maximum.Y, Boundings.Minimum.Z);
            Vertices[7].Colour = new Vector3(1.0f);

            Indices = new ushort[] {
            0, 2, 3,
            3, 1, 0,
            4, 5, 7,
            7, 6, 4,
            0, 1, 5,
            5, 4, 0,
            1, 3, 7,
            7, 5, 1,
            3, 2, 6,
            6, 7, 3,
            2, 0, 4,
            4, 6, 2
            };

            return true;
        }

        public bool InitBuffer(Device device)
        {
            VertexBuffer = Buffer.Create(device, BindFlags.VertexBuffer, Vertices);
            IndexBuffer = Buffer.Create(device, BindFlags.IndexBuffer, Indices);
            return true;
        }

        public void ReleaseModel()
        {
            Vertices = null;
            Indices = null;
        }

        public void ShutdownBuffers()
        { 
            IndexBuffer?.Dispose();
            IndexBuffer = null;
            VertexBuffer?.Dispose();
            VertexBuffer = null;
        }
        public bool Render(Device device, DeviceContext deviceContext, Matrix Transform, Camera camera, LightClass light)
        {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<VertexLayouts.BBoxLayout.Vertex>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.LineList;

            Shader.SetSceneVariables(deviceContext, Transform, camera, light);
            Shader.Render(deviceContext, 12 * 3, 0);
            return true;
        }


    }
}
