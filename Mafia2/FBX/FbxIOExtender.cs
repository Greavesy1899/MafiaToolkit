using Fbx;

namespace Mafia2.FBX
{
    public static partial class FbxIOExtender
    {
        /// <summary>
        /// Create FbxNode with node name and value already set.
        /// </summary>
        /// <param name="name">name of node</param>
        /// <param name="value">value of node</param>
        /// <returns></returns>
        public static FbxNode CreateNode(this FbxNode node, string name, object value)
        {
            node.Name = name;
            node.Value = value;
            return node;
        }

        /// <summary>
        /// Create FbxNode with node name and a set of properties.
        /// </summary>
        /// <param name="name">name of node</param>
        /// <param name="properties">properties of node</param>
        /// <returns></returns>
        public static FbxNode CreatePropertyNode (this FbxNode node, string name, object[] properties)
        {
            node.Name = name;
            
            foreach(object prop in properties)
                node.Properties.Add(prop);

            return node;
        }
    }
}
