using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxGeometry : FbxObject
    {
        FbxNode[] properties;
        double[] vertices;
        int[] triangles;
        byte version;
        FbxLayerElementNormal normals;
        FbxLayerElementUV uvs;
        FbxLayerElementMaterial materials;

        public double[] Vertices {
            get { return vertices; }
            set { vertices = value; }
        }
        public int[] Triangles {
            get { return triangles; }
            set { triangles = value; }
        }
        public FbxLayerElementNormal LayerNormal {
            get { return normals; }
            set { normals = value; }
        }
        public FbxLayerElementUV LayerUV {
            get { return uvs; }
            set { uvs = value; }
        }
        public FbxLayerElementMaterial LayerMaterial {
            get { return materials; }
            set { materials = value; }
        }
        public override void ConvertFromNode(FbxNode node)
        {
            base.ConvertFromNode(node);

            foreach (FbxNode n in node.Nodes)
            {
                switch(n.Name)
                {
                    case "Properties70":
                        properties = n.Nodes.ToArray();
                        break;
                    case "Vertices":
                        vertices = (double[])n.Value;
                        break;
                    case "PolygonVertexIndex":
                        triangles = (int[])n.Value;
                        break;
                    case "GeometryVersion":
                        version = (byte)n.Value;
                        break;
                    case "LayerElementNormal":
                        normals = new FbxLayerElementNormal(n);
                        break;
                    case "LayerElementUV":
                        uvs = new FbxLayerElementUV(n);
                        break;
                    case "LayerElementMaterial":
                        materials = new FbxLayerElementMaterial(n);
                        break;
                    case "Layer":
                        Console.WriteLine("Need to work on this!!: LAYER");
                        break;
                    default:
                        Console.WriteLine("Non-Implemented or unknown node was passed: " + n.Name);
                        break;
                }
            }
        }
    }
}
