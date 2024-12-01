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
        // TODO: Make cells selectable and toggleable
        // Right now all cells render at once and you can't see anything

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


            /*for (int i = 0; i != translokator.ObjectGroups.Length; i++)
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


                for (int i = 0; i < cellBoundingBox.Length; i++)
                {
                    cellBoundingBox[i].SetColour(System.Drawing.Color.Blue, true);
                    cellBoundingBox[i].Init(cellBounds[i]);
                    cellBoundingBox[i].InitBuffers(device, deviceContext);
                    cellBoundingBox[i].DoRenderInstancesOnly = true;
                }
            }
        }

        public void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            foreach (var cell in cellBoundingBox)
            {
                cell.Render(device, deviceContext, camera);
                //break;
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

        //private int GetCell(Vector3 position)
        //{
        //    var offsetX = position.X - origin.X;
        //    var offsetY = position.Y - origin.Y;
        //    var gridX = Math.Abs(Math.Floor(offsetX / cellSize.X));
        //    var gridY = Math.Abs(Math.Floor(offsetY / cellSize.Y));
        //    var intX = Convert.ToUInt32(Math.Min(gridX, width - 1));
        //    var intY = Convert.ToUInt32(Math.Min(gridY, height - 1));
        //    return (int)(intX + (int)(intY * width));
        //}

        //public void SetSelectedCell(int index)
        //{
        //    currentCell = index;
        //}

        //public TreeNode GetTreeNodes()
        //{
        //    TreeNode[] ChildCells = new TreeNode[cells.Length];
        //
        //    for (int i = 0; i < cells.Length; i++)
        //    {
        //        TreeNode Child = new TreeNode();
        //        Child.Text = string.Format("CELL {0}", i);
        //        Child.Name = cells[i].GetRefID().ToString();
        //        Child.Tag = cells[i];
        //        ChildCells[i] = Child;
        //    }
        //
        //    TreeNode Parent = new TreeNode();
        //    Parent.Text = string.Format("Parent");
        //    Parent.Tag = this;
        //    Parent.Nodes.AddRange(ChildCells);
        //
        //    return Parent;
        //}
    }
}
