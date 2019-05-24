using System.IO;
using SharpDX;
using Utils.SharpDXExtensions;

namespace ApexSDK
{
    public class ScaleAlongVelocityModifier : Modifier
    {
        private float scaleFactor;

        public float ScaleFactor {
            get { return scaleFactor; }
            set { scaleFactor = value; }
        }

        public ScaleAlongVelocityModifier()
        {
            Type = ModifierType.ModifierType_ScaleAlongVelocity;
        }

        public ScaleAlongVelocityModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.ModifierType_ScaleAlongVelocity;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            scaleFactor = reader.ReadSingle();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            writer.Write(scaleFactor);
        }

        public override string ToString()
        {
            return string.Format("ScaleFactor: {0}", scaleFactor);
        }
    }
}
