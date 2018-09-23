using Fbx;

namespace Mafia2.FBX
{
    public class FbxLayerElementUV : FbxLayer
    {
        double[] uvs;

        public double[] UVs {
            get { return uvs; }
            set { uvs = value; }
        }

        /// <summary>
        /// Fill LayerElementNormal data from node.
        /// </summary>
        /// <param name="node"></param>
        public FbxLayerElementUV(FbxNode node) : base(node)
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
                    case "UV":
                        uvs = (double[])n.Value;
                        break;
                }
            }
        }
    }
}
