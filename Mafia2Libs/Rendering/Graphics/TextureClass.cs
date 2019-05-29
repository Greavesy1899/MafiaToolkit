using SharpDX.Direct3D11;
using System.Diagnostics;
using Utils.Settings;

namespace Rendering.Graphics
{
    public static class TextureLoader
    {
        public static ShaderResourceView LoadTexture(Device d3d, string fileName)
        {
            Resource ddsResource;
            ShaderResourceView _temp;
            DDSTextureLoader.DDS_ALPHA_MODE mode;

            string texturePath = "";
            if (!System.IO.File.Exists(fileName))
            {
                Debug.WriteLine(string.Format("FAILED TO LOAD {0}", fileName));
                return RenderStorageSingleton.Instance.TextureCache[0];
            }
            else
            {
                string mipDds = fileName.Insert(0, "MIP_");
                if (System.IO.File.Exists(mipDds))
                    texturePath = mipDds;
                else
                    texturePath = fileName;
            }
            //Debug.WriteLine(string.Format("Loading {0}\tDevice State {1}", fileName, device));
            DDSTextureLoader.CreateDDSTextureFromFile(d3d, texturePath, out ddsResource, out _temp, 4096, out mode);
            return _temp;
        }
    }
}