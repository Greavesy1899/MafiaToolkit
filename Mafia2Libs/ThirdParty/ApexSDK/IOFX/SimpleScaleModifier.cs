using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ApexSDK
{
    public class SimpleScaleModifier : Modifier
    {
        private Vector3 scaleFactor;

        public Vector3 ScaleFactor {
            get { return scaleFactor; }
            set { scaleFactor = value; }
        }

        public SimpleScaleModifier()
        {
            Type = ModifierType.SimpleScale;
        }

        public SimpleScaleModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.SimpleScale;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            scaleFactor = Vector3Utils.ReadFromFile(reader);
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            Vector3Utils.WriteToFile(scaleFactor, writer);
        }

        public override string ToString()
        {
            return string.Format("ScaleFactor: {0}", scaleFactor);
        }
    }
}
