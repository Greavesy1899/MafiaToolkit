using ResourceTypes.Navigation.Traffic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using Vortice.Direct3D11;
using Vortice.Mathematics;
using Color = System.Drawing.Color;

namespace Rendering.Graphics
{
    public class RenderRoad : IRenderer
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RenderLine Spline { get; set; }
        public BoundingBox BBox { get; set; }
        Render2DPlane[] Planes;

        public RenderRoad()
        {
            DoRender = true;
            Transform = Matrix4x4.Identity;
            Planes = new Render2DPlane[0];
            Spline = new RenderLine();
        }

        public void Init(IRoadDefinition RoadDefinition, IRoadSpline RoadSpline)
        {
            float LeftLanesWidth = 0.0f;
            for(int i = 0; i < RoadDefinition.OppositeLanesCount; i++)
            {
                LeftLanesWidth += RoadDefinition.Lanes[i].Width;
            }

            Planes = new Render2DPlane[RoadDefinition.Lanes.Count];
            float CurrentOffset = -LeftLanesWidth;
            for(int i = 0; i < RoadDefinition.Lanes.Count; i++)
            {
                ILaneDefinition LaneDefinition = RoadDefinition.Lanes[i];

                Color LaneColour = Color.White;
                switch (LaneDefinition.LaneType)
                {
                    case LaneType.MainRoad:
                        LaneColour = Color.Chartreuse;
                        break;
                    case LaneType.Byroad:
                        LaneColour = Color.Fuchsia;
                        break;
                    case LaneType.ExclImpassable:
                        LaneColour = Color.DimGray;
                        break;
                    case LaneType.EmptyRoad:
                        LaneColour = Color.Yellow;
                        break;
                    case LaneType.Parking:
                        LaneColour = Color.CornflowerBlue;
                        break;
                }

                float zOffset = RoadDefinition.Direction == RoadDirection.Backwards ? -0.5f : -1.0f;

                Vector3[] Points = RoadSpline.Points.Select(v => new Vector3(v.X, v.Y, v.Z + zOffset)).ToArray();
                if (RoadDefinition.Direction == RoadDirection.Towards)
                {               
                    Points = Points.Reverse().ToArray();
                }    

                Render2DPlane Lane = new Render2DPlane();
                Lane.Init(LaneDefinition, Points, LaneDefinition.Width, CurrentOffset, zOffset, LaneColour);
                Planes[i] = Lane;

                CurrentOffset += LaneDefinition.Width;
            }

            Spline = new RenderLine();
            Spline.SetUnselectedColour(System.Drawing.Color.White);
            Spline.Init(RoadSpline.Points.ToArray());
            BBox = BoundingBox.CreateFromPoints(RoadSpline.Points.ToArray());
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext context)
        {
            Spline.InitBuffers(d3d, context);

            foreach (Render2DPlane plane in Planes)
            {
                plane.InitBuffers(d3d, context);
            }
        }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            if (!DoRender)
            {
                return;
            }

            if (!camera.CheckBBoxFrustum(Transform, BoundingBox))
                return;

            Spline.Render(device, deviceContext, camera);

            foreach (Render2DPlane plane in Planes)
            {
                plane.Render(device, deviceContext, camera);
            }
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            Spline.Shutdown();

            foreach (Render2DPlane plane in Planes)
            {
                plane.Shutdown();
            }
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            Spline.UpdateBuffers(device, deviceContext);

            foreach (Render2DPlane plane in Planes)
            {
                plane.UpdateBuffers(device, deviceContext);
            }
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
