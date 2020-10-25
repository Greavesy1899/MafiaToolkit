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

        private Plane[] frustumPlanes = new Plane[6];

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

        public void ContructFrustum(bool bNormalise = false)
        {
            Matrix inverse = Matrix.Invert(ProjectionMatrix);

            // Left clipping plane
            Plane LeftPlane = new Plane(ProjectionMatrix.M14 + ProjectionMatrix.M11, ProjectionMatrix.M24 + ProjectionMatrix.M21, ProjectionMatrix.M34 + ProjectionMatrix.M31, ProjectionMatrix.M44 + ProjectionMatrix.M41);
            frustumPlanes[0] = LeftPlane;

            // Right clipping plane
            Plane RightPlane = new Plane(ProjectionMatrix.M14 + ProjectionMatrix.M11, ProjectionMatrix.M24 + ProjectionMatrix.M21, ProjectionMatrix.M34 + ProjectionMatrix.M31, ProjectionMatrix.M44 + ProjectionMatrix.M41);
            frustumPlanes[1] = RightPlane;

            // Top clipping plane
            Plane TopPlane = new Plane(ProjectionMatrix.M14 + ProjectionMatrix.M12, ProjectionMatrix.M24 + ProjectionMatrix.M22, ProjectionMatrix.M34 + ProjectionMatrix.M32, ProjectionMatrix.M44 + ProjectionMatrix.M42);
            frustumPlanes[2] = TopPlane;

            // Bottom clipping plane
            Plane BottomPlane = new Plane(ProjectionMatrix.M14 + ProjectionMatrix.M12, ProjectionMatrix.M24 + ProjectionMatrix.M22, ProjectionMatrix.M34 + ProjectionMatrix.M32, ProjectionMatrix.M44 + ProjectionMatrix.M42);
            frustumPlanes[3] = BottomPlane;

            // Near clipping plane
            Plane NearPlane = new Plane(ProjectionMatrix.M13, ProjectionMatrix.M23, ProjectionMatrix.M33, ProjectionMatrix.M43);
            frustumPlanes[4] = NearPlane;

            // Far clipping plane
            Plane FarPlane = new Plane(ProjectionMatrix.M14 - ProjectionMatrix.M13, ProjectionMatrix.M24 - ProjectionMatrix.M23, ProjectionMatrix.M34 - ProjectionMatrix.M33, ProjectionMatrix.M44 - ProjectionMatrix.M44);
            frustumPlanes[5] = FarPlane;

            // Normalise if asked too.
            if (bNormalise)
            {
                for (int i = 0; i < frustumPlanes.Length; i++)
                {
                    Vector3 Normal = frustumPlanes[i].Normal;
                    float Mag = (float)Math.Sqrt(Normal.X * Normal.X + Normal.Y * Normal.Y + Normal.Z * Normal.Z);

                    Normal.X /= Mag;
                    Normal.Y /= Mag;
                    Normal.Z /= Mag;

                    frustumPlanes[i].Normal = Normal;
                }
            }
        }

        private float Frustum_DistanceToPoint(Plane ClippingPlane, Vector3 Position)
        {
            Vector3 Normal = ClippingPlane.Normal;
            return Normal.X * Position.X + Normal.Y * Position.Y + Normal.Z * Position.Z + ClippingPlane.D;
        }

        private int Frustum_ClassifyPoint(Plane ClippingPlane, Vector3 Position)
        {
            float Distance = Frustum_DistanceToPoint(ClippingPlane, Position);
            Console.Write(Distance);

            if(Distance < 0.0f)
            {
                // Negative
                return -1;
            }
            else if(Distance > 0.0f)
            {
                // Position
                return 1;
            }
            else
            {
                // On Plane
                return 0;
            }
        }

        public bool CheckBBoxFrustum(Vector3 Position, BoundingBox box)
        {
            bool result = false;
            for (int i = 0; i < 6; i++) 
            {
                if(result)
                {
                    return true;
                }

                int Result = Frustum_ClassifyPoint(frustumPlanes[i], Position + box.Center);
                result = Result == 1;
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
            ContructFrustum();
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
