using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxGeometry
    {
        int id;
        string name;
        string type;
        FbxNode[] properties;
        double[] vertices;
        int[] triangles;
        byte version;
        FbxLayerElementNormal normals;
        FbxLayerElementUV uvs;
        FbxLayerElementMaterial materials;

        public void ConvertFromNode(FbxNode node)
        {
            id = (int)node.Properties[0];
            name = (string)node.Properties[1];
            type = (string)node.Properties[2];

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
