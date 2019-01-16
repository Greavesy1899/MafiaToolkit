using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System.Windows.Forms;
using System.Collections.Generic;
using Mafia2;

namespace ModelViewer.Programming.GraphicClasses
{
    public class RenderModel
    {
        public struct ModelPart
        {
            public ushort[] Indices;
            public string TextureName;
            public ShaderResourceView Texture;
            public Buffer IndexBuffer;
        }

        private Buffer VertexBuffer { get; set; }
        private Buffer IndexBuffer { get; set; }
        public ShaderClass.Vertex[] Vertices { get; private set; }
        public RenderBoundingBox BoundingBox { get; private set; }
        public ModelPart[] ModelParts { get; set; }
        public Matrix Transform { get; private set; }
        public bool DoRender { get; set; }

        public RenderModel()
        {
            Transform = Matrix.Identity;
            BoundingBox = new RenderBoundingBox();
        }

        public bool Init(Device device)
        {
            //if (!LoadModel(new FileInfo(modelFormatFilename)))
            //{
            //    MessageBox.Show("unable to load model " + modelFormatFilename);
            //    return false;
            //}
            if (!InitBuffer(device))
            {
                MessageBox.Show("unable to init buffer");
                return false;
            }
            if (!LoadTexture(device))
            {
                MessageBox.Show("unable to load texture");
                return false;
            }
            return true;
        }

        public void SetTransform(float posX, float posY, float posZ, Matrix33 rotation)
        {
            Matrix m_trans = Matrix.Identity;
            m_trans[0, 0] = rotation.M00;
            m_trans[0, 1] = rotation.M01;
            m_trans[0, 2] = rotation.M02;
            m_trans[1, 0] = rotation.M10;
            m_trans[1, 1] = rotation.M11;
            m_trans[1, 2] = rotation.M12;
            m_trans[2, 0] = rotation.M20;
            m_trans[2, 1] = rotation.M21;
            m_trans[2, 2] = rotation.M22;
            m_trans[3, 0] = posX;
            m_trans[3, 1] = posY;
            m_trans[3, 2] = posZ;
            Transform = m_trans;
        }

        //CLEANUP WHEN MERGED
        public bool ConvertM2ModelToRenderModel(M2TStructure structure)
        {
            Vertices = new ShaderClass.Vertex[structure.Lods[0].Vertices.Length];
            ModelParts = new ModelPart[structure.Lods[0].Parts.Length];

            for (int i = 0; i != Vertices.Length; i++)
            {
                ShaderClass.Vertex newVertex = new ShaderClass.Vertex();
                Vertex oldVertex = structure.Lods[0].Vertices[i];

                newVertex.position = new SharpDX.Vector3(oldVertex.Position.X, oldVertex.Position.Y, oldVertex.Position.Z);
                newVertex.normal = new SharpDX.Vector3(oldVertex.Normal.X, oldVertex.Normal.Y, oldVertex.Normal.Z);

                if (oldVertex.UVs[0] == null)
                    newVertex.texture = new SharpDX.Vector2(0.0f, 1.0f);
                else
                    newVertex.texture = new SharpDX.Vector2(oldVertex.UVs[0].X, oldVertex.UVs[0].Y);

                Vertices[i] = newVertex;
            }

            for (int i = 0; i != ModelParts.Length; i++)
            {
                ModelPart part = new ModelPart();
                part.TextureName = structure.Lods[0].Parts[i].Material;
                List<ushort> inds = new List<ushort>();

                for (int x = 0; x != structure.Lods[0].Parts[i].Indices.Length; x++)
                {
                    inds.Add(structure.Lods[0].Parts[i].Indices[x].S1);
                    inds.Add(structure.Lods[0].Parts[i].Indices[x].S2);
                    inds.Add(structure.Lods[0].Parts[i].Indices[x].S3);
                }
                part.Indices = inds.ToArray();
                ModelParts[i] = part;
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
        private bool InitBuffer(Device device)
        {
            VertexBuffer = Buffer.Create(device, BindFlags.VertexBuffer, Vertices);

            for (int x = 0; x != ModelParts.Length; x++)
                ModelParts[x].IndexBuffer = Buffer.Create(device, BindFlags.IndexBuffer, ModelParts[x].Indices);

            BoundingBox.InitBuffer(device);
            return true;
        }
        private bool LoadTexture(Device device)
        {
            for (int x = 0; x != ModelParts.Length; x++)
            {
                TextureClass Texture = new TextureClass();
                bool result = Texture.Init(device, ModelParts[x].TextureName);
                ModelParts[x].Texture = Texture.TextureResource;

                if (!result)
                    return false;
            }
            return true;
        }
        private void ReleaseTextures()
        {
            for (int x = 0; x != ModelParts.Length; x++)
            {
                ModelParts[x].Texture?.Dispose();
                ModelParts[x].Texture = null;
            }

            BoundingBox.ReleaseTextures();
        }
        private void ReleaseModel()
        {
            Vertices = null;
            for (int x = 0; x != ModelParts.Length; x++)
                ModelParts[x].Indices = null;
            BoundingBox.ReleaseModel();
        }
        private void ShutdownBuffers()
        {
            for (int x = 0; x != ModelParts.Length; x++)
            {
                ModelParts[x].IndexBuffer?.Dispose();
                ModelParts[x].IndexBuffer = null;
            }
            BoundingBox.ShutdownBuffers();
            VertexBuffer?.Dispose();
            VertexBuffer = null;
        }
        private void RenderBuffers(DeviceContext deviceContext)
        {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<ShaderClass.Vertex>(), 0));
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }
    }
}
