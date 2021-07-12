using System.Numerics;

namespace Rendering.Graphics
{
    public class LightClass
    {
        public Vector4 AmbientColor { get; set; }
        public Vector4 DiffuseColour { get; set; }
        public Vector3 Direction { get; set; }
        public Vector4 SpecularColor { get; set; }
        public float SpecularPower { get; set; }

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

        public override bool Equals(object obj)
        {
            var @class = obj as LightClass;
            return @class != null &&
                   AmbientColor.Equals(@class.AmbientColor) &&
                   DiffuseColour.Equals(@class.DiffuseColour) &&
                   Direction.Equals(@class.Direction) &&
                   SpecularColor.Equals(@class.SpecularColor) &&
                   SpecularPower == @class.SpecularPower;
        }
    }
}