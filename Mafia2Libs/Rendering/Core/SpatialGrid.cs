using SharpDX;
using System;
using ResourceTypes.Translokator;
using SharpDX.Direct3D11;
using Rendering.Graphics;
using Utils.StringHelpers;
using ResourceTypes.Navigation;
using Utils.SharpDXExtensions;

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
        private RenderBoundingBox boundingBox;

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
            var min = new Vector3(mesh.BoundMin, -10.0f);
            var max = new Vector3(mesh.BoundMax, 10.0f);

            var tempX = min.X;
            min.X = min.Y;
            min.Y = tempX;

            tempX = max.X;
            max.X = max.Y;
            max.Y = tempX;

            gridBounds = new BoundingBox(min, max);
            Console.WriteLine(gridBounds.ToString());

            width = mesh.CellSizeY;
            height = mesh.CellSizeX;
            origin = gridBounds.Minimum;
            cellSize = new Vector2(gridBounds.Width / width, gridBounds.Height / height);
            cells = new SpatialCell[width * height];

            //for(int i = 0; i < )
            var index = 0;
            for (int i = 0; i < width; i++)
            {
                for (int x = 0; x < height; x++)
                {
                    var extents = new BoundingBox();
                    extents.Minimum = new Vector3(origin.X + cellSize.X * x, origin.Y + cellSize.Y * i, 10.0f);
                    extents.Maximum = new Vector3(origin.X + cellSize.X * (x + 1), origin.Y + cellSize.Y * (i + 1), 10.0f);
                    cells[index] = new SpatialCell(mesh.Cells[index]);
                    index++;
                }
            }
        }

        public SpatialGrid(TranslokatorLoader translokator)
        {
            bIsReady = true;
            gridBounds = translokator.Bounds;
            width = 5;
            height = 5;
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
                currentCell = GetCell(camera.Position);
                cells[currentCell].Render(device, deviceContext, camera);

                if (previousCell != currentCell)
                {
                    //boundingBox.Update(cells[currentCell].BoundingBox);
                    //boundingBox.UpdateBuffers(device, deviceContext);
                    previousCell = currentCell;
                }
                boundingBox.Render(device, deviceContext, camera);
            }
        }

        public void Shutdown()
        {
            boundingBox?.Shutdown();
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
