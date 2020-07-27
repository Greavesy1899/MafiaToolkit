using SharpDX.Direct3D11;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace Rendering.Graphics
{
    public sealed class RenderStorageSingleton
    {
        public List<RenderLine> SplineStorage;
        public Dictionary<ulong, RenderStaticCollision> StaticCollisions;
        public Dictionary<ulong, ShaderResourceView> TextureCache;
        public Dictionary<ulong, Image> TextureThumbnails;
        public ShaderManager ShaderManager;
        public RenderPrefabs Prefabs;
        private bool isInit;

        RenderStorageSingleton()
        {
            SplineStorage = new List<RenderLine>();
            StaticCollisions = new Dictionary<ulong, RenderStaticCollision>();
            TextureCache = new Dictionary<ulong, ShaderResourceView>();
            TextureThumbnails = new Dictionary<ulong, Image>();
            ShaderManager = new ShaderManager();
            Prefabs = new RenderPrefabs();
        }

        public bool IsInitialised()
        {
            return isInit;
        }

        public bool Initialise(DirectX11Class D3D)
        {
            if (!ShaderManager.Init(D3D.Device))
            {
                MessageBox.Show("Failed to initialize Shader Manager!");
                return false;
            }
            //this is backup!
            Instance.TextureCache.Add(0, TextureLoader.LoadTexture(D3D.Device, D3D.DeviceContext, "texture.dds"));
            Instance.TextureCache.Add(1, TextureLoader.LoadTexture(D3D.Device, D3D.DeviceContext, "default_n.dds"));
            isInit = true;
            return true;
        }

        public void Shutdown()
        {
            foreach (KeyValuePair<ulong, ShaderResourceView> texture in TextureCache)
            {
                texture.Value.Dispose();
            }

            foreach (RenderLine line in SplineStorage)
            {
                line.Shutdown();
            }

            foreach (KeyValuePair<ulong, RenderStaticCollision> col in StaticCollisions)
            {
                col.Value.Shutdown();
            }

            SplineStorage.Clear();
            StaticCollisions.Clear();
            TextureCache.Clear();
            TextureThumbnails.Clear();
            ShaderManager.Shutdown();
            Prefabs.Shutdown();
            isInit = false;
        }

        public static RenderStorageSingleton Instance {
            get {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested()
            {
            }

            internal static readonly RenderStorageSingleton instance = new RenderStorageSingleton();
        }
    }
}
