using System;
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
        Render2DPlane[] lanes;

        public RenderRoad()
        {
            DoRender = true;
            Transform = Matrix.Identity;
            lanes = new Render2DPlane[0];
        }

        public void Init(ResourceTypes.Navigation.SplineProperties properties)
        {
            RenderLine line = null;

            if (properties.unk3 > 4096 && properties.unk3 < 4128)
            {
                line = RenderStorageSingleton.Instance.SplineStorage[properties.unk3 - 4096];
            }
            else if (properties.unk3 > 24576 && properties.unk3 < 25332)
            {
                line = RenderStorageSingleton.Instance.SplineStorage[properties.unk3 - 24576];
            }
            else if (properties.unk3 > 16384 && properties.unk3 < 16900)
            {
                line = RenderStorageSingleton.Instance.SplineStorage[properties.unk3 - 16384];
            }
            else if (properties.unk3 > 32768 && properties.unk3 < 36864)
            {
                line = RenderStorageSingleton.Instance.SplineStorage[properties.unk3 - 32768];
            }
            else if (properties.unk3 > 36864)
            {
                line = RenderStorageSingleton.Instance.SplineStorage[properties.unk3 - 36864];
            }
            else
            {
                line = RenderStorageSingleton.Instance.SplineStorage[properties.unk3];
            }
            //List editPoints = (Vector3[])line.Points.Clone();
            lanes = new Render2DPlane[0];
            Spline = line;
            //for (int i = 0; i != properties.laneSize0; i++)
            //{
            //    Render2DPlane lane = new Render2DPlane();
            //    lane.Init(ref editPoints, properties.lanes[i], properties.unk2);
            //    lanes[i] = lane;
            //}
        }

        public override void InitBuffers(Device d3d)
        {
            Spline.InitBuffers(d3d);

            foreach (Render2DPlane plane in lanes)
                plane.InitBuffers(d3d);
        }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera, LightClass light)
        {
            if (!DoRender)
                return;

            Spline.Render(device, deviceContext, camera, light);

            foreach (Render2DPlane plane in lanes)
                plane.Render(device, deviceContext, camera, light);
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
            Spline.Shutdown();

            foreach (Render2DPlane plane in lanes)
                plane.Shutdown();
        }

        public override void UpdateBuffers(DeviceContext device)
        {
            Spline.UpdateBuffers(device);

            foreach (Render2DPlane plane in lanes)
                plane.UpdateBuffers(device);
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
