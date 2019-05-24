using System;
using System.IO;

namespace ApexSDK
{
    public class RandomRotationModifier : Modifier
    {
        private float minRotation;
        private float maxRotation;

        public float MinRotation {
            get { return minRotation; }
            set { minRotation = value; }
        }
        public float MaxRotation {
            get { return maxRotation; }
            set { maxRotation = value; }
        }

        public RandomRotationModifier()
        {
            Type = ModifierType.ModifierType_RandomRotation;
        }

        public RandomRotationModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.ModifierType_RandomRotation;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            minRotation = reader.ReadSingle();
            maxRotation = reader.ReadSingle();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            writer.Write(minRotation);
            writer.Write(maxRotation);
        }

        public override string ToString()
        {
            return string.Format("RandomRotationModifierParams");
        }
    }
}
