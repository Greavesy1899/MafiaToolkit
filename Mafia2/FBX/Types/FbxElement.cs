using Fbx;
using System;
using System.Collections.Generic;

namespace Mafia2.FBX
{
    public class FbxElement
    {
        FbxModel model;
        FbxGeometry geometry;
        Dictionary<int, FbxVideo> videos = new Dictionary<int, FbxVideo>();
        Dictionary<int, FbxMaterial> materials = new Dictionary<int, FbxMaterial>();
        Dictionary<int, FbxTexture> textures = new Dictionary<int, FbxTexture>();

        public FbxModel Model {
            get { return model; }
            set { model = value; }
        }
        public FbxGeometry Geometry {
            get { return geometry; }
            set { geometry = value; }
        }
        public Dictionary<int, FbxVideo> Videos {
            get { return videos; }
            set { videos = value; }
        }
        public Dictionary<int, FbxMaterial> Materials {
            get { return materials; }
            set { materials = value; }
        }
        public Dictionary<int, FbxTexture> Textures {
            get { return textures; }
            set { textures = value; }
        }
    }
}
