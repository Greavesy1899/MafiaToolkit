using ResourceTypes.Navigation;
using SharpDX;
using SharpDX.Direct3D11;
using Utils.StringHelpers;
using Utils.Types;

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
            Transform = Matrix.Identity;
            BBox = new RenderBoundingBox();
            Path = new RenderLine();

            OwnGraphics = InGraphicsOwner;
        }

        public void Init(AnimalTrafficLoader.AnimalTrafficPath path)
        {
            ATP = path;
            BBox.Init(path.BoundingBox);

            Vector3[] points = new Vector3[path.Vectors.Length];
            for(int i = 0; i != path.Vectors.Length; i++)
            {
                points[i] = path.Vectors[i].Position;
            }
            Path.Init(points);
        }

        public override void InitBuffers(Device d3d, DeviceContext deviceContext)
        {
            BBox.InitBuffers(d3d, deviceContext);
            Path.InitBuffers(d3d, deviceContext);

            BBoxHandle = StringHelpers.GetNewRefID();
            OwnGraphics.OurPrimitiveManager.PushPrimitiveObject(Core.PrimitiveType.Box, BBoxHandle, BBox);

            //PathHandle = StringHelpers.GetNewRefID();
            //OwnGraphics.OurPrimitiveManager.PushPrimitiveObject(Core.PrimitiveType.Line, PathHandle, Path);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            if (DoRender != false)
            {
                //BBox.Render(device, deviceContext, camera);
                Path.Render(device, deviceContext, camera);
            }
        }

        public override void Select()
        {
            //BBox.Select();
            Path.Select();
        }
        public override void SetTransform(Matrix matrix)
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
            Path.Unselect();
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            //BBox.UpdateBuffers(device, deviceContext);
            Path.UpdateBuffers(device, deviceContext);
        }
    }
}
