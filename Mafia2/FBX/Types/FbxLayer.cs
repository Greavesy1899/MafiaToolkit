using Fbx;

namespace Mafia2.FBX
{
    public class FbxLayer
    {
        byte version;
        string name;
        string mappingInformationType;
        string referenceInformationType;

        /// <summary>
        /// Fills data with passed Fbx node.
        /// </summary>
        /// <param name="node"></param>
        public FbxLayer(FbxNode node)
        {
            ConvertLayer(node);
        }

        public virtual void ConvertLayer(FbxNode node)
        {
            foreach(FbxNode n in node.Nodes)
            {
                switch(n.Name)
                {
                    case "Version":
                        version = (byte)n.Value;
                        break;
                    case "Name":
                        name = (string)n.Value;
                        break;
                    case "MappingInformationType":
                        mappingInformationType = (string)n.Value;
                        break;
                    case "ReferenceInformationType":
                        referenceInformationType = (string)n.Value;
                        break;
                }
            }
        }
    }
}
