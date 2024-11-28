using Rendering.Core;
using ResourceTypes.Navigation;
using System.Collections.Generic;
using System.Numerics;
using Toolkit.Core;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public class RenderNav : IRenderer
    {
        private OBJData data = null;

        private GraphicsClass OwnGraphics;

        // Variables related to the Path Vertices
        private PrimitiveBatch PathVertexBatch = null;
        private List<RenderBoundingBox> BoundingBoxes;
        private int SelectedIndex;

        // Variables relating to connections between points
        private PrimitiveBatch PointConnectionsBatch = null;
        private List<RenderLine> ConnectionsList;

        public RenderNav(GraphicsClass InGraphicsClass)
        {
            OwnGraphics = InGraphicsClass;

            SelectedIndex = -1;
            BoundingBoxes = new List<RenderBoundingBox>();

            ConnectionsList = new List<RenderLine>();

            string ConnectionList = string.Format("NavConnectionList_{0}", RefManager.GetNewRefID());
            PointConnectionsBatch = new PrimitiveBatch(PrimitiveType.Line, ConnectionList);
        }

        public void Init(OBJData data)
        {
            DoRender = true;
            this.data = data;

            string VertexBatchID = string.Format("NavObjData_{0}", RefManager.GetNewRefID());
            PathVertexBatch = new PrimitiveBatch(PrimitiveType.Box, VertexBatchID);
            foreach (OBJData.VertexStruct Vertex in data.vertices)
            {
                RenderBoundingBox navigationBox = new RenderBoundingBox();
                navigationBox.Init(new BoundingBox(new Vector3(-0.1f), new Vector3(0.1f)));
                navigationBox.SetColour(System.Drawing.Color.Green);
                navigationBox.SetTransform(Matrix4x4.CreateTranslation(Vertex.Position));

                int PathHandle = RefManager.GetNewRefID();
                PathVertexBatch.AddObject(PathHandle, navigationBox);
                BoundingBoxes.Add(navigationBox);
            }

            OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(PathVertexBatch);
            OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(PointConnectionsBatch);
        }

        public void SelectNode(int Index)
        {
            // TODO: Big problem here - The graphics class isn't aware of the selecting logic here.
            // So we'll one day need to support the graphics class aware of this and deselect this whenever another
            // object has been selected.
            if (SelectedIndex != -1)
            {
                BoundingBoxes[SelectedIndex].Unselect();
            }

            // Move the selection to the new Vertex
            BoundingBoxes[Index].Select();
            SelectedIndex = Index;

            // Render debug work
            OBJData.VertexStruct PathPoint = data.vertices[Index];
            RenderLine FromA = CreateConnectionLine(PathPoint, data.vertices[PathPoint.Unk3], System.Drawing.Color.Yellow);
            RenderLine FromB = CreateConnectionLine(PathPoint, data.vertices[PathPoint.Unk4], System.Drawing.Color.Brown);
            RenderLine FromC = CreateConnectionLine(PathPoint, data.vertices[PathPoint.Unk5], System.Drawing.Color.Red);

            PointConnectionsBatch.ClearObjects();
            PointConnectionsBatch.AddObject(RefManager.GetNewRefID(), FromA);
            PointConnectionsBatch.AddObject(RefManager.GetNewRefID(), FromB);
            PointConnectionsBatch.AddObject(RefManager.GetNewRefID(), FromC);

            foreach (var IncomingPoint in PathPoint.IncomingConnections)
            {
                RenderLine Connection = CreateConnectionLine(PathPoint, IncomingPoint, System.Drawing.Color.Green);
                PointConnectionsBatch.AddObject(RefManager.GetNewRefID(), Connection);
            }

            foreach (var OutgoingPoint in PathPoint.OutgoingConnections)
            {
                RenderLine Connection = CreateConnectionLine(PathPoint, OutgoingPoint, System.Drawing.Color.Blue);
                PointConnectionsBatch.AddObject(RefManager.GetNewRefID(), Connection);
            }

            PointConnectionsBatch.SetIsDirty();
        }

        private RenderLine CreateConnectionLine(OBJData.VertexStruct FromPoint, OBJData.VertexStruct ToPoint, System.Drawing.Color Colour)
        {
            RenderLine navigationLine = new RenderLine();
            navigationLine.SetUnselectedColour(Colour);
            navigationLine.SetSelectedColour(System.Drawing.Color.Red);
            navigationLine.Init(new Vector3[2] { FromPoint.Position, ToPoint.Position });

            return navigationLine;
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext) { }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera) { }

        public override void Select() { }

        public override void SetTransform(Matrix4x4 matrix)
        {
            Transform = matrix;
        }

        public override void Shutdown() { }

        public override void Unselect() { }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext) { }
    }
}
