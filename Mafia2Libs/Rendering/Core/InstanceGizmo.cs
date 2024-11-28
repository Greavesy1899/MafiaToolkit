using Rendering.Graphics;
using ResourceTypes.Translokator;
using System.Numerics;
using System.Runtime.CompilerServices;
using Utils.Logging;
using Utils.VorticeUtils;
using Vortice.Direct3D;
using Vortice.Direct3D11;

namespace Rendering.Core
{
    public class InstanceGizmo
    {
        private Vector3 ModelScale = new Vector3(0.015f);
        // Variable for rendering
        public RenderModel InstanceModel;

        public InstanceGizmo(RenderModel InModel)
        {
            InstanceModel = InModel;
        }

        public void InitBuffers(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            InstanceModel.InitBuffers(d3d, d3dContext);//texture should be loaded either differently or rendersingleton should precache it
            InstanceModel.AOTexture = LoadTexture(d3d, d3dContext);
        }
        
        public ID3D11ShaderResourceView LoadTexture(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            try
            {
                ID3D11Resource ddsResource;
                ID3D11ShaderResourceView _temp;
                DDSTextureLoader.DDS_ALPHA_MODE mode;
                DDSTextureLoader.CreateDDSTextureFromFile(d3d, d3dContext, "Resources/Translokator_Texture.dds", out ddsResource, out _temp, 4096, out mode);
                return _temp;
            }
            catch
            {
                Log.WriteLine(string.Format("Failed to load file: {0}", "Resources/Translokator_Texture.dds"), LoggingTypes.FATAL, LogCategoryTypes.IO);
                return null;
            }
        }

        public void UpdateInstanceBuffer(Instance instance, ID3D11Device d3d)
        {
            Matrix4x4 newtransform = MatrixUtils.SetMatrix(instance.Quaternion, ModelScale, instance.Position);

            if (!InstanceModel.InstanceTransforms.ContainsKey(instance.RefID))
            {
                InstanceModel.InstanceTransforms.Add(instance.RefID, Matrix4x4.Transpose(newtransform));
            }
            else
            {
                InstanceModel.InstanceTransforms[instance.RefID] = Matrix4x4.Transpose(newtransform);
            }
            
            InstanceModel.ReloadInstanceBuffer(d3d);
        }

        public void UpdateBuffers(ID3D11Device d3d, ID3D11DeviceContext d3dContext)
        {
            InstanceModel.UpdateBuffers(d3d, d3dContext);
        }

        public void Render(ID3D11Device d3d, ID3D11DeviceContext d3dContext, Camera camera)//render only instances
        {
            if (InstanceModel.InstanceTransforms.Count > 0)
            {
                VertexBufferView VertexBufferView = new VertexBufferView(InstanceModel.GetVB(), Unsafe.SizeOf<VertexLayouts.NormalLayout.Vertex>(), 0);//polish so getib/vb doesnt have to be used
                d3dContext.IASetVertexBuffers(0, VertexBufferView);
                d3dContext.IASetIndexBuffer(InstanceModel.GetIB(), Vortice.DXGI.Format.R32_UInt, 0);
                d3dContext.IASetPrimitiveTopology(PrimitiveTopology.TriangleList);
                d3dContext.PSSetShaderResource(2, InstanceModel.AOTexture);

                InstanceModel.RenderInstances(d3dContext, camera, d3d);
            } 
        }

        public void InstanceTranslokator(Instance instance,ID3D11Device device = null)
        {
            Matrix4x4 newtransform = new Matrix4x4();

            newtransform = MatrixUtils.SetMatrix(instance.Quaternion, new Vector3(0.015f,0.015f,0.015f), instance.Position);//fbx to m2t enlarged the mesh so, beware gltf

            if (!InstanceModel.InstanceTransforms.ContainsKey(instance.RefID))
            {
                InstanceModel.InstanceTransforms.Add(instance.RefID, Matrix4x4.Transpose(newtransform));
                if (device!=null)
                {
                    InstanceModel.ReloadInstanceBuffer(device);
                }
            }
        }

        public void Select(int InstanceId)
        {
            InstanceModel.SelectInstance(InstanceId);
        }

        public void Unselect()
        {
            InstanceModel.UnselectInstance();
        }
        
        public void Shutdown()
        {
            InstanceModel.Shutdown();
        }
    }
}
