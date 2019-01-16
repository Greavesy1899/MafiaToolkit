using SharpDX;

namespace ModelViewer.Programming.GraphicClasses
{
    public class LightClass
    {
        public Vector4 AmbientColor { get; private set; }
        public Vector4 DiffuseColour { get; private set; }
        public Vector3 Direction { get; set; }
        public Vector4 SpecularColor { get; private set; }
        public float SpecularPower { get; private set; }

        public void SetAmbientColor(float red, float green, float blue, float alpha)
        {
            AmbientColor = new Vector4(red, green, blue, alpha);
        }
        public void SetDiffuseColour(float red, float green, float blue, float alpha)
        {
            DiffuseColour = new Vector4(red, green, blue, alpha);
        }
        public void SetSpecularColor(float red, float green, float blue, float alpha)
        {
            SpecularColor = new Vector4(red, green, blue, alpha);
        }
        public void SetSpecularPower(float power)
        {
            SpecularPower = power;
        }
    }
}