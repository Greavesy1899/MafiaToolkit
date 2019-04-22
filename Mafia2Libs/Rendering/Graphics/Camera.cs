using Mafia2Tool;
using SharpDX;
using System;
using System.Diagnostics;
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
        private Vector2 mousePosition;

        public Camera()
        {
            Right = new Vector3(1, 0, 0);
            Up = new Vector3(0, 1, 0);
            Look = new Vector3(0, 0, 1);
            ViewMatrix = Matrix.Identity;
            ProjectionMatrix = Matrix.Identity;
            mousePosition = new Vector2();
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

        public void ContructFrustrum()
        {
            Matrix matrix = ProjectionMatrix;
            float min = -matrix.M43 / -matrix.M33;
            float rev = ToolkitSettings.ScreenDepth / (ToolkitSettings.ScreenDepth - min);
            matrix.M33 = rev;
            matrix.M43 = -rev * min;

            matrix = ViewMatrix * matrix;

            //Planes[0] = Plane.Normalize(new Plane((vp.M14 + vp.M11), (vp.M24 + vp.M21), (vp.M34 + vp.M31), (vp.M44 + vp.M41)));
            //Planes[1] = Plane.Normalize(new Plane((vp.M14 - vp.M11), (vp.M24 - vp.M21), (vp.M34 - vp.M31), (vp.M44 - vp.M41)));
            //Planes[2] = Plane.Normalize(new Plane((vp.M14 - vp.M12), (vp.M24 - vp.M22), (vp.M34 - vp.M32), (vp.M44 - vp.M42)));
            //Planes[3] = Plane.Normalize(new Plane((vp.M14 + vp.M12), (vp.M24 + vp.M22), (vp.M34 + vp.M32), (vp.M44 + vp.M42)));
            //Planes[4] = Plane.Normalize(new Plane((vp.M13), (vp.M23), (vp.M33), 0.0f));//(vp.M43));
            //Planes[5] = Plane.Normalize(new Plane((vp.M14 - vp.M13), (vp.M24 - vp.M23), (vp.M34 - vp.M33), (vp.M44 - vp.M43)));

            frustrumPlanes[0] = new Plane(matrix.M14 + matrix.M13, matrix.M24 + matrix.M23, matrix.M34 + matrix.M33, matrix.M44 + matrix.M43);
            frustrumPlanes[0].Normalize();
            frustrumPlanes[1] = new Plane(matrix.M14 - matrix.M13, matrix.M24 - matrix.M23, matrix.M34 - matrix.M33, matrix.M44 - matrix.M43);
            frustrumPlanes[1].Normalize();
            frustrumPlanes[2] = new Plane(matrix.M14 + matrix.M11, matrix.M24 + matrix.M21, matrix.M34 + matrix.M31, matrix.M44 + matrix.M41);
            frustrumPlanes[2].Normalize();
            frustrumPlanes[3] = new Plane(matrix.M14 - matrix.M11, matrix.M24 - matrix.M21, matrix.M34 - matrix.M31, matrix.M44 - matrix.M41);
            frustrumPlanes[3].Normalize();
            frustrumPlanes[4] = new Plane(matrix.M14 - matrix.M12, matrix.M24 - matrix.M22, matrix.M34 - matrix.M32, matrix.M44 - matrix.M42);
            frustrumPlanes[4].Normalize();
            frustrumPlanes[5] = new Plane(matrix.M14 + matrix.M12, matrix.M24 + matrix.M22, matrix.M34 + matrix.M32, matrix.M44 + matrix.M42);
            frustrumPlanes[5].Normalize();
        }

        public bool CheckBBoxFrustrum(BoundingBox box)
        {
            for (int i = 0; i < 6; i++)
            {
                if (Plane.DotCoordinate(frustrumPlanes[i], new Vector3(box.Center.X - box.Size.X, box.Center.Y - box.Size.Y, box.Center.Z - box.Size.Z)) >= 0.0f)
                    continue;
                if (Plane.DotCoordinate(frustrumPlanes[i], new Vector3(box.Center.X + box.Size.X, box.Center.Y - box.Size.Y, box.Center.Z - box.Size.Z)) >= 0.0f)
                    continue;
                if (Plane.DotCoordinate(frustrumPlanes[i], new Vector3(box.Center.X - box.Size.X, box.Center.Y + box.Size.Y, box.Center.Z - box.Size.Z)) >= 0.0f)
                    continue;
                if (Plane.DotCoordinate(frustrumPlanes[i], new Vector3(box.Center.X - box.Size.X, box.Center.Y - box.Size.Y, box.Center.Z + box.Size.Z)) >= 0.0f)
                    continue;
                if (Plane.DotCoordinate(frustrumPlanes[i], new Vector3(box.Center.X + box.Size.X, box.Center.Y + box.Size.Y, box.Center.Z - box.Size.Z)) >= 0.0f)
                    continue;
                if (Plane.DotCoordinate(frustrumPlanes[i], new Vector3(box.Center.X + box.Size.X, box.Center.Y - box.Size.Y, box.Center.Z + box.Size.Z)) >= 0.0f)
                    continue;
                if (Plane.DotCoordinate(frustrumPlanes[i], new Vector3(box.Center.X - box.Size.X, box.Center.Y + box.Size.Y, box.Center.Z + box.Size.Z)) >= 0.0f)
                    continue;
                if (Plane.DotCoordinate(frustrumPlanes[i], new Vector3(box.Center.X + box.Size.X, box.Center.Y + box.Size.Y, box.Center.Z + box.Size.Z)) >= 0.0f)
                    continue;

                return false;
            }
            return true;
        }

        public void SetProjectionMatrix()
        {
            ProjectionMatrix = Matrix.PerspectiveFovRH((float)(Math.PI / 4), (ToolkitSettings.Width / ToolkitSettings.Height), ToolkitSettings.ScreenNear, ToolkitSettings.ScreenDepth);
        }

        public Ray GetPickingRay(Vector2 sp, Vector2 screenDims)
        {
            Matrix pm, v;
            pm = ProjectionMatrix;
            v = ViewMatrix;
            var vx = (2.0f * sp.X / screenDims.X - 1.0f) / pm.M11;
            var vy = (-2.0f * sp.Y / screenDims.Y + 1.0f) / pm.M22;
            var ray = new Ray(new Vector3(), new Vector3(vx, vy, 1.0f));
            var invView = Matrix.Invert(v);
            var toWorld = invView;
            ray = new Ray(Vector3.TransformCoordinate(ray.Position, toWorld), Vector3.TransformNormal(ray.Direction, toWorld));
            ray.Direction.Normalize();
            return ray;
        }

        public void UpdateMousePosition(Vector2 sp)
        {
            mousePosition.X = (sp.X / ToolkitSettings.Width) * 2.0f - 1.0f;
            mousePosition.Y = (sp.Y / ToolkitSettings.Height) * -2.0f + 1.0f;
        }

        public void Render()
        {
            UpdateViewMatrix();
            ContructFrustrum();
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
