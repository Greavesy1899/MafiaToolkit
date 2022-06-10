using ResourceTypes.Collisions;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Utils.Extensions;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Color = System.Drawing.Color;

namespace Rendering.Graphics
{
    public class RenderStaticCollision : IRenderer
    {
        public VertexLayouts.CollisionLayout.Vertex[] Vertices { get; private set; }
        public uint[] Indices { get; private set; }
        public BaseShader Shader;
        private CollisionMaterials[] materials;
        public Color SelectionColour { get; private set; }
        public RenderStaticCollision()
        {
            DoRender = true;
            Transform = Matrix4x4.Identity;
            SelectionColour = Color.White;
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext context)
        {
            vertexBuffer = d3d.CreateBuffer<VertexLayouts.CollisionLayout.Vertex>(BindFlags.VertexBuffer, Vertices, 0, ResourceUsage.Default, CpuAccessFlags.None);
            indexBuffer = d3d.CreateBuffer<uint>(BindFlags.IndexBuffer, Indices, 0, ResourceUsage.Default, CpuAccessFlags.None);
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
        public void ConvertCollisionToRender(ulong CollisionName, TriangleMesh triangleMesh)
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
                normals[i] = Vector3.Normalize(normals[i]);
                Vertices[i].Normal = normals[i];
            }
        }

        public override void Render(DirectX11Class Dx11Object, Camera camera)
        {
            if (!DoRender)
            {
                return;
            }

            Dx11Object.DeviceContext.IASetVertexBuffer(0, vertexBuffer, Unsafe.SizeOf<VertexLayouts.CollisionLayout.Vertex>());
            Dx11Object.DeviceContext.IASetIndexBuffer(indexBuffer, Vortice.DXGI.Format.R32_UInt, 0);
            Dx11Object.SetPrimitiveTopology(PrimitiveTopology.TriangleList);

            var MatParams = new BaseShader.MaterialParameters(null, SelectionColour.Normalize());
            Shader.SetSceneVariables(Dx11Object.DeviceContext, Transform, camera);
            Shader.SetShaderParameters(Dx11Object.Device, Dx11Object.DeviceContext, MatParams);
            Shader.SetEditorParameters(Dx11Object.Device, Dx11Object.DeviceContext, MatParams);

            Shader.Render(Dx11Object, PrimitiveTopology.TriangleList, Indices.Length, 0);
        }

        public override void Shutdown()
        {
            base.Shutdown();

            Indices = null;
            Vertices = null;
            materials = null;
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if(bIsUpdatedNeeded)
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
