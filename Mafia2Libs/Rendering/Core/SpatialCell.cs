using Rendering.Graphics;
using ResourceTypes.Navigation;
using System.Collections.Generic;
using Toolkit.Core;
using Utils.StringHelpers;
using Vortice.Direct3D11;
using Vortice.Mathematics;

namespace Rendering.Core
{
    public class SpatialCell
    {
        Dictionary<int, IRenderer> assets;
        private BoundingBox boundingBox;

        public BoundingBox BoundingBox {
            get { return boundingBox; }
        }
        public SpatialCell(BoundingBox extents)
        {
            assets = new Dictionary<int, IRenderer>();
            boundingBox = extents;
        }

        public SpatialCell(KynogonRuntimeMesh.Cell cellData, BoundingBox extents)
        {
            assets = new Dictionary<int, IRenderer>();
            RenderNavCell cell = new RenderNavCell();
            cell.Init(cellData);
            assets.Add(RefManager.GetNewRefID(), cell);
            boundingBox = extents;
        }

        public void AddAsset(IRenderer asset, int key)
        {
            assets.Add(key, asset);
        }

        public void Initialise(ID3D11Device device, ID3D11DeviceContext deviceContext)
        {
            foreach (KeyValuePair<int, IRenderer> entry in assets)
            {
                entry.Value.InitBuffers(device, deviceContext);
            }
        }

        public void Render(ID3D11Device device, ID3D11DeviceContext deviceContext, Camera camera)
        {
            foreach (KeyValuePair<int, IRenderer> entry in assets)
            {
                entry.Value.UpdateBuffers(device, deviceContext);
                entry.Value.Render(device, deviceContext, camera);
            }
        }

        public void Shutdown()
        {
            foreach (KeyValuePair<int, IRenderer> entry in assets)
            {
                entry.Value.Shutdown();
            }
        }

        public override string ToString()
        {
            return boundingBox.ToString();
        }
    }
}
