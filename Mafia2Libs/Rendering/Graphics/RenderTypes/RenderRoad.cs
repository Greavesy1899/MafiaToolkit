using ResourceTypes.Navigation;
using System.ComponentModel;
using SharpDX;
using SharpDX.Direct3D11;
using Utils.Types;

namespace Rendering.Graphics
{
    public class RenderRoad : IRenderer
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RenderLine Spline { get; set; }
        public BoundingBox BBox { get; set; }
        Render2DPlane[] towardLanes;
        Render2DPlane[] backwardLanes;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SplineProperties Toward { get { return toward; } set { toward = value; } }
        public bool HasToward;
        private SplineProperties toward;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SplineProperties Backward { get { return backward; } set { backward = value; } }
        public bool HasBackward;
        private SplineProperties backward;

        public RenderRoad()
        {
            DoRender = true;
            Transform = Matrix.Identity;
            towardLanes = new Render2DPlane[0];
            backwardLanes = new Render2DPlane[0];
            Spline = new RenderLine();
        }

        public void Init(SplineDefinition data)
        {
            Spline.SetUnselectedColour(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
            Spline.Init(data.points);
            Vector3[] editPoints = (Vector3[])data.points.Clone();

            if (data.hasToward)
            {
                towardLanes = new Render2DPlane[data.toward.LaneSize0];

                for (int i = 0; i != data.toward.LaneSize0; i++)
                {
                    Render2DPlane lane = new Render2DPlane();
                    lane.Init(ref editPoints, data.toward.Lanes[i], data.toward.Flags);
                    towardLanes[i] = lane;
                }

                Toward = data.toward;
                HasToward = data.hasToward;
            }

            editPoints = (Vector3[])data.points.Clone();

            if (data.hasBackward)
            {
                backwardLanes = new Render2DPlane[data.backward.LaneSize0];

                for (int i = 0; i != data.backward.LaneSize0; i++)
                {
                    Render2DPlane lane = new Render2DPlane();
                    lane.Init(ref editPoints, data.backward.Lanes[i], data.backward.Flags);
                    backwardLanes[i] = lane;
                }

                Backward = data.backward;
                HasBackward = data.hasBackward;
            }

            BBox = BoundingBox.FromPoints(editPoints);
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            Spline.InitBuffers(d3d, context);

            foreach (Render2DPlane plane in towardLanes)
                plane.InitBuffers(d3d, context);

            foreach (Render2DPlane plane in backwardLanes)
                plane.InitBuffers(d3d, context);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            Spline.Render(device, deviceContext, camera, light);

            foreach (Render2DPlane plane in towardLanes)
                plane.Render(device, deviceContext, camera, light);

            foreach (Render2DPlane plane in backwardLanes)
                plane.Render(device, deviceContext, camera, light);
        }

        public override void SetTransform(Matrix matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            Spline.Shutdown();

            foreach (Render2DPlane plane in towardLanes)
                plane.Shutdown();

            foreach (Render2DPlane plane in backwardLanes)
                plane.Shutdown();
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            Spline.UpdateBuffers(device, deviceContext);

            foreach (Render2DPlane plane in towardLanes)
                plane.UpdateBuffers(device, deviceContext);

            foreach (Render2DPlane plane in backwardLanes)
                plane.UpdateBuffers(device, deviceContext);
        }

        public override void Select()
        {
            Spline.Select();
        }

        public override void Unselect()
        {
            Spline.Unselect();
        }
    }
}
