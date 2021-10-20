using System;
using Utils.Settings;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Mathematics;

namespace Rendering.Graphics
{
    public class DirectX11Class
    {
        public int VideoCardMemory { get; private set; }
        public string VideoCardDescription { get; private set; }
        public IDXGISwapChain SwapChain { get; private set; }
        public ID3D11Device Device { get; private set; }
        public ID3D11DeviceContext DeviceContext { get; private set; }
        public ID3D11DepthStencilState DepthStencilState { get; set; }
        private ID3D11RenderTargetView m_RenderTargetView { get; set; }
        private ID3D11Texture2D m_depthStencilBuffer { get; set; }
        private ID3D11DepthStencilView m_DepthStencilView { get; set; }  
        private ID3D11RasterizerState m_RSSolid { get; set; }
        private ID3D11RasterizerState m_RSWireFrame { get; set; }
        private ID3D11RasterizerState m_RSCullSolid { get; set; }
        private ID3D11RasterizerState m_RSCullWireFrame { get; set; }

        private RasterizerDescription m_RSDesc;
        private FillMode m_FillMode = FillMode.Solid;
        private CullMode m_CullMode = CullMode.Back;
        public DirectX11Class()
        { }

        public bool Init(IntPtr WindowHandle)
        {
            var factory = DXGI.CreateDXGIFactory1<IDXGIFactory1>();
            var adapter = factory.GetAdapter1(0);
            var monitor = adapter.GetOutput(0);
            var modes = monitor.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);
            var rational = new Rational(0, 1);
            var adapterDescription = adapter.Description;
            //VideoCardMemory = adapterDescription.DedicatedVideoMemory >> 10 >> 10;
            //VideoCardDescription = adapterDescription.Description.Trim('\0');
            monitor.Dispose();
            adapter.Dispose();

            var swapChainDesc = new SwapChainDescription()
            {
                BufferCount = 2,
                BufferDescription = new ModeDescription(1920, 1080, rational, Format.R8G8B8A8_UNorm),
                Usage = Usage.RenderTargetOutput,
                OutputWindow = WindowHandle,
                SampleDescription = new SampleDescription(1, 0),
                IsWindowed = true,
                Flags = SwapChainFlags.None,
                SwapEffect = SwapEffect.Discard
            };

            // Create Device and DeviceContext
            ID3D11Device TempDevice = null;
            ID3D11DeviceContext TempDeviceContext = null;
            D3D11.D3D11CreateDevice(adapter, DriverType.Hardware, DeviceCreationFlags.None, null, out TempDevice, out TempDeviceContext);

            Device = TempDevice.QueryInterface<ID3D11Device1>();
            DeviceContext = TempDeviceContext.QueryInterface<ID3D11DeviceContext1>();
            TempDevice.Dispose();
            TempDeviceContext.Dispose();

            // Create SwapChain
            SwapChain = factory.CreateSwapChain(Device, swapChainDesc);
            factory.MakeWindowAssociation(WindowHandle, WindowAssociationFlags.IgnoreAltEnter);

            var backBuffer = SwapChain.GetBuffer<ID3D11Texture2D>(0);
            m_RenderTargetView = Device.CreateRenderTargetView(backBuffer);
            backBuffer.Dispose();

            // Create blend state
            BlendDescription bsd = new BlendDescription()
            {
                AlphaToCoverageEnable = false,//true,
                IndependentBlendEnable = false,
            };
            bsd.RenderTarget[0].BlendOperationAlpha = BlendOperation.Add;
            bsd.RenderTarget[0].BlendOperation = BlendOperation.Add;
            bsd.RenderTarget[0].DestinationBlendAlpha = Blend.One;
            bsd.RenderTarget[0].DestinationBlend = Blend.InverseSourceAlpha;
            bsd.RenderTarget[0].IsBlendEnabled = true;
            bsd.RenderTarget[0].RenderTargetWriteMask = ColorWriteEnable.All;
            bsd.RenderTarget[0].SourceBlendAlpha = Blend.Zero;
            bsd.RenderTarget[0].SourceBlend = Blend.SourceAlpha;
            bsd.AlphaToCoverageEnable = true;

