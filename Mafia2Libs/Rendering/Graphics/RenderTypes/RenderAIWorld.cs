using Rendering.Core;
using ResourceTypes.Navigation;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.StringHelpers;

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

            string BoxID = string.Format("AIWorld_{0}", StringHelpers.GetNewRefID());
            AIWorldBatch = new PrimitiveBatch(PrimitiveType.Box, BoxID);
            WorldInfo.PopulatePrimitiveBatch(AIWorldBatch);

            OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(AIWorldBatch);
        }

        public void RequestUpdate()
        {
            OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(AIWorldBatch);

            string BoxID = string.Format("AIWorld_{0}", StringHelpers.GetNewRefID());
            AIWorldBatch = new PrimitiveBatch(PrimitiveType.Box, BoxID);
            InitWorldInfo.PopulatePrimitiveBatch(AIWorldBatch);

            OwnGraphics.OurPrimitiveManager.AddPrimitiveBatch(AIWorldBatch);
        }

        public override void InitBuffers(Device d3d, DeviceContext deviceContext) { }

        public override void Render(Device device, DeviceContext deviceContext, Camera camera) { }

        public override void Select() { }

        public override void SetTransform(Matrix matrix) { }

        public override void Shutdown()
        {
            OwnGraphics.OurPrimitiveManager.RemovePrimitiveBatch(AIWorldBatch);
            AIWorldBatch = null;
        }

        public override void Unselect() { }

        public override void UpdateBuffers(Device device, DeviceContext deviceContext) { }
    }
}
