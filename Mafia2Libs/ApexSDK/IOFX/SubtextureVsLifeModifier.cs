using System.IO;

namespace ApexSDK
{
    public class SubtextureVsLifeModifier : Modifier
    {
        private int numControlPoints;
        private float[] controlPointsX;
        private float[] controlPointsY;

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

        public SubtextureVsLifeModifier()
        {
            Type = ModifierType.SubtextureVsLife;
        }

        public SubtextureVsLifeModifier(BinaryReader reader)
        {
            ReadFromFile(reader);
            Type = ModifierType.SubtextureVsLife;
        }

        public override void ReadFromFile(BinaryReader reader)
        {
            numControlPoints = reader.ReadInt32();
            controlPointsY = controlPointsX = new float[numControlPoints];

            for (int i = 0; i != numControlPoints; i++)
                controlPointsX[i] = reader.ReadSingle();

            for (int i = 0; i != numControlPoints; i++)
                controlPointsY[i] = reader.ReadSingle();
        }

        public override void WriteToFile(BinaryWriter writer)
        {
            writer.Write(controlPointsX.Length);

            for(int i = 0; i != controlPointsX.Length; i++)
                writer.Write(controlPointsX[i]);

            for (int i = 0; i != controlPointsY.Length; i++)
                writer.Write(controlPointsY[i]);
        }

        public override string ToString()
        {
            return string.Format("SubtextureVsLifeModifierParams");
        }
    }
}
