using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxVideo : FbxObject
    {
        string videoType;
        object[] properties;
        byte useMipMap;
        string fileName;
        string relativeFileName;

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
                    case "Type":
                        videoType = (string)n.Value;
                        break;
                    case "UseMipMap":
                        useMipMap = (byte)n.Value;
                        break;
                    case "Filename":
                        fileName = (string)n.Value;
                        break;
                    case "RelativeFilename":
                        relativeFileName = (string)n.Value;
                        break;
                    default:
                        Console.WriteLine("Non-Implemented or unknown node was passed: " + n.Name);
                        break;
                }
            }
        }
    }
}
