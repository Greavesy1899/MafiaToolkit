using Mafia2Tool;
using SharpDX.Direct3D11;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Utils.Logging;
using Utils.Settings;

namespace Rendering.Graphics
{
    public static class TextureLoader
    {
        private static string GetTextureFromPath(string fileName)
        {
            string path = "";
            if (!fileName.Contains(".ifl"))
            {
                path = Path.Combine(SceneData.ScenePath, fileName);
                if (File.Exists(path))
                {
                    string mip = Path.Combine(SceneData.ScenePath, "MIP_" + fileName);
                    if (File.Exists(mip) && ToolkitSettings.UseMIPS)
                        return mip;
                    else
                        return path;
                }

                if (!string.IsNullOrEmpty(ToolkitSettings.TexturePath) || Directory.Exists(ToolkitSettings.TexturePath))
                {
                    path = Path.Combine(ToolkitSettings.TexturePath, fileName);
                    if (File.Exists(path))
                    {
                        string mip = Path.Combine(ToolkitSettings.TexturePath, "MIP_" + fileName);
                        if (File.Exists(mip) && ToolkitSettings.UseMIPS)
                            return mip;
                        else
                            return path;
                    }
                }
            }
            path = Path.Combine("Resources", "texture.dds");
            if (File.Exists(path))
            {
                return path;
            }
            else
            {
                Log.WriteLine(string.Format("Failed to load file: {0}", path), LoggingTypes.FATAL, LogCategoryTypes.IO);
                throw new System.Exception("Unable to locate texture.dds! This should be located in the texture folder assigned. You can assign it in Options > Render > Texture Directory.");
            }
        }

        public static ShaderResourceView LoadTexture(Device d3d, DeviceContext d3dContext, string fileName)
        {
            try
            {
                Resource ddsResource;
                ShaderResourceView _temp;
                DDSTextureLoader.DDS_ALPHA_MODE mode;
                string texturePath = GetTextureFromPath(fileName);
                DDSTextureLoader.CreateDDSTextureFromFile(d3d, d3dContext, texturePath, out ddsResource, out _temp, 4096, out mode);
                return _temp;
            }
            catch
            {
                Log.WriteLine(string.Format("Failed to load file: {0}", fileName), LoggingTypes.FATAL, LogCategoryTypes.IO);
                return null;
            }
            
        }
    }
}