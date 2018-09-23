using Fbx;

namespace Mafia2.FBX
{
    public class FbxLayerElementNormal : FbxLayer
    {
        double[] normals;
        byte[] normalsW;

        public double[] Normals {
            get { return normals; }
            set { normals = value; }
        }

        /// <summary>
        /// Fill LayerElementNormal data from node.
        /// </summary>
        /// <param name="node"></param>
        public FbxLayerElementNormal(FbxNode node) : base(node)
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
                    case "Normals":
                        normals = (double[])n.Value;
                        break;
                    case "NormalsW":
                        normalsW = (byte[])n.Value;
                        break;
                }
            }
        }
    }
}
