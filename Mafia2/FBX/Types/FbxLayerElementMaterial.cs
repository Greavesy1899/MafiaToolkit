using Fbx;

namespace Mafia2.FBX
{
    public class FbxLayerElementMaterial : FbxLayer
    {
        byte[] materials;

        /// <summary>
        /// Fill LayerElementNormal data from node.
        /// </summary>
        /// <param name="node"></param>
        public FbxLayerElementMaterial(FbxNode node) : base(node)
        {
            ConvertLayer(node);
        }

        public override void ConvertLayer(FbxNode node)
        {
            base.ConvertLayer(node);
            foreach (FbxNode n in node.Nodes)
            {
                switch (n.Name)
                {
                    case "Materials":
                        materials = (byte[])n.Value;
                        break;
                }
            }
        }
    }
}
