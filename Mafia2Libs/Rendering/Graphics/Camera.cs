using Mafia2Tool;
using SharpDX;
using System;
using Utils.Settings;

namespace Rendering.Graphics
{
    public class Camera
    {
        public Vector3 Position = new Vector3(0);
        public Vector3 Rotation = new Vector3(0);
        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }

        private Vector3 Look { get; set; }
        private Vector3 Right { get; set; }
        private Vector3 Up { get; set; }

        private Plane[] frustrumPlanes = new Plane[6];

        public Camera()
        {
            Right = new Vector3(1, 0, 0);
            Up = new Vector3(0, 1, 0);
            Look = new Vector3(0, 0, 1);
            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.Identity;
        }

        public void UpdateViewMatrix()
        {
            var r = Right;
            var u = Up;
            var l = Look;
            var p = Position;
            l = Vector3.Normalize(l);
            u = Vector3.Normalize(Vector3.Cross(l, r));
            r = Vector3.Cross(u, l);
            var x = -Vector3.Dot(p, r);
            var y = -Vector3.Dot(p, u);
            var z = -Vector3.Dot(p, l);
            Right = r;
            Up = u;
            Look = l;
            var v = new Matrix();
            v.Column1 = new Vector4(Right, x);
            v.Column2 = new Vector4(Up, y);
            v.Column3 = new Vector4(Look, z);
            v.Column4 = new Vector4(0, 0, 0, 1);
            ViewMatrix = v;
        }

        public void ContructFrustrum()
        {
            Matrix inverse = Matrix.Invert(ProjectionMatrix);

            frustrumPlanes[0] = Plane.Normalize(Plane.Transform(new Plane(0.0f, 0.0f, 1.0f, 0.0f), inverse));
            frustrumPlanes[1] = Plane.Normalize(Plane.Transform(new Plane(0.0f, 0.0f, -1.0f, 1.0f), inverse));
            frustrumPlanes[2] = Plane.Normalize(Plane.Transform(new Plane(1.0f, 0.0f, 0.0f, 1.0f), inverse));
            frustrumPlanes[3] = Plane.Normalize(Plane.Transform(new Plane(-1.0f, 0.0f, 0.0f, 1.0f), inverse));
            frustrumPlanes[4] = Plane.Normalize(Plane.Transform(new Plane(0.0f, -1.0f, 0.0f, 1.0f), inverse));
            frustrumPlanes[5] = Plane.Normalize(Plane.Transform(new Plane(0.0f, 1.0f, 0.0f, 1.0f), inverse));

        }

        public bool CheckBBoxFrustrum(BoundingBox box)
        {
            bool result = false;
            for (int i = 0; i < 6; i++) 
            {
                float distance = Vector3.Distance(box.Maximum, frustrumPlanes[i].Normal);
                float distance2 = Vector3.Distance(box.Minimum, frustrumPlanes[i].Normal);

                if(distance < 0 || distance2 < 0)
                {
                    return true;
                }
            }
            return result;
        }

        public void SetProjectionMatrix(int width, int height)
        {
            width = (width == 0 ? 1024 : width);
            height = (height == 0 ? 768 : height);
            float aspectRatio = (float)width / (float)height;
            ProjectionMatrix = Matrix.PerspectiveFovRH(MathUtil.DegreesToRadians(ToolkitSettings.FieldOfView), aspectRatio, ToolkitSettings.ScreenNear, ToolkitSettings.ScreenDepth);
        }

        public Ray GetPickingRay(Vector2 pos, Vector2 screenDims)
        {
            var invViewProj = Matrix.Invert(ViewMatrix * ProjectionMatrix);
            var farPoint = Vector3.TransformCoordinate(
                new Vector3(2.0f * pos.X / screenDims.X - 1.0f, -2.0f * pos.Y / screenDims.Y + 1.0f, 1f),
                invViewProj
            );
            var direction = farPoint - Position;
            direction.Normalize();
            var ray = new Ray(Position, direction);
            return ray;
        }

        public void Render()
        {
            UpdateViewMatrix();
            ContructFrustrum();
        }

        public void SetRotation(float pitch, float yaw, float roll = 0.0f)
        {
            Right = new Vector3(1, 0, 0);
            Up = new Vector3(0, 1, 0);
            Look = new Vector3(0, 0, 1);
            Pitch(pitch);
            Yaw(yaw);
        }

        public void Pitch(float angle)
        {
            angle = MathUtil.DegreesToRadians(angle);
            Rotation.X = angle;
            var r = Matrix.RotationAxis(Right, angle);
            Up = Vector3.TransformNormal(Up, r);
            Look = Vector3.TransformNormal(Look, r);
        }

        public void Yaw(float angle)
        {
            angle = MathUtil.DegreesToRadians(angle);
            Rotation.Y = angle;
            var r = Matrix.RotationZ(angle);
            Right = Vector3.TransformNormal(Right, r);
            Up = Vector3.TransformNormal(Up, r);
            Look = Vector3.TransformNormal(Look, r);
        }
    }
}
