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
        public int IndexOffset { get; set; }
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
            Spline.SetUnselectedColour(System.Drawing.Color.White);
            Spline.Init(data.Points);
            Vector3[] editPoints = (Vector3[])data.Points.Clone();

            if (data.HasToward)
            {
                towardLanes = new Render2DPlane[data.Toward.LaneSize0];

                for (int i = 0; i != data.Toward.LaneSize0; i++)
                {
                    Render2DPlane lane = new Render2DPlane();
                    lane.Init(ref editPoints, data.Toward.Lanes[i], data.Toward.Flags);
                    towardLanes[i] = lane;
                }

                Toward = data.Toward;
                HasToward = data.HasToward;
            }

            editPoints = (Vector3[])data.Points.Clone();

            if (data.HasBackward)
            {
                backwardLanes = new Render2DPlane[data.Backward.LaneSize0];

                for (int i = 0; i != data.Backward.LaneSize0; i++)
                {
                    Render2DPlane lane = new Render2DPlane();
                    lane.Init(ref editPoints, data.Backward.Lanes[i], data.Backward.Flags);
                    backwardLanes[i] = lane;
                }

                Backward = data.Backward;
                HasBackward = data.HasBackward;
            }

            BBox = BoundingBox.FromPoints(editPoints);
            IndexOffset = data.IndexOffset;
        }

        public override void InitBuffers(Device d3d, DeviceContext context)
        {
            Spline.InitBuffers(d3d, context);

            foreach (Render2DPlane plane in towardLanes)
                plane.InitBuffers(d3d, context);

            foreach (Render2DPlane plane in backwardLanes)
                plane.InitBuffers(d3d, context);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
                return;

            Spline.Render(device, deviceContext, camera);

            foreach (Render2DPlane plane in towardLanes)
                plane.Render(device, deviceContext, camera);

            foreach (Render2DPlane plane in backwardLanes)
                plane.Render(device, deviceContext, camera);
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
