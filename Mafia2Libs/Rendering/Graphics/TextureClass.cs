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
            string dds = fileName.Remove(fileName.Length - 4, 4);
            dds += ".dds";
            Resource ddsResource;
            ShaderResourceView _temp;
            DDSTextureLoader.DDS_ALPHA_MODE mode;

            string texturePath = "";
            if (!System.IO.File.Exists(SystemConfigClass.DataFilePath + dds))
            {
                Debug.WriteLine("FAILED TO LOAD {0}", dds);
                texturePath = "texture.dds";
            }
            else
            {
                string mipDds = dds.Insert(0, "MIP_");
                if (System.IO.File.Exists(SystemConfigClass.DataFilePath + mipDds))
                    texturePath = mipDds;
                else
                    texturePath = dds;
            }
            Debug.WriteLine(string.Format("Loading {0}\tDevice State {1}", dds, device));
            DDSTextureLoader.CreateDDSTextureFromFile(device, SystemConfigClass.DataFilePath + texturePath, out ddsResource, out _temp, 4096, out mode);
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