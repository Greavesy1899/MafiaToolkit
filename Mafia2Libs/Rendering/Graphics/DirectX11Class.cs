using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Utils.Settings;

namespace Rendering.Graphics
{
    public class DirectX11Class
    {
        public int VideoCardMemory { get; private set; }
        public string VideoCardDescription { get; private set; }
        public SwapChain SwapChain { get; private set; }
        public SharpDX.Direct3D11.Device Device { get; private set; }
        public DeviceContext DeviceContext { get; private set; }
        public DepthStencilState DepthStencilState { get; set; }
        private RenderTargetView m_RenderTargetView { get; set; }
        private Texture2D m_depthStencilBuffer { get; set; }
        private DepthStencilView m_DepthStencilView { get; set; }  
        private RasterizerState m_RSSolid { get; set; }
        private RasterizerState m_RSWireFrame { get; set; }
        private RasterizerState m_RSCullSolid { get; set; }
        private RasterizerState m_RSCullWireFrame { get; set; }

        private RasterizerStateDescription m_RSDesc;
        private FillMode m_FillMode = FillMode.Solid;
        private CullMode m_CullMode = CullMode.Back;

        public DirectX11Class()
        { }

        public bool Init(IntPtr WindowHandle)
        {
            var factory = new Factory1();
            var adapter = factory.GetAdapter1(0);
            var monitor = adapter.GetOutput(0);
            var modes = monitor.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);
            var rational = new Rational(0, 1);
            var adapterDescription = adapter.Description;
            VideoCardMemory = adapterDescription.DedicatedVideoMemory >> 10 >> 10;
            VideoCardDescription = adapterDescription.Description.Trim('\0');
            monitor.Dispose();
            adapter.Dispose();
            factory.Dispose();

            var swapChainDesc = new SwapChainDescription()
            {
                BufferCount = 2,
                ModeDescription = new ModeDescription(1920, 1080, rational, Format.R8G8B8A8_UNorm),
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
            m_RenderTargetView = new RenderTargetView(device, backBuffer);
            backBuffer.Dispose();

            BlendStateDescription bsd = new BlendStateDescription()
            {
                AlphaToCoverageEnable = false,//true,
                IndependentBlendEnable = false,
            };
            bsd.RenderTarget[0].AlphaBlendOperation = BlendOperation.Add;
            bsd.RenderTarget[0].BlendOperation = BlendOperation.Add;
            bsd.RenderTarget[0].DestinationAlphaBlend = BlendOption.One;
            bsd.RenderTarget[0].DestinationBlend = BlendOption.InverseSourceAlpha;
            bsd.RenderTarget[0].IsBlendEnabled = true;
            bsd.RenderTarget[0].RenderTargetWriteMask = ColorWriteMaskFlags.All;
            bsd.RenderTarget[0].SourceAlphaBlend = BlendOption.Zero;
            bsd.RenderTarget[0].SourceBlend = BlendOption.SourceAlpha;
            //bsDefault = new BlendState(device, bsd);

            bsd.AlphaToCoverageEnable = true;
            BlendState bsAlpha = new BlendState(device, bsd);

            DeviceContext.OutputMerger.BlendState = bsAlpha;
            BuildDepthStencilView(1920, 1080);

            m_RSDesc = new RasterizerStateDescription()
            {
                IsAntialiasedLineEnabled = false,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = .0f,
                IsDepthClipEnabled = false,
                FillMode = FillMode.Solid,
                IsFrontCounterClockwise = true,
                IsMultisampleEnabled = true,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = .0f
            };

            
            m_RSCullSolid = new RasterizerState(Device, m_RSDesc);
            m_RSDesc.CullMode = CullMode.None;
            m_RSSolid = new RasterizerState(Device, m_RSDesc);
            m_RSDesc.FillMode = FillMode.Wireframe;
            m_RSWireFrame = new RasterizerState(Device, m_RSDesc);
            m_RSDesc.CullMode = CullMode.Back;
            m_RSCullWireFrame = new RasterizerState(Device, m_RSDesc);

            UpdateRasterizer();
            return true;
        }

