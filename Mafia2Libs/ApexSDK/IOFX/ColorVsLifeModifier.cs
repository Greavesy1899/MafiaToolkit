using Mafia2;
using System;
using System.IO;
using Utils.StringHelpers;

namespace ApexSDK
{
    public class ColorVsLifeModifier : Modifier
    {
        private ColorChannel colorChannel;
        private int numControlPoints;
        private float[] controlPointsX;
        private float[] controlPointsY;

        public ColorChannel ColorChannel {
            get { return colorChannel; }
            set { colorChannel = value; }
        }
        public int NumControlPoints {
            get { return numControlPoints; }
            set { numControlPoints = value; }
        }
        public float[] ControlPointsX {
            get { return controlPointsX; }
            set { controlPointsX = value; }
        }
        public float[] ControlPointsY {
            get { return controlPointsY; }
            set { controlPointsY = value; }
        }

        public ColorVsLifeModifier()
        {
            Type = ModifierType.ModifierType_ColorVsLife;
        }

        public ColorVsLifeModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.ModifierType_ColorVsLife;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            Enum.TryParse(StringHelpers.ReadString32(reader), out colorChannel);
            numControlPoints = reader.ReadInt32();
            controlPointsY = controlPointsX = new float[numControlPoints];

            for (int i = 0; i != numControlPoints; i++)
                controlPointsX[i] = reader.ReadSingle();

            for (int i = 0; i != numControlPoints; i++)
                controlPointsY[i] = reader.ReadSingle();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("ColorVsLifeModifierParams");
        }
    }
}
