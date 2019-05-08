using SharpDX.Direct3D11;
using System.Collections.Generic;

namespace Rendering.Graphics
{
    public sealed class RenderStorageSingleton
    {
        public List<RenderLine> SplineStorage;
        public Dictionary<ulong, RenderStaticCollision> StaticCollisions;
        public Dictionary<ulong, ShaderResourceView> TextureCache;
        public ShaderManager ShaderManager;

        RenderStorageSingleton()
        {
            SplineStorage = new List<RenderLine>();
            StaticCollisions = new Dictionary<ulong, RenderStaticCollision>();
            TextureCache = new Dictionary<ulong, ShaderResourceView>();
            ShaderManager = new ShaderManager();
        }

        public void Shutdown()
        {
           foreach(KeyValuePair<ulong, ShaderResourceView> texture in TextureCache)
                texture.Value.Dispose();

           foreach(RenderLine line in SplineStorage)
                line.Shutdown();

            foreach (KeyValuePair<ulong, RenderStaticCollision> col in StaticCollisions)
                col.Value.Shutdown();

            SplineStorage.Clear();
            StaticCollisions.Clear();
            TextureCache.Clear();
            ShaderManager.Shutdown();
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
