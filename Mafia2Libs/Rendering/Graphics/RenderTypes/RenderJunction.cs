using System;
using System.ComponentModel;
using ResourceTypes.Navigation;
using SharpDX;
using SharpDX.Direct3D11;
using Utils.Types;

namespace Rendering.Graphics
{
    public class RenderJunction : IRenderer
    {
        RenderLine[] Splines;
        RenderLine Boundary;
        JunctionDefinition data;

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public JunctionDefinition Data {
            get { return data; }
            set { data = value; }
        }

        public void Init(JunctionDefinition data)
        {
            if (data.Boundaries != null)
            {
                //boundary init
                if (data.Boundaries.Length > 0)
                {
                    Vector3[] extraPoints = new Vector3[data.Boundaries.Length + 1];
                    Array.Copy(data.Boundaries, extraPoints, data.Boundaries.Length);
                    extraPoints[extraPoints.Length - 1] = extraPoints[0];
                    Boundary = new RenderLine();
                    Boundary.SetSelectedColour(new Vector4(1.0f, 0.0f, 0.0f, 1.0f));
                    Boundary.SetUnselectedColour(new Vector4(1.0f));
                    Boundary.Init(extraPoints);
                }
            }

            if (data.Splines != null)
            {
                //do spline
                Splines = new RenderLine[data.Splines.Length];
                for (int i = 0; i < data.Splines.Length; i++)
                {
                    RenderLine line = new RenderLine();
                    line.SetUnselectedColour(new Vector4(1.0f, 0.87f, 0f, 1.0f));
                    line.Init(data.Splines[i].Path);
                    Splines[i] = line;
                }
            }
            DoRender = true;
            this.data = data;
        }

        public override void InitBuffers(Device d3d, DeviceContext deviceContext)
        {
            if (Boundary != null)
                Boundary.InitBuffers(d3d, deviceContext);

            if (Splines != null)
            {
                foreach (var spline in Splines)
                    spline.InitBuffers(d3d, deviceContext);
            }
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            if (Boundary != null)
                Boundary.Render(device, deviceContext, camera, light);

            if (Splines != null)
            {
                foreach (var spline in Splines)
                    spline.Render(device, deviceContext, camera, light);
            }
        }

        public override void Select()
        {
            if (Boundary != null)
                Boundary.Select();
            isUpdatedNeeded = true;
        }

        public override void SetTransform(Vector3 position, Matrix33 rotation)
        {
            Matrix m_trans = Matrix.Identity;
            m_trans[0, 0] = rotation.M00;
            m_trans[0, 1] = rotation.M01;
            m_trans[0, 2] = rotation.M02;
            m_trans[1, 0] = rotation.M10;
            m_trans[1, 1] = rotation.M11;
            m_trans[1, 2] = rotation.M12;
            m_trans[2, 0] = rotation.M20;
            m_trans[2, 1] = rotation.M21;
            m_trans[2, 2] = rotation.M22;
            m_trans[3, 0] = position.X;
            m_trans[3, 1] = position.Y;
            m_trans[3, 2] = position.Z;
            Transform = m_trans;
        }

        public override void SetTransform(Matrix matrix)
        {
            this.Transform = matrix;
        }

        public override void Shutdown()
        {
            if (Boundary != null)
                Boundary.Shutdown();

            if (Splines != null)
            {
                foreach (var spline in Splines)
                    spline.Shutdown();
            }
        }

        public override void Unselect()
        {
            if (Boundary != null)
                Boundary.Unselect();
            isUpdatedNeeded = true;
        }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext)
        {
            if(isUpdatedNeeded)
            {
                if(Boundary != null)
                    Boundary.UpdateBuffers(device, deviceContext);

                if (Splines != null)
                {
                    foreach (var spline in Splines)
                        spline.UpdateBuffers(device, deviceContext);
                }

                isUpdatedNeeded = false;
            }
        }
    }
}
