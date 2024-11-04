using Rendering.Graphics;
using System;
using System.Numerics;
using Utils.VorticeUtils;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Core
{
    class GizmoTool
    {
        // General variable to whether the user is moving it.
        private bool bIsActive;

        // Variables for rendering
        private RenderModel GizmoModel;

        // Variables when calculating the delta
        private Vector3 PreviousIntersection;
        private Vector3 CurrentIntersection;


        public GizmoTool(RenderModel InModel)
        {
            GizmoModel = InModel;
            PreviousIntersection = Vector3.Zero;
            CurrentIntersection = Vector3.Zero;
            bIsActive = false;
        }

        public void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            GizmoModel.InitBuffers(d3d, d3dContext,null);
        }

        public void UpdateBuffers(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            GizmoModel.UpdateBuffers(d3d, d3dContext);
        }

        public void Render(ID3D11Device d3d, ID3D11DeviceContext d3dContext, Camera camera)
        {
            GizmoModel.Render(d3d, d3dContext, camera);
        }
        
        // TODO: Consider this as an event?
        public void OnSelectEntry(Matrix4x4 newTransform, bool bDoRender)
        {
            GizmoModel.SetTransform(newTransform);
            GizmoModel.DoRender = bDoRender;
        }

        public void Shutdown()
        {
            GizmoModel.Shutdown();
        }

        public void Activate()
        {
            bIsActive = true;
        }

        public void Deactivate()
        {
            bIsActive = false;
        }

        public void ManipulateGizmo(Camera Camera, int PointX, int PointY, int Width, int Height)
        {
            Activate();
            if(!bIsActive || !GizmoModel.DoRender)
            {
                return;
            }

            Vector3 WorldPosition = Vector3.Zero;
            Ray CameraRay = Camera.GetPickingRay(new Vector2(PointX, PointY), new Vector2(Width, Height));
            int MaterialID = GetMaterial(ref WorldPosition, CameraRay);
            CurrentIntersection = WorldPosition;

            // Do Stuff
            Console.WriteLine("Current: " + CurrentIntersection);
            Console.WriteLine("Previous: " + PreviousIntersection);
            Vector3 delta = CurrentIntersection - PreviousIntersection;

            if (CurrentIntersection != Vector3.Zero)
            {
                if (delta != Vector3.Zero)
                {
                    Matrix4x4 transform = GizmoModel.Transform;
                    transform.Translation += delta;
                    GizmoModel.SetTransform(transform);
                }
            }

            PreviousIntersection = CurrentIntersection;
        }

        private int GetMaterial(ref Vector3 WorldPosition, Ray CameraRay)
        {
            var lowest = float.MaxValue;
            var materialID = -1;
            
            Matrix4x4 InvertedWM = Matrix4x4.Identity;
            Matrix4x4.Invert(GizmoModel.Transform, out InvertedWM);
            var localRay = new Ray(
                Vector3Utils.TransformCoordinate(CameraRay.Position, InvertedWM),
                Vector3.TransformNormal(CameraRay.Direction, InvertedWM)
            );

            var bbox = GizmoModel.BoundingBox;

            if (localRay.Intersects(bbox) > 0.0f)
            {
                return -1;
            }

            for(var i = 0; i < GizmoModel.LODs[0].ModelParts.Length; i++)
            {
                var ModelPart = GizmoModel.LODs[0].ModelParts[i];
                var StartIndex = ModelPart.StartIndex / 3;
                for (var x = StartIndex; x < StartIndex + ModelPart.NumFaces; x++)
                {
                    var v0 = GizmoModel.LODs[0].Vertices[GizmoModel.LODs[0].Indices[x * 3]].Position;
                    var v1 = GizmoModel.LODs[0].Vertices[GizmoModel.LODs[0].Indices[x * 3 + 1]].Position;
                    var v2 = GizmoModel.LODs[0].Vertices[GizmoModel.LODs[0].Indices[x * 3 + 2]].Position;
                    float t = 1.0f;

                    /*if (!localRay.Intersects(v0, v1, v2, out t))
                    {
                        continue;
                    }*/

                    WorldPosition = CameraRay.Position + t * CameraRay.Direction;
                    var distance = (WorldPosition - CameraRay.Position).LengthSquared();
                    if (distance < lowest)
                    {
                        lowest = distance;
                        materialID = i;
                    }
                }
            }

            return materialID;
        }
    }
}
