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
            Matrix vp = ProjectionMatrix;
            float min = -vp.M43 / -vp.M33;
            float rev = ToolkitSettings.ScreenDepth / (ToolkitSettings.ScreenDepth - min);
            vp.M33 = rev;
            vp.M43 = -rev * min;

            vp = ViewMatrix * vp;

            frustrumPlanes[0] = Plane.Normalize(new Plane((vp.M14 + vp.M11), (vp.M24 + vp.M21), (vp.M34 + vp.M31), (vp.M44 + vp.M41)));
            frustrumPlanes[1] = Plane.Normalize(new Plane((vp.M14 - vp.M11), (vp.M24 - vp.M21), (vp.M34 - vp.M31), (vp.M44 - vp.M41)));
            frustrumPlanes[2] = Plane.Normalize(new Plane((vp.M14 - vp.M12), (vp.M24 - vp.M22), (vp.M34 - vp.M32), (vp.M44 - vp.M42)));
            frustrumPlanes[3] = Plane.Normalize(new Plane((vp.M14 + vp.M12), (vp.M24 + vp.M22), (vp.M34 + vp.M32), (vp.M44 + vp.M42)));
            frustrumPlanes[4] = Plane.Normalize(new Plane((vp.M13), (vp.M23), (vp.M33), 0.0f));//(vp.M43));
            frustrumPlanes[5] = Plane.Normalize(new Plane((vp.M14 - vp.M13), (vp.M24 - vp.M23), (vp.M34 - vp.M33), (vp.M44 - vp.M43)));

            //frustrumPlanes[0] = new Plane(matrix.M14 + matrix.M13, matrix.M24 + matrix.M23, matrix.M34 + matrix.M33, matrix.M44 + matrix.M43);
            //frustrumPlanes[0].Normalize();
            //frustrumPlanes[1] = new Plane(matrix.M14 - matrix.M13, matrix.M24 - matrix.M23, matrix.M34 - matrix.M33, matrix.M44 - matrix.M43);
            //frustrumPlanes[1].Normalize();
            //frustrumPlanes[2] = new Plane(matrix.M14 + matrix.M11, matrix.M24 + matrix.M21, matrix.M34 + matrix.M31, matrix.M44 + matrix.M41);
            //frustrumPlanes[2].Normalize();
            //frustrumPlanes[3] = new Plane(matrix.M14 - matrix.M11, matrix.M24 - matrix.M21, matrix.M34 - matrix.M31, matrix.M44 - matrix.M41);
            //frustrumPlanes[3].Normalize();
            //frustrumPlanes[4] = new Plane(matrix.M14 - matrix.M12, matrix.M24 - matrix.M22, matrix.M34 - matrix.M32, matrix.M44 - matrix.M42);
            //frustrumPlanes[4].Normalize();
            //frustrumPlanes[5] = new Plane(matrix.M14 + matrix.M12, matrix.M24 + matrix.M22, matrix.M34 + matrix.M32, matrix.M44 + matrix.M42);
            //frustrumPlanes[5].Normalize();
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

        public void SetProjectionMatrix(int width, int height)
        {
            if(width == 0 || height == 0)
            {
                width = 1024;
                height = 768;
            }
            ProjectionMatrix = Matrix.PerspectiveFovRH(MathUtil.DegreesToRadians(ToolkitSettings.FieldOfView), (width / height), ToolkitSettings.ScreenNear, ToolkitSettings.ScreenDepth);
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

        public Ray GetPickingRay(int x, int y, int height, int width, Matrix wMatrix)
        {
            Matrix matrix = wMatrix * ProjectionMatrix * ViewMatrix;
            Ray ray = Ray.GetPickRay(x, y, new ViewportF(0, 0, width, height, ToolkitSettings.ScreenNear, ToolkitSettings.ScreenDepth), matrix);
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
