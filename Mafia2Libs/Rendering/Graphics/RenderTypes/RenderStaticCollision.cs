using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using ResourceTypes.Collisions;
using Buffer = SharpDX.Direct3D11.Buffer;
using System.Collections.Generic;
using System.Linq;
using ResourceTypes.Collisions.Opcode;

namespace Rendering.Graphics
{
    public class RenderStaticCollision : IRenderer
    {
        public RenderBoundingBox BoundingBox { get; set; }
        public VertexLayouts.CollisionLayout.Vertex[] Vertices { get; private set; }
        public uint[] Indices { get; private set; }
        public BaseShader Shader;
        private CollisionMaterials[] materials;
        public Vector4 SelectionColour { get; private set; }
        public RenderStaticCollision()
        {
            DoRender = true;
            Transform = Matrix.Identity;
            BoundingBox = new RenderBoundingBox();
            SelectionColour = new Vector4(1.0f);
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            vertexBuffer = Buffer.Create(d3d, BindFlags.VertexBuffer, Vertices);
            indexBuffer = Buffer.Create(d3d, BindFlags.IndexBuffer, Indices);
            Shader = RenderStorageSingleton.Instance.ShaderManager.shaders[2];
            BoundingBox.InitBuffers(d3d, context);
        }

        public void ConvertCollisionToRender(ResourceTypes.ItemDesc.CollisionConvex convex)
        {
            DoRender = true;
            BoundingBox = new RenderBoundingBox();
            BoundingBox.Init(new BoundingBox(new Vector3(-0.5f), new Vector3(0.5f)));
            BoundingBox.DoRender = false;

            Indices = convex.indices;
            Vertices = new VertexLayouts.CollisionLayout.Vertex[convex.vertices.Length];
            materials = new CollisionMaterials[convex.vertices.Length];
            for (int i = 0; i != convex.vertices.Length; i++)
            {
                VertexLayouts.CollisionLayout.Vertex vertex = new VertexLayouts.CollisionLayout.Vertex();
                vertex.Position = convex.vertices[i];
                vertex.Normal = new Vector3(0.0f);
                vertex.Colour = new Vector4(1.0f);
                Vertices[i] = vertex;
            }
            CalculateNormals();
        }
        public void ConvertCollisionToRender(TriangleMesh triangleMesh)
        { 
            DoRender = true;
            BoundingBox = new RenderBoundingBox();
            BoundingBox.Init(triangleMesh.BoundingBox);
            BoundingBox.DoRender = false;

            Indices = triangleMesh.Triangles.SelectMany(t => new[] { t.v0, t.v1, t.v2 }).ToArray();
            Vertices = new VertexLayouts.CollisionLayout.Vertex[triangleMesh.Vertices.Count];
            materials = triangleMesh.MaterialIndices.Select(m => (CollisionMaterials)m).ToArray();
            for (int i = 0; i != triangleMesh.Vertices.Count; i++)
            {
                VertexLayouts.CollisionLayout.Vertex vertex = new VertexLayouts.CollisionLayout.Vertex();
                vertex.Position = triangleMesh.Vertices[i];
                vertex.Normal = new Vector3(0.0f);
                vertex.Colour = new Vector4(1.0f);
                Vertices[i] = vertex;
            }

            int materialIDX = 0;
            for(int i = 0; i != triangleMesh.Triangles.Count; i++)
            {
                switch(materials[materialIDX])
                {
                    case CollisionMaterials.GrassAndSnow:
                        Vertices[triangleMesh.Triangles[i].v0].Colour = new Vector4(0, 0.4f, 0, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v1].Colour = new Vector4(0, 0.4f, 0, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v2].Colour = new Vector4(0, 0.4f, 0, 1.0f);
                        break;
                    case CollisionMaterials.Water:
                        Vertices[triangleMesh.Triangles[i].v0].Colour = new Vector4(0, 0.3f, 0.8f, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v1].Colour = new Vector4(0, 0.3f, 0.8f, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v2].Colour = new Vector4(0, 0.3f, 0.8f, 1.0f);
                        break;
                    case CollisionMaterials.Gravel:
                    case CollisionMaterials.Tarmac:
                    case CollisionMaterials.Sidewalk:
                    case CollisionMaterials.SidewalkEdge:
                        Vertices[triangleMesh.Triangles[i].v0].Colour = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v1].Colour = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v2].Colour = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
                        break;
                    case CollisionMaterials.Mud:
                        Vertices[triangleMesh.Triangles[i].v0].Colour = new Vector4(0.4f, 0.2f, 0.0f, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v1].Colour = new Vector4(0.4f, 0.2f, 0.0f, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v2].Colour = new Vector4(0.4f, 0.2f, 0.0f, 1.0f);
                        break;
                    case CollisionMaterials.PlayerCollision:
                        Vertices[triangleMesh.Triangles[i].v0].Colour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v1].Colour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                        Vertices[triangleMesh.Triangles[i].v2].Colour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                        break;
                    default:
                        Vertices[triangleMesh.Triangles[i].v0].Colour = new Vector4(1.0f);
                        Vertices[triangleMesh.Triangles[i].v1].Colour = new Vector4(1.0f);
                        Vertices[triangleMesh.Triangles[i].v2].Colour = new Vector4(1.0f);
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

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            BoundingBox.Render(device, deviceContext, camera, light);
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexLayouts.CollisionLayout.Vertex>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(indexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            Shader.SetSceneVariables(deviceContext, Transform, camera);
            Shader.SetShaderParameters(device, deviceContext, new BaseShader.MaterialParameters(null, SelectionColour));
            Shader.Render(deviceContext, PrimitiveTopology.TriangleList, Indices.Length, 0);
        }

        public override void SetTransform(Matrix matrix)
        {
            Transform = matrix;
            BoundingBox.SetTransform(matrix);
        }

        public override void Shutdown()
        {
            Indices = null;
            Vertices = null;
            materials = null;
            BoundingBox.Shutdown();
            BoundingBox = null;
            vertexBuffer?.Dispose();
            vertexBuffer = null;
            indexBuffer?.Dispose();
            indexBuffer = null;
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            BoundingBox.UpdateBuffers(device, deviceContext);
            if(isUpdatedNeeded)
            {
                //todo: implement this!
            }
        }

        public override void Select()
        {
            BoundingBox.Select();
            SelectionColour = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
            BoundingBox.DoRender = true;
        }

        public override void Unselect()
        {
            BoundingBox.Unselect();
            SelectionColour = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            BoundingBox.DoRender = false;
        }
    }
}
