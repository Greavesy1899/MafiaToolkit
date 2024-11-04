using Rendering.Core;
using ResourceTypes.Navigation.Traffic;
using System.ComponentModel;
using System.Numerics;
using Toolkit.Core;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public class RenderJunction : IRenderer
    {
        RenderLine boundary;

        [Browsable(false)]
        public RenderLine Boundary {
            get { return boundary; }
            set { boundary = value; }
        }

        private ICrossroad OurData;
        private PrimitiveBatch SplineBatch;

        private GraphicsClass OwnerGraphics;

        public void Init(ICrossroad data, GraphicsClass InGraphics)
        {
            OurData = data;
            OwnerGraphics = InGraphics;

            SplineBatch = new PrimitiveBatch(PrimitiveType.Line, string.Format("RenderJunction_{0}", RefManager.GetNewRefID()));
            OwnerGraphics.OurPrimitiveManager.AddPrimitiveBatch(SplineBatch);

            InitBoundary();
            InitSplines();
            DoRender = true;
        }

        private void InitBoundary()
        {
            //if (data.Boundaries != null)
            //{
            //    //boundary init
            //    if (data.Boundaries.Length > 0)
            //    {
            //        Vector3[] extraPoints = new Vector3[data.Boundaries.Length + 1];
            //        Array.Copy(data.Boundaries, extraPoints, data.Boundaries.Length);
            //        extraPoints[extraPoints.Length - 1] = extraPoints[0];
            //        Boundary = new RenderLine();
            //        Boundary.SetSelectedColour(Color.Red);
            //        Boundary.SetUnselectedColour(Color.White);
            //        Boundary.Init(extraPoints);
            //    }
            //}
        }

        private void InitSplines()
        {
            for (int i = 0; i < OurData.Junctions.Count; i++)
            {
                IRoadJunction Junction = OurData.Junctions[i];

                RenderLine Spline = new RenderLine();
                Spline.Init(Junction.Spline.Points.ToArray());
                SplineBatch.AddObject(RefManager.GetNewRefID(), Spline);
            }
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext)
        {
            if (Boundary != null)
            {
                Boundary.InitBuffers(d3d, deviceContext);
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

            if (Boundary != null)
            {
                Boundary.Render(device, deviceContext, camera);
            }
        }

        public override void Select()
        {
            if (Boundary != null)
            {
                Boundary.Select();
            }
            bIsUpdatedNeeded = true;
        }

        public override void SetTransform(Matrix4x4 matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            if (Boundary != null)
            {
                Boundary.Shutdown();
            }
        }

        public override void Unselect()
        {
            if (Boundary != null)
            {
                Boundary.Unselect();
            }
            bIsUpdatedNeeded = true;
        }

        public void UpdateVertices()
        {
            if (boundary != null)
            {
                //boundary.Points = data.Boundaries;
            }
            else
            {
                InitBoundary();
            }

            InitSplines();
            bIsUpdatedNeeded = true;
        }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if(bIsUpdatedNeeded)
            {
                if (Boundary != null)
                {
                    Boundary.UpdateBuffers(device, deviceContext);
                }

                bIsUpdatedNeeded = false;
            }
        }
    }
}
