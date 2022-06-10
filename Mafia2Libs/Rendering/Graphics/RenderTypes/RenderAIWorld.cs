using Rendering.Core;
using ResourceTypes.Navigation;
using System.Numerics;
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

        public override void Shutdown()
        {
            base.Shutdown();

            OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(AIWorldBatch);
            AIWorldBatch = null;
        }
    }
}
