using Rendering.Graphics;
using ResourceTypes.Translokator;
using System.Collections.Generic;
using System.Numerics;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Core
{
    public class TranslokatorSpatialGrid
    {
        private BoundingBox gridBounds;
        private BoundingBox[] cellBounds = new BoundingBox[0];
        private RenderBoundingBox boundingBox;
        private RenderBoundingBox[] cellBoundingBox = new RenderBoundingBox[0];
        private bool bIsReady = false;
        public TranslokatorSpatialGrid()
        {
            
        }

        public TranslokatorSpatialGrid(TranslokatorLoader translokator)
        {
            Build(translokator);   
        }

        public void Build(TranslokatorLoader translokator)
        {
            bIsReady = true;

            gridBounds = translokator.Bounds;
            Vector3 origin = gridBounds.Min;

            cellBounds = new BoundingBox[translokator.Grids.Length];
            cellBoundingBox = new RenderBoundingBox[translokator.Grids.Length];

            for (int k = 0; k < translokator.Grids.Length; k++)
            {
                List<Matrix4x4> CellTransforms = new();

                int width = translokator.Grids[k].Width;
                int height = translokator.Grids[k].Height;
                var cellSize = translokator.Grids[k].CellSize;

                Vector3 Min = new Vector3(0.0f);
                Vector3 Max = new Vector3(cellSize.X, cellSize.Y, gridBounds.Depth);

                cellBounds[k] = new(Min, Max);
                cellBoundingBox[k] = new();

                for (int i = 0; i < height; i++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Vector3 Position = new Vector3(origin.X + cellSize.X * x, origin.Y + cellSize.Y * i, origin.Z);
                        Matrix4x4 transform = Matrix4x4.Identity;
                        transform.Translation = Position;
                        CellTransforms.Add(Matrix4x4.Transpose(transform));
                    }
                }

                cellBoundingBox[k].SetInstanceTransforms(CellTransforms);
            }
        }

        public void Initialise(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            if (bIsReady)
            {
                boundingBox = new RenderBoundingBox();
                boundingBox.SetColour(System.Drawing.Color.Red, true);
                boundingBox.Init(gridBounds);
                boundingBox.InitBuffers(device, deviceContext);
                boundingBox.DoRender = false;


                for (int i = 0; i < cellBoundingBox.Length; i++)
                {
                    cellBoundingBox[i].SetColour(System.Drawing.Color.Blue, true);
                    cellBoundingBox[i].Init(cellBounds[i]);
                    cellBoundingBox[i].InitBuffers(device, deviceContext);
                    cellBoundingBox[i].DoRenderInstancesOnly = true;
                    cellBoundingBox[i].DoRender = false;
                }
            }
        }

        public void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            if (!bIsReady)
            {
                return;
            }

            foreach (var cell in cellBoundingBox)
            {
                cell.Render(device, deviceContext, camera);
            }

            boundingBox.Render(device, deviceContext, camera);
        }

        public void Shutdown()
        {
            boundingBox?.Shutdown();

            foreach (var cell in cellBoundingBox)
            {
                cell?.Shutdown();
            }
        }

        public void SetGridEnabled(int index, bool enabled)
        {
            if (index < cellBoundingBox.Length)
            {
                cellBoundingBox[index].DoRender = enabled;
            }

            // We want to render the general bounding box only if one or more grids is enabled
            boundingBox.DoRender = false;

            foreach (var cell in cellBoundingBox)
            {
                if (cell.DoRender)
                {
                    boundingBox.DoRender = true;
                    break;
                }
            }
        }
    }
}
