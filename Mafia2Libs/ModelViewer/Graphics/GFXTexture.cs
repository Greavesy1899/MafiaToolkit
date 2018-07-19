using SharpDX.Direct3D11;
using SharpDX.WIC;
using ModelViewer.System;
using System;
using System.Diagnostics;

namespace ModelViewer.Graphics
{
    public class TextureClass
    {
        public ShaderResourceView TextureResource { get; private set; }
        public bool Init(Device device, string fileName)
        {
            try
            {
                using (var texture = LoadFromFile(device, new ImagingFactory(), fileName))
                {
                    ShaderResourceViewDescription srvDesc = new ShaderResourceViewDescription()
                    {
                        Format = texture.Description.Format,
                        Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2D,
                    };
                    srvDesc.Texture2D.MostDetailedMip = 0;
                    srvDesc.Texture2D.MipLevels = -1;

                    TextureResource = new ShaderResourceView(device, texture, srvDesc);
                    device.ImmediateContext.GenerateMips(TextureResource);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Shutdown()
        {
            TextureResource?.Dispose();
            TextureResource = null;
        }
        public Texture2D LoadFromFile(Device device, ImagingFactory factory, string fileName)
        {
            //System.Drawing.Bitmap bitmap;
            BitmapSource bs;
            try
            {
                //bitmap = DDS.LoadImage(SystemConfigClass.TexturePath + fileName, false);
                //bitmap.Save(SystemConfigClass.DataFilePath + fileName + ".bmp");
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                //Debug.WriteLine("Using game directory..");
                //bitmap = DDS.LoadImage(SystemConfigClass.GamePath + "/Textures/" + fileName, false);
                //bitmap.Save(SystemConfigClass.DataFilePath + fileName + ".bmp");

            }
            bs = LoadBitmap(factory, SystemConfigClass.DataFilePath + fileName + ".bmp");
            return CreateTextureFromBitmap(device, bs);
        }
        public BitmapSource LoadBitmap(ImagingFactory factory, string filename)
        {
            var bitmapDecoder = new BitmapDecoder(factory, filename, DecodeOptions.CacheOnDemand);
            var result = new FormatConverter(factory);
            result.Initialize(bitmapDecoder.GetFrame(0), PixelFormat.Format32bppPRGBA, BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);
            return result;
        }
        public Texture2D CreateTextureFromBitmap(Device device, BitmapSource bitmapSource)
        {
            int stride = bitmapSource.Size.Width * 4;
            using (var buffer = new SharpDX.DataStream(bitmapSource.Size.Height * stride, true, true))
            {
                bitmapSource.CopyPixels(stride, buffer);
                return new Texture2D(device, new Texture2DDescription()
                {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                    Usage = ResourceUsage.Default,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Format = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.GenerateMipMaps,
                    SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                },
                new SharpDX.DataRectangle(buffer.DataPointer, stride));
            }
        }

    }
}