using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxModel : FbxObject
    {
        byte version;
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
