using Gibbed.Squish;
using Mafia2Tool;
using ResourceTypes.Materials;
using SharpDX.Direct3D11;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Utils.Extensions;
using Utils.Logging;
using Utils.Settings;

namespace Rendering.Graphics
{
    public static class TextureLoader
    {
        private static bool ThumbnailCallback()
        {
            return false;
        }

        private static string GetTextureFromPath(string fileName, bool bAllowMIPs = true)
        {
            string path = "";
            bool bUseMIPs = bAllowMIPs && ToolkitSettings.UseMIPS;

            if (!fileName.Contains(".ifl"))
            {
                path = Path.Combine(SceneData.ScenePath, fileName);
                if (File.Exists(path))
                {
                    string mip = Path.Combine(SceneData.ScenePath, "MIP_" + fileName);
                    return (File.Exists(mip) && bUseMIPs ? mip : path);
                }

                if (!string.IsNullOrEmpty(ToolkitSettings.TexturePath) || Directory.Exists(ToolkitSettings.TexturePath))
                {
                    path = Path.Combine(ToolkitSettings.TexturePath, fileName);
                    if (File.Exists(path))
                    {
                        string mip = Path.Combine(ToolkitSettings.TexturePath, "MIP_" + fileName);
                        return (File.Exists(mip) && bUseMIPs ? mip : path);
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

        private static Image LoadDDSSquish(string name, ulong hash = 0)
        {
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            DdsFile dds = new DdsFile();

            // We do not have the texture present so we'll fallback to our empty default texture.
            if(!File.Exists(name))
            {
                return RenderStorageSingleton.Instance.TextureThumbnails[0];
            }

            using (var stream = File.Open(name, FileMode.Open))
            {
                try
                {
                    dds.Load(stream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error Message when using DDS Squish: {0}", ex.Message));
                    return LoadDDSSquish("Resources/texture.dds").GetThumbnailImage(128, 120, myCallback, IntPtr.Zero);
                }

            }
            Image Thumbnail = dds.Image().GetThumbnailImage(128, 120, myCallback, IntPtr.Zero);
            Debug.Assert(Thumbnail != null, string.Format("Thumbnail is wrong here? Trying to load: {0}", name));

            if(Thumbnail != null && hash != 0)
            {
                RenderStorageSingleton.Instance.TextureThumbnails.TryAdd(hash, Thumbnail);
            }

            return Thumbnail;
        }

        public static Image LoadThumbnail(string texturePath)
        {
            return LoadDDSSquish(texturePath);
        }

        public static Image LoadThumbnail(Material material)
        {
            string TexturePath = string.Empty;
            Image Thumbnail = null;
            ulong SamplerHash = 0;
            if (material != null)
            {
                MaterialSampler Sampler = material.GetSamplerByKey("S000");

                if (Sampler != null)
                {
                    // If our storage doesn't contain a thumbnail, then we go ahead and produce another.
                    if (!RenderStorageSingleton.Instance.TextureThumbnails.TryGetValue(Sampler.TextureHash, out Thumbnail))
                    {
                        SamplerHash = Sampler.TextureHash;
                        TexturePath = GetTextureFromPath(Sampler.File, false);
                    }
                }
            }
            else
            {
                Thumbnail = RenderStorageSingleton.Instance.TextureThumbnails[1];
            }

            return (Thumbnail != null ? Thumbnail : LoadDDSSquish(TexturePath, SamplerHash));
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