        private void BuildDepthStencilView(int w, int h)
        {
            var depthBufferDesc = new Texture2DDescription()
            {
                Width = w,
                Height = h,
                MipLevels = 0,
                ArraySize = 1,
                Format = Format.D24_UNorm_S8_UInt,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            m_depthStencilBuffer = new Texture2D(Device, depthBufferDesc);

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

            m_DepthStencilView = new DepthStencilView(Device, m_depthStencilBuffer, depthStencilViewDesc);
            DeviceContext.OutputMerger.SetTargets(m_DepthStencilView, m_RenderTargetView);
        }

        public void ToggleCullMode()
        {
            m_CullMode = m_CullMode == CullMode.None ? CullMode.Back : CullMode.None;
            UpdateRasterizer();
        }
        public void ToggleFillMode()
        {
            m_FillMode = m_FillMode == FillMode.Solid ? FillMode.Wireframe : FillMode.Solid;
            UpdateRasterizer();
        }
        private void UpdateRasterizer()
        {
            if(m_CullMode == CullMode.None && m_FillMode == FillMode.Solid)
                DeviceContext.Rasterizer.State = m_RSSolid;
            else if (m_CullMode == CullMode.None && m_FillMode == FillMode.Wireframe)
                DeviceContext.Rasterizer.State = m_RSWireFrame;
            else if (m_CullMode == CullMode.Back && m_FillMode == FillMode.Solid)
                DeviceContext.Rasterizer.State = m_RSCullSolid;
            else if (m_CullMode == CullMode.Back && m_FillMode == FillMode.Wireframe)
                DeviceContext.Rasterizer.State = m_RSCullWireFrame;

            DeviceContext.Rasterizer.SetViewport(0, 0, 1920, 1080, 0, 1);
        }
        public void Shutdown()
        {
            SwapChain?.SetFullscreenState(false, null);
            m_RSSolid?.Dispose();
            m_RSSolid = null;
            m_RSWireFrame?.Dispose();
            m_RSWireFrame = null;
            m_RSCullSolid?.Dispose();
            m_RSCullSolid = null;
            m_RSCullWireFrame?.Dispose();
            m_RSCullWireFrame = null;
            m_DepthStencilView?.Dispose();
            m_DepthStencilView = null;
            DepthStencilState?.Dispose();
            DepthStencilState = null;
            m_depthStencilBuffer?.Dispose();
            m_depthStencilBuffer = null;
            m_RenderTargetView?.Dispose();
            m_RenderTargetView = null;
            DeviceContext?.Dispose();
            DeviceContext = null;
            Device?.Dispose();
            Device = null;
            SwapChain?.Dispose();
            SwapChain = null;
        }

        public void BeginScene(float red, float green, float blue, float alpha)
        {
            DeviceContext.ClearDepthStencilView(m_DepthStencilView, DepthStencilClearFlags.Depth, 1, 0);
            DeviceContext.ClearRenderTargetView(m_RenderTargetView, new Color4(red, green, blue, alpha));
        }

        public void Resize(int w, int h)
        {
            DeviceContext.OutputMerger.SetRenderTargets(null, null, null);
            m_RenderTargetView.Dispose();
            SwapChain.ResizeBuffers(0, w, h, Format.Unknown, SwapChainFlags.None);
            Texture2D buffer = SwapChain.GetBackBuffer<Texture2D>(0);
            m_RenderTargetView = new RenderTargetView(Device, buffer);
            buffer.Dispose();
            BuildDepthStencilView(w, h);
            DeviceContext.OutputMerger.SetRenderTargets(m_DepthStencilView, m_RenderTargetView);
            DeviceContext.Rasterizer.SetViewport(0, 0, w, h, ToolkitSettings.ScreenNear, ToolkitSettings.ScreenDepth);
        }

        public void EndScene()
        {
            if (ToolkitSettings.VSync)
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