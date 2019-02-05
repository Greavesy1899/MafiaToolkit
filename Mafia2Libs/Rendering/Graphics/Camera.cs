using Mafia2Tool;
using SharpDX;
using System;
using System.Diagnostics;

namespace Rendering.Graphics
{
    public class Camera
    {
        public Vector3 Position = new Vector3(0);
        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        private float RotationX { get; set; }
        private float RotationY { get; set; }
        private float RotationZ { get; set; }

        private Vector3 Look { get; set; }
        private Vector3 Right { get; set; }
        private Vector3 Up { get; set; }

        public Camera()
        {
            Right = new Vector3(1, 0, 0);
            Up = new Vector3(0, 1, 0);
            Look = new Vector3(0, 0, 1);
            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.Identity;
        }

        public void LookAt(Vector3 pos, Vector3 target, Vector3 up)
        {
            Position = pos;
            Look = Vector3.Normalize(target - pos);
            Right = Vector3.Normalize(Vector3.Cross(up, Look));
            Up = Vector3.Cross(Look, Right);
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
            v[0, 0] = Right.X;
            v[1, 0] = Right.Y;
            v[2, 0] = Right.Z;
            v[3, 0] = x;

            v[0, 1] = Up.X;
            v[1, 1] = Up.Y;
            v[2, 1] = Up.Z;
            v[3, 1] = y;

            v[0, 2] = Look.X;
            v[1, 2] = Look.Y;
            v[2, 2] = Look.Z;
            v[3, 2] = z;

            v[0, 3] = v[1, 3] = v[2, 3] = 0;
            v[3, 3] = 1;

            ViewMatrix = v;
        }

        public void SetProjectionMatrix()
        {
            ProjectionMatrix = Matrix.PerspectiveFovRH((float)(Math.PI / 4), (ToolkitSettings.Width / ToolkitSettings.Height), ToolkitSettings.ScreenNear, ToolkitSettings.ScreenDepth); ;
        }

        public Ray GetPickingRay(Vector2 sp, Vector2 screenDims, Matrix Proj)
        {
            var p = Proj;
            // convert screen pixel to view space
            var vx = (2.0f * sp.X / screenDims.X - 1.0f) / p.M11;
            var vy = (-2.0f * sp.Y / screenDims.Y + 1.0f) / p.M22;

            var ray = new Ray(new Vector3(), new Vector3(vx, vy, 1.0f));
            var v = ViewMatrix;
            var invView = Matrix.Invert(v);


            var toWorld = invView;

            ray = new Ray(Vector3.TransformCoordinate(ray.Position, toWorld), Vector3.TransformNormal(ray.Direction, toWorld));

            ray.Direction.Normalize();
            return ray;
        }

        public void Render()
        {
            UpdateViewMatrix();
        }

        public void Pitch(float angle)
        {
            angle *= 0.0174532925f;
            var r = Matrix.RotationAxis(Right, angle);
            Up = Vector3.TransformNormal(Up, r);
            Look = Vector3.TransformNormal(Look, r);
        }

        public void Yaw(float angle)
        {
            angle *= 0.0174532925f;
            var r = Matrix.RotationZ(angle);
            Right = Vector3.TransformNormal(Right, r);
            Up = Vector3.TransformNormal(Up, r);
            Look = Vector3.TransformNormal(Look, r);
        }
    }
}
