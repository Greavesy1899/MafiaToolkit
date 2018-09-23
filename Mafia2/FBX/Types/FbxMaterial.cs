using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxMaterial
    {
        int id;
        string name;
        string type;
        byte version;
        string shadingModel;
        byte multiLayer;
        object[] properties;

        public string Name {
            get { return name; }
            set { name = value; }
        }
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
                    case "MultiLayer":
                        multiLayer = (byte)n.Value;
                        break;
                    case "ShadingModel":
                        shadingModel = (string)n.Value;
                        break;
                    case "Version":
                        version = (byte)n.Value;
                        break;
                    default:
                        Console.WriteLine("Non-Implemented or unknown node was passed: " + n.Name);
                        break;
                }
            }
        }
    }
}
