using System.Numerics;
using Rendering.Graphics;
using ResourceTypes.BufferPools;
using ResourceTypes.FrameResource;
using ResourceTypes.Navigation;
using Vortice.Mathematics;

namespace Rendering.Factories
{
    public static class RenderableFactory
    {
        public static RenderBoundingBox BuildBoundingBox(BoundingBox BBox, Matrix4x4 WorldTransform)
        {
            RenderBoundingBox RenderBox = new RenderBoundingBox();
            RenderBox.Init(BBox);
            RenderBox.SetTransform(WorldTransform);
            return RenderBox;
        }

        public static RenderPlane3D BuildPlane3D(Vector4[] Planes, Matrix4x4 WorldTransform)
        {
            RenderPlane3D Plane3D = new RenderPlane3D();
            Plane3D.InitPlanes(Planes, WorldTransform);
            Plane3D.SetTransform(WorldTransform);
            return Plane3D;
        }

        public static RenderModel BuildRenderModelFromFrame(FrameObjectSingleMesh Mesh)
        {
            if (Mesh.MaterialIndex == -1 && Mesh.MeshIndex == -1)
            {
                return null;
            }

            FrameGeometry geom = Mesh.GetGeometry();
            FrameMaterial mat = Mesh.GetMaterial();
            IndexBuffer[] indexBuffers = new IndexBuffer[geom.LOD.Length];
            VertexBuffer[] vertexBuffers = new VertexBuffer[geom.LOD.Length];

            // We need to retrieve buffers first.
            for (int c = 0; c != geom.LOD.Length; c++)
            {
                indexBuffers[c] = Mesh.GetIndexBuffer(c);
                vertexBuffers[c] = Mesh.GetVertexBuffer(c);
            }

            if (indexBuffers[0] == null || vertexBuffers[0] == null)
            {
                return null;
            }
            RenderModel model = new RenderModel();
            model.ConvertFrameToRenderModel(Mesh, geom, mat, indexBuffers, vertexBuffers);
            return model;
        }

        public static RenderAIWorld BuildAIWorld(GraphicsClass InGraphics, AIWorld InWorldInfo)
        {
            RenderAIWorld OurAIWorld = new RenderAIWorld(InGraphics);
            OurAIWorld.Init(InWorldInfo);
            return OurAIWorld;
        }

        // TODO: Rubbish, redo. Old code.
/*        public static RenderStaticCollision BuildRenderItemDesc(ulong refID)
        {
            foreach (var itemDesc in SceneData.ItemDescs)
            {
                if (itemDesc.frameRef == refID)
                {
                    if (itemDesc.colType == ResourceTypes.ItemDesc.CollisionTypes.Convex)
                    {
                        RenderStaticCollision iDesc = new RenderStaticCollision();
                        iDesc.SetTransform(itemDesc.Matrix);
                        iDesc.ConvertCollisionToRender((ResourceTypes.ItemDesc.CollisionConvex)itemDesc.collision);
                        return iDesc;
                    }
                }
            }
            return null;
        }*/
    }
}
