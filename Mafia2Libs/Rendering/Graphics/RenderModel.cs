using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using System.Windows.Forms;
using Mafia2;

namespace Rendering.Graphics
{
    public class RenderModel
    {
        public struct ModelPart
        {
            public Material Material;
            public ulong MaterialHash;
            public string TextureName;
            public ShaderResourceView Texture;
            public uint StartIndex;
            public uint NumFaces;
        }
        private string AOTextureName { get; set; }
        public ShaderResourceView AOTexture { get; set; }
        private Buffer VertexBuffer { get; set; }
        private Buffer IndexBuffer { get; set; }
        public ShaderClass.Vertex[] Vertices { get; private set; }
        public RenderBoundingBox BoundingBox { get; private set; }
        public ModelPart[] ModelParts { get; set; }
        public ushort[] Indices { get; private set; }

        //new
        public struct LOD
        {
            public Buffer IndexBuffer { get; private set; }
            public Buffer VertexBuffer { get; private set; }
            public ModelPart[] ModelParts { get; set; }
            public ushort[] Indices { get; set; }
        }

        public LOD[] LODs { get; private set; }
        public Matrix Transform { get; private set; }
        public bool DoRender { get; set; }

        public RenderModel()
        {
            Transform = Matrix.Identity;
            BoundingBox = new RenderBoundingBox();
        }

        public bool Init(Device device)
        {
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

        public void SetTransform(Vector3 position, Matrix33 rotation)
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
            m_trans[3, 0] = position.X;
            m_trans[3, 1] = position.Y;
            m_trans[3, 2] = position.Z;
            Transform = m_trans;
        }

        //CLEANUP WHEN MERGED
        public bool ConvertM2ModelToRenderModel(M2TStructure structure)
        {
            AOTextureName = structure.AOTexture;
            Vertices = new ShaderClass.Vertex[structure.Lods[0].Vertices.Length];
            ModelParts = new ModelPart[structure.Lods[0].Parts.Length];

            for (int i = 0; i != Vertices.Length; i++)
            {
                ShaderClass.Vertex newVertex = new ShaderClass.Vertex();
                Vertex oldVertex = structure.Lods[0].Vertices[i];

                newVertex.position = oldVertex.Position;
                newVertex.normal = oldVertex.Normal;

                if (oldVertex.UVs[0] == null)
                    newVertex.tex0 = new Vector2(0.0f, 1.0f);
                else
                    newVertex.tex0 = oldVertex.UVs[0];

                if (oldVertex.UVs[3] == null)
                    newVertex.tex7 = new Vector2(0.0f, 1.0f);
                else
                    newVertex.tex7 = oldVertex.UVs[3];

                Vertices[i] = newVertex;
            }

            Indices = structure.Lods[0].Indices;
            for (int i = 0; i != ModelParts.Length; i++)
            {
                ModelPart part = new ModelPart();
                part.TextureName = structure.Lods[0].Parts[i].Material;
                part.StartIndex = structure.Lods[0].Parts[i].StartIndex;
                part.NumFaces = structure.Lods[0].Parts[i].NumFaces;
                ModelParts[i] = part;
            }

            return true;
        }

        public bool ConvertFrameToRenderModel(FrameObjectSingleMesh mesh, FrameGeometry geom, FrameMaterial mats, IndexBuffer[] indexBuffers, VertexBuffer[] vertexBuffers)
        {
            if (mesh == null || geom == null || mats == null || indexBuffers == null || vertexBuffers == null)
                return false;

            SetTransform(mesh.Matrix.Position, mesh.Matrix.Rotation);
            DoRender = true;
            BoundingBox = new RenderBoundingBox();
            BoundingBox.Init(mesh.Boundings);
            LODs = new LOD[geom.NumLods];

            for(int i = 0; i != geom.NumLods; i++)
            {
                LOD lod = new LOD();
                lod.Indices = indexBuffers[i].Data;
                lod.ModelParts = new ModelPart[mats.LodMatCount[i]];

                for (int z = 0; z != mats.Materials[i].Length; z++)
                {
                    lod.ModelParts[z] = new ModelPart();
                    lod.ModelParts[z].NumFaces = (uint)mats.Materials[i][z].NumFaces;
                    lod.ModelParts[z].StartIndex = (uint)mats.Materials[i][z].StartIndex;
                    lod.ModelParts[z].MaterialHash = mats.Materials[i][z].MaterialHash;
                }

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
            IndexBuffer = Buffer.Create(device, BindFlags.IndexBuffer, Indices);

            BoundingBox.InitBuffer(device);
            return true;
        }
        private bool LoadTexture(Device device)
        {
            TextureClass AOTextureClass = new TextureClass();
            bool result = AOTextureClass.Init(device, AOTextureName);
            AOTexture = AOTextureClass.TextureResource;

            if (!result)
                return false;

            for (int x = 0; x != ModelParts.Length; x++)
            {
                TextureClass Texture = new TextureClass();
                result = Texture.Init(device, ModelParts[x].TextureName);
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
            AOTexture?.Dispose();
            AOTexture = null;
            BoundingBox.ReleaseTextures();
        }
        private void ReleaseModel()
        {
            Vertices = null;
            Indices = null;
            BoundingBox.ReleaseModel();
        }
        private void ShutdownBuffers()
        {
            BoundingBox.ShutdownBuffers();
            VertexBuffer?.Dispose();
            VertexBuffer = null;
            IndexBuffer?.Dispose();
            IndexBuffer = null;
        }
        private void RenderBuffers(DeviceContext deviceContext)
        {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(VertexBuffer, Utilities.SizeOf<ShaderClass.Vertex>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(IndexBuffer, SharpDX.DXGI.Format.R16_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }
    }
}
