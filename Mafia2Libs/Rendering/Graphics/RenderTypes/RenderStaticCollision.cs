using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using ResourceTypes.Collisions;
using Buffer = SharpDX.Direct3D11.Buffer;
using Color = System.Drawing.Color;
using System.Collections.Generic;
using System.Linq;
using ResourceTypes.Collisions.Opcode;
using Utils.Extensions;

namespace Rendering.Graphics
{
    public class RenderStaticCollision : IRenderer
    {
        public VertexLayouts.CollisionLayout.Vertex[] Vertices { get; private set; }
        public uint[] Indices { get; private set; }
        public BaseShader Shader;
        private CollisionMaterials[] materials;
        public Color SelectionColour { get; private set; }

        private VertexBufferBinding OurVertexBufferBinding;

        public RenderStaticCollision()
        {
            DoRender = true;
            Transform = Matrix.Identity;
            SelectionColour = Color.White;
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, Vertices);
            indexBuffer = Buffer.Create(d3d, BindFlags.IndexBuffer, Indices);

            OurVertexBufferBinding = new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexLayouts.CollisionLayout.Vertex>(), 0);

            Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[2];
        }

        public void ConvertCollisionToRender(ResourceTypes.ItemDesc.CollisionConvex convex)
        {
            DoRender = true;
            BoundingBox = new BoundingBox(new Vector3(-0.5f), new Vector3(0.5f));

            Indices = convex.indices;
            Vertices = new VertexLayouts.CollisionLayout.Vertex[convex.vertices.Length];
            materials = new CollisionMaterials[convex.vertices.Length];
            for (int i = 0; i != convex.vertices.Length; i++)
            {
                VertexLayouts.CollisionLayout.Vertex vertex = new VertexLayouts.CollisionLayout.Vertex();
                vertex.Position = convex.vertices[i];
                vertex.Normal = new Vector3(0.0f);
                vertex.Colour = SelectionColour.ToArgb();
                Vertices[i] = vertex;
            }
            CalculateNormals();
        }
        public void ConvertCollisionToRender(TriangleMesh triangleMesh)
        { 
            DoRender = true;
            BoundingBox = triangleMesh.BoundingBox;

            Indices = triangleMesh.Triangles.SelectMany(t => new[] { t.v0, t.v1, t.v2 }).ToArray();
            Vertices = new VertexLayouts.CollisionLayout.Vertex[triangleMesh.Vertices.Count];
            materials = triangleMesh.MaterialIndices.Select(m => (CollisionMaterials)m).ToArray();
            for (int i = 0; i != triangleMesh.Vertices.Count; i++)
            {
                VertexLayouts.CollisionLayout.Vertex vertex = new VertexLayouts.CollisionLayout.Vertex();
                vertex.Position = triangleMesh.Vertices[i];
                vertex.Normal = new Vector3(0.0f);
                vertex.Colour = SelectionColour.ToArgb();
                Vertices[i] = vertex;
            }

            int materialIDX = 0;
            for(int i = 0; i != triangleMesh.Triangles.Count; i++)
            {
                int currentColour = Color.White.ToArgb();
                switch(materials[materialIDX])
                {
                    case CollisionMaterials.GrassAndSnow:
                        currentColour = Color.FromArgb(255, 0, 102, 0).ToArgb();
                        Vertices[triangleMesh.Triangles[i].v0].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v1].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v2].Colour = currentColour;
                        break;
                    case CollisionMaterials.Water:
                        currentColour = Color.FromArgb(255, 77, 204, 255).ToArgb();
                        Vertices[triangleMesh.Triangles[i].v0].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v1].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v2].Colour = currentColour;
                        break;
                    case CollisionMaterials.Gravel:
                    case CollisionMaterials.Tarmac:
                    case CollisionMaterials.Sidewalk:
                    case CollisionMaterials.SidewalkEdge:
                        currentColour = Color.FromArgb(255, 128, 128, 128).ToArgb();
                        Vertices[triangleMesh.Triangles[i].v0].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v1].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v2].Colour = currentColour;
                        break;
                    case CollisionMaterials.Mud:
                        currentColour = Color.FromArgb(255, 102, 51, 0).ToArgb();
                        Vertices[triangleMesh.Triangles[i].v0].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v1].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v2].Colour = currentColour;
                        break;
                    case CollisionMaterials.PlayerCollision:
                        Vertices[triangleMesh.Triangles[i].v0].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v1].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v2].Colour = currentColour;
                        break;
                    default:
                        Vertices[triangleMesh.Triangles[i].v0].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v1].Colour = currentColour;
                        Vertices[triangleMesh.Triangles[i].v2].Colour = currentColour;
                        break;

                }
                materialIDX++;
            }
            CalculateNormals();
        }

        private void CalculateNormals()
        {
            List<Vector3> surfaceNormals = new List<Vector3>();
            Vector3[] normals = new Vector3[Vertices.Length];
            int index = 0;
            var normal = new Vector3();
            while (index < Indices.Length)
            {
                var edge1 = Vertices[Indices[index]].Position - Vertices[Indices[index + 1]].Position;
                var edge2 = Vertices[Indices[index]].Position - Vertices[Indices[index + 2]].Position;
                normal = Vector3.Cross(edge1, edge2);
                normals[Indices[index]] += normal;
                normals[Indices[index + 1]] += normal;
                normals[Indices[index + 2]] += normal;
                surfaceNormals.Add(normal);
                index += 3;
            }
            surfaceNormals.Add(normal);


            for (int i = 0; i < Vertices.Length; i++)
            {
                normals[i].Normalize();
                Vertices[i].Normal = normals[i];
            }
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
                return;

            deviceContext.InputAssembler.SetVertexBuffers(0, OurVertexBufferBinding);
            deviceContext.InputAssembler.SetIndexBuffer(indexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Shader.SetSceneVariables(deviceContext, Transform, camera);
            Shader.SetShaderParameters(device, deviceContext, new BaseShader.MaterialParameters(null, SelectionColour.Normalize()));
            Shader.Render(deviceContext, PrimitiveTopology.TriangleList, Indices.Length, 0);
        }

        public override void SetTransform(Matrix matrix)
        {
            Transform = matrix;
        }

        public override void Shutdown()
        {
            Indices = null;
            Vertices = null;
            materials = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
            indexBuffer?.Dispose();
            indexBuffer = null;
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            if(isUpdatedNeeded)
            {
                //todo: implement this!
            }
        }

        public override void Select()
        {
            SelectionColour = Color.Red;
        }

        public override void Unselect()
        {
            SelectionColour = Color.White;
        }
    }
}
