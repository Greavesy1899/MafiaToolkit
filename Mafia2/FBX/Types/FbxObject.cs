using Fbx;
using System;

namespace Mafia2.FBX
{
    public class FbxObject
    {
        long id;
        string name;
        string type;

        public long ID {
            get { return id; }
            set { id = value; }
        }
        public string Name {
            get { return name; }
            set { name = value; }
        }
        public string Type {
            get { return type; }
            set { type = value; }
        }

        public virtual void ConvertFromNode(FbxNode node)
        {
            id = Convert.ToInt64(node.Properties[0]);
            name = (string)node.Properties[1];
            type = (string)node.Properties[2];
        }
    }
}
