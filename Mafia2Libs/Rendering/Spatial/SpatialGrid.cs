using Rendering.Graphics;
using ResourceTypes.Navigation;
using ResourceTypes.Translokator;
using System;
using System.Numerics;
using System.Windows.Forms;
using Toolkit.Core;
using Utils.StringHelpers;
using Utils.VorticeUtils;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Core
{
    public class SpatialGrid
    {
        private Vector3 origin;
        private int width;
        private int height;
        private Vector2 cellSize;
        private SpatialCell[] cells;
        private BoundingBox gridBounds;
        private BoundingBox cellBounds;
        private RenderBoundingBox boundingBox;
        private RenderBoundingBox cellBoundingBox;

        private int previousCell = -1;
        private int currentCell = -1;

        private GraphicsClass OwnGraphicsClass;
        private bool bIsReady = false;
        private Matrix4x4 Transform
        {
            get
            { 
                Matrix4x4 m = new();
                m.Translation = origin;
                return m;
            }
        }
        public SpatialGrid()
        {
            cells = new SpatialCell[0];
        }

        public SpatialGrid(GraphicsClass InGraphics, KynogonRuntimeMesh mesh)
        {
            OwnGraphicsClass = InGraphics;

            bIsReady = true;
            var min = new Vector3(mesh.BoundMin, float.MinValue);
            var max = new Vector3(mesh.BoundMax, float.MaxValue);

            gridBounds = new BoundingBox(min, max);

            width = mesh.CellSizeY;
            height = mesh.CellSizeX;
            origin = new Vector3(gridBounds.Min.X, gridBounds.Min.Y, 0.0f);
            cellSize = new Vector2(gridBounds.GetWidth() / width, gridBounds.GetHeight() / height);
            cells = new SpatialCell[width * height];

            Vector3 TempMin = gridBounds.Min;
            Vector3 TempMax = gridBounds.Max;

            var index = 0;
            for (int i = 0; i < width; i++)
            {
                for (int x = 0; x < height; x++)
                {
                    KynogonRuntimeMesh.Cell cell = mesh.Cells[index];

                    foreach (var set in cell.Sets)
                    {
                        if (gridBounds.Min.Z < set.X)
                        {
                            TempMin.Z = set.X;
                        }
                        if (gridBounds.Max.Z > set.Y)
                        {
                            TempMax.Z = set.Y;
                        }
                    }

                    // Construct cell extents
                    Vector3 Min = new Vector3(origin.X + cellSize.X * x, origin.Y + cellSize.Y * i, 0.0f);
                    Vector3 Max = new Vector3(origin.X + cellSize.X * (x + 1), origin.Y + cellSize.Y * (i + 1), 0.0f);
                    BoundingBox CellExtents = new BoundingBox(Min, Max);

                    // Construct Init params
                    SpatialCell_ObjDataParams InitParams = new SpatialCell_ObjDataParams();
                    InitParams.OwnGraphics = OwnGraphicsClass;
                    InitParams.CellExtents = CellExtents;
                    InitParams.CellInfo = cell;

                    // Construct ObjData cell
                    SpatialCell_ObjData ObjDataCell = new SpatialCell_ObjData(InitParams);
                    ObjDataCell.PreInitialise();
                    cells[index] = ObjDataCell;

                    index++;
                }
            }
        }

        public SpatialGrid(GraphicsClass InGraphics, TranslokatorLoader translokator)
        {
            OwnGraphicsClass = InGraphics;

            bIsReady = true;
            gridBounds = translokator.Bounds;
            width = translokator.Grids[0].Width;
            height = translokator.Grids[0].Height;
            cellSize = new Vector2(gridBounds.GetWidth() / width, gridBounds.GetHeight() / height);
            cells = new SpatialCell[width * height];
            origin = gridBounds.Min;

            var index = 0;
            /*for (int i = 0; i < width; i++)
            {
                for (int x = 0; x < height; x++)
                {
                    var extents = new BoundingBox();
                    extents.Min = new Vector3(origin.X + cellSize.X * x, origin.Y + cellSize.Y * i, 10.0f);
                    extents.Max = new Vector3(origin.X + cellSize.X * (x + 1), origin.Y + cellSize.Y * (i + 1), 10.0f);
                    cells[index++] = new SpatialCell(InGraphics, extents);
                }
            }

            for (int i = 0; i != translokator.ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = translokator.ObjectGroups[i];

                for (int x = 0; x != objectGroup.NumObjects; x++)
                {
                    ResourceTypes.Translokator.Object obj = objectGroup.Objects[x];

                    for (int y = 0; y != obj.NumInstances; y++)
                    {
                        Instance instance = obj.Instances[y];
                        var cell = GetCell(instance.Position);
                        RenderBoundingBox box = new RenderBoundingBox();
                        box.SetTransform(Matrix4x4.CreateTranslation(instance.Position));
                        box.Init(new BoundingBox(-Vector3.One, Vector3.One));
                        cells[cell].AddAsset(box, RefManager.GetNewRefID());
                    }
                }
            }*/
        }

        public void Initialise(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if (bIsReady)
            {
                boundingBox = new RenderBoundingBox();
                boundingBox.SetColour(System.Drawing.Color.Red, true);
                boundingBox.Init(gridBounds);
                boundingBox.InitBuffers(device, deviceContext);

                cellBoundingBox = new RenderBoundingBox();
                cellBoundingBox.SetColour(System.Drawing.Color.Blue, true);
                cellBoundingBox.Init(cellBounds);
                cellBoundingBox.InitBuffers(device, deviceContext);

                foreach (var cell in cells)
                {
                    cell.Initialise(device, deviceContext);
                }
            }
        }

        public void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            if (bIsReady)
            {
                if (!camera.CheckBBoxFrustum(Transform, gridBounds))
                    return;

                if (currentCell != -1)
                {
                    cells[currentCell].Render(device, deviceContext, camera);
                }

                
                foreach (var cell in cells)
                {
                    cell.Render(device, deviceContext, camera);
                }

                /*cellBoundingBox.Render(device, deviceContext, camera);
                currentCell = GetCell(camera.Position);
                //cells[currentCell].Render(device, deviceContext, camera);
                //Debug.WriteLine(cells[currentCell].BoundingBox.ToString());
                if (previousCell != currentCell)
                {
                    BoundingBox newBounds = cells[currentCell].BoundingBox;
                    newBounds.Min.Z = gridBounds.Min.Z;
                    newBounds.Max.Z = gridBounds.Max.Z;

                    cellBoundingBox.Update(newBounds);
                    cellBoundingBox.UpdateBuffers(device, deviceContext);
                    previousCell = currentCell;
                }
                boundingBox.Render(device, deviceContext, camera);
                */
            }
        }

        public void Shutdown()
        {
            boundingBox?.Shutdown();
            cellBoundingBox?.Shutdown();
            foreach (var cell in cells)
            {
                cell.Shutdown();
            }
        }

        private int GetCell(Vector3 position)
        {
            var offsetX = position.X - origin.X;
            var offsetY = position.Y - origin.Y;
            var gridX = Math.Abs(Math.Floor(offsetX / cellSize.X));
            var gridY = Math.Abs(Math.Floor(offsetY / cellSize.Y));
            var intX = Convert.ToUInt32(Math.Min(gridX, width - 1));
            var intY = Convert.ToUInt32(Math.Min(gridY, height - 1));
            return (int)(intX + (int)(intY * width));
        }

        public void SetSelectedCell(int index)
        {
            currentCell = index;
        }

        public TreeNode GetTreeNodes()
        {
            TreeNode[] ChildCells = new TreeNode[cells.Length];

            for(int i = 0; i < cells.Length; i++)
            {
                TreeNode Child = new TreeNode();
                Child.Text = string.Format("CELL {0}", i);
                Child.Name = cells[i].GetRefID().ToString();
                Child.Tag = cells[i];
                ChildCells[i] = Child;
            }

            TreeNode Parent = new TreeNode();
            Parent.Text = string.Format("Parent");
            Parent.Tag = this;
            Parent.Nodes.AddRange(ChildCells);

            return Parent;
        }
    }
}
