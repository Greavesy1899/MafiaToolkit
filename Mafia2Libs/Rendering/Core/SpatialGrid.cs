using SharpDX;
using System;
using ResourceTypes.Translokator;
using SharpDX.Direct3D11;
using Rendering.Graphics;
using Utils.StringHelpers;
using ResourceTypes.Navigation;
using Utils.SharpDXExtensions;
using System.Diagnostics;

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

        private bool bIsReady = false;

        public SpatialGrid()
        {
            cells = new SpatialCell[0];
        }

        public SpatialGrid(KynogonRuntimeMesh mesh)
        {
            bIsReady = true;
            var min = new Vector3(mesh.BoundMin, float.MinValue);
            var max = new Vector3(mesh.BoundMax, float.MaxValue);

            gridBounds = new BoundingBox(min, max);

            width = mesh.CellSizeY;
            height = mesh.CellSizeX;
            origin = new Vector3(gridBounds.Minimum.X, gridBounds.Minimum.Y, 0.0f);
            cellSize = new Vector2(gridBounds.Width / width, gridBounds.Height / height);
            cells = new SpatialCell[width * height];

            var index = 0;
            for (int i = 0; i < width; i++)
            {
                for (int x = 0; x < height; x++)
                {
                    KynogonRuntimeMesh.Cell cell = mesh.Cells[index];

                    foreach (var set in cell.Sets)
                    {
                        if (gridBounds.Minimum.Z < set.X)
                        {
                            gridBounds.Minimum.Z = set.X;
                        }
                        if (gridBounds.Maximum.Z > set.Y)
                        {
                            gridBounds.Maximum.Z = set.Y;
                        }
                    }
                
                    var extents = new BoundingBox();
                    extents.Minimum = new Vector3(origin.X + cellSize.X * x, origin.Y + cellSize.Y * i, 0.0f);
                    extents.Maximum = new Vector3(origin.X + cellSize.X * (x + 1), origin.Y + cellSize.Y * (i + 1), 0.0f);
                    cells[index] = new SpatialCell(cell, extents);

                    index++;
                }
            }
        }

        public SpatialGrid(TranslokatorLoader translokator)
        {
            bIsReady = true;
            gridBounds = translokator.Bounds;
            width = translokator.Grids[0].Width;
            height = translokator.Grids[0].Height;
            cellSize = new Vector2(gridBounds.Width / width, gridBounds.Height / height);
            cells = new SpatialCell[width * height];
            origin = gridBounds.Minimum;

            var index = 0;
            for (int i = 0; i < width; i++)
            {
                for (int x = 0; x < height; x++)
                {
                    var extents = new BoundingBox();
                    extents.Minimum = new Vector3(origin.X + cellSize.X * x, origin.Y + cellSize.Y * i, 10.0f);
                    extents.Maximum = new Vector3(origin.X + cellSize.X * (x + 1), origin.Y + cellSize.Y * (i + 1), 10.0f);
                    cells[index++] = new SpatialCell(extents);
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
                        box.SetTransform(Matrix.Translation(instance.Position));
                        box.Init(new BoundingBox(-Vector3.One, Vector3.One));
                        cells[cell].AddAsset(box, StringHelpers.GetNewRefID());
                    }
                }
            }
        }

        public void Initialise(Device device, DeviceContext deviceContext)
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

        public void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            if (bIsReady)
            {
                //foreach (var cell in cells)
                //{
                //    cell.Render(device, deviceContext, camera);
                //}

                cellBoundingBox.Render(device, deviceContext, camera);
                currentCell = GetCell(camera.Position);
                cells[currentCell].Render(device, deviceContext, camera);
                //Debug.WriteLine(cells[currentCell].BoundingBox.ToString());
                if (previousCell != currentCell)
                {
                    BoundingBox newBounds = cells[currentCell].BoundingBox;
                    newBounds.Minimum.Z = gridBounds.Minimum.Z;
                    newBounds.Maximum.Z = gridBounds.Maximum.Z;

                    cellBoundingBox.Update(newBounds);
                    cellBoundingBox.UpdateBuffers(device, deviceContext);
                    previousCell = currentCell;
                }
                boundingBox.Render(device, deviceContext, camera);
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
    }
}
