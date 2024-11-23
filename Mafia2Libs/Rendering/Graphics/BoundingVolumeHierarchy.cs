using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Utils.VorticeUtils;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public class Bin
    {
        public BoundingBox Bounds { get; set; } = BoundingBox.Zero;
        public int TriangleCount { get; set; }
    }

    public class BVHNode
    {
        public BoundingBox Bounds { get; set; } = BoundingBox.Zero;
        public int LeftChild { get; set; }
        public int FirstTriangleIndex { get; set; }
        public int TriangleCount { get; set; }
        public bool IsLeaf { get => TriangleCount > 0; }
        public RenderBoundingBox RenderObject { get; set; } = new(); //TODO: Remove this debugging code
    }

    public class BVH
    {
        public BVHNode[] Nodes { get; set; } = new BVHNode[0];
        public int RootNodeID { get; set; }
        public int UsedNodesCount { get; set; }
        public bool IsBuilding = false;
        public bool FinishedBuilding = false;

        private List<(uint x, uint y, uint z, Vector3 centroid)> Triangles { get; set; } = new();
        private List<int> TriangleIndices { get; set; } = new();
        private VertexLayouts.NormalLayout.Vertex[] Vertices { get; set; } = null;
        private bool UseBinnedBVH = true; // Slower to compute BVH, but faster for raycasting, enabled because we build BVH structures in the background
        private int MaxBins = 10;         // We should probably disable Binned BVH building if we detect the computer has 2 or fewer threads, if that can even happen nowadays

        public BVH()
        {

        }

        public void Build(VertexLayouts.NormalLayout.Vertex[] vertices, uint[] indices)
        {
            if (FinishedBuilding)
            {
                return;
            }

            Vertices = vertices;

            for (int i = 0; i < indices.Count(); i+=3)
            {
                (uint x, uint y, uint z, Vector3 centroid) triangle = new();
                triangle.x = indices[i];
                triangle.y = indices[i+1];
                triangle.z = indices[i+2];
                triangle.centroid = (Vertices[(int)triangle.x].Position + Vertices[(int)triangle.y].Position + Vertices[(int)triangle.z].Position) * 0.3333333f;

                Triangles.Add(triangle);
            }

            for (int i = 0; i < Triangles.Count; i++)
            {
                TriangleIndices.Add(i);
            }

            Nodes = new BVHNode[(Triangles.Count * 2) - 1];

            // Initialize the root node
            Nodes[RootNodeID] = new();
            Nodes[RootNodeID].LeftChild = 0;
            Nodes[RootNodeID].FirstTriangleIndex = 0;
            Nodes[RootNodeID].TriangleCount = Triangles.Count;
            UpdateNodeBounds(Nodes[RootNodeID]);

            Nodes[RootNodeID].RenderObject.Init(Nodes[RootNodeID].Bounds);

            // Start recursive splitting
            SubdivideNode(Nodes[RootNodeID]);

            UsedNodesCount++;

            // Clean up
            BVHNode[] tempNodes = new BVHNode[UsedNodesCount];
            Array.Copy(Nodes, 0, tempNodes, 0, UsedNodesCount);
            Nodes = tempNodes;

            FinishedBuilding = true;
            IsBuilding = false;
        }

        public void UpdateNodeBounds(BVHNode node)
        {
            Vector3 Min = new Vector3(1e30f);
            Vector3 Max = new Vector3(-1e30f);
            for (int i = node.FirstTriangleIndex; i < node.TriangleCount + node.FirstTriangleIndex; i++)
            {
                var t = Triangles[TriangleIndices[i]];
                Min.X = MathF.Min(Min.X, Vertices[t.x].Position.X); // This is shit
                Min.Y = MathF.Min(Min.Y, Vertices[t.x].Position.Y);
                Min.Z = MathF.Min(Min.Z, Vertices[t.x].Position.Z);
                Min.X = MathF.Min(Min.X, Vertices[t.y].Position.X);
                Min.Y = MathF.Min(Min.Y, Vertices[t.y].Position.Y);
                Min.Z = MathF.Min(Min.Z, Vertices[t.y].Position.Z);
                Min.X = MathF.Min(Min.X, Vertices[t.z].Position.X);
                Min.Y = MathF.Min(Min.Y, Vertices[t.z].Position.Y);
                Min.Z = MathF.Min(Min.Z, Vertices[t.z].Position.Z);
                Max.X = MathF.Max(Max.X, Vertices[t.x].Position.X);
                Max.Y = MathF.Max(Max.Y, Vertices[t.x].Position.Y);
                Max.Z = MathF.Max(Max.Z, Vertices[t.x].Position.Z);
                Max.X = MathF.Max(Max.X, Vertices[t.y].Position.X);
                Max.Y = MathF.Max(Max.Y, Vertices[t.y].Position.Y);
                Max.Z = MathF.Max(Max.Z, Vertices[t.y].Position.Z);
                Max.X = MathF.Max(Max.X, Vertices[t.z].Position.X);
                Max.Y = MathF.Max(Max.Y, Vertices[t.z].Position.Y);
                Max.Z = MathF.Max(Max.Z, Vertices[t.z].Position.Z);
            }

            node.Bounds = new(Min, Max);
        }

        public void SubdivideNode(BVHNode node)
        {
            // We cannot split quads
            if (node.TriangleCount <= 2) return;

            int axis = 0;
            float splitPos = 0.0f;

            if (UseBinnedBVH)
            {
                // Determine the split axis and position by using Surface Area Heuristic combined
                // with bins, slower to compute but faster for raycasting
                var bestPos = FindBestSplitPlane(node);
                
                axis = bestPos.axis;
                splitPos = bestPos.splitPos;
            }
            else
            {
                // Determine split axis and position by splitting in half on the longest axis
                // Faster to compute, but slower for raycasting
                axis = 0;

                if (node.Bounds.Extent.Y > node.Bounds.Extent.X)
                {
                    axis = 1;
                }

                if (node.Bounds.Extent.Z > node.Bounds.Extent[axis])
                {
                    axis = 2;
                }

                splitPos = node.Bounds.Min[axis] + node.Bounds.Extent[axis] * 0.5f;
            }


            // Reorganize and separate the triangles for this node
            int i = node.FirstTriangleIndex;
            int j = i + node.TriangleCount - 1;
            
            while (i <= j)
            {
                if (Triangles[TriangleIndices[i]].centroid[axis] < splitPos)
                {
                    i++;
                }
                else
                {
                    var x = TriangleIndices[i];
                
                    TriangleIndices[i] = TriangleIndices[j];
                    TriangleIndices[j] = x;
                
                    j--;
                }   
            }

            // Skip splitting if one of the sides contains no triangles
            int LeftSideTriangleCount = i - node.FirstTriangleIndex;

            if (LeftSideTriangleCount == 0 || LeftSideTriangleCount == node.TriangleCount)
            {
                return;
            }

            // Initialize the child nodes and compute their bounding boxes
            UsedNodesCount++;
            int LeftChildID = UsedNodesCount;
            UsedNodesCount++;
            int RightChildID = UsedNodesCount;

            Nodes[LeftChildID] = new();
            Nodes[RightChildID] = new();

            Nodes[LeftChildID].FirstTriangleIndex = node.FirstTriangleIndex;
            Nodes[LeftChildID].TriangleCount = LeftSideTriangleCount;
            Nodes[RightChildID].FirstTriangleIndex = i;
            Nodes[RightChildID].TriangleCount = node.TriangleCount - LeftSideTriangleCount;

            node.LeftChild = LeftChildID;
            node.TriangleCount = 0;

            UpdateNodeBounds(Nodes[LeftChildID]);
            UpdateNodeBounds(Nodes[RightChildID]);

            Nodes[LeftChildID].RenderObject.Init(Nodes[LeftChildID].Bounds);
            Nodes[RightChildID].RenderObject.Init(Nodes[RightChildID].Bounds);

            // Recursively split the child nodes
            SubdivideNode(Nodes[LeftChildID]);
            SubdivideNode(Nodes[RightChildID]);
        }

        private (int axis, float splitPos, float bestCost) FindBestSplitPlane(BVHNode node)
        {
            float bestCost = 1e30f;
            int axis = 0;
            float splitPos = 0.5f;

            for (int testAxis = 0; testAxis < 3; testAxis++)
            {
                float boundsMin = 1e30f, boundsMax = -1e30f;

                for (int i = 0; i < node.TriangleCount; i++)
                {
                    var triangle = Triangles[TriangleIndices[node.FirstTriangleIndex + i]];
                    boundsMin = MathF.Min(boundsMin, triangle.centroid[testAxis]);
                    boundsMax = MathF.Max(boundsMax, triangle.centroid[testAxis]);
                }

                if (boundsMin == boundsMax) continue;

                // Initialize bins
                Bin[] bins = new Bin[MaxBins];
                float scale = MaxBins / (boundsMax - boundsMin);

                Vector3 Min = new Vector3(1e30f);
                Vector3 Max = new Vector3(-1e30f);

                for (int i = 0; i < node.TriangleCount; i++)
                {
                    var triangle = Triangles[TriangleIndices[node.FirstTriangleIndex + i]];
                    int binIdx = (int)MathF.Min(MaxBins - 1, (int)((triangle.centroid[testAxis] - boundsMin) * scale));

                    if (bins[binIdx] == null)
                    {
                        bins[binIdx] = new();
                    }

                    bins[binIdx].TriangleCount++;
                    bins[binIdx].Bounds = bins[binIdx].Bounds.Grow(Vertices[triangle.x].Position);
                    bins[binIdx].Bounds = bins[binIdx].Bounds.Grow(Vertices[triangle.y].Position);
                    bins[binIdx].Bounds = bins[binIdx].Bounds.Grow(Vertices[triangle.z].Position);
                }

                // Initialize the planes between bins
                float[] leftArea = new float[MaxBins - 1];
                float[] rightArea = new float[MaxBins - 1];
                int[] leftCount = new int[MaxBins - 1];
                int[] rightCount = new int[MaxBins - 1];
                BoundingBox leftBox = BoundingBox.Zero;
                BoundingBox rightBox = BoundingBox.Zero;
                int leftSum = 0;
                int rightSum = 0;

                for (int i = 0; i < MaxBins - 1; i++)
                {
                    if (bins[i] == null)
                    {
                        continue;
                    }

                    leftSum += bins[i].TriangleCount;
                    leftCount[i] = leftSum;
                    leftBox = bins[i].Bounds;
                    leftArea[i] = leftBox.Extent.X * leftBox.Extent.Y + leftBox.Extent.Y * leftBox.Extent.Z + leftBox.Extent.Z * leftBox.Extent.X;

                    if (bins[MaxBins - 1 - i] == null)
                    {
                        continue;
                    }

                    rightSum += bins[MaxBins - 1 - i].TriangleCount;
                    rightCount[MaxBins - 2 - i] = rightSum;
                    rightBox = bins[MaxBins - 1 - i].Bounds;
                    rightArea[MaxBins - 2 - i] = rightBox.Extent.X * rightBox.Extent.Y + rightBox.Extent.Y * rightBox.Extent.Z + rightBox.Extent.Z * rightBox.Extent.X;
                }

                // Compute SAH cost for the bin planes
                scale = (boundsMax - boundsMin) / MaxBins;

                for (int i = 0; i < MaxBins - 1; i++)
                {
                    float planeCost = leftCount[i] * leftArea[i] + rightCount[i] * rightArea[i];
                    if (planeCost < bestCost)
                    {
                        axis = testAxis;
                        splitPos = boundsMin + scale * (i + 1);
                        bestCost = planeCost;
                    }
                }
            }
            return (axis, splitPos, bestCost);
        }

        public (float distance, Vector3 pos) Intersect(Ray ray)
        {
            BVHNode node = Nodes[RootNodeID]; 
            BVHNode[] stack = new BVHNode[64];
            uint stackPtr = 0;

            float lowestDistance = float.MaxValue;
            Vector3 WorldPosIntersect = Vector3.Zero;

            while (true)
            {
                if (node.IsLeaf)
                {
                    for (int i = 0; i < node.TriangleCount; i++)
                    {
                        var triangle = Triangles[TriangleIndices[node.FirstTriangleIndex + i]];

                        float t;

                        if (!Toolkit.Mathematics.Collision.RayIntersectsTriangle(ref ray, ref Vertices[triangle.x].Position, ref Vertices[triangle.y].Position, ref Vertices[triangle.z].Position, out t))
                        {
                            continue;
                        }

                        var worldPosition = ray.Position + t * ray.Direction;
                        var distance = (worldPosition - ray.Position).LengthSquared();

                        if (distance < lowestDistance)
                        {
                            lowestDistance = distance;
                            WorldPosIntersect = worldPosition;
                        }
                    }

                    if (stackPtr == 0)
                    {
                        break;
                    }
                    else
                    {
                        node = stack[stackPtr];
                        stackPtr--;
                    }

                    continue;
                }

                BVHNode child1 = Nodes[node.LeftChild];
                BVHNode child2 = Nodes[node.LeftChild + 1];

                float? dist1 = ray.Intersects(child1.Bounds);
                float? dist2 = ray.Intersects(child2.Bounds);

                dist1 = dist1 == null ? 0.0f : dist1;
                dist2 = dist2 == null ? 0.0f : dist2;

                if (dist1 < dist2)
                {
                    float? distTemp = dist1;
                    BVHNode tempNode = child1;

                    dist1 = dist2;
                    dist2 = distTemp;
                    child1 = child2;
                    child2 = tempNode;
                }

                if (dist1 == 0.0f)
                {
                    if (stackPtr == 0)
                    {
                        break;
                    }
                    else
                    {
                        node = stack[stackPtr];
                        stackPtr--;
                    }
                }
                else
                {
                    node = child1;

                    if (dist2 != 0.0f)
                    {
                        stackPtr++;

                        stack[stackPtr] = child2;
                    } 
                }
            }

            return (lowestDistance, WorldPosIntersect);
        }
    }
}
