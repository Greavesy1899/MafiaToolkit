using System.IO;

namespace ApexSDK
{
    public class RandomScaleModifier : Modifier
    {
        private float minScale;
        private float maxScale;

        public float MinScale {
            get { return minScale; }
            set { minScale = value; }
        }
        public float MaxScale {
            get { return maxScale; }
            set { maxScale = value; }
        }

        public RandomScaleModifier()
        {
            Type = ModifierType.RandomScale;
        }

        public RandomScaleModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.RandomScale;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            minScale = reader.ReadSingle();
            maxScale = reader.ReadSingle();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            writer.Write(minScale);
            writer.Write(maxScale);
        }

        public override string ToString()
        {
            return "RandomScaleModifierParams";
        }
    }
}
