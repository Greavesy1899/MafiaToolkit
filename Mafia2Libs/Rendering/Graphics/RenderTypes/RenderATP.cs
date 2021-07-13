﻿using ResourceTypes.Navigation;
using System.Numerics;
using Toolkit.Core;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public class RenderATP : IRenderer
    {
        public RenderBoundingBox BBox { get; set; }
        public RenderLine Path { get; set; }
        public AnimalTrafficLoader.AnimalTrafficPath ATP { get; set; }

        private int BBoxHandle = 0;
        private int PathHandle = 0;
        private GraphicsClass OwnGraphics;

        // TODO: Make OwnGraphics in the base class
        public RenderATP(GraphicsClass InGraphicsOwner)
        {
            DoRender = true;
            Transform = Matrix4x4.Identity;
            BBox = new RenderBoundingBox();
            Path = new RenderLine();
            OwnGraphics = InGraphicsOwner;
        }

        public void Init(AnimalTrafficLoader.AnimalTrafficPath path)
        {
            ATP = path;
            BBox.Init(path.BoundingBox);
           
            if (path.Vectors.Length > 1)
            {
                Vector3[] points = new Vector3[path.Vectors.Length];
                for (int i = 0; i != path.Vectors.Length; i++)
                {
                    points[i] = path.Vectors[i].Position;
                }

                Path.Init(points);
            }
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext)
        {
            if (Path.GetVertices.Length > 1)
            {
                BBox.InitBuffers(d3d, deviceContext);
                Path.InitBuffers(d3d, deviceContext);

                BBoxHandle = RefManager.GetNewRefID();
                //OwnGraphics.OurPrimitiveManager.PushPrimitiveObject(Core.PrimitiveType.Box, BBoxHandle, BBox);

                PathHandle = RefManager.GetNewRefID();
               // OwnGraphics.OurPrimitiveManager.PushPrimitiveObject(Core.PrimitiveType.Line, PathHandle, Path);
            }
        }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            if (DoRender != false)
            {
                //BBox.Render(device, deviceContext, camera);
                //Path.Render(device, deviceContext, camera);
            }
        }

        public override void Select()
        {
            //BBox.Select();
            //Path.Select();
        }
        public override void SetTransform(Matrix4x4 matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            BBox.Shutdown();
            Path.Shutdown();
        }

        public override void Unselect()
        {
            //BBox.Unselect();
            //Path.Unselect();
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            //BBox.UpdateBuffers(device, deviceContext);
            //Path.UpdateBuffers(device, deviceContext);
        }
    }
}
