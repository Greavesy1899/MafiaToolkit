using System;
using System.Numerics;
using Toolkit.Mathematics;
using Utils.Settings;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public class Camera
    {
        public Vector3 Position = new Vector3(0);
        public Vector3 Rotation = new Vector3(0);
        public Matrix4x4 ViewMatrix { get; private set; }
        public Matrix4x4 ProjectionMatrix { get; private set; }
        public Matrix4x4 ViewMatrixTransposed { get; private set; }
        public Matrix4x4 ProjectionMatrixTransposed { get;  private set; }

        private Vector3 Look { get; set; }
        private Vector3 Right { get; set; }
        private Vector3 Up { get; set; }

        private Plane[] frustumPlanes = new Plane[6];

        public Camera()
        {
            Right = new Vector3(1, 0, 0);
            Up = new Vector3(0, 1, 0);
            Look = new Vector3(0, 0, 1);

            ViewMatrix = Matrix4x4.Identity;
            ProjectionMatrix = Matrix4x4.Identity;
            ViewMatrixTransposed = Matrix4x4.Identity;
            ProjectionMatrixTransposed = Matrix4x4.Identity;
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
            var v = new Matrix4x4();
            v.SetColumn(0, new Vector4(Right, x));
            v.SetColumn(1, new Vector4(Up, y));
            v.SetColumn(2, new Vector4(Look, z));
            v.SetColumn(3, new Vector4(0, 0, 0, 1));
            ViewMatrix = v;

            // Transose ViewMatrix
            ViewMatrixTransposed = Matrix4x4.Transpose(ViewMatrix);
        }

        public void ConstructFrustum(bool normalize = false)
        {
            // Combine the View and Projection matrices to get the ViewProjection matrix
            Matrix4x4 viewProjectionMatrix = ViewMatrix * ProjectionMatrix;

            // Extract the frustum planes from the ViewProjection matrix
            // Left clipping plane
            frustumPlanes[0] = new Plane(
                viewProjectionMatrix.M14 + viewProjectionMatrix.M11,
                viewProjectionMatrix.M24 + viewProjectionMatrix.M21,
                viewProjectionMatrix.M34 + viewProjectionMatrix.M31,
                viewProjectionMatrix.M44 + viewProjectionMatrix.M41
            );

            // Right clipping plane
            frustumPlanes[1] = new Plane(
                viewProjectionMatrix.M14 - viewProjectionMatrix.M11,
                viewProjectionMatrix.M24 - viewProjectionMatrix.M21,
                viewProjectionMatrix.M34 - viewProjectionMatrix.M31,
                viewProjectionMatrix.M44 - viewProjectionMatrix.M41
            );

            // Top clipping plane
            frustumPlanes[2] = new Plane(
                viewProjectionMatrix.M14 - viewProjectionMatrix.M12,
                viewProjectionMatrix.M24 - viewProjectionMatrix.M22,
                viewProjectionMatrix.M34 - viewProjectionMatrix.M32,
                viewProjectionMatrix.M44 - viewProjectionMatrix.M42
            );

            // Bottom clipping plane
            frustumPlanes[3] = new Plane(
                viewProjectionMatrix.M14 + viewProjectionMatrix.M12,
                viewProjectionMatrix.M24 + viewProjectionMatrix.M22,
                viewProjectionMatrix.M34 + viewProjectionMatrix.M32,
                viewProjectionMatrix.M44 + viewProjectionMatrix.M42
            );

            // Near clipping plane
            frustumPlanes[4] = new Plane(
                viewProjectionMatrix.M13,
                viewProjectionMatrix.M23,
                viewProjectionMatrix.M33,
                viewProjectionMatrix.M43
            );

            // Far clipping plane
            frustumPlanes[5] = new Plane(
                viewProjectionMatrix.M14 - viewProjectionMatrix.M13,
                viewProjectionMatrix.M24 - viewProjectionMatrix.M23,
                viewProjectionMatrix.M34 - viewProjectionMatrix.M33,
                viewProjectionMatrix.M44 - viewProjectionMatrix.M43
            );

            // Normalize the planes if requested
            if (normalize)
            {
                for (int i = 0; i < frustumPlanes.Length; i++)
                {
                    Vector3 normal = frustumPlanes[i].Normal;
                    float magnitude = normal.Length();

                    if (magnitude > 0)
                    {
                        frustumPlanes[i].Normal = normal / magnitude;
                        frustumPlanes[i].D /= magnitude;
                    }
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

        public bool CheckBBoxFrustum(Matrix4x4 Transform, BoundingBox box)
        {
            Vector3[] corners = box.GetCorners();

            for (int i = 0; i < 6; i++)
            {
                bool allOutside = true;
                for (int j = 0; j < corners.Length; j++)
                {
                    if (Frustum_ClassifyPoint(frustumPlanes[i], Vector3.Transform(corners[j], Transform)) >= 0)
                    {
                        allOutside = false;
                        break;
                    }
                }
                if (allOutside)
                {
                    return false; // The box is completely outside of this plane.
                }
            }
            return true; // The box is at least partially inside all planes.
        }


        public void SetProjectionMatrix(int width, int height)
        {
            width = (width == 0 ? 1024 : width);
            height = (height == 0 ? 768 : height);
            float aspectRatio = (float)width / (float)height;
            ProjectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.ToRadians(ToolkitSettings.FieldOfView), aspectRatio, ToolkitSettings.ScreenNear, ToolkitSettings.ScreenDepth);

            // Transpose Projection Matrix
            ProjectionMatrixTransposed = Matrix4x4.Transpose(ProjectionMatrix);
        }

        public Ray GetPickingRay(Vector2 pos, Vector2 screenDims)
        {
            Matrix4x4 InvertedViewProj = Matrix4x4.Identity;
            Matrix4x4.Invert(ViewMatrix * ProjectionMatrix, out InvertedViewProj);
            var farPoint = Vector3Utils.TransformCoordinate(
                new Vector3(2.0f * pos.X / screenDims.X - 1.0f, -2.0f * pos.Y / screenDims.Y + 1.0f, 1f),
                InvertedViewProj
            );

            Vector3 NormalisedDir = Vector3.Normalize(farPoint - Position);
            var ray = new Ray(Position, NormalisedDir);
            return ray;
        }

        public void Render()
        {
            UpdateViewMatrix();
            ConstructFrustum();
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
            angle = MathHelper.ToRadians(angle);
            Rotation.X = angle;
            var r = Matrix4x4.CreateFromAxisAngle(Right, angle);
            Up = Vector3.TransformNormal(Up, r);
            Look = Vector3.TransformNormal(Look, r);
        }

        public void Yaw(float angle)
        {
            angle = MathHelper.ToRadians(angle);
            Rotation.Y = angle;
            var r = Matrix4x4.CreateRotationZ(angle);
            Right = Vector3.TransformNormal(Right, r);
            Up = Vector3.TransformNormal(Up, r);
            Look = Vector3.TransformNormal(Look, r);
        }
    }
}
