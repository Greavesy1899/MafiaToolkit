using System;
using System.IO;

namespace ApexSDK
{
    public class RandomSubtextureModifier : Modifier
    {
        private float minSubtexture;
        private float maxSubtexture;

        public float MinSubtexture {
            get { return minSubtexture; }
            set { minSubtexture = value; }
        }
        public float MaxSubtexture {
            get { return maxSubtexture; }
            set { maxSubtexture = value; }
        }

        public RandomSubtextureModifier()
        {
            Type = ModifierType.RandomSubtexture;
        }

        public RandomSubtextureModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.RandomSubtexture;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            minSubtexture = reader.ReadSingle();
            maxSubtexture = reader.ReadSingle();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            writer.Write(minSubtexture);
            writer.Write(maxSubtexture);
        }

        public override string ToString()
        {
            return "RandomSubtextureModifierParams";
        }
    }
}
