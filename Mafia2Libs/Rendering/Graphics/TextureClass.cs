using SharpDX.Direct3D11;
using ModelViewer.Programming.SystemClasses;
using System.Diagnostics;

namespace ModelViewer.Programming.GraphicClasses
{
    public class TextureClass
    {
        public ShaderResourceView TextureResource { get; private set; }
        public bool Init(Device device, string fileName)
        {
            Resource ddsResource;
            ShaderResourceView _temp;
            DDSTextureLoader.DDS_ALPHA_MODE mode;

            string texturePath = "";
            if (!System.IO.File.Exists(Mafia2Tool.ToolkitSettings.DataPath + fileName))
            {
                Debug.WriteLine("FAILED TO LOAD {0}", fileName);
                texturePath = "texture.dds";
            }
            else
            {
                string mipDds = fileName.Insert(0, "MIP_");
                if (System.IO.File.Exists(Mafia2Tool.ToolkitSettings.DataPath + mipDds))
                    texturePath = mipDds;
                else
                    texturePath = fileName;
            }
            Debug.WriteLine(string.Format("Loading {0}\tDevice State {1}", fileName, device));
            DDSTextureLoader.CreateDDSTextureFromFile(device, Mafia2Tool.ToolkitSettings.DataPath + texturePath, out ddsResource, out _temp, 4096, out mode);
            TextureResource = _temp;
            return true;
        }
        public void Shutdown()
        {
            TextureResource?.Dispose();
            TextureResource = null;
        }
    }
}