using Mafia2;
using System;
using System.IO;
using Utils.StringHelpers;

namespace ApexSDK
{
    public class RotationModifier : Modifier
    {
        private ApexMeshParticleRollType rollType;
        private float maxSettleRate;
        private float maxRotationRate;

        public ApexMeshParticleRollType RollType {
            get { return rollType; }
            set { rollType = value; }
        }
        public float MaxSettleRate {
            get { return maxSettleRate; }
            set { maxSettleRate = value; }
        }
        public float MaxRotationRate {
            get { return maxRotationRate; }
            set { maxRotationRate = value; }
        }

        public RotationModifier()
        {
            Type = ModifierType.ModifierType_Rotation;
        }

        public RotationModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.ModifierType_Rotation;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            Enum.TryParse(StringHelpers.ReadString32(reader), out rollType);
            maxSettleRate = reader.ReadSingle();
            maxRotationRate = reader.ReadSingle();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("RotationModifierParams");
        }
    }
}
