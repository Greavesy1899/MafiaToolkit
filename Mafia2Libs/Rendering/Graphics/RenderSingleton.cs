using SharpDX;
using SharpDX.Direct3D11;
using System.Collections.Generic;
namespace Rendering.Graphics
{
    public sealed class RenderStorageSingleton
    {
        public Dictionary<ulong, ShaderResourceView> TextureCache;
        public ShaderManager ShaderManager;

        RenderStorageSingleton()
        {
            TextureCache = new Dictionary<ulong, ShaderResourceView>();
            ShaderManager = new ShaderManager();
        }

        public void Shutdown()
        {
           foreach(KeyValuePair<ulong, ShaderResourceView> texture in TextureCache)
            {
                texture.Value.Dispose();
            }
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
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly RenderStorageSingleton instance = new RenderStorageSingleton();
        }
    }
}
