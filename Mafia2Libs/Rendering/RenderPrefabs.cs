using ResourceTypes.Materials;

namespace Rendering
{
    public class RenderPrefabs
    {
        public IMaterial GizmoRed { get; set; }
        public IMaterial GizmoBlue { get; set; }
        public IMaterial GizmoGreen { get; set; }

        public RenderPrefabs()
        {
            GizmoRed = new IMaterial();
            GizmoBlue = new IMaterial();
            GizmoGreen = new IMaterial();
            GizmoRed.MaterialName.String = "GizmoRed";
            GizmoRed.MaterialName.Hash = 1337;
            GizmoBlue.MaterialName.String = "GizmoBlue";
            GizmoBlue.MaterialName.Hash = 1338;
            GizmoGreen.MaterialName.String = "GizmoGreen";
            GizmoGreen.MaterialName.Hash = 1339;
            GizmoRed.ShaderID = GizmoBlue.ShaderID = GizmoGreen.ShaderID = 3854590933660942049;
            GizmoRed.ShaderHash = GizmoBlue.ShaderHash = GizmoGreen.ShaderHash = 601151254;

            MaterialParameter param = new MaterialParameter();
            param.ID = "C002";
            param.Paramaters = new float[4] { 1.0f, 0.0f, 0.0f, 1.0f };
            GizmoRed.Parameters.Add(param);

            param = new MaterialParameter();
            param.ID = "C002";
            param.Paramaters = new float[4] { 0.0f, 0.0f, 1.0f, 1.0f };
            GizmoBlue.Parameters.Add(param);

            param = new MaterialParameter();
            param.ID = "C002";
            param.Paramaters = new float[4] { 0.0f, 1.0f, 0.0f, 1.0f };
            GizmoGreen.Parameters.Add(param);
        }

        public void Shutdown()
        {
            GizmoRed = GizmoGreen = GizmoBlue = null;
        }
    }
}
