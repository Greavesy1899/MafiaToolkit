using Mafia2;
using System;
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
            Type = ModifierType.ModifierType_RandomScale;
        }

        public RandomScaleModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.ModifierType_RandomScale;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            minScale = reader.ReadSingle();
            maxScale = reader.ReadSingle();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("RandomScaleModifierParams");
        }
    }
}
