using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Mafia2Tool;

namespace Rendering.Graphics
{
    public class DirectX11Class
    {
        private bool VerticalSyncEnabled { get; set; }
        public int VideoCardMemory { get; private set; }
        public string VideoCardDescription { get; private set; }
        private SwapChain SwapChain { get; set; }
        public SharpDX.Direct3D11.Device Device { get; private set; }
        public DeviceContext DeviceContext { get; private set; }
        private RenderTargetView RenderTargetView { get; set; }
        private Texture2D DepthStencilBuffer { get; set; }
        public DepthStencilState DepthStencilState { get; set; }
        private DepthStencilView DepthStencilView { get; set; }
        private RasterizerState RasterState { get; set; }

        private RasterizerStateDescription Rasterizer;

        public DirectX11Class()
        { }

        public bool Init(IntPtr WindowHandle)
        {
            var factory = new Factory1();
            var adapter = factory.GetAdapter1(0);
            var monitor = adapter.GetOutput(0);
            var modes = monitor.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);
            var rational = new Rational(0, 1);

            if (ToolkitSettings.VSync)
            {
                foreach (var mode in modes)
                {
                    if (mode.Width == ToolkitSettings.Width && mode.Height == ToolkitSettings.Height)
                    {
                        rational = new Rational(mode.RefreshRate.Numerator, mode.RefreshRate.Denominator);
                        break;
                    }
                }
            }

            var adapterDescription = adapter.Description;
            VideoCardMemory = adapterDescription.DedicatedVideoMemory >> 10 >> 10;
            VideoCardDescription = adapterDescription.Description.Trim('\0');
            monitor.Dispose();
            adapter.Dispose();
            factory.Dispose();

            var swapChainDesc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(ToolkitSettings.Width, ToolkitSettings.Height, rational, Format.R8G8B8A8_UNorm),
                Usage = Usage.RenderTargetOutput,
                OutputHandle = WindowHandle,
                SampleDescription = new SampleDescription(1, 0),
                IsWindowed = true,
                Flags = SwapChainFlags.None,
                SwapEffect = SwapEffect.Discard
            };

            SharpDX.Direct3D11.Device device;
            SwapChain swapChain;
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, swapChainDesc, out device, out swapChain);
            Device = device;
            SwapChain = swapChain;
            DeviceContext = device.ImmediateContext;
            var backBuffer = Texture2D.FromSwapChain<Texture2D>(SwapChain, 0);
            RenderTargetView = new RenderTargetView(device, backBuffer);
            backBuffer.Dispose();

            var depthBufferDesc = new Texture2DDescription()
            {
                Width = ToolkitSettings.Width,
                Height = ToolkitSettings.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            DepthStencilBuffer = new Texture2D(device, depthBufferDesc);

            var depthStencilDecs = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,
                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,
                FrontFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always,
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                }
            };

            DepthStencilState = new DepthStencilState(Device, depthStencilDecs);
            DeviceContext.OutputMerger.SetDepthStencilState(DepthStencilState, 1);

            var depthStencilViewDesc = new DepthStencilViewDescription()
            {
                Format = Format.D24_UNorm_S8_UInt,
                Dimension = DepthStencilViewDimension.Texture2D,
                Texture2D = new DepthStencilViewDescription.Texture2DResource()
                {
                    MipSlice = 0
                }
            };

            DepthStencilView = new DepthStencilView(Device, DepthStencilBuffer, depthStencilViewDesc);
            DeviceContext.OutputMerger.SetTargets(DepthStencilView, RenderTargetView);
            Rasterizer = new RasterizerStateDescription()
            {
                IsAntialiasedLineEnabled = true,
                CullMode = CullMode.None,
                DepthBias = 10,
                DepthBiasClamp = .0f,
                IsDepthClipEnabled = false,
                FillMode = FillMode.Solid,
                IsFrontCounterClockwise = true,
                IsMultisampleEnabled = true,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = .0f
            };

            UpdateRasterizer();
            return true;
        }
        public void ToggleCullMode()
        {
            Rasterizer.CullMode = (Rasterizer.CullMode == CullMode.None ? CullMode.Back : CullMode.None);
            UpdateRasterizer();
        }
        public void ToggleFillMode()
        {
            Rasterizer.FillMode = (Rasterizer.FillMode == FillMode.Solid ? FillMode.Wireframe : FillMode.Solid);
            UpdateRasterizer();
        }
        private void UpdateRasterizer()
        {
            RasterState = new RasterizerState(Device, Rasterizer);
            DeviceContext.Rasterizer.State = RasterState;
            DeviceContext.Rasterizer.SetViewport(0, 0, ToolkitSettings.Width, ToolkitSettings.Height, 0, 1);
        }
        public void SwapFillMode(FillMode mode)
        {
            Rasterizer.FillMode = mode;
            UpdateRasterizer(); 
        }
        public void Shutdown()
        {
            SwapChain?.SetFullscreenState(false, null);
            RasterState?.Dispose();
            RasterState = null;
            DepthStencilView?.Dispose();
            DepthStencilView = null;
            DepthStencilState?.Dispose();
            DepthStencilState = null;
            DepthStencilBuffer?.Dispose();
            DepthStencilBuffer = null;
            RenderTargetView?.Dispose();
            RenderTargetView = null;
            DeviceContext?.Dispose();
            DeviceContext = null;
            Device?.Dispose();
            Device = null;
            SwapChain?.Dispose();
            SwapChain = null;
        }
        /// <summary>
        /// Begin The scene. The Parameters are colours.
        /// </summary>
        public void BeginScene(float red, float green, float blue, float alpha)
        {
            DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1, 0);
            DeviceContext.ClearRenderTargetView(RenderTargetView, new Color4(red, green, blue, alpha));
        }
        public void EndScene()
        {
            if (VerticalSyncEnabled)
            {
                SwapChain.Present(1, 0);
            }
            else
            {
                SwapChain.Present(0, 0);
            }
        }
    }
}