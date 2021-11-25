using Rendering.Graphics;
using System.Numerics;

namespace Rendering.Core
{
    public class WorldSettings
    {
        public LightClass Lighting { get; set; }
        
        public int RenderMode { get; set; }
        public bool RenderSky { get; set; }

        public WorldSettings()
        {
            Lighting = new LightClass();
            RenderMode = 2;
            RenderSky = false;
        }

        public void SetupLighting()
        {
            Lighting.SetAmbientColor(0.5f, 0.5f, 0.5f, 1f);
            Lighting.SetDiffuseColour(0.5f, 0.5f, 0.5f, 1f);
            Lighting.Direction = new Vector3(-0.2f, -1f, -0.3f);
            Lighting.SetSpecularColor(1.0f, 1.0f, 1.0f, 1.0f);
            Lighting.SetSpecularPower(255.0f);
        }

        public void Shutdown()
        {
            Lighting = null;
        }
    }
}
