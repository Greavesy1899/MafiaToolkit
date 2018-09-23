using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxModel
    {
        int id;
        string name;
        string type;
        byte version;
        object[] properties;

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
