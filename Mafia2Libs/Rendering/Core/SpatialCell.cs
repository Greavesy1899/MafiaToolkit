using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rendering.Graphics;
using SharpDX.Direct3D11;
using SharpDX;
using ResourceTypes.Navigation;
using Utils.StringHelpers;

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

        public SpatialCell(KynogonRuntimeMesh.Cell cellData)
        {
            assets = new Dictionary<int, IRenderer>();
            RenderNavCell cell = new RenderNavCell();
            cell.Init(cellData);
            assets.Add(StringHelpers.GetNewRefID(), cell);
            //cell.BoundingBox = cellData.b
        }

        public void AddAsset(IRenderer asset, int key)
        {
            assets.Add(key, asset);
        }

        public void Initialise(Device device, DeviceContext deviceContext)
        {
            foreach (KeyValuePair<int, IRenderer> entry in assets)
            {
                entry.Value.InitBuffers(device, deviceContext);
            }
        }

        public void Render(Device device, DeviceContext deviceContext, Camera camera)
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
            return string.Format("Asset Count: {0}", assets.Count);
        }
    }
}
