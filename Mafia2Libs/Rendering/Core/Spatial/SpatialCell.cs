using System;
using System.Collections.Generic;
using Rendering.Graphics;
using SharpDX.Direct3D11;
using SharpDX;
using ResourceTypes.Navigation;
using Utils.StringHelpers;

namespace Rendering.Core
{
    public class SpatialCell_InitParams
    {
        public BoundingBox CellExtents { get; set; }
        public GraphicsClass OwnGraphics { get; set; }
    }

    public class SpatialCell
    {
        protected Dictionary<int, IRenderer> assets;
        protected BoundingBox boundingBox;
        protected GraphicsClass OwnGraphicsClass;
        protected int RefID;

        public BoundingBox BoundingBox {
            get { return boundingBox; }
        }

        public SpatialCell(SpatialCell_InitParams InitParams)
        {
            OwnGraphicsClass = InitParams.OwnGraphics;
            boundingBox = InitParams.CellExtents;

            assets = new Dictionary<int, IRenderer>();

            RefID = StringHelpers.GetNewRefID();
        }

        public void AddAsset(IRenderer asset, int key)
        {
            assets.Add(key, asset);
        }

        public virtual void PreInitialise() { }
        public void Initialise(Device device, DeviceContext deviceContext)
        {
            foreach (KeyValuePair<int, IRenderer> entry in assets)
            {
                entry.Value.InitBuffers(device, deviceContext);
            }
        }

        public void Render(Device device, DeviceContext deviceContext, Camera camera)
        {
            /*foreach (KeyValuePair<int, IRenderer> entry in assets)
            {
                entry.Value.UpdateBuffers(device, deviceContext);
                entry.Value.Render(device, deviceContext, camera);
            }*/
        }

        public virtual void SetVisibility(bool bNewVisiblity) { }

        public void Shutdown()
        {
            foreach (KeyValuePair<int, IRenderer> entry in assets)
            {
                entry.Value.Shutdown();
            }
        }

        public int GetRefID()
        {
            return RefID;
        }

        public override string ToString()
        {
            return boundingBox.ToString();
        }
    }
}
