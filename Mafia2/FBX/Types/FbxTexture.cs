using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxTexture
    {
        int id;
        string name;
        string type;
        string textureType;
        byte version;
        string textureName;
        object[] properties;
        string media;
        string fileName;
        string relativeFileName;
        object[] modelUVTranslation;
        object[] modelUVScaling;
        string textureAlphaSource;
        object[] cropping;

        public void ConvertFromNode(FbxNode node)
        {
            id = (int)node.Properties[0];
            name = (string)node.Properties[1];
            type = (string)node.Properties[2];

            foreach (FbxNode n in node.Nodes)
            {
                switch(n.Name)
                {
                    case "Type":
                        textureType = (string)n.Value;
                        break;
                    case "Version":
                        version = (byte)n.Value;
                        break;
                    case "TextureName":
                        textureName = (string)n.Value;
                        break;
                    case "Properties70":
                        properties = n.Nodes.ToArray();
                        break;
                    case "Media":
                        media = (string)n.Value;
                        break;
                    case "FileName":
                        fileName = (string)n.Value;
                        break;
                    case "RelativeFilename":
                        relativeFileName = (string)n.Value;
                        break;
                    case "ModelUVTranslation":
                        modelUVTranslation = n.Properties.ToArray();
                        break;
                    case "ModelUVScaling":
                        modelUVScaling = n.Properties.ToArray();
                        break;
                    case "Texture_Alpha_Source":
                        textureAlphaSource = (string)n.Value;
                        break;
                    case "Cropping":
                        cropping = n.Properties.ToArray();
                        break;
                    default:
                        Console.WriteLine("Non-Implemented or unknown node was passed: " + n.Name);
                        break;
                }
            }
        }
    }
}
