using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxMaterial : FbxObject
    {
        byte version;
        string shadingModel;
        byte multiLayer;
        object[] properties;

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
