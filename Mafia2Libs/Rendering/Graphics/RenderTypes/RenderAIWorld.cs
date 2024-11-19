using System.Numerics;
using Rendering.Core;
using ResourceTypes.Navigation;
using Toolkit.Core;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public class RenderAIWorld : IRenderer
    {
        private GraphicsClass OwnGraphics;

        private AIWorld InitWorldInfo;
        private PrimitiveBatch AIWorldBatch;

        public RenderAIWorld(GraphicsClass InOwnGraphics)
        {
            OwnGraphics = InOwnGraphics;
        }

        public void Init(AIWorld WorldInfo)
        {
            InitWorldInfo = WorldInfo;

            string BoxID = string.Format("AIWorld_{0}", RefManager.GetNewRefID());
            AIWorldBatch = new PrimitiveBatch(PrimitiveType.Box, BoxID);
            WorldInfo.PopulatePrimitiveBatch(AIWorldBatch);

            OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(AIWorldBatch);
        }

        public void RequestUpdate()
        {
            OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(AIWorldBatch);

            string BoxID = string.Format("AIWorld_{0}", RefManager.GetNewRefID());
            AIWorldBatch = new PrimitiveBatch(PrimitiveType.Box, BoxID);
            InitWorldInfo.PopulatePrimitiveBatch(AIWorldBatch);

            OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(AIWorldBatch);
        }

        public override void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext deviceContext) { }

        public override void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera) { }

        public override void Select() { }

        public override void SetTransform(Matrix4x4 matrix) { }

        public override void Shutdown()
        {
            OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(AIWorldBatch);
            AIWorldBatch = null;
        }

        public override void Unselect() { }

        public override void UpdateBuffers(ID3D11Device device, ID3D11DeviceContext deviceContext) { }
    }
}