            ID3D11BlendState bsAlpha = Device.CreateBlendState(bsd);

            // Set Blend State
            DeviceContext.OMSetBlendState(bsAlpha);
            BuildDepthStencilView(1920, 1080);

            // Create rasterizers
            m_RSDesc = new RasterizerDescription()
            {
                AntialiasedLineEnable = false,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = .0f,
                DepthClipEnable = false,
                FillMode = FillMode.Solid,
                FrontCounterClockwise = true,
                MultisampleEnable = true,
                ScissorEnable = false,
                SlopeScaledDepthBias = .0f
            };

            m_RSCullSolid = Device.CreateRasterizerState(m_RSDesc);
            m_RSDesc.CullMode = CullMode.None;
            m_RSSolid = Device.CreateRasterizerState(m_RSDesc);
            m_RSDesc.FillMode = FillMode.Wireframe;
            m_RSWireFrame = Device.CreateRasterizerState(m_RSDesc);
            m_RSDesc.CullMode = CullMode.Back;
            m_RSCullWireFrame = Device.CreateRasterizerState(m_RSDesc);

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

            m_depthStencilBuffer = Device.CreateTexture2D(depthBufferDesc);

            var depthStencilDecs = new DepthStencilDescription()
            {
                DepthEnable = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthFunc = ComparisonFunction.Less,
                StencilEnable = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,
                FrontFace = new DepthStencilOperationDescription()
                {
                    StencilFailOp = StencilOperation.Keep,
                    StencilDepthFailOp = StencilOperation.Incr,
                    StencilPassOp = StencilOperation.Keep,
                    StencilFunc = ComparisonFunction.Always,
                },
                BackFace = new DepthStencilOperationDescription()
                {
                    StencilFailOp = StencilOperation.Keep,
                    StencilDepthFailOp = StencilOperation.Decr,
                    StencilPassOp = StencilOperation.Keep,
                    StencilFunc = ComparisonFunction.Always,
                }
            };

            DepthStencilState = Device.CreateDepthStencilState(depthStencilDecs);
            DeviceContext.OMSetDepthStencilState(DepthStencilState);

            var depthStencilViewDesc = new DepthStencilViewDescription()
            {
                Format = Format.D24_UNorm_S8_UInt,
                ViewDimension = DepthStencilViewDimension.Texture2D,
                Texture2D = new Texture2DDepthStencilView()
                {
                    MipSlice = 0
                }
            };

            m_DepthStencilView = Device.CreateDepthStencilView(m_depthStencilBuffer, depthStencilViewDesc);
            DeviceContext.OMSetRenderTargets(m_RenderTargetView, m_DepthStencilView);
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
            if (m_CullMode == CullMode.None && m_FillMode == FillMode.Solid)
            {
                DeviceContext.RSSetState(m_RSCullSolid);
            }
            else if (m_CullMode == CullMode.None && m_FillMode == FillMode.Wireframe)
            {
                DeviceContext.RSSetState(m_RSWireFrame);
            }
            else if (m_CullMode == CullMode.Back && m_FillMode == FillMode.Solid)
            {
                DeviceContext.RSSetState(m_RSCullSolid);
            }
            else if (m_CullMode == CullMode.Back && m_FillMode == FillMode.Wireframe)
            {
                DeviceContext.RSSetState(m_RSCullWireFrame);
            }

            DeviceContext.RSSetViewport(0, 0, 1920, 1080, 0, 1);
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
            // TODO:
            //DeviceContext.OMSetRenderTargets(null);
            m_RenderTargetView.Dispose();
            SwapChain.ResizeBuffers(0, w, h, Format.Unknown, SwapChainFlags.None);
            ID3D11Texture2D buffer = SwapChain.GetBuffer<ID3D11Texture2D>(0);
            //m_RenderTargetView = 
            //buffer.Dispose();

            //BuildDepthStencilView(w, h);

            //DeviceContext.OMSetRenderTargets(m_RenderTargetView, m_DepthStencilView);
            //DeviceContext.RSSetViewport(0, 0, w, h, ToolkitSettings.ScreenNear, ToolkitSettings.ScreenDepth);
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