using Mafia2Tool;
using Rendering.Graphics;
using ResourceTypes.BufferPools;
using ResourceTypes.FrameResource;
using SharpDX;

namespace Rendering.Factories
{
    public static class RenderableFactory
    {
        public static RenderBoundingBox BuildBoundingBox(BoundingBox BBox, Matrix WorldTransform)
        {
            RenderBoundingBox RenderBox = new RenderBoundingBox();
            RenderBox.Init(BBox);
            RenderBox.SetTransform(WorldTransform);
            return RenderBox;
        }

        public static RenderModel BuildRenderModelFromFrame(FrameObjectSingleMesh Mesh)
        {
            if (Mesh.MaterialIndex == -1 && Mesh.MeshIndex == -1)
            {
                return null;
            }

            FrameGeometry geom = SceneData.FrameResource.FrameGeometries[Mesh.Refs[FrameEntryRefTypes.Geometry]];
            FrameMaterial mat = SceneData.FrameResource.FrameMaterials[Mesh.Refs[FrameEntryRefTypes.Material]];
            IndexBuffer[] indexBuffers = new IndexBuffer[geom.LOD.Length];
            VertexBuffer[] vertexBuffers = new VertexBuffer[geom.LOD.Length];

            // We need to retrieve buffers first.
            for (int c = 0; c != geom.LOD.Length; c++)
            {
                indexBuffers[c] = SceneData.IndexBufferPool.GetBuffer(geom.LOD[c].IndexBufferRef.Hash);
                vertexBuffers[c] = SceneData.VertexBufferPool.GetBuffer(geom.LOD[c].VertexBufferRef.Hash);
            }

            if (indexBuffers[0] == null || vertexBuffers[0] == null)
            {
                return null;
            }
            RenderModel model = new RenderModel();
            model.ConvertFrameToRenderModel(Mesh, geom, mat, indexBuffers, vertexBuffers);
            return model;
        }

        // TODO: Rubbish, redo. Old code.
        public static RenderStaticCollision BuildRenderItemDesc(ulong refID)
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
        }
    }
}